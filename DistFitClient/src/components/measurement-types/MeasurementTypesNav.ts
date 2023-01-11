import { EventAggregator, IDisposable } from 'aurelia';
import { IMeasurementType } from '../../domain/IMeasurementType';
import { MeasurementTypeRepository } from '../../repositories/MeasurementTypeRepository';

export class MeasurementTypesNav {

    private subscriptions: IDisposable[] = [];

    measurementTypes: IMeasurementType[] = [];
    activeMeasurementType: IMeasurementType;

    constructor(private eventAggregator: EventAggregator, 
            private repo: MeasurementTypeRepository) {

        repo.getAllMeasurementTypesAsync().then((res) => {
            this.measurementTypes = res;
        });
        this.activeMeasurementType = this.measurementTypes[0];
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