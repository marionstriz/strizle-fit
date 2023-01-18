import { IdentityService } from './../../services/IdentityService';
import { EventAggregator, IDisposable } from 'aurelia';
import { IMeasurementType } from '../../domain/IMeasurementType';
import { MeasurementTypeService } from '../../services/MeasurementTypeService';

export class MeasurementTypesNav {

    private subscriptions: IDisposable[] = [];

    measurementTypes: IMeasurementType[] = [];
    activeMeasurementType: IMeasurementType | null = null;

    constructor(private identityService: IdentityService,
            private eventAggregator: EventAggregator, 
            private repo: MeasurementTypeService) {

        this.subscriptions.push(this.eventAggregator.subscribe("newActiveMeasurementType", 
            (type: IMeasurementType | null) => this.activeMeasurementType = type));

        repo.getAllAsync(identityService).then((res) => {
            if (res.error != null) {
                identityService.logout();
            } else {
                this.measurementTypes = res.data!;
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
        let newMeasurementType = this.measurementTypes[index];
        if (this.activeMeasurementType !== newMeasurementType) {
            this.activeMeasurementType = this.measurementTypes[index];

            this.eventAggregator.publish("newActiveMeasurementType", this.activeMeasurementType);
        }
    }
}