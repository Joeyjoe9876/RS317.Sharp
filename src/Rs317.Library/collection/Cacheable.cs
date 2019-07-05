
public class Cacheable : Linkable
{
	public Cacheable nextCacheable;
	public Cacheable previousCacheable;

	public sealed void unlinkCacheable()
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
