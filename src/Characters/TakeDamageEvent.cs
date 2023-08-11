namespace Characters;

public class TakeDamageEvent : PriorityList<TakeDamageDelegate>
{
	public bool Invoke(ref Damage damage)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < base._items.Count; i++)
		{
			if (base._items[i].value(ref damage))
			{
				return true;
			}
		}
		return false;
	}
}
