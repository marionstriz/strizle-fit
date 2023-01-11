import { IRouter, EventAggregator } from 'aurelia';
import IdentityState from '../../state/IdentityState';
import { IdentityService } from './../../services/IdentityService';
export class Login {

    errorMsg: string | null = null;
    email: string = '';
    password: string = '';

    constructor(@IRouter private router: IRouter,
        private eventAggregator: EventAggregator,
        private identityService: IdentityService, 
        private identityState: IdentityState) {
    }

    async loginClickedAsync() {
        var res = await this.identityService.loginAsync(this.email, this.password);

        if (res.status == 400 || res.status == 404) {
            this.errorMsg = "Invalid email/password";
            setTimeout(() => {
                this.errorMsg = null;
            }, 5000)
        } else {
            let user = {
                id: res.data!.id,
                firstName: res.data!.firstName,
                lastName: res.data!.lastName,
                email: res.data!.email
            };

            this.identityState.user = user;
            this.identityState.jwt = res.data!.token;
            this.identityState.refreshToken = res.data!.refreshToken;

            await this.router.load('');
        }
    }

}