import Link from 'next/link';

export default function Footer() {
  const currentYear = new Date().getFullYear();

  return (
    <footer className='bg-gray-800 text-white mt-auto'>
      <div className='container mx-auto px-4 sm:px-6 lg:px-8 py-8'>
        <div className='grid grid-cols-1 md:grid-cols-4 gap-8'>
          {/* Company Info */}
          <div className='col-span-1 md:col-span-2'>
            <h3 className='text-xl font-bold mb-4'>Auction by Ant</h3>
            <p className='text-gray-300 mb-4'>
              Your trusted platform for finding and bidding on amazing items.
              Discover unique treasures and get the best deals at our online
              auctions.
            </p>
            <div className='flex space-x-4'>
              <a
                href='#'
                className='text-gray-300 hover:text-white transition-colors'
                aria-label='Facebook'
              >
                <svg
                  className='w-5 h-5'
                  fill='currentColor'
                  viewBox='0 0 24 24'
                >
                  <path d='M24 12.073c0-6.627-5.373-12-12-12s-12 5.373-12 12c0 5.99 4.388 10.954 10.125 11.854v-8.385H7.078v-3.47h3.047V9.43c0-3.007 1.792-4.669 4.533-4.669 1.312 0 2.686.235 2.686.235v2.953H15.83c-1.491 0-1.956.925-1.956 1.874v2.25h3.328l-.532 3.47h-2.796v8.385C19.612 23.027 24 18.062 24 12.073z' />
                </svg>
              </a>
              <a
                href='#'
                className='text-gray-300 hover:text-white transition-colors'
                aria-label='Twitter'
              >
                <svg
                  className='w-5 h-5'
                  fill='currentColor'
                  viewBox='0 0 24 24'
                >
                  <path d='M23.953 4.57a10 10 0 01-2.825.775 4.958 4.958 0 002.163-2.723c-.951.555-2.005.959-3.127 1.184a4.92 4.92 0 00-8.384 4.482C7.69 8.095 4.067 6.13 1.64 3.162a4.822 4.822 0 00-.666 2.475c0 1.71.87 3.213 2.188 4.096a4.904 4.904 0 01-2.228-.616v.06a4.923 4.923 0 003.946 4.827 4.996 4.996 0 01-2.212.085 4.936 4.936 0 004.604 3.417 9.867 9.867 0 01-6.102 2.105c-.39 0-.779-.023-1.17-.067a13.995 13.995 0 007.557 2.209c9.053 0 13.998-7.496 13.998-13.985 0-.21 0-.42-.015-.63A9.935 9.935 0 0024 4.59z' />
                </svg>
              </a>
              <a
                href='#'
                className='text-gray-300 hover:text-white transition-colors'
                aria-label='Instagram'
              >
                <svg
                  className='w-5 h-5'
                  fill='currentColor'
                  viewBox='0 0 24 24'
                >
                  <path d='M12.017 0C5.396 0 .029 5.367.029 11.987c0 6.62 5.367 11.987 11.988 11.987 6.62 0 11.987-5.367 11.987-11.987C24.014 5.367 18.637.001 12.017.001zM8.449 16.988c-1.297 0-2.448-.49-3.323-1.297C4.198 14.895 3.726 13.78 3.726 12.5c0-1.297.49-2.448 1.297-3.323.825-.893 1.94-1.365 3.22-1.365 1.297 0 2.448.49 3.323 1.297.893.825 1.365 1.94 1.365 3.22 0 1.297-.49 2.448-1.297 3.323-.825.893-1.94 1.365-3.22 1.365zm7.718-6.062h-2.3v6.062h-2.3v-6.062h-1.4v-1.96h1.4v-1.2c0-.825.275-1.5.825-2.025.55-.55 1.225-.825 2.025-.825h1.95v1.96h-1.2c-.275 0-.5.1-.675.3-.175.2-.275.45-.275.75v1.04h2.15l-.3 1.96z' />
                </svg>
              </a>
            </div>
          </div>

          {/* Quick Links */}
          <div>
            <h4 className='text-lg font-semibold mb-4'>Quick Links</h4>
            <ul className='space-y-2'>
              <li>
                <Link
                  href='/'
                  className='text-gray-300 hover:text-white transition-colors'
                >
                  Browse Auctions
                </Link>
              </li>
              <li>
                <Link
                  href='/auction/create'
                  className='text-gray-300 hover:text-white transition-colors'
                >
                  Sell Items
                </Link>
              </li>
              <li>
                <a
                  href='/about'
                  className='text-gray-300 hover:text-white transition-colors'
                >
                  About Us
                </a>
              </li>
              <li>
                <a
                  href='/contact'
                  className='text-gray-300 hover:text-white transition-colors'
                >
                  Contact
                </a>
              </li>
            </ul>
          </div>

          {/* Support */}
          <div>
            <h4 className='text-lg font-semibold mb-4'>Support</h4>
            <ul className='space-y-2'>
              <li>
                <a
                  href='/help'
                  className='text-gray-300 hover:text-white transition-colors'
                >
                  Help Center
                </a>
              </li>
              <li>
                <a
                  href='/faq'
                  className='text-gray-300 hover:text-white transition-colors'
                >
                  FAQ
                </a>
              </li>
              <li>
                <a
                  href='/terms'
                  className='text-gray-300 hover:text-white transition-colors'
                >
                  Terms of Service
                </a>
              </li>
              <li>
                <a
                  href='/privacy'
                  className='text-gray-300 hover:text-white transition-colors'
                >
                  Privacy Policy
                </a>
              </li>
            </ul>
          </div>
        </div>

        {/* Bottom Bar */}
        <div className='border-t border-gray-700 mt-8 pt-8 flex flex-col md:flex-row justify-between items-center'>
          <p className='text-gray-300 text-sm mb-4 md:mb-0'>
            © {currentYear} Auction by Ant. All rights reserved.
          </p>
          <div className='flex space-x-6 text-sm'>
            <a
              href='/terms'
              className='text-gray-300 hover:text-white transition-colors'
            >
              Terms
            </a>
            <a
              href='/privacy'
              className='text-gray-300 hover:text-white transition-colors'
            >
              Privacy
            </a>
            <a
              href='/cookies'
              className='text-gray-300 hover:text-white transition-colors'
            >
              Cookies
            </a>
          </div>
        </div>
      </div>
    </footer>
  );
}
