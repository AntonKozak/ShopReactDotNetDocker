import EmptyFilter from '@/app/components/navbar/EmptyFilter';
import { Suspense } from 'react';

interface PageProps {
  searchParams: Promise<{ callbackUrl?: string }>;
}

export default async function SignInPage({ searchParams }: PageProps) {
  const params = await searchParams;
  const callbackUrl = params.callbackUrl;

  return (
    <Suspense fallback={<div>Loading...</div>}>
      <EmptyFilter
        title='You need to be logged in to do that'
        subtitle='Please click below to login'
        showLogin
        callbackUrl={callbackUrl}
      />
    </Suspense>
  );
}
