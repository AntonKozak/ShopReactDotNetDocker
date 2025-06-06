'use client';

import { useState } from 'react';

interface FilterSortProps {
  onFilterChange?: (filters: any) => void;
  onSortChange?: (sort: string) => void;
}

export default function FilterSort({
  onFilterChange,
  onSortChange,
}: FilterSortProps) {
  const [isFilterOpen, setIsFilterOpen] = useState(false);
  const [sortBy, setSortBy] = useState('ending-soon');

  const handleSortChange = (value: string) => {
    setSortBy(value);
    onSortChange?.(value);
  };

  return (
    <div className='mb-6 sm:mb-8'>
      {/* Desktop Filter Bar */}
      <div className='hidden lg:flex items-center justify-between bg-white p-4 rounded-lg shadow-sm border'>
        <div className='flex items-center gap-4'>
          <span className='text-sm font-medium text-gray-700'>Filter by:</span>
          <select className='px-3 py-2 border rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-yellow-500'>
            <option>All Categories</option>
            <option>Luxury Cars</option>
            <option>Sports Cars</option>
            <option>Classic Cars</option>
          </select>
          <select className='px-3 py-2 border rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-yellow-500'>
            <option>All Years</option>
            <option>2020+</option>
            <option>2010-2019</option>
            <option>Before 2010</option>
          </select>
        </div>

        <div className='flex items-center gap-4'>
          <span className='text-sm font-medium text-gray-700'>Sort by:</span>
          <select
            value={sortBy}
            onChange={(e) => handleSortChange(e.target.value)}
            className='px-3 py-2 border rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-yellow-500'
          >
            <option value='ending-soon'>Ending Soon</option>
            <option value='newest'>Newest First</option>
            <option value='price-low'>Price: Low to High</option>
            <option value='price-high'>Price: High to Low</option>
          </select>
        </div>
      </div>

      {/* Mobile Filter Bar */}
      <div className='lg:hidden'>
        <div className='flex items-center justify-between mb-4'>
          <button
            onClick={() => setIsFilterOpen(true)}
            className='flex items-center gap-2 px-4 py-2 bg-white border rounded-lg shadow-sm'
          >
            <svg
              className='w-4 h-4'
              fill='none'
              stroke='currentColor'
              viewBox='0 0 24 24'
            >
              <path
                strokeLinecap='round'
                strokeLinejoin='round'
                strokeWidth={2}
                d='M3 4a1 1 0 011-1h16a1 1 0 011 1v2.586a1 1 0 01-.293.707l-6.414 6.414a1 1 0 00-.293.707V17l-4 4v-6.586a1 1 0 00-.293-.707L3.293 7.707A1 1 0 013 7V4z'
              />
            </svg>
            <span className='text-sm font-medium'>Filters</span>
          </button>

          <select
            value={sortBy}
            onChange={(e) => handleSortChange(e.target.value)}
            className='px-3 py-2 border rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-yellow-500'
          >
            <option value='ending-soon'>Ending Soon</option>
            <option value='newest'>Newest</option>
            <option value='price-low'>Price ↑</option>
            <option value='price-high'>Price ↓</option>
          </select>
        </div>

        {/* Mobile Filter Modal */}
        {isFilterOpen && (
          <div className='fixed inset-0 z-50 bg-black bg-opacity-50'>
            <div className='bg-white h-full max-w-sm ml-auto'>
              <div className='flex items-center justify-between p-4 border-b'>
                <h3 className='text-lg font-semibold'>Filters</h3>
                <button
                  onClick={() => setIsFilterOpen(false)}
                  className='p-2 rounded-lg hover:bg-gray-100'
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
              </div>

              <div className='p-4 space-y-6'>
                <div>
                  <label className='block text-sm font-medium text-gray-700 mb-2'>
                    Category
                  </label>
                  <select className='w-full px-3 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-yellow-500'>
                    <option>All Categories</option>
                    <option>Luxury Cars</option>
                    <option>Sports Cars</option>
                    <option>Classic Cars</option>
                  </select>
                </div>

                <div>
                  <label className='block text-sm font-medium text-gray-700 mb-2'>
                    Year
                  </label>
                  <select className='w-full px-3 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-yellow-500'>
                    <option>All Years</option>
                    <option>2020+</option>
                    <option>2010-2019</option>
                    <option>Before 2010</option>
                  </select>
                </div>

                <div>
                  <label className='block text-sm font-medium text-gray-700 mb-2'>
                    Price Range
                  </label>
                  <div className='space-y-2'>
                    <input
                      type='range'
                      min='0'
                      max='100000'
                      className='w-full'
                    />
                    <div className='flex justify-between text-sm text-gray-500'>
                      <span>$0</span>
                      <span>$100,000+</span>
                    </div>
                  </div>
                </div>
              </div>

              <div className='absolute bottom-0 left-0 right-0 p-4 border-t bg-white'>
                <div className='flex gap-3'>
                  <button className='flex-1 px-4 py-2 border rounded-lg'>
                    Clear All
                  </button>
                  <button
                    onClick={() => setIsFilterOpen(false)}
                    className='flex-1 px-4 py-2 bg-yellow-500 text-white rounded-lg font-medium'
                  >
                    Apply Filters
                  </button>
                </div>
              </div>
            </div>
          </div>
        )}
      </div>
    </div>
  );
}
