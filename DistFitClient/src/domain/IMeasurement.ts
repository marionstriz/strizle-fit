import { IMeasurementType } from './IMeasurementType';
import { IUnit } from './IUnit';

export interface IMeasurement {
    value: number;
    valueUnitId: string;
    valueUnit?: IUnit;
    measurementTypeId: string;
    measurementType?: IMeasurementType;
    measuredAt: Date;
}