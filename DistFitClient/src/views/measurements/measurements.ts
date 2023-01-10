import { IMeasurementType } from '../../domain/IMeasurementType';

export class Measurements {

    measurementTypes: IMeasurementType[] = []
    activeMeasurementType: IMeasurementType;

    constructor() {
        let weightType = {name: 'Weight'};
        let waistCircumType = {name: 'Waist circumference'};

        this.measurementTypes.push(weightType);
        this.measurementTypes.push(waistCircumType);

        this.activeMeasurementType = this.measurementTypes[0];
    }
}