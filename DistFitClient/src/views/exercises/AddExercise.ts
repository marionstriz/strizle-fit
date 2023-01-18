import { IUserExercise } from './../../domain/IUserExercise';
import { UserExerciseService } from './../../services/UserExerciseService';
import { IPerformance } from './../../domain/IPerformance';
import { ExerciseTypeService } from './../../services/ExerciseTypeService';
import { PerformanceService } from './../../services/PerformanceService';
import { IExerciseType } from './../../domain/IExerciseType';
import { IRouter } from "aurelia";
import { IdentityService } from "../../services/IdentityService";
import Helpers from '../../helpers/Helpers';
import {v4 as uuidv4} from 'uuid';

export class AddExercise {

    error?: string;

    exerciseTypes: IExerciseType[] = [];
    userExercises: IUserExercise[] = [];

    typeId?: string;
    performedDate?: Date;
    performedTime?: string;

    constructor(private identityService: IdentityService,
        private exerciseTypeService: ExerciseTypeService,
        private userExerciseService: UserExerciseService,
        private performanceService: PerformanceService,
        @IRouter private router: IRouter) {

            exerciseTypeService.getAllAsync(identityService).then(res => {
                if (res.error != null) {
                    identityService.logout();
                } else {
                    this.exerciseTypes = res.data!;
                }
            })

            userExerciseService.getAllAsync(identityService).then(res => {
                if (res.error != null) {
                    identityService.logout();
                } else {
                    this.userExercises = res.data!;
                }
            })
    }

    async backToListAsync() {
        await this.router.load('/exercises');
    }

    async addAndGoToListAsync() {
        let perf = await this.addAsync();
        if (!perf) return;
        await this.backToListAsync();
    }

    async addAndGoToSetsAsync() {
        let perf = await this.addAsync();
        if (!perf) return;
        await this.router.load('/sets/' + perf.id);
    }

    async addAsync(): Promise<IPerformance | null> {
        this.error = '';
        if (!this.typeId || !this.performedDate || !this.performedTime) {
            this.error = 'Please enter all values';
            return null;
        }

        let userExercise = this.userExercises.find(ue => ue.exerciseTypeId === this.typeId!);

        if (!userExercise) {
            let newUserExercise: IUserExercise = {
                id: uuidv4(),
                exerciseTypeId: this.typeId!
            }
            let res = await this.userExerciseService.addAsync(newUserExercise, this.identityService);

            if (res.error) {
                this.error = 'An error occurred, please try again';
                return null;
            }
            userExercise = newUserExercise;
        }

        let newDate = Helpers.mergeDateAndTime(this.performedDate!, this.performedTime!);

        let newPerf: IPerformance = {
            id: uuidv4(),
            userExerciseId: userExercise.id!,
            performedAt: newDate
        }

        let res = await this.performanceService.addAsync(newPerf, this.identityService);

        if (res.error) {
            this.error = 'An error occurred, please try again';
            return null;
        }
        return newPerf;
    }
}