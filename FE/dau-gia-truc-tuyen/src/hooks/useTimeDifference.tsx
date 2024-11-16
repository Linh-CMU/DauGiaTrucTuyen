import { parseDateTime } from '@utils/helper';
import { useEffect, useState } from 'react';

const parseTimeRoundToSeconds = (timeRound: string = '00:00'): number => {
  const [hours, minutes] = timeRound.split(':').map(Number);
  return hours * 3600 + minutes * 60;
};

const secondsToTimeFormat = (seconds: number): string => {
  const hrs = Math.floor(seconds / 3600);
  const mins = Math.floor((seconds % 3600) / 60);
  return `${String(hrs).padStart(2, '0')}:${String(mins).padStart(2, '0')}`;
};

const useTimeDifference = (roomDate: string, timeRound: string) => {
  const [greaterTime, setGreaterTime] = useState<string | null>(null);
  const [isIntime, setIsIntime] = useState<boolean>(true);

  useEffect(() => {
    const currentDate = new Date();
    const roomDateTime = parseDateTime(roomDate);

    const differenceInSeconds = Math.floor((roomDateTime.getTime() - currentDate.getTime()) / 1000);
    const roundTimeInSeconds = parseTimeRoundToSeconds(timeRound);
    if (differenceInSeconds > roundTimeInSeconds || differenceInSeconds < 0) {
      setIsIntime(false);
    } else {
      setIsIntime(true);
      const timeToCountdown = Math.abs(differenceInSeconds);
      setGreaterTime(secondsToTimeFormat(timeToCountdown));
    }
  }, [roomDate, timeRound]);

  return { greaterTime, isIntime };
};
export default useTimeDifference;
