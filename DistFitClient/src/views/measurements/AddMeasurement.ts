import { Measurements } from './Measurements';
import { IMeasurement } from './../../domain/IMeasurement';
import { MeasurementService } from './../../services/MeasurementService';
import { IMeasurementType } from "../../domain/IMeasurementType";
import { IUnit } from "../../domain/IUnit";
import { IdentityService } from "../../services/IdentityService";
import { MeasurementTypeService } from "../../services/MeasurementTypeService";
import { UnitService } from "../../services/UnitService";
import { EventAggregator, IRouter } from 'aurelia';
import Helpers from '../../helpers/Helpers';

export class AddMeasurement {

    measurementTypes: IMeasurementType[] = [];
    units: IUnit[] = [];

    validUnits: IUnit[] = []

    typeId?: string;
    unitId?: string;
    value?: number;
    measuredDate?: Date;
    measuredTime?: string;

    error?: string;
    load: boolean;
    
    constructor(private identityService: IdentityService,
        private typeService: MeasurementTypeService,
        private measurementService: MeasurementService,
        private unitService: UnitService,
        private eventAggregator: EventAggregator,
        @IRouter private router: IRouter) {

        this.load = false;
        unitService.getAllAsync(identityService).then((res) => {
            if (res.error != null) {
                identityService.logout();
            } else {
                this.units = res.data!;
                typeService.getAllAsync(identityService).then((res) => {
                    if (res.error != null) {
                        identityService.logout();
                    } else {
                        this.measurementTypes = res.data!.sort((a, b) => {
                            return a.name.localeCompare(b.name)
                        });
        
                        this.changeUnitsToType(this.measurementTypes[0].id);
                    }
                });
            }
        });
    }

    changeUnitsToType(typeId: string) {
        let type = this.measurementTypes.find(t => t.id == typeId);

        if (type?.name == 'Weight') {
            this.validUnits = this.units.filter(u => u.symbol === 'kg' || u.symbol === 'lbs');
        } else if (type?.name === 'Height' || type?.name === 'Waist circumference' || type?.name === 'Hip circumference') {
            this.validUnits = this.units.filter(u => u.symbol === 'cm' || u.symbol === 'in');
        }
        
        else this.validUnits = this.units;
    }

    async addMeasurementAsync() {

        if (this.load) return;
        this.error = '';
        if (!this.value || !this.unitId || !this.typeId || !this.measuredDate || !this.measuredTime) {
            this.error = 'Please enter all values';
            return;
        }

        let newDate = Helpers.mergeDateAndTime(this.measuredDate!, this.measuredTime!);

        var measurement: IMeasurement = {
            value: this.value!,
            valueUnitId: this.unitId!,
            measurementTypeId: this.typeId!,
            measuredAt: newDate
        }

        this.load = true;
        await this.measurementService.addAsync(measurement, this.identityService);

        await this.router.load('/measurements/graphs/' + this.typeId! + '/scsa');
    }

    async backToListAsync() {
        if (this.load) return;
        this.load = true;
        await this.router.load('/measurements');
    }
}