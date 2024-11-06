export const convertDate = (startTime: string, endDate: string) => {
    const [day, month, year] = endDate?.split('/') || '0'; 
    const formattedDate = `${year}-${month}-${day}`;
    const isoDateString = `${formattedDate}T${startTime}:00`;
    const targetDate = new Date(isoDateString);
    return targetDate;
}