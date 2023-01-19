import { UnitService } from './../../services/UnitService';
import { IUnit } from './../../domain/IUnit';
import { ISetEntry } from './../../domain/ISetEntry';
import { SetService } from './../../services/SetService';
import { IPerformance } from './../../domain/IPerformance';
import { IRouter, Params } from "aurelia";
import { IdentityService } from "../../services/IdentityService";
import { PerformanceService } from "../../services/PerformanceService";
import Helpers from '../../helpers/Helpers';
import {v4 as uuidv4} from 'uuid';

export class Sets {

    performanceId: string = '';
    performance?: IPerformance;
    sets: ISetEntry[] = [];

    units: IUnit[] = [];
    quantityUnits: IUnit[] = [];
    weightUnits: IUnit[] = [];

    adding: boolean = false;
    load: boolean = false;

    quantityValue?: number;
    quantityUnitId?: string;
    weightValue?: number;
    weightUnitId?: string;

    error?: string;
    addingError?: string;

    constructor(private identityService: IdentityService,
        private setService: SetService,
        private performanceService: PerformanceService,
        private unitService: UnitService,
        @IRouter private router: IRouter) {
    }

    async loading(params: Params) {
        this.performanceId = params.id!;

        let res = await this.performanceService.getAsync(this.performanceId, this.identityService);
        let errorString = 'Unable to retrieve data, please try again or contact administrator';

        if (res.error) {
            this.error = errorString;
        } else {
            this.performance = res.data!
        }

        let setRes = await this.setService.getByPerformanceIdAsync(this.performanceId, this.identityService);

        if (setRes.error) {
            this.error = errorString;
        } else {
            this.sets = setRes.data!
        }

        await this.setUnitsAsync(errorString);
    }

    toLocaleDateString(dateString: string): string {
        return Helpers.toLocaleDateString(dateString);
    }

    isNoWeightPerformance() {
        return this.performance && this.performance.userExercise!.exerciseType!.name === 'Running';
    }

    loadPerformancesAsync() {
        if (this.load) return;
        this.router.load('/exercises');
    }

    async deleteSetAsync(index: number) {
        if (this.load) return;
        this.load = true;
        let btn = document.querySelector('.delete-btn-' + index);
        btn!.innerHTML = '...';

        let set = this.sets.at(index);

        let res = await this.setService.deleteAsync(set!.id!, this.identityService);
        if (res.error) {
            console.log(set!.id!);
            this.error = 'Delete failed, please try again';
        } else {
            this.sets.splice(index, 1);
        }
        this.load = false;
    }

    addingSet() {
        this.adding = true;
    }

    async addSetAsync() {
        if (!this.quantityValue || !this.quantityUnitId || 
            !this.isNoWeightPerformance() && (!this.weightValue || !this.weightUnitId)) {
                this.addingError = 'All values must be filled';
                return;
        }
        if (this.load) return;
        this.load = true;
        let newSet: ISetEntry = {
            id: uuidv4(),
            performanceId: this.performanceId,
            quantity: this.quantityValue!,
            quantityUnitId: this.quantityUnitId!,
            weight: this.weightValue,
            weightUnitId: this.weightUnitId
        };
        
        let res = await this.setService.addAsync(newSet, this.identityService);
        if (res.error) {
            this.addingError = 'Could not add set, please try again';
            this.load = false;
        } else {
            newSet.quantityUnit = this.units.find(u => u.id === this.quantityUnitId);
            newSet.weightUnit = this.units.find(u => u.id === this.weightUnitId);

            this.sets.push(newSet);
            this.cancelAdding();
        }
    }

    cancelAdding() {
        this.quantityValue = undefined;
        this.quantityUnitId = undefined;
        this.weightValue = undefined;
        this.weightUnitId = undefined;
        this.addingError = undefined;
        this.adding = false;
        this.load = false;
    }

    private async setUnitsAsync(errorString: string): Promise<void> {
        let unitRes = await this.unitService.getAllAsync(this.identityService);

        if (unitRes.error) {
            this.error = errorString;
            return;
        } else {
            this.units = unitRes.data!
        }

        let exTypeName = this.performance?.userExercise?.exerciseType!.name;
        if (exTypeName === 'Squat' || exTypeName === 'Deadlift') {
            this.quantityUnits = this.units.filter(u => u.symbol === 'x');
            this.weightUnits = this.units.filter(u => u.symbol === 'kg' || u.symbol === 'lbs');
        }
    }
}