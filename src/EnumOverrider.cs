using System;

public class EnumOverrider<T> where T : Enum
{
	public bool @override;

	public T value;

	public void Override(ref T value)
	{
		if (@override)
		{
			value = this.value;
		}
	}
}
