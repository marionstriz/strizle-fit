import { bindable } from 'aurelia';
import { IMeasurementType } from '../../domain/IMeasurementType';

export class MeasurementTypesNav {

    @bindable
    measurementTypes: IMeasurementType[] = [];
    @bindable
    activeMeasurementType: IMeasurementType | undefined;

    makeActive(index: number) {
        this.activeMeasurementType = this.measurementTypes[index];
    }
}