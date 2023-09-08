public class IntOverrider
{
	public bool @override;

	public int value;

	public void Override(ref int value)
	{
		if (@override)
		{
			value = this.value;
		}
	}
}
