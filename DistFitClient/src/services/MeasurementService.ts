import { BaseService } from './BaseService';
import { IMeasurement } from './../domain/IMeasurement';
import httpClient from "../http-client";

export class MeasurementService extends BaseService<IMeasurement> {

    constructor() {
        super('/measurement');
    }

    async getByMeasurementTypeId(id: string, token: string): Promise<IMeasurement[]> {
        let res = await httpClient.get(`${this.path}/type/${id}`, {
            headers: {
                "Authorization": "bearer " + token
        }});
        return res.data;
    }
}