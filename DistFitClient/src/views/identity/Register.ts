import { IRouter } from 'aurelia';
import { IdentityService } from '../../services/IdentityService';

export class Register {

    error: string | null = null;

    email: string = '';
    emailError: string | null = null;

    firstName: string = '';
    firstNameError: string | null = null;

    lastName: string = '';
    lastNameError: string | null = null;

    password: string = '';
    passwordError: string | null = null;

    passwordConf: string = '';

    constructor(@IRouter private router: IRouter,
        private identityService: IdentityService) {
    }

    async registerClickedAsync() {

        this.setErrors();

        if (this.hasErrors()) {
            return;
        }
        var res = await this.identityService.registerAsync(this.email, this.firstName, this.lastName, this.password);

        if (res.error) {
            this.error = 'An error occurred, please try again or contact the administrator';

        } else {
            await this.router.load('');
        }
    }

    private setErrors() {
        this.error = null;
        this.emailError = null;
        this.firstNameError = null;
        this.lastNameError = null;
        this.passwordError = null;

        if (this.email.length < 5 || this.email.length > 254) {
            this.emailError = 'Must be 5-254 characters in length';
        }

        if (!this.firstName.trim() || this.firstName.length > 254) {
            this.firstNameError = 'Must be 1-254 characters in length';
        }

        if (!this.lastName.trim() || this.lastName.length > 254) {
            this.lastNameError = 'Must be 1-254 characters in length';
        }

        if (this.password.length < 6 || !/\d/.test(this.password) || 
            !/[A-Z]/.test(this.password) || !/[a-z]/.test(this.password) || /^[A-Za-z0-9]*$/.test(this.password)) {

            this.passwordError = `Must be more than 6 characters in length and contain a number, upper and lower case
            letter and non-alphanumeric character`;
        } else if (this.password !== this.passwordConf) {
            this.passwordError = `Passwords do not match, please try again`;
        }
    }

    private hasErrors(): boolean {
        return this.error !== null || this.emailError !== null || 
        this.firstNameError !== null || this.lastNameError !== null || this.passwordError !== null;
    }

    changePasswordVisibility() {
        var input = document.getElementById('password') as HTMLInputElement;

        if (input.type === 'password') {
            input.type = 'text';
        } else {
            input.type = 'password';
        }
    }
}