public class DoubleBuffered<T>
{
	public T Current { get; protected set; }

	public T Next { get; protected set; }

	public DoubleBuffered(T current, T next)
	{
		Current = current;
		Next = next;
	}

	public T Swap()
	{
		T current = Current;
		Current = Next;
		Next = current;
		return Current;
	}
}
