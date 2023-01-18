import { IRouter } from 'aurelia';
import { IdentityService } from '../../services/IdentityService';
export class Login {

    errorMsg: string | null = null;
    email: string = '';
    password: string = '';

    constructor(@IRouter private router: IRouter,
        private identityService: IdentityService) {
    }

    async loginClickedAsync() {
        var res = await this.identityService.loginAsync(this.email, this.password);

        if (res.status == 400 || res.status == 401 || res.status == 404) {
            this.errorMsg = "Invalid email/password";
            setTimeout(() => {
                this.errorMsg = null;
            }, 5000)

        } else {
            await this.router.load('');
        }
    }

    changePasswordVisibility() {
        var input = document.getElementById('typePassword') as HTMLInputElement;

        if (input.type === 'password') {
            input.type = 'text';
        } else {
            input.type = 'password';
        }
    }
}