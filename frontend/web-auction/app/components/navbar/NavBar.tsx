import { getCurrentUser } from '@/app/actions/authActions';
import LoginButton from '../LoginButton';
import UserActions from '../UserActions';
import Logo from './Logo';
import MobileSearch from './MobileSearch';
import Search from './Search';

export default async function NavBar() {
  const user = await getCurrentUser();

  return (
    <header className='sticky top-0 z-50 flex items-center justify-between bg-gray-800 px-4 py-3 text-white shadow-lg'>
      {/* Logo Section - Responsive sizing */}
      <Logo />
      {/* Search Section - Responsive */}
      <Search />

      {/* Action Buttons - Responsive */}
      <div className='flex items-center gap-2 sm:gap-4'>
        <MobileSearch />
        {user ? <UserActions user={user} /> : <LoginButton />}
      </div>
    </header>
  );
}
