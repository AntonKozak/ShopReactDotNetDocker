import { Auction } from '@/app/types';
import Link from 'next/link';
import CarImage from '../components/CarImage';
import CountdownTimer from '../components/CountdownTimer';
import CurrentBid from './CurrentBid';

type Props = {
  auction: Auction;
};

export default function AuctionCard({ auction }: Props) {
  return (
    <Link href={`/auction/details/${auction.id}`}>
      <div className='relative w-full bg-gray-200 aspect-[16/10] rounded-lg overflow-hidden'>
        <CarImage
          key={auction.id}
          imageUrl={auction.imageUrl}
          model={auction.model}
        />
        <div className='absolute bottom-2 right-2 font-bold text-white bg-gray-800 bg-opacity-75 px-2 py-1 rounded'>
          <CountdownTimer auctionEnd={auction.auctionEnd} />
        </div>
        <div className='absolute top-2 right-2'>
          <CurrentBid
            reservePrice={auction.reservePrice}
            amount={auction.currentHighBid}
          />
        </div>
      </div>
      <div className='flex justify-between items-center mt-4'>
        <h3 className='text-gray-700'>
          {auction.make} {auction.model}
        </h3>
        <p className='font-semibold text-sm'>{auction.year}</p>
      </div>
    </Link>
  );
}
