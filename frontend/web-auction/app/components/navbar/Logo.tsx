'use client';

import { useParamsStore } from '@/app/hooks/useParamsStore';
import { MdGavel } from 'react-icons/md';

export default function Logo() {
  const reset = useParamsStore((state) => state.resetParams);

  return (
    <div
      onClick={reset}
      className='cursor-pointer flex items-center gap-2 text-xl sm:text-2xl lg:text-3xl font-semibold text-yellow-500 transition-colors hover:text-yellow-400'
    >
      <MdGavel className='text-2xl sm:text-3xl lg:text-4xl transition-colors group-hover:text-yellow-400' />
      <div className='hidden sm:block'>Ant_Auction</div>
      <div className='block sm:hidden'>Ant</div>
    </div>
  );
}
