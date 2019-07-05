
public class Linkable
{
	public long id;
	public Linkable next;
	public Linkable previous;

	public sealed void unlink()
	{
		if(previous == null)
		{
			return;
		}

		previous.next = next;
		next.previous = previous;
		next = null;
		previous = null;
	}
}
