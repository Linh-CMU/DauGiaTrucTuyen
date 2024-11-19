import React, { useEffect, useState } from 'react';

const CountdownTimer = (props: any) => {
  const { targetDate } = props;
  const calculateTimeLeft = (): { [key: string]: number } => {
    const difference = targetDate.getTime() - new Date().getTime();
    let timeLeft: { [key: string]: number } = {};

    if (difference > 0) {
      timeLeft = {
        days: Math.floor(difference / (1000 * 60 * 60 * 24)),
        hours: Math.floor((difference / (1000 * 60 * 60)) % 24),
        minutes: Math.floor((difference / 1000 / 60) % 60),
        seconds: Math.floor((difference / 1000) % 60),
      };
    }

    return timeLeft;
  };

  const [timeLeft, setTimeLeft] = useState(calculateTimeLeft());

  useEffect(() => {
    const timer = setInterval(() => {
      setTimeLeft(calculateTimeLeft());
    }, 1000);

    return () => clearInterval(timer);
  }, [targetDate]);

  const renderTime = () => {
    const { days, hours, minutes, seconds } = timeLeft;

    return (
      <div className="flex space-x-4 text-xs justify-center text-white">
        {days !== undefined && (
          <p className="flex flex-col text-black">
            {days}
            <span>ngày</span>
          </p>
        )}
        <span className="text-black font-bold">:</span>
        {hours !== undefined && (
          <p className="flex flex-col text-black">
            {hours}
            <span>giờ</span>
          </p>
        )}
        <span className="text-black font-bold">:</span>
        {minutes !== undefined && (
          <p className="flex flex-col text-black">
            {minutes}
            <span>phút</span>
          </p>
        )}
        <span className="text-black font-bold">:</span>
        {seconds !== undefined && (
          <p className="flex flex-col text-black">
            {seconds}
            <span>giây</span>
          </p>
        )}
      </div>
    );
  };

  return (
    <div
      style={{  padding: '8px 24px'}}
    >
      {Object.keys(timeLeft).length ? renderTime() : <span>Time's up!</span>}
    </div>
  );
};

export default CountdownTimer;
