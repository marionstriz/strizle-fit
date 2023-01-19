import { IProgramSaved } from './../domain/IProgramSaved';
import { BaseService } from './BaseService';

export class ProgramSavedService extends BaseService<IProgramSaved> {

    constructor() {
        super('/programsaved');
    }
}