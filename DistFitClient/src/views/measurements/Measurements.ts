import { IMeasurement } from './../../domain/IMeasurement';
import { MeasurementTypeService } from './../../services/MeasurementTypeService';
import { MeasurementService } from '../../services/MeasurementService';
import { IdentityService } from '../../services/IdentityService';
import { EventAggregator, IDisposable, IRouter, Params } from 'aurelia';
import { IMeasurementType } from '../../domain/IMeasurementType';
import { IValueCalculationArray } from '../../domain/IValueCalculationArray';
import { Chart, Point } from 'chart.js/auto';
import Helpers from '../../helpers/Helpers';

export class Measurements {

    private subscriptions: IDisposable[] = [];
    successMsg?: string;
    message?: string;

    measurementType: IMeasurementType | null = null;
    measurements: IMeasurement[] | null = null;

    calcArray: IValueCalculationArray<IMeasurement>[] | null = null;

    constructor(private identityService: IdentityService,
        private measurementService: MeasurementService,
        private measurementTypeService: MeasurementTypeService,
        private eventAggregator: EventAggregator,
        @IRouter private router: IRouter) {
        
        this.subscriptions.push(this.eventAggregator.subscribe('newActiveMeasurementType', 
            (type: IMeasurementType | null) => this.newMeasurementTypeReceived(type)));
    }

    detached() {
        // remove all the listeners
        this.subscriptions.forEach(subscription => {
            subscription.dispose();
        });
        this.subscriptions = [];
    }

    success(msg: string) {
        this.successMsg = msg;
        setTimeout(() => {
            this.successMsg = undefined;
        }, 5000);
    }

    async loading(params: Params) {
        if (!params.id) {
            this.toRecentMeasurementsAsync();
            return;
        };
        let res = await this.measurementTypeService.getAsync(params.id, this.identityService);

        if (res.error) return;
        let type = res.data!

        this.eventAggregator.publish('newActiveMeasurementType', type);

        if (params.confirmed === 'scsa') {
            this.success('Measurement added');
        }
    }

    async newMeasurementTypeReceived(type: IMeasurementType | null) {
        this.measurementType = type;

        if (!type) {
            this.calcArray = null;
            return;
        }

        this.nullAll();
        this.message = 'Loading...';

        let res = await this.measurementService.getByMeasurementTypeIdAsync(type.id, this.identityService);

        if (res.error) {
            this.identityService.logout();
            return;
        }
        if (res.data!.length === 0) {
            this.message = `No ${this.measurementType!.name.toLowerCase()} measurements for this user yet!`;
        }
        else if (type.name === 'Weight') {
            this.setWeightCalcs(res.data!);
        } else this.setLengthCalcs(res.data!);

        if (this.calcArray) {
            let measurements = this.calcArray[0].data;
            this.buildChart(measurements);
        }
        let collapsable = document.querySelector('#typesNav');

        if (collapsable) {
            collapsable.classList.remove('show');
        }
    }

    setMeasurementsToCalcArray(index: number) {

        this.measurements = this.calcArray![index].data;

        this.buildChart(this.measurements);
    }

    private setWeightCalcs(data: IMeasurement[]) {

        this.calcArray = [];

        let kgArray: IMeasurement[] = [];
        let lbsArray: IMeasurement[] = [];

        data.forEach(msrm => {

            if (msrm.valueUnit!.symbol === 'kg') {
                kgArray.push(msrm);
                let lbsMsrm: IMeasurement = {...msrm, value: Math.round(msrm.value * 22.046) / 10};
                lbsMsrm.valueUnit = {name: 'Pounds', symbol: 'lbs'};
                lbsArray.push(lbsMsrm);
            } else if (msrm.valueUnit!.symbol === 'lbs') {
                lbsArray.push(msrm);
                let kgMsrm: IMeasurement = {...msrm, value: Math.round(msrm.value / 0.22046) / 10};
                kgMsrm.valueUnit = {name: 'Kilograms', symbol: 'kg'};
                kgArray.push(kgMsrm);
            }
        });

        this.calcArray.push({valueType: 'kg', data: kgArray});
        this.calcArray.push({valueType: 'lbs', data: lbsArray});
    }

    private setLengthCalcs(data: IMeasurement[]) {
        this.calcArray = [];

        let cmArray: IMeasurement[] = [];
        let inchArray: IMeasurement[] = [];

        data.forEach(msrm => {

            if (msrm.valueUnit!.symbol === 'cm') {
                cmArray.push(msrm);
                let inchMsrm: IMeasurement = {...msrm, value: Math.round(msrm.value * 3.937) / 10};
                inchMsrm.valueUnit = {name: 'Inch', symbol: 'in'};
                inchArray.push(inchMsrm);
            } else if (msrm.valueUnit!.symbol === 'in') {
                inchArray.push(msrm);
                let cmMsrm: IMeasurement = {...msrm, value: Math.round(msrm.value * 25.4) / 10};
                cmMsrm.valueUnit = {name: 'Centimeter', symbol: 'cm'};
                cmArray.push(cmMsrm);
            }
        });

        this.calcArray.push({valueType: 'cm', data: cmArray});
        this.calcArray.push({valueType: 'in', data: inchArray});
    }

    private buildChart(measurements: IMeasurement[]) {

        this.destroyChart();

        var ctxL = (document.getElementById("scatterChart")! as HTMLCanvasElement).getContext('2d');

        measurements?.sort((a, b) => new Date(a.measuredAt).valueOf() - new Date(b.measuredAt).valueOf());

        const data = measurements?.map(m => {
            return {
                x: new Date(m.measuredAt),
                y: m.value
        }});

        var unitSymbol = measurements?.at(0)?.valueUnit!.symbol;

        new Chart(
            ctxL!,
            {
                type: 'scatter',
                data: {
                    datasets: [{
                        data: data,
                        showLine: true
                    }]
                },
                options: {
                    animation: {
                        duration: 0
                    },
                    scales: {
                        x: {
                            ticks: {
                                callback: function(value, index, ticks) {
                                    return new Date(value).toLocaleDateString();
                                }
                            }
                        }
                    },
                    plugins: {
                        legend: {
                            display: false
                        },
                        tooltip: {
                            callbacks: {
                                label: function(context) {
                                    let index = context.dataIndex;
                                    let point = (context.dataset.data[index] as Point);
                                    let date = point.x;
                                    return date.toLocaleString() + ', ' + point.y + unitSymbol;
                                }
                            }
                        }
                    }
                }
            }
        );
        this.measurements = measurements;
    }

    private destroyChart() {
        Chart.getChart("scatterChart")?.destroy();
    }

    toLocaleDateString(dateString: string): string {
        return Helpers.toLocaleDateString(dateString);
    }

    async toRecentMeasurementsAsync() {
        this.destroyChart();
        this.message = 'Loading...';
        this.measurements = [];
        this.measurementService.getAllAsync(this.identityService).then(res => {
            if (res.error) {
                this.identityService.logout();
                return;
            }
            this.measurements = res.data!.sort((a, b) => new Date(b.measuredAt).valueOf() - new Date(a.measuredAt).valueOf());
            if (this.measurements.length > 0) {
                this.message = 'My Recent Measurements';
            } else this.message = 'You have no measurements yet.'
        });

        this.eventAggregator.publish("newActiveMeasurementType", null);
    }

    async toAddMeasurementAsync() {
        await this.router.load('/measurements/add');
    }

    private nullAll() {
        this.measurements = null;
        this.calcArray = null;
        this.destroyChart();
    }
}