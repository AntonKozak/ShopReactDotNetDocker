import type { Metadata } from 'next';
import Footer from './components/Footer';
import NavBar from './components/navbar/NavBar';
import './globals.css';
import ToasterProvider from './providers/ToasterProvider';

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
      <body className='min-h-screen bg-gray-200 flex flex-col'>
        <ToasterProvider />
        <NavBar />
        <main className='container mx-auto px-4 sm:px-6 lg:px-8 pt-6 sm:pt-8 lg:pt-10 pb-8 flex-grow'>
          {children}
        </main>
        <Footer />
      </body>
    </html>
  );
}
