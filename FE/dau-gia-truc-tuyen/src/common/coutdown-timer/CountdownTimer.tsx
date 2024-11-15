import React, { useEffect, useState } from 'react';

const CountdownTimer = (props: any) => {
  const { targetDate } = props;
  console.log(targetDate,"targetDate")
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
          <p className="flex flex-col">
            {days}
            <span>ngày</span>
          </p>
        )}
        {hours !== undefined && (
          <p className="flex flex-col">
            {hours}
            <span>giờ</span>
          </p>
        )}
        {minutes !== undefined && (
          <p className="flex flex-col">
            {minutes}
            <span>phút</span>
          </p>
        )}
        {seconds !== undefined && (
          <p className="flex flex-col">
            {seconds}
            <span>giây</span>
          </p>
        )}
      </div>
    );
  };

  return (
    <div
      className="countdown-timer text-center"
      style={{ backgroundColor: '#27AE60', padding: '8px 24px' }}
    >
      <h2 className="text-xs text-white font-bold mb-1">Thời gian đấu già còn</h2>
      {Object.keys(timeLeft).length ? renderTime() : <span>Time's up!</span>}
    </div>
  );
};

export default CountdownTimer;
