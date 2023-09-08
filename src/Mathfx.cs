using System;
using UnityEngine;

public static class Mathfx
{
	public enum EaseMethod
	{
		Lerp = 0,
		SmoothStep = 1,
		Hermite = 2,
		Sinerp = 3,
		Coserp = 4,
		Berp = 5,
		EaseInQuad = 0,
		EaseOutQuad = 1,
		EaseInOutQuad = 2,
		EaseInCubic = 3,
		EaseOutCubic = 4,
		EaseInOutCubic = 5,
		EaseInQuart = 6,
		EaseOutQuart = 7,
		EaseInOutQuart = 8,
		EaseInQuint = 9,
		EaseOutQuint = 10,
		EaseInOutQuint = 11,
		EaseInSine = 12,
		EaseOutSine = 13,
		EaseInOutSine = 14,
		EaseInExpo = 15,
		EaseOutExpo = 16,
		EaseInOutExpo = 17,
		EaseInCirc = 18,
		EaseOutCirc = 19,
		EaseInOutCirc = 20,
		Linear = 21,
		Spring = 22,
		EaseInBounce = 23,
		EaseOutBounce = 24,
		EaseInOutBounce = 25,
		EaseInBack = 26,
		EaseOutBack = 27,
		EaseInOutBack = 28,
		EaseInElastic = 29,
		EaseOutElastic = 30,
		EaseInOutElastic = 31
	}

	public delegate float EaseFunction(float start, float end, float value);

	public static EaseFunction GetEaseFunction(EaseMethod method)
	{
		return method switch
		{
			EaseMethod.Hermite => Hermite, 
			EaseMethod.Sinerp => Sinerp, 
			EaseMethod.Coserp => Coserp, 
			EaseMethod.Berp => Berp, 
			EaseMethod.SmoothStep => SmoothStep, 
			_ => Lerp, 
		};
	}

	public static float Ease(EaseMethod method, float start, float end, float value)
	{
		return GetEaseFunction(method)(start, end, value);
	}

	public static float Hermite(float start, float end, float value)
	{
		return Mathf.Lerp(start, end, value * value * (3f - 2f * value));
	}

	public static Vector2 Hermite(Vector2 start, Vector2 end, float value)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2(Hermite(start.x, end.x, value), Hermite(start.y, end.y, value));
	}

	public static Vector3 Hermite(Vector3 start, Vector3 end, float value)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		return new Vector3(Hermite(start.x, end.x, value), Hermite(start.y, end.y, value), Hermite(start.z, end.z, value));
	}

	public static float Sinerp(float start, float end, float value)
	{
		return Mathf.Lerp(start, end, Mathf.Sin(value * (float)Math.PI * 0.5f));
	}

	public static Vector2 Sinerp(Vector2 start, Vector2 end, float value)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2(Mathf.Lerp(start.x, end.x, Mathf.Sin(value * (float)Math.PI * 0.5f)), Mathf.Lerp(start.y, end.y, Mathf.Sin(value * (float)Math.PI * 0.5f)));
	}

	public static Vector3 Sinerp(Vector3 start, Vector3 end, float value)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		return new Vector3(Mathf.Lerp(start.x, end.x, Mathf.Sin(value * (float)Math.PI * 0.5f)), Mathf.Lerp(start.y, end.y, Mathf.Sin(value * (float)Math.PI * 0.5f)), Mathf.Lerp(start.z, end.z, Mathf.Sin(value * (float)Math.PI * 0.5f)));
	}

	public static float Coserp(float start, float end, float value)
	{
		return Mathf.Lerp(start, end, 1f - Mathf.Cos(value * (float)Math.PI * 0.5f));
	}

	public static Vector2 Coserp(Vector2 start, Vector2 end, float value)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2(Coserp(start.x, end.x, value), Coserp(start.y, end.y, value));
	}

	public static Vector3 Coserp(Vector3 start, Vector3 end, float value)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		return new Vector3(Coserp(start.x, end.x, value), Coserp(start.y, end.y, value), Coserp(start.z, end.z, value));
	}

	public static float Berp(float start, float end, float value)
	{
		value = Mathf.Clamp01(value);
		value = (Mathf.Sin(value * (float)Math.PI * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + 1.2f * (1f - value));
		return start + (end - start) * value;
	}

	public static Vector2 Berp(Vector2 start, Vector2 end, float value)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2(Berp(start.x, end.x, value), Berp(start.y, end.y, value));
	}

	public static Vector3 Berp(Vector3 start, Vector3 end, float value)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		return new Vector3(Berp(start.x, end.x, value), Berp(start.y, end.y, value), Berp(start.z, end.z, value));
	}

	public static float SmoothStep(float x, float min, float max)
	{
		x = Mathf.Clamp(x, min, max);
		float num = (x - min) / (max - min);
		float num2 = (x - min) / (max - min);
		return -2f * num * num * num + 3f * num2 * num2;
	}

	public static Vector2 SmoothStep(Vector2 vec, float min, float max)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2(SmoothStep(vec.x, min, max), SmoothStep(vec.y, min, max));
	}

	public static Vector3 SmoothStep(Vector3 vec, float min, float max)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		return new Vector3(SmoothStep(vec.x, min, max), SmoothStep(vec.y, min, max), SmoothStep(vec.z, min, max));
	}

	public static float Lerp(float start, float end, float value)
	{
		return (1f - value) * start + value * end;
	}

	public static Vector3 NearestPoint(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = Vector3.Normalize(lineEnd - lineStart);
		float num = Vector3.Dot(point - lineStart, val);
		return lineStart + num * val;
	}

	public static Vector3 NearestPointStrict(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = lineEnd - lineStart;
		Vector3 val2 = Vector3.Normalize(val);
		float num = Vector3.Dot(point - lineStart, val2);
		return lineStart + Mathf.Clamp(num, 0f, Vector3.Magnitude(val)) * val2;
	}

	public static float Bounce(float x)
	{
		return Mathf.Abs(Mathf.Sin(6.28f * (x + 1f) * (x + 1f)) * (1f - x));
	}

	public static Vector2 Bounce(Vector2 vec)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2(Bounce(vec.x), Bounce(vec.y));
	}

	public static Vector3 Bounce(Vector3 vec)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		return new Vector3(Bounce(vec.x), Bounce(vec.y), Bounce(vec.z));
	}

	public static bool Approx(float val, float about, float range)
	{
		return Mathf.Abs(val - about) < range;
	}

	public static bool Approx(Vector3 val, Vector3 about, float range)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val2 = val - about;
		return ((Vector3)(ref val2)).sqrMagnitude < range * range;
	}

	public static float Clerp(float start, float end, float value)
	{
		float num = 0f;
		float num2 = 360f;
		float num3 = Mathf.Abs((num2 - num) / 2f);
		float num4 = 0f;
		float num5 = 0f;
		if (end - start < 0f - num3)
		{
			num5 = (num2 - start + end) * value;
			return start + num5;
		}
		if (end - start > num3)
		{
			num5 = (0f - (num2 - end + start)) * value;
			return start + num5;
		}
		return start + (end - start) * value;
	}
}
