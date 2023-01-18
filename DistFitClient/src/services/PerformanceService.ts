import { BaseService } from './BaseService';
import { IPerformance } from '../domain/IPerformance';
import { IdentityService } from './IdentityService';
import IServiceResult from '../domain/IServiceResult';

export class PerformanceService extends BaseService<IPerformance> {

    constructor() {
        super('/performance');
    }

    async getByExerciseTypeIdAsync(id: string, identityService: IdentityService)
            : Promise<IServiceResult<IPerformance[]>> {

        return await this.getRequestToPathAsync(`${this.path}/type/${id}`, identityService) as IServiceResult<IPerformance[]>;
    }
}