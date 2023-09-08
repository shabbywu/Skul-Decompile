using System;
using Characters;

[Serializable]
public class CharacterStatusKindBoolArray : EnumArray<CharacterStatus.Kind, bool>
{
	public CharacterStatusKindBoolArray(params bool[] values)
	{
		int num = Math.Min(base.Array.Length, values.Length);
		for (int i = 0; i < num; i++)
		{
			base.Array[i] = values[i];
		}
	}
}
