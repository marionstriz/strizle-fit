import { IExerciseType } from './../domain/IExerciseType';
import { BaseService } from './BaseService';

export class ExerciseTypeService extends BaseService<IExerciseType> {

    constructor() {
        super('/exercisetype');
    }
}