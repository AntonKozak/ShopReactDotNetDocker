'use client';
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
  if (completed) {
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
          <span suppressHydrationWarning={true}>
            {days}:{zeroPad(hours)}:{zeroPad(minutes)}:{zeroPad(seconds)}
          </span>
        )}
      </div>
    );
  } else {
    return (
      <span>
        {days}:{hours}:{minutes}:{seconds}
      </span>
    );
  }
};

type Props = {
  auctionEnd: string;
};

export default function CountdownTimer({ auctionEnd }: Props) {
  return (
    <div>
      <Countdown date={auctionEnd} renderer={render} />
    </div>
  );
}
