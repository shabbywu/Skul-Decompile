using System;

[Serializable]
public class ReorderableFloatArray : ReorderableArray<float>
{
	public ReorderableFloatArray()
	{
	}

	public ReorderableFloatArray(params float[] @default)
	{
		values = @default;
	}
}
