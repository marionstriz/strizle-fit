import { IPerformance } from './IPerformance';
import { IUnit } from "./IUnit";

export interface ISetEntry {
    id?: string,
    quantity: number,
    quantityUnitId: string,
    quantityUnit?: IUnit,
    weight?: number,
    weightUnitId?: string,
    weightUnit?: IUnit,
    performanceId: string,
    performance?: IPerformance
}