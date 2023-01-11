import { AxiosError, AxiosResponse } from "axios";
import IJwtResponse from "../domain/IJwtResponse";
import IServiceResult from "../domain/IServiceResult";
import httpClient from "../http-client";

export class IdentityService {

    async loginAsync(email: string, password: string)
            :Promise<IServiceResult<IJwtResponse | null>> {

        let res;
        let loginInfo = {email, password};
        try {
            res = await httpClient.post('/identity/account/login', loginInfo);
        } catch (e) {
            let response = {
                status: (e as AxiosError).response!.status,
                data: null
            }
            return response;
        }
        return {
            status: res.status,
            data: res.data
        };
    }
}