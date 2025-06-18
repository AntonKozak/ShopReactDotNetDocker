'use client';

import { useParamsStore } from '@/app/hooks/useParamsStore';
import qs from 'query-string';
import { useEffect, useState } from 'react';
import { useShallow } from 'zustand/shallow';
import { getData } from '../actions/auctionsActions';
import EmptyFilter from '../components/navbar/EmptyFilter';
import Filters from '../components/navbar/Filters';
import PaginationPage from '../components/Pagination';
import { useAuctionStore } from '../hooks/useAuctionStore';
import AuctionCard from './AuctionCard';

export default function Listning() {
  const [loading, setLoading] = useState(true);

  const params = useParamsStore(
    useShallow((state) => ({
      pageNumber: state.pageNumber,
      pageSize: state.pageSize,
      pageCount: state.pageCount,
      searchTerm: state.searchTerm,
      orderBy: state.orderBy,
      filterBy: state.filterBy,
      seller: state.seller,
      winner: state.winner,
    }))
  );

  const data = useAuctionStore(
    useShallow((state) => ({
      auctions: state.auctions,
      totalCount: state.totalCount,
      pageCount: state.pageCount,
    }))
  );

  const setData = useAuctionStore((state) => state.setData);
  const setParams = useParamsStore((state) => state.setParams);
  const url = qs.stringifyUrl(
    { url: '', query: params },
    { skipEmptyString: true }
  );

  function setPageNumber(pageNumber: number) {
    setParams({ pageNumber });
  }

  useEffect(() => {
    getData(url).then((data) => {
      setData(data);
      setLoading(false);
    });
  }, [url, setData]);

  if (loading) return <h3>Loading...</h3>;

  return (
    <>
      <Filters />
      {data.totalCount === 0 ? (
        <EmptyFilter showReset />
      ) : (
        <>
          <div className='grid grid-cols-4 gap-6'>
            {data &&
              data.auctions.map((auction) => (
                <AuctionCard key={auction.id} auction={auction} />
              ))}
          </div>
          <div className='flex justify-center mt-4'>
            <PaginationPage
              onPageChange={setPageNumber}
              currentPage={params.pageNumber}
              pageCount={data.pageCount}
            />
          </div>
        </>
      )}
    </>
  );
}
