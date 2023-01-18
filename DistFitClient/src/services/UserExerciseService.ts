import { IUserExercise } from './../domain/IUserExercise';
import { BaseService } from './BaseService';

export class UserExerciseService extends  BaseService<IUserExercise> {
    
    constructor() {
        super('/userexercise');
    }
}