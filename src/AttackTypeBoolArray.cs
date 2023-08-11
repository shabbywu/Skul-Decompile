using System;
using Characters;

[Serializable]
public class AttackTypeBoolArray : EnumArray<Damage.AttackType, bool>
{
	public AttackTypeBoolArray()
	{
	}

	public AttackTypeBoolArray(params bool[] values)
	{
		int num = Math.Min(base.Array.Length, values.Length);
		for (int i = 0; i < num; i++)
		{
			base.Array[i] = values[i];
		}
	}
}
