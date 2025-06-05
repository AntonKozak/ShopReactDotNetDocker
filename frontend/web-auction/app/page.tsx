import Listning from './components/auction/Listning';
import FilterSort from './components/FilterSort';

export default function Home() {
  return (
    <div>
      <FilterSort />
      <Listning />
    </div>
  );
}
