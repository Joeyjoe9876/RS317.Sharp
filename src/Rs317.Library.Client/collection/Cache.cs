namespace Rs317.Sharp
{
	/// <summary>
	/// A least-recently-used cache
	/// </summary>
	public class Cache
	{
		private Cacheable empty;
		private int size;
		private int available;
		private LinkableHashMap hashmap;
		private CacheableQueue retrievedItems;

		public Cache(int length)
		{
			empty = new Cacheable();
			retrievedItems = new CacheableQueue();
			hashmap = new LinkableHashMap(1024);

			size = length;
			available = length;
		}

		public Cacheable get(long key)
		{
			Cacheable item = (Cacheable)hashmap.get(key);

			if(item != null)
			{
				retrievedItems.push(item);
			}

			return item;
		}

		public void put(Cacheable item, long key)
		{
			if(available == 0)
			{
				Cacheable oldest = retrievedItems.pop();
				oldest.unlink();
				oldest.unlinkCacheable();

				if(oldest == empty)
				{
					Cacheable secondOldest = retrievedItems.pop();
					secondOldest.unlink();
					secondOldest.unlinkCacheable();
				}
			}
			else
			{
				available--;
			}

			hashmap.put(key, item);
			retrievedItems.push(item);
			return;
		}

		public void clear()
		{
			while(true)
			{
				Cacheable oldest = retrievedItems.pop();

				if(oldest == null)
				{
					available = size;
					return;
				}

				oldest.unlink();
				oldest.unlinkCacheable();
			}
		}
	}
}