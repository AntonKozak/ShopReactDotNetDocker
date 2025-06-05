import { MdGavel } from 'react-icons/md';
import MobileSearch from './MobileSearch';

export default function NavBar() {
  return (
    <header className='sticky top-0 z-50 flex items-center justify-between bg-gray-800 px-4 py-3 text-white shadow-lg'>
      {/* Logo Section - Responsive sizing */}
      <div className='flex items-center gap-2 text-xl sm:text-2xl lg:text-3xl font-semibold text-yellow-500'>
        <MdGavel className='text-2xl sm:text-3xl lg:text-4xl' />
        <div className='hidden sm:block'>Ant_Auction</div>
        <div className='block sm:hidden'>Ant</div>
      </div>

      {/* Search Section - Responsive */}
      <div className='flex-1 max-w-md mx-4 hidden md:block'>
        <input
          type='text'
          placeholder='Search auctions...'
          className='w-full px-4 py-2 rounded-lg bg-gray-700 text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-yellow-500'
        />
      </div>

      {/* Action Buttons - Responsive */}
      <div className='flex items-center gap-2 sm:gap-4'>
        <MobileSearch />
        <button className='hidden sm:flex items-center gap-2 px-4 py-2 bg-yellow-500 text-gray-900 rounded-lg font-medium hover:bg-yellow-400 transition-colors'>
          <span className='hidden lg:inline'>Sign In</span>
          <span className='lg:hidden'>Login</span>
        </button>
        <button className='sm:hidden p-2 rounded-lg hover:bg-gray-700'>
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
              d='M4 6h16M4 12h16M4 18h16'
            />
          </svg>
        </button>
      </div>
    </header>
  );
}
