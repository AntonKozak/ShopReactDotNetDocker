import { Auction, PagedResult } from '@/app/types';
import AuctionCard from './AuctionCard';

async function getData(): Promise<PagedResult<Auction>> {
  const res = await fetch('http://localhost:6001/search');

  if (!res.ok) {
    throw new Error('Failed to fetch data');
  }

  return res.json();
}

export default async function Listning() {
  const data = await getData();

  return (
    <div className='grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4 p-4'>
      {data &&
        data.results.map((auction: Auction) => (
          <AuctionCard key={auction.id} auction={auction} />
        ))}
    </div>
  );
}
