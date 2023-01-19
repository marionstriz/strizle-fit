import { IUnit } from "./IUnit";

export interface IProgram {
    id?: string,
    name: string,
    description: string,
    duration: number,
    durationUnitId: number,
    durationUnit?: IUnit
}