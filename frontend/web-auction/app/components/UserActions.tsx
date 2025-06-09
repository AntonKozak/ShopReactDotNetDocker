'use client';

import { Dropdown, DropdownDivider, DropdownItem } from 'flowbite-react';
import { User } from 'next-auth';
import Link from 'next/link';
import { AiFillCar, AiFillTrophy, AiOutlineLogout } from 'react-icons/ai';
import { HiCog, HiUser } from 'react-icons/hi';

interface UserActionsProps {
  user: User;
}

export default function UserActions({ user }: UserActionsProps) {
  const handleSignOut = async () => {
    window.location.href = '/api/auth/signout';
  };

  return (
    <Dropdown inline label={`Welcome ${user.name}`} className='cursor-pointer'>
      <DropdownItem icon={HiUser}>My Auctions</DropdownItem>
      <DropdownItem icon={AiFillTrophy}>Auctions won</DropdownItem>
      <DropdownItem icon={AiFillCar}>Sell my item</DropdownItem>
      <DropdownItem icon={HiCog}>
        <Link href='/session'>Session (dev only!)</Link>
      </DropdownItem>
      <DropdownDivider />{' '}
      <DropdownItem icon={AiOutlineLogout} onClick={handleSignOut}>
        Sign out
      </DropdownItem>
    </Dropdown>
  );
}
