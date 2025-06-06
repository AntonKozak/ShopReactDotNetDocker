'use client';

import { useEffect, useState } from 'react';
import { FaSearch } from 'react-icons/fa';
import { useParamsStore } from '../../hooks/useParamsStore';

export default function Search() {
  const setParams = useParamsStore((state) => state.setParams);
  const searchTermToUpdate = useParamsStore((state) => state.searchTerm);
  const [searchTerm, setSearchTerm] = useState('');

  useEffect(() => {
    if (searchTermToUpdate == '') setSearchTerm('');
  }, [searchTermToUpdate]);

  function handleChanges(e: React.ChangeEvent<HTMLInputElement>) {
    setSearchTerm(e.target.value);
  }

  function handleSearch() {
    setParams({ searchTerm });
  }

  return (
    <div className='flex-1 max-w-md mx-4 hidden md:block'>
      <div className='relative'>
        <input
          onKeyDown={(e) => {
            if (e.key === 'Enter') {
              e.preventDefault();
              handleSearch();
            }
          }}
          type='text'
          onChange={handleChanges}
          value={searchTerm}
          placeholder='Search auctions...'
          className='w-full pl-4 pr-12 py-2 rounded-xl bg-gray-700 text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-yellow-500'
        />
        <button
          type='submit'
          className='absolute right-2 top-1/2 -translate-y-1/2 p-1 rounded-full hover:bg-gray-600 transition-colors'
        >
          <FaSearch
            size={22}
            className='text-gray-400 hover:text-yellow-500 transition-colors'
            onClick={handleSearch}
          />
        </button>
      </div>
    </div>
  );
}
