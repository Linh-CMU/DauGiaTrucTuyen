export const convertDate = (startTime: string, endDate: string) => {
    const [day, month, year] = endDate?.split('/') || '0'; 
    const formattedDate = `${year}-${month}-${day}`;
    const isoDateString = `${formattedDate}T${startTime}:00`;
    const targetDate = new Date(isoDateString);
    return targetDate;
}

export const parseDateTime = (dateTime: string): Date => {
  const [date, time] = dateTime.split(" ");
  const [day, month, year] = date.split("/").map(Number);
  const [hours, minutes] = time.split(":").map(Number);
  return new Date(year, month - 1, day, hours, minutes);
};
