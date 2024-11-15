import { useEffect, useState } from 'react';

interface CountDownTimeForRoomProps {
  timeRound?: string  | undefined;
  setIsTimeOut: (isTimeOut: boolean) => void;
}

const CountDownTimeForRoom = ({ timeRound, setIsTimeOut }: CountDownTimeForRoomProps) => {
  const [timeLeft, setTimeLeft] = useState<number | null>(null);

  useEffect(() => {
    if (timeRound && typeof timeRound === 'string' && timeRound.includes(":")) {
      const [hours, minutes] = timeRound.split(":").map(Number);
      const totalSeconds = (hours * 3600) + (minutes * 60);
      setTimeLeft(totalSeconds);

      // Countdown interval
      const intervalId = setInterval(() => {
        setTimeLeft((prevTime) => {
          if (prevTime && prevTime > 0) {
            return prevTime - 1;
          } else {
            clearInterval(intervalId);
            setIsTimeOut(true); // Set timeout to true when countdown ends
            return 0;
          }
        });
      }, 1000);

      // Cleanup on component unmount
      return () => clearInterval(intervalId);
    } else {
      setTimeLeft(0);
    }
  }, [timeRound, setIsTimeOut]);

  // Convert seconds left to hh:mm:ss format
  const formatTime = (seconds: number) => {
    const hrs = Math.floor(seconds / 3600);
    const mins = Math.floor((seconds % 3600) / 60);
    const secs = seconds % 60;
    return `${String(hrs).padStart(2, '0')}:${String(mins).padStart(2, '0')}:${String(secs).padStart(2, '0')}`;
  };

  return (
    <div className="flex items-center justify-center bg-gray-100 w-full">
      <div className="bg-white p-4 rounded-lg shadow-lg text-center w-full">
        <h1 className="text-xs font-bold mb-2">Thời gian đấu giá</h1>
        <div className="text-xl font-mono text-blue-600">
          {timeLeft !== null ? formatTime(timeLeft) : '00:00:00'}
        </div>
      </div>
    </div>
  );
};

export default CountDownTimeForRoom;
