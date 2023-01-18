import { BaseService } from './BaseService';
import { IMeasurement } from './../domain/IMeasurement';
import { IdentityService } from './IdentityService';
import IServiceResult from '../domain/IServiceResult';

export class MeasurementService extends BaseService<IMeasurement> {

    constructor() {
        super('/measurement');
    }

    async getByMeasurementTypeIdAsync(id: string, identityService: IdentityService)
            : Promise<IServiceResult<IMeasurement[]>> {

        return await this.getRequestToPathAsync(`${this.path}/type/${id}`, identityService) as IServiceResult<IMeasurement[]>;
    }
}