import { IRouter } from "aurelia";
import IdentityState from "../state/IdentityState";

export class Home {

    constructor(private identityState: IdentityState,
        @IRouter private router: IRouter) {
    }

    async loadMeasurementsAsync() {
        await this.router.load('/measurements');
    }

    async loadLoginAsync() {
        await this.router.load('/login');
    }

    unimplemented() {
        console.log("unimplemented");
    }
}