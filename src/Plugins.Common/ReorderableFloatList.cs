using System;
using System.Collections.Generic;

[Serializable]
public class ReorderableFloatList : ReorderableList<float>
{
	public ReorderableFloatList()
	{
	}

	public ReorderableFloatList(params float[] @default)
	{
		values = new List<float>(@default);
	}
}
