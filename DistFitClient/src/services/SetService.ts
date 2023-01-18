import { BaseService } from './BaseService';
import { ISetEntry } from '../domain/ISetEntry';
import { IdentityService } from './IdentityService';
import IServiceResult from '../domain/IServiceResult';

export class SetService extends BaseService<ISetEntry> {

    constructor() {
        super('/setentry');
    }

    async getByPerformanceIdAsync(id: string, identityService: IdentityService)
            : Promise<IServiceResult<ISetEntry[]>> {

        return await this.getRequestToPathAsync(`${this.path}/performance/${id}`, identityService) as IServiceResult<ISetEntry[]>;
    }
}