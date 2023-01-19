import { ISetEntry } from './../../domain/ISetEntry';
import { SetService } from './../../services/SetService';
import { IPerformance } from './../../domain/IPerformance';
import { IExerciseType } from './../../domain/IExerciseType';
import { PerformanceService } from './../../services/PerformanceService';
import { EventAggregator, IDisposable, IRouter } from "aurelia";
import { IdentityService } from "../../services/IdentityService";
import Helpers from '../../helpers/Helpers';
import { IValueCalculationArray } from '../../domain/IValueCalculationArray';
import Chart, { Point } from 'chart.js/auto';

export class Exercises {

    private subscriptions: IDisposable[] = [];

    message?: string;

    exerciseType: IExerciseType | null = null;

    maxSetEntries?: ISetEntry[];
    calcArray: IValueCalculationArray<ISetEntry>[] | null = null;
    
    performances: IPerformance[] = []

    constructor(private identityService: IdentityService,
        private performanceService: PerformanceService,
        private setService: SetService,
        private eventAggregator: EventAggregator,
        @IRouter private router: IRouter) {

        this.setAllPerformances();

        this.subscriptions.push(this.eventAggregator.subscribe('newActiveExerciseType', 
            (type: IExerciseType | null) => this.newExerciseTypeReceived(type)));
    }

    detached() {
        // remove all the listeners
        this.subscriptions.forEach(subscription => {
            subscription.dispose();
        });
        this.subscriptions = [];
    }

    async newExerciseTypeReceived(type: IExerciseType | null) {
        this.exerciseType = type;

        if (!type) {
            this.calcArray = null;
            return;
        }
        this.nullAll();
        this.message = 'Loading...';

        let res = await this.performanceService.getByExerciseTypeIdAsync(type.id, this.identityService);

        if (res.error) {
            this.identityService.logout();
            return;
        }
        if (res.data!.length === 0) {
            this.message = `No ${this.exerciseType!.name.toLowerCase()} set entries for this user yet!`;
        }
        else if (type.name === 'Squat' || type.name === 'Deadlift') {
            await this.setWeightCalcsAsync(res.data!);
        } else this.nullAll();

        if (this.calcArray) {
            let maxSetEntries = this.calcArray[0].data;
            if (maxSetEntries.length === 0) {
                this.calcArray = null;
                this.message = `No ${this.exerciseType!.name.toLowerCase()} set entries for this user yet!`;
            } else this.buildChart(maxSetEntries);
        }
        let collapsable = document.querySelector('#typesNav');

        if (collapsable) {
            collapsable.classList.remove('show');
        }
    }

    loadSetPage(perfId: string, index: number) {
        let btn = document.querySelector('.entry-btn-' + index);
        btn!.innerHTML = 'Loading...';

        this.router.load("/sets/" + perfId);
    }

    toLocaleDateString(dateString: string): string {
        return Helpers.toLocaleDateString(dateString);
    }

    async toRecentExercisesAsync() {
        this.setAllPerformances();

        this.eventAggregator.publish("newActiveExerciseType", null);
    }

    async toAddExerciseAsync() {
        await this.router.load('/exercises/add');
    }

    private async setWeightCalcsAsync(data: IPerformance[]) {

        this.calcArray = [];

        let kgArray: ISetEntry[] = [];
        let lbsArray: ISetEntry[] = [];

        for (let perf of data) {
            let res = await this.setService.getByPerformanceIdAsync(perf.id!, this.identityService);

            if (res.error || res.data!.length === 0) continue;

            let max = res.data!.sort((a, b) => b.weight! - a.weight!).at(0)!;

            if (max.weightUnit!.symbol === 'kg') {
                kgArray.push(max);
                let lbs: ISetEntry = {...max, weight: Math.round(max.weight! * 22.046) / 10};
                lbs.weightUnit = {name: 'Pounds', symbol: 'lbs'};
                lbsArray.push(lbs);
            } else if (max.weightUnit!.symbol === 'lbs') {
                lbsArray.push(max);
                let kgs: ISetEntry = {...max, weight: Math.round(max.weight! / 0.22046) / 10};
                kgs.weightUnit = {name: 'Kilograms', symbol: 'kg'};
                kgArray.push(kgs);
            }
        }

        this.calcArray.push({valueType: 'kg', data: kgArray});
        this.calcArray.push({valueType: 'lbs', data: lbsArray});
    }

    private setAllPerformances() {
        this.destroyChart();
        this.message = 'Loading...';
        this.performances = [];
        this.performanceService.getAllAsync(this.identityService).then(res => {
            if (res.error) {
                this.identityService.logout();
                return;
            }
            this.performances = res.data!.sort((a, b) => new Date(b.performedAt).valueOf() - new Date(a.performedAt).valueOf());
    
            if (this.performances.length > 0) {
                this.message = 'My Recent Exercises';
            } else this.message = 'You have no exercise performances yet.'
        })
    }

    private nullAll() {
        this.performances = [];
        this.calcArray = null;
    }

    setSetEntriesToCalcArray(index: number) {

        this.maxSetEntries = this.calcArray![index].data;

        this.buildChart(this.maxSetEntries);
    }

    private buildChart(maxSetEntries: ISetEntry[], isWeighted: boolean = true) {

        this.destroyChart();

        var ctxL = (document.getElementById("scatterChart")! as HTMLCanvasElement).getContext('2d');

        maxSetEntries?.sort((a, b) => new Date(a.performance!.performedAt).valueOf() - new Date(b.performance!.performedAt).valueOf());

        const data = maxSetEntries?.map(m => {
            return {
                x: new Date(m.performance!.performedAt),
                y: isWeighted ? m.weight : m.quantity
        }});

        var weightUnitSymbol = maxSetEntries?.at(0)?.weightUnit?.symbol;

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
                                    return date.toLocaleString() + ', ' + point.y + weightUnitSymbol;
                                }
                            }
                        }
                    }
                }
            }
        );
        this.maxSetEntries = maxSetEntries;
    }
    
    private destroyChart() {
        Chart.getChart("scatterChart")?.destroy();
    }
}