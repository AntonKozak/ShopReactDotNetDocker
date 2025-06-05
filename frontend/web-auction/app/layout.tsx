import type { Metadata } from 'next';
import NavBar from './components/NavBar';
import './globals.css';

export const metadata: Metadata = {
  title: 'Auction by Ant',
  description: 'Buy your favorite items at the best price',
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang='en'>
      <body className='min-h-screen bg-gray-50'>
        <NavBar />
        <main className='container mx-auto px-4 sm:px-6 lg:px-8 pt-6 sm:pt-8 lg:pt-10 pb-8'>
          {children}
        </main>
      </body>
    </html>
  );
}
