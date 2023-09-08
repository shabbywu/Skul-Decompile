public class BoolOverrider
{
	public bool @override;

	public bool value;

	public void Override(ref bool value)
	{
		if (@override)
		{
			value = this.value;
		}
	}
}
