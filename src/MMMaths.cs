using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class MMMaths
{
	public static readonly Random random = new Random();

	public static Vector2 Vector3ToVector2(Vector3 target)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2(target.x, target.y);
	}

	public static Vector3 Vector2ToVector3(Vector2 target)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		return new Vector3(target.x, target.y, 0f);
	}

	public static Vector3 Vector2ToVector3(Vector2 target, float newZValue)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		return new Vector3(target.x, target.y, newZValue);
	}

	public static Vector2 RandomPointWithinBounds(Bounds bounds)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		return RandomVector2(Vector2.op_Implicit(((Bounds)(ref bounds)).min), Vector2.op_Implicit(((Bounds)(ref bounds)).max));
	}

	public static Vector2 RandomVector2(Vector2 minimum, Vector2 maximum)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2(Random.Range(minimum.x, maximum.x), Random.Range(minimum.y, maximum.y));
	}

	public static Vector3 RandomVector3(Vector3 minimum, Vector3 maximum)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		return new Vector3(Random.Range(minimum.x, maximum.x), Random.Range(minimum.y, maximum.y), Random.Range(minimum.z, maximum.z));
	}

	public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, float angle)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		angle *= (float)Math.PI / 180f;
		float num = Mathf.Cos(angle) * (point.x - pivot.x) - Mathf.Sin(angle) * (point.y - pivot.y) + pivot.x;
		float num2 = Mathf.Sin(angle) * (point.x - pivot.x) + Mathf.Cos(angle) * (point.y - pivot.y) + pivot.y;
		return new Vector3(num, num2, 0f);
	}

	public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angle)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = point - pivot;
		val = Quaternion.Euler(angle) * val;
		point = val + pivot;
		return point;
	}

	public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion quaternion)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = point - pivot;
		val = quaternion * val;
		point = val + pivot;
		return point;
	}

	public static float AngleBetween(Vector2 vectorA, Vector2 vectorB)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		float num = Vector2.Angle(vectorA, vectorB);
		if (Vector3.Cross(Vector2.op_Implicit(vectorA), Vector2.op_Implicit(vectorB)).z > 0f)
		{
			num = 360f - num;
		}
		return num;
	}

	public static int Sum(params int[] thingsToAdd)
	{
		int num = 0;
		for (int i = 0; i < thingsToAdd.Length; i++)
		{
			num += thingsToAdd[i];
		}
		return num;
	}

	public static int RollADice(int numberOfSides)
	{
		return Random.Range(1, numberOfSides);
	}

	public static bool PercentChance(int percent)
	{
		return Random.Range(0, 100) + 1 <= percent;
	}

	public static bool PercentChance(Random random, int percent)
	{
		return random.Next(0, 100) + 1 <= percent;
	}

	public static bool Chance(float normalizedChance)
	{
		return Random.value <= normalizedChance;
	}

	public static bool Chance(Random random, float normalizedChance)
	{
		return random.NextDouble() <= (double)normalizedChance;
	}

	public static bool Chance(double normalizedChance)
	{
		return (double)Random.value <= normalizedChance;
	}

	public static bool Chance(Random random, double normalizedChance)
	{
		return random.NextDouble() <= normalizedChance;
	}

	public static float Approach(float from, float to, float amount)
	{
		if (from < to)
		{
			from += amount;
			if (from > to)
			{
				return to;
			}
		}
		else
		{
			from -= amount;
			if (from < to)
			{
				return to;
			}
		}
		return from;
	}

	public static float Remap(float x, float A, float B, float C, float D)
	{
		return C + (x - A) / (B - A) * (D - C);
	}

	public static double Remap(double x, double A, double B, double C, double D)
	{
		return C + (x - A) / (B - A) * (D - C);
	}

	public static float RoundToClosest(float value, float[] possibleValues)
	{
		if (possibleValues.Length == 0)
		{
			return 0f;
		}
		float num = possibleValues[0];
		foreach (float num2 in possibleValues)
		{
			if (Mathf.Abs(num - value) > Mathf.Abs(num2 - value))
			{
				num = num2;
			}
		}
		return num;
	}

	public static bool RandomBool()
	{
		return Random.Range(0, 2) == 1;
	}

	public static Quaternion RandomRotation2D()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		return Quaternion.Euler(0f, 0f, (float)Random.Range(0, 360));
	}

	public static Vector2 Min(Vector2 vectorA, Vector2 vectorB)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2(Mathf.Min(vectorA.x, vectorB.x), Mathf.Min(vectorA.y, vectorB.y));
	}

	public static Vector2 Max(Vector2 vectorA, Vector2 vectorB)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2(Mathf.Max(vectorA.x, vectorB.x), Mathf.Max(vectorA.y, vectorB.y));
	}

	public static int CompareDistanceFrom(float a, float b, float origin)
	{
		float num = Mathf.Abs(origin - a);
		float value = Mathf.Abs(origin - b);
		return num.CompareTo(value);
	}

	public static bool Range(byte value, byte min, byte max)
	{
		if (value >= min)
		{
			return value <= max;
		}
		return false;
	}

	public static bool Range(short value, short min, short max)
	{
		if (value >= min)
		{
			return value <= max;
		}
		return false;
	}

	public static bool Range(int value, int min, int max)
	{
		if (value >= min)
		{
			return value <= max;
		}
		return false;
	}

	public static bool Range(long value, long min, long max)
	{
		if (value >= min)
		{
			return value <= max;
		}
		return false;
	}

	public static bool Range(float value, float min, float max)
	{
		if (value >= min)
		{
			return value <= max;
		}
		return false;
	}

	public static bool Range(double value, double min, double max)
	{
		if (value >= min)
		{
			return value <= max;
		}
		return false;
	}

	public static bool Range(float value, Vector2 range)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		if (value >= range.x)
		{
			return value <= range.y;
		}
		return false;
	}

	public static bool Range(float value, Vector2Int range)
	{
		if (value >= (float)((Vector2Int)(ref range)).x)
		{
			return value <= (float)((Vector2Int)(ref range)).y;
		}
		return false;
	}

	public static bool Range(int value, Vector2Int range)
	{
		if (value >= ((Vector2Int)(ref range)).x)
		{
			return value <= ((Vector2Int)(ref range)).y;
		}
		return false;
	}

	public static int[] MultipleRandomWithoutDuplactes(Random random, int count, int min, int max)
	{
		if (max <= min || count < 0 || (count > max - min && max - min > 0))
		{
			throw new ArgumentOutOfRangeException($"Range {min} to {max} ({max - min} values), or count {count} is illegal");
		}
		HashSet<int> hashSet = new HashSet<int>();
		for (int i = max - count; i < max; i++)
		{
			if (!hashSet.Add(random.Next(min, i + 1)))
			{
				hashSet.Add(i);
			}
		}
		int[] array = hashSet.ToArray();
		array.PseudoShuffle(random);
		return array;
	}

	public static int[] MultipleRandomWithoutDuplactes(int count, int min, int max)
	{
		return MultipleRandomWithoutDuplactes(random, count, min, max);
	}

	public static Vector2 BezierCurve(Vector2[] points, float time)
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		int num = points.Length - 1;
		float[] array = new float[points.Length];
		array[0] = 1f;
		for (int i = 1; i < array.Length; i++)
		{
			array[i] = array[i - 1] * ((float)num - (float)(i - 1)) / (float)i;
		}
		Vector2[] array2 = (Vector2[])(object)new Vector2[points.Length];
		for (int j = 0; j < array2.Length; j++)
		{
			array2[j] = points[j] * array[j] * Mathf.Pow(time, (float)j) * Mathf.Pow(1f - time, (float)(num - j));
		}
		Vector2 val = Vector2.zero;
		Vector2[] array3 = array2;
		foreach (Vector2 val2 in array3)
		{
			val += val2;
		}
		return val;
	}

	public static Vector3 GetBezierCurve(Vector3[] points, float time)
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		int num = points.Length - 1;
		float[] array = new float[points.Length];
		array[0] = 1f;
		for (int i = 1; i < array.Length; i++)
		{
			array[i] = array[i - 1] * ((float)num - (float)(i - 1)) / (float)i;
		}
		Vector3[] array2 = (Vector3[])(object)new Vector3[points.Length];
		for (int j = 0; j < array2.Length; j++)
		{
			array2[j] = points[j] * array[j] * Mathf.Pow(time, (float)j) * Mathf.Pow(1f - time, (float)(num - j));
		}
		Vector3 val = Vector3.zero;
		Vector3[] array3 = array2;
		foreach (Vector3 val2 in array3)
		{
			val += val2;
		}
		return val;
	}
}
