import { IRouter } from "aurelia";
import { IProgram } from "../../domain/IProgram";
import { IdentityService } from "../../services/IdentityService";

export class Programs {

    programs: IProgram[] = [];

    constructor(private identityService: IdentityService,
        @IRouter private router: IRouter) {
        
    }
}