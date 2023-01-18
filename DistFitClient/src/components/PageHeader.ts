import { IRouter } from "aurelia";
import { IdentityService } from "../services/IdentityService";

export class PageHeader {

    constructor(private identityService: IdentityService,
        @IRouter private router: IRouter) {
    }

    async logoutAsync() {
        this.identityService.logout();
        await this.router.load('/home');
    }
}