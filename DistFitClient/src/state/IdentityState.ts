import IAppUser from "../domain/IAppUser";

export default class IdentityState {

    jwt: string | null = null;
    refreshToken: string | null = null;
    user: IAppUser | null = null;
}