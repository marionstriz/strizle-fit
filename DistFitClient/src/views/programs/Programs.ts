import { ProgramSavedService } from './../../services/ProgramSavedService';
import { IRouter } from "aurelia";
import { IProgram } from "../../domain/IProgram";
import { IdentityService } from "../../services/IdentityService";

export class Programs {

    load: boolean = false;
    programs: IProgram[] = [];

    constructor(private identityService: IdentityService,
        private programSavedService: ProgramSavedService,
        @IRouter private router: IRouter) {
        
            this.load = true;
            programSavedService.getAllAsync(this.identityService).then((res) => {

                if (res.error) {
                    identityService.logout();
                    return;
                }
                this.programs = res.data!.map(ps => ps.program!);

                this.load = false;
            });
    }
}