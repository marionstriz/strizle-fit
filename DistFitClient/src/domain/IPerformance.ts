import { IUserExercise } from "./IUserExercise";

export interface IPerformance {
    id?: string,
    performedAt: Date,
    userExerciseId: string,
    userExercise?: IUserExercise
}   