import { BaseService } from './BaseService';
import { IUnit } from '../domain/IUnit';

export class UnitService extends BaseService<IUnit> {

    constructor() {
        super('/unit');
    }
}