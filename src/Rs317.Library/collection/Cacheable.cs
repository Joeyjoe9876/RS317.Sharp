namespace Rs317.Sharp
{
	public class Cacheable : Linkable
	{
		public Cacheable nextCacheable;
		public Cacheable previousCacheable;

		public void unlinkCacheable()
		{
			if(previousCacheable == null)
			{
				return;
			}

			previousCacheable.nextCacheable = nextCacheable;
			nextCacheable.previousCacheable = previousCacheable;
			nextCacheable = null;
			previousCacheable = null;
		}
	}
}
