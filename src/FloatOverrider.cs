public class FloatOverrider
{
	public bool @override;

	public float value;

	public void Override(ref float value)
	{
		if (@override)
		{
			value = this.value;
		}
	}
}
