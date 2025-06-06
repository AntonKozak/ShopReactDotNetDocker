'use client';

import { useParamsStore } from '@/app/hooks/useParamsStore';
import { Auction, PagedResult } from '@/app/types';
import queryString from 'query-string';
import { useEffect, useState } from 'react';
import { useShallow } from 'zustand/shallow';
import { getData } from '../../actions/auctionsActions';
import EmptyFilter from '../EmptyFilter';
import Filters from '../navbar/Filters';
import PaginationPage from '../Pagination';
import AuctionCard from './AuctionCard';

export default function Listning() {
  const [data, setData] = useState<PagedResult<Auction>>();
  const params = useParamsStore(
    useShallow((state) => ({
      pageNumber: state.pageNumber,
      pageSize: state.pageSize,
      pageCount: state.pageCount,
      searchTerm: state.searchTerm,
      orderBy: state.orderBy,
      filterBy: state.filterBy,
    }))
  );
  const setParams = useParamsStore(useShallow((state) => state.setParams));
  const url = queryString.stringifyUrl(
    {
      url: '',
      query: params,
    },
    { skipEmptyString: true }
  );

  function setPageNumber(pageNumber: number) {
    setParams({ pageNumber });
  }

  useEffect(() => {
    getData(url).then((data) => {
      setData(data);
    });
  }, [url]);

  if (!data) return <h3>Loading ...</h3>;

  return (
    <>
      <Filters />
      {data.totalCount === 0 ? (
        <EmptyFilter showReset />
      ) : (
        <>
          <div className='grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4 p-4'>
            {data &&
              data.results.map((auction) => (
                <AuctionCard key={auction.id} auction={auction} />
              ))}
          </div>
          <div className='flex justify-center items-center mt-4'>
            <PaginationPage
              currentPage={params.pageNumber}
              pageCount={data.pageCount}
              onPageChange={setPageNumber}
            />
          </div>
        </>
      )}
    </>
  );
}
