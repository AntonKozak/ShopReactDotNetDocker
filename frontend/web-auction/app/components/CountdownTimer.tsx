'use client';
import { useEffect, useState } from 'react';
import Countdown, { zeroPad } from 'react-countdown';

const render = ({
  days,
  hours,
  minutes,
  seconds,
  completed,
}: {
  days: number;
  hours: number;
  minutes: number;
  seconds: number;
  completed: boolean;
}) => {
  return (
    <div
      className={`
        border-2 border-white text-white py-1 px-2 rounded-lg flex justify-center
        ${
          completed
            ? 'bg-red-600'
            : days === 0 && hours < 10
            ? 'bg-amber-600'
            : 'bg-green-600'
        }
    `}
    >
      {completed ? (
        <span>Auction finished</span>
      ) : (
        <span>
          {days}:{zeroPad(hours)}:{zeroPad(minutes)}:{zeroPad(seconds)}
        </span>
      )}
    </div>
  );
};

type Props = {
  auctionEnd: string;
};

export default function CountdownTimer({ auctionEnd }: Props) {
  const [isMounted, setIsMounted] = useState(false);

  useEffect(() => {
    setIsMounted(true);
  }, []);

  // Show a loading state during hydration to prevent mismatch
  if (!isMounted) {
    // Create a temporary placeholder that calculates time remaining on client
    const now = new Date();
    const end = new Date(auctionEnd);
    const isCompleted = now >= end;

    return (
      <div
        className={`border-2 border-white text-white py-1 px-2 rounded-lg flex justify-center ${
          isCompleted ? 'bg-red-600' : 'bg-gray-600'
        }`}
      >
        <span>{isCompleted ? 'Auction finished' : 'Loading...'}</span>
      </div>
    );
  }

  return (
    <div>
      <Countdown date={auctionEnd} renderer={render} />
    </div>
  );
}
