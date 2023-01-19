import { IProgram } from "./IProgram";

export interface IProgramSaved {
    id?: string,
    isCreator: boolean,
    programId: string,
    program?: IProgram,
    startedAt?: Date,
    finishedAt?: Date
}