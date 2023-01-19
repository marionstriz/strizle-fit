import { IdentityService } from './../services/IdentityService';
import { IRouter } from "aurelia";

export class Home {

    constructor(private identityService: IdentityService,
        @IRouter private router: IRouter) {
    }

    async loadExercisesAsync() {
        await this.router.load('/exercises/add');
    }

    async loadMeasurementsAsync() {
        await this.router.load('/measurements/add');
    }

    async loadLoginAsync() {
        await this.router.load('/login');
    }

    async loadRegisterAsync() {
        await this.router.load('/register');
    }

    async loadProgramsAsync() {
        await this.router.load('/programs')
    }

    unimplemented() {
        console.log("unimplemented");
    }
}