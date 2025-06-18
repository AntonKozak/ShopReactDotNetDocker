'use client';

import { getBidsForAuction } from '@/app/actions/auctionsActions';
import Heading from '@/app/components/Heading';
import EmptyFilter from '@/app/components/navbar/EmptyFilter';
import { useBidStore } from '@/app/hooks/useBidStore';
import { numberWithCommas } from '@/app/lib/numberWithComma';
import { Auction, Bid } from '@/app/types';
import { User } from 'next-auth';
import { useEffect, useState } from 'react';
import toast from 'react-hot-toast';
import BidForm from './BidForm';
import BidItem from './BidItem';

type Props = {
  user: User | null;
  auction: Auction;
};

export default function BidList({ user, auction }: Props) {
  const [loading, setLoading] = useState(true);
  const [isMounted, setIsMounted] = useState(false);
  const bids = useBidStore((state) => state.bids);
  const setBids = useBidStore((state) => state.setBids);
  const open = useBidStore((state) => state.open);
  const setOpen = useBidStore((state) => state.setOpen);

  // Move this calculation inside useEffect to avoid hydration issues
  const [openForBids, setOpenForBids] = useState(false);

  const highBid = bids.reduce(
    (prev, current) =>
      prev > current.amount
        ? prev
        : current.bidStatus.includes('Accepted')
        ? current.amount
        : prev,
    0
  );

  useEffect(() => {
    setIsMounted(true);
    // Calculate openForBids on client side to avoid hydration mismatch
    setOpenForBids(new Date(auction.auctionEnd) > new Date());
  }, [auction.auctionEnd]);
  useEffect(() => {
    getBidsForAuction(auction.id)
      .then((res: Bid[] | { error: string }) => {
        console.log('Bids for auction:', res);
        if ('error' in res) {
          throw new Error(res.error);
        }
        setBids(res);
      })
      .catch((error) => {
        toast.error(error.message);
      })
      .finally(() => setLoading(false));
  }, [auction.id, setBids]);

  useEffect(() => {
    if (isMounted) {
      setOpen(openForBids);
    }
  }, [openForBids, setOpen, isMounted]);

  if (loading) return <span>Loading bids...</span>;

  return (
    <div className='rounded-lg shadow-md'>
      <div className='py-2 px-4 bg-white'>
        <div className='sticky top-0 bg-white p-2'>
          <Heading
            title={`Current high bid is $${numberWithCommas(highBid)}`}
          />
        </div>
      </div>

      <div className='overflow-auto h-[350px] flex flex-col-reverse px-2'>
        {bids.length === 0 ? (
          <EmptyFilter
            title='No bids for this item'
            subtitle='Please feel free to make a bid'
          />
        ) : (
          <>
            {bids.map((bid) => (
              <BidItem key={bid.id} bid={bid} />
            ))}
          </>
        )}
      </div>

      <div className='px-2 pb-2 text-gray-500'>
        {!open ? (
          <div className='flex items-center justify-center p-2 text-lg font-semibold'>
            This auction has finished
          </div>
        ) : !user ? (
          <div className='flex items-center justify-center p-2 text-lg font-semibold'>
            Please login to place a bid
          </div>
        ) : user && user.username === auction.seller ? (
          <div className='flex items-center justify-center p-2 text-lg font-semibold'>
            You cannot bid on your own auction
          </div>
        ) : (
          <BidForm auctionId={auction.id} highBid={highBid} />
        )}
      </div>
    </div>
  );
}
