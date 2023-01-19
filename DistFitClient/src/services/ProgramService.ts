import { IProgram } from '../domain/IProgram';
import { BaseService } from './BaseService';

export class ProgramService extends BaseService<IProgram> {

    constructor() {
        super('/program');
    }
}