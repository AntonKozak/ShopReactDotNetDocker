'use client';

import { Pagination } from 'flowbite-react';

type Props = {
  currentPage: number;
  pageCount: number;
  onPageChange: (page: number) => void;
};

export default function PaginationPage({
  currentPage,
  pageCount,
  onPageChange,
}: Props) {
  return (
    <Pagination
      currentPage={currentPage}
      totalPages={pageCount}
      onPageChange={(page) => {
        onPageChange(page);
      }}
      layout='pagination'
      showIcons={true}
      className='flex justify-center items-center mt-4 text-grey-600 dark:text-gray-400'
    />
  );
}
