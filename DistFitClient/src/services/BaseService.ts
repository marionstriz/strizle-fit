import { AxiosError } from 'axios';
import IServiceResult from "../domain/IServiceResult";
import httpClient from "../http-client";
import { IdentityService } from './IdentityService';

export class BaseService<TEntity> {

    constructor(protected path: string) {
    }

    async getAllAsync(identityService: IdentityService)
        : Promise<IServiceResult<TEntity[]>> {
        
        return await this.getRequestToPathAsync(this.path, identityService) as IServiceResult<TEntity[]>;
    }

    async getAsync(id: string, identityService: IdentityService) : Promise<IServiceResult<TEntity>> {

        return await this.getRequestToPathAsync(this.path + '/' + id, identityService) as IServiceResult<TEntity>;
    }

    async addAsync(entity: TEntity, identityService: IdentityService): Promise<IServiceResult<TEntity>> {
        return await this.postRequestToPathAsync(entity, this.path, identityService);
    }

    async deleteAsync(id: string, identityService: IdentityService) {
        return await this.deleteRequestToPathAsync(this.path + '/' + id, identityService);
    }

    protected async postRequestToPathAsync(entity: TEntity, path: string, identityService: IdentityService)
        : Promise<IServiceResult<TEntity>> {

        return await this.requestToPathAsync(path, identityService, httpClient.post, this.postRequestToPathAsync, entity) as IServiceResult<TEntity>;
    }

    protected async getRequestToPathAsync(path: string, identityService: IdentityService)
            : Promise<IServiceResult<TEntity[] | TEntity>> {

        return await this.requestToPathAsync(path, identityService, httpClient.get, this.getRequestToPathAsync);
    }

    protected async deleteRequestToPathAsync(path: string, identityService: IdentityService)
            : Promise<IServiceResult<any>> {

        return await this.requestToPathAsync(path, identityService, httpClient.delete, this.deleteRequestToPathAsync);
    }

    protected async requestToPathAsync(path: string, identityService: IdentityService, clientMethod: Function, recursionMethod: Function, entity?: TEntity, retry: boolean = false)
            : Promise<IServiceResult<TEntity[] | TEntity>> {

        let res;
        try {
            if (entity) {
                res = await clientMethod(path, entity, {
                    headers: {
                        "Authorization": "bearer " + identityService.identityState.jwt
                    }});
            } else {
                res = await clientMethod(path, {
                    headers: {
                        "Authorization": "bearer " + identityService.identityState.jwt
                    }});
            }
        } catch (e) {
            let response = {
                status: (e as AxiosError).response!.status,
                error: (e as AxiosError).response!.data
            }
            if (response?.status === 401 && !retry && 
                identityService.identityState.jwt && identityService.identityState.refreshToken) {

                let refreshRes = await identityService.refreshIdentityAsync();
                if (refreshRes.error) {
                    return response;
                }
                if (entity) {
                    return await recursionMethod(entity, path, identityService, true);
                } else {
                    return await recursionMethod(path, identityService, true)
                };
            }
            return response;
        }
        return res;
    }
}