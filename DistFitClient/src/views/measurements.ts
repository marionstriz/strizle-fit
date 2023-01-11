import { EventAggregator, IDisposable } from 'aurelia';
import { IMeasurementType } from '../domain/IMeasurementType';
import IdentityState from '../state/IdentityState';

export class Measurements {

    private subscriptions: IDisposable[] = [];

    measurementType: IMeasurementType | undefined;

    constructor(private identityState: IdentityState,
        private eventAggregator: EventAggregator) {

        this.subscriptions.push(this.eventAggregator.subscribe("newActiveMeasurementType", 
            (type: IMeasurementType) => this.newMeasurementTypeReceived(type)));
        
        
    }

    newMeasurementTypeReceived(type: IMeasurementType) {
        this.measurementType = type;
    }
}