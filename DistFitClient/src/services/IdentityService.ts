import { AxiosError } from "axios";
import IJwtResponse from "../domain/IJwtResponse";
import IServiceResult from "../domain/IServiceResult";
import httpClient from "../http-client";
import IdentityState from "../state/IdentityState";

export class IdentityService {

    private readonly storageKey = 's09890hökladsu-aHoiJLIOGHugh0w-OKO**dgkifhgHiJKLOyhslk';
    identityState: IdentityState = new IdentityState();

    async loginAsync(email: string, password: string)
            : Promise<IServiceResult<IJwtResponse>> {

        let loginInfo = {email, password};

        let res = await this.postPayloadToPathAsync('/identity/account/login', loginInfo)

        if (res.error) return res;

        this.setIdentityDataToState(res.data!);
        
        return res;
    }

    async registerAsync(email: string, firstName: string, lastName: string, password: string)
            : Promise<IServiceResult<IJwtResponse>> {

        let registerInfo = {email, firstName, lastName, password};

        let res = await this.postPayloadToPathAsync('/identity/account/register', registerInfo)

        if (res.error) return res;

        this.setIdentityDataToState(res.data!);

        return res;
    }
    
    async refreshIdentityAsync(): Promise<IServiceResult<IJwtResponse>> {

        let refreshInfo = {jwt: this.identityState.jwt, refreshToken: this.identityState.refreshToken}

        let res = await this.postPayloadToPathAsync('/identity/account/refreshtoken', refreshInfo)

        if (res.error) return res;

        this.identityState.jwt = res.data!.token;
        this.identityState.refreshToken = res.data!.refreshToken;

        this.setIdentityStateToLocalStorage();
        return res;

    }

    logout(): void {
        this.identityState.user = null;

        window.localStorage.removeItem(this.storageKey);
    }

    checkLocalStorageForIdentity(): void {
        let storedIdentityStateJson = window.localStorage.getItem(this.storageKey);

        if (storedIdentityStateJson !== null) {
        let storedIdentityState = JSON.parse(storedIdentityStateJson!);

        if (storedIdentityState.user !== null) {
            this.identityState.user = storedIdentityState.user;
        }
        this.identityState.jwt = storedIdentityState.jwt;
        this.identityState.refreshToken = storedIdentityState.refreshToken;
        }
    }

    private setIdentityStateToLocalStorage() {
        window.localStorage.setItem(this.storageKey, JSON.stringify(this.identityState));
    }

    private async postPayloadToPathAsync(path: string, payload: unknown): Promise<IServiceResult<IJwtResponse>>  {
        let res;
        try {
            res = await httpClient.post(path, payload);
        } catch (e) {
            let response = {
                status: (e as AxiosError).response!.status,
                error: (e as AxiosError).response!.data
            }
            return response;
        }
        return res;
    }

    private setIdentityDataToState(identityData: IJwtResponse): void {
        let user = {
            id: identityData.id,
            firstName: identityData.firstName,
            lastName: identityData.lastName,
            email: identityData.email
        };

        this.identityState.user = user;
        this.identityState.jwt = identityData.token;
        this.identityState.refreshToken = identityData.refreshToken;

        this.setIdentityStateToLocalStorage();
    }
}