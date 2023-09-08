using UnityEngine;

public static class RectUtility
{
	public static void UseWidth(this Rect rect, float width)
	{
		((Rect)(ref rect)).x = ((Rect)(ref rect)).x + width;
		((Rect)(ref rect)).width = ((Rect)(ref rect)).width - width;
	}

	public static void UseHeight(this Rect rect, float height)
	{
		((Rect)(ref rect)).y = ((Rect)(ref rect)).y + height;
		((Rect)(ref rect)).height = ((Rect)(ref rect)).height - height;
	}

	public static Rect SubWidth(this Rect rect, float startPercent, float endPercent)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		Rect result = rect;
		float num = endPercent - startPercent;
		((Rect)(ref result)).width = ((Rect)(ref rect)).width * num;
		((Rect)(ref result)).x = ((Rect)(ref result)).x + ((Rect)(ref rect)).width * startPercent;
		return result;
	}

	public static Rect SubHeight(this Rect rect, float startPercent, float endPercent)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		Rect result = rect;
		float num = endPercent - startPercent;
		((Rect)(ref result)).height = ((Rect)(ref rect)).height * num;
		((Rect)(ref result)).y = ((Rect)(ref result)).y + ((Rect)(ref rect)).height * startPercent;
		return result;
	}

	public static Rect[] GetHorizontal(this Rect rect, float height, float margin, params float[] weights)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		Rect rect2 = rect;
		((Rect)(ref rect2)).height = height;
		Rect[] array = (Rect[])(object)new Rect[weights.Length];
		float num = 0f;
		foreach (float num2 in weights)
		{
			num += num2;
		}
		float num3 = 0f;
		for (int j = 0; j < weights.Length; j++)
		{
			if (j == weights.Length - 1)
			{
				array[j] = rect2.SubWidth(num3, num3 += weights[j] / num);
			}
			else
			{
				array[j] = rect2.SubWidth(num3, (num3 += weights[j] / num) - margin);
			}
		}
		return array;
	}

	public static Rect[] GetVertical(this Rect rect, float width, float margin, params float[] weights)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		Rect rect2 = rect;
		((Rect)(ref rect2)).width = width;
		Rect[] array = (Rect[])(object)new Rect[weights.Length];
		float num = 0f;
		foreach (float num2 in weights)
		{
			num += num2;
		}
		float num3 = 0f;
		for (int j = 0; j < weights.Length; j++)
		{
			if (j == weights.Length - 1)
			{
				array[j] = rect2.SubHeight(num3, num3 += weights[j] / num);
			}
			else
			{
				array[j] = rect2.SubHeight(num3, (num3 += weights[j] / num) - margin);
			}
		}
		return array;
	}
}
