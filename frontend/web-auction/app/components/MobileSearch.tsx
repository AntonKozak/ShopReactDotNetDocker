'use client';

import { useState } from 'react';

export default function MobileSearch() {
  const [isOpen, setIsOpen] = useState(false);

  return (
    <div className='md:hidden'>
      {/* Search Button */}
      <button
        onClick={() => setIsOpen(true)}
        className='p-2 rounded-lg hover:bg-gray-700 transition-colors'
      >
        <svg
          className='w-5 h-5'
          fill='none'
          stroke='currentColor'
          viewBox='0 0 24 24'
        >
          <path
            strokeLinecap='round'
            strokeLinejoin='round'
            strokeWidth={2}
            d='M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z'
          />
        </svg>
      </button>

      {/* Mobile Search Overlay */}
      {isOpen && (
        <div className='fixed inset-0 z-50 bg-black bg-opacity-50'>
          <div className='bg-gray-800 p-4 m-4 rounded-lg'>
            <div className='flex items-center gap-3 mb-4'>
              <button
                onClick={() => setIsOpen(false)}
                className='p-2 rounded-lg hover:bg-gray-700'
              >
                <svg
                  className='w-5 h-5'
                  fill='none'
                  stroke='currentColor'
                  viewBox='0 0 24 24'
                >
                  <path
                    strokeLinecap='round'
                    strokeLinejoin='round'
                    strokeWidth={2}
                    d='M6 18L18 6M6 6l12 12'
                  />
                </svg>
              </button>
              <input
                type='text'
                placeholder='Search auctions...'
                className='flex-1 px-4 py-2 rounded-lg bg-gray-700 text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-yellow-500'
                autoFocus
              />
            </div>
            <div className='text-sm text-gray-400'>
              Start typing to search for cars, brands, or models...
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
