import { BaseService } from './BaseService';
import { IMeasurementType } from '../domain/IMeasurementType';

export class MeasurementTypeService extends BaseService<IMeasurementType> {

    constructor() {
        super('/measurementtype');
    }
}