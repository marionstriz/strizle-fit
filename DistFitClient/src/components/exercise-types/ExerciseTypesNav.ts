import { UserExerciseService } from './../../services/UserExerciseService';
import { IExerciseType } from './../../domain/IExerciseType';
import { IdentityService } from './../../services/IdentityService';
import { EventAggregator, IDisposable } from 'aurelia';

export class ExerciseTypesNav {

    private subscriptions: IDisposable[] = [];

    exerciseTypes: IExerciseType[] = [];
    activeExerciseType: IExerciseType | null = null;

    constructor(private identityService: IdentityService,
            private eventAggregator: EventAggregator, 
            private repo: UserExerciseService) {

        this.subscriptions.push(this.eventAggregator.subscribe("newActiveExerciseType", 
            (type: IExerciseType | null) => this.activeExerciseType = type));

        repo.getAllAsync(identityService).then((res) => {
            if (res.error != null) {
                identityService.logout();
            } else {
                this.exerciseTypes = res.data!.map(ue => ue.exerciseType!);
            }
        });
    }

    detached() {
        // remove all the listeners
        this.subscriptions.forEach(subscription => {
            subscription.dispose();
        });
        this.subscriptions = [];
    }

    makeActive(index: number) {
        let newExerciseType = this.exerciseTypes[index];
        if (this.activeExerciseType !== newExerciseType) {
            this.activeExerciseType = this.exerciseTypes[index];

            this.eventAggregator.publish("newActiveExerciseType", this.activeExerciseType);
        }
    }
}