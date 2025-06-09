'use client';

import { Button } from 'flowbite-react';
import { signIn } from 'next-auth/react';
import { useParamsStore } from '../hooks/useParamsStore';
import Heading from './Heading';

type Props = {
  title?: string;
  subtitle?: string;
  showReset?: boolean;
  showLogin?: boolean;
  callbackUrl?: string;
};

export default function EmptyFilter({
  title = 'No matches',
  subtitle = 'Try to change filter',
  showReset,
  showLogin,
  callbackUrl,
}: Props) {
  const reset = useParamsStore((state) => state.resetParams);

  return (
    <div
      className='
        flex flex-col gap-2 items-center justify-center h-[40vh] shadow-lg
    '
    >
      <Heading title={title} subtitle={subtitle} center />
      <div className='mt-4'>
        {showReset && (
          <Button outline onClick={reset}>
            Remove filters
          </Button>
        )}
        {showLogin && (
          <Button
            outline
            onClick={() => signIn('id-server', { redirectTo: callbackUrl })}
          >
            Login
          </Button>
        )}
      </div>
    </div>
  );
}
