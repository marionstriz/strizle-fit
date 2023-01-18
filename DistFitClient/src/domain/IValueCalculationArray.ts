import { IMeasurement } from './IMeasurement';

export interface IValueCalculationArray<TEntity> {
    valueType: string,
    data: TEntity[]
}