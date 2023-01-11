import { AxiosResponse } from 'axios';
import { AxiosError } from 'axios';
import IServiceResult from "../domain/IServiceResult";
import httpClient from "../http-client";
import IdentityState from "../state/IdentityState";

export class BaseService<TEntity> {

    constructor(protected path: string) {
    }

    async getAllAsync(identityState: IdentityState): Promise<IServiceResult<TEntity[]> | AxiosResponse> {
        try {
            let res = await httpClient.get(this.path, {
                headers: {
                    "Authorization": "bearer " + identityState.jwt
                }});
            return res;
        } catch (e) {
            let response = (e as AxiosError).response;
            if (response?.status === 401 && identityState.jwt) {

            }
            return response!;
        }
    }

    async addAsync(entity: TEntity, identityState: IdentityState): Promise<IServiceResult<TEntity>> {
        let res = await httpClient.post(this.path, entity, {
            headers: {
                "Authorization": "bearer " + identityState.jwt
        }});
        return res;
    }
}