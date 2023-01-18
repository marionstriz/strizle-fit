export default class Helpers {

    public static toLocaleDateString(dateString: string): string {
        return new Date(dateString).toLocaleString();
    }

    public static mergeDateAndTime(date: Date, time: string): Date {

        let timeSplit = time.split(':');
        let hours = Number.parseInt(timeSplit[0]);
        let minutes = Number.parseInt(timeSplit[1]);

        let newDate = new Date(date);
        newDate.setHours(hours);
        newDate.setMinutes(minutes);

        return newDate;
    }
}