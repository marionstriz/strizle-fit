import { IExerciseType } from "./IExerciseType";

export interface IUserExercise {
    id?: string,
    exerciseTypeId: string,
    exerciseType?: IExerciseType
}