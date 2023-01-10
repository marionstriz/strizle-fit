import { IExerciseType } from './domain/IExerciseType';

export class MyApp {

  public message: string = '';
  public exerciseTypes: IExerciseType[] = [];

  newTypeName: string = '';

  attached() {
    this.updateTime();
    this.exerciseTypes.push({name: "Squat"})
  }

  updateTime() {
    let welcomeMessage = 'Welcome! Current UTC time is ';

    let date = new Date();
    this.message = welcomeMessage + date.getUTCHours() + ':' + date.getUTCMinutes();

    setTimeout(() => {
      this.updateTime();
    }, 10000)
  }

  addNewItemOnClick() {
    this.addNewItem();
  }

  addNewItemKeyDown(event: KeyboardEvent) {
    if (event.key === 'Enter') {
      this.addNewItem();
    }
    return true;
  }

  addNewItem() {
    this.exerciseTypes.push({name: this.newTypeName});

    this.newTypeName = '';
  }

  removeItem(index: number, event: PointerEvent) {
    console.log(index);

    this.exerciseTypes.splice(index, 1);
  }
}
