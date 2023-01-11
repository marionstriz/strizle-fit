import { IRouter } from "aurelia";
import IdentityState from "../state/IdentityState";

export class PageHeader {

    constructor(private identityState: IdentityState,
        @IRouter private router: IRouter) {
    }

    async logoutAsync() {
        this.identityState.user = null;
        this.identityState.user = null;
        this.identityState.user = null;

        await this.router.load('/home');
    }
}