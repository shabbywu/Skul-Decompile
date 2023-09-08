using System;

namespace Southpaw;

public class Random
{
	public static System.Random random = new System.Random();

	public static double NextDouble()
	{
		return random.NextDouble();
	}

	public static double NextDouble(double minValue, double maxValue)
	{
		return minValue + random.NextDouble() * (maxValue - minValue);
	}

	public static int NextInt(double minValue, double maxValue)
	{
		return (int)(minValue + random.NextDouble() * (maxValue + 1.0 - minValue));
	}
}
