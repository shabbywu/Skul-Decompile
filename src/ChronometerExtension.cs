using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class ChronometerExtension
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IEnumerator WaitForSeconds(this ChronometerBase chronometer, float seconds)
	{
		while (seconds >= 0f)
		{
			yield return null;
			seconds -= chronometer?.deltaTime ?? Chronometer.global.deltaTime;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float TimeScale(this ChronometerBase chronometer)
	{
		return chronometer?.timeScale ?? Chronometer.global.timeScale;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float DeltaTime(this ChronometerBase chronometer)
	{
		return chronometer?.deltaTime ?? Chronometer.global.deltaTime;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float SmoothDeltaTime(this ChronometerBase chronometer)
	{
		return chronometer?.deltaTime ?? Chronometer.global.smoothDeltaTime;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float FixedDeltaTime(this ChronometerBase chronometer)
	{
		return chronometer?.deltaTime ?? Chronometer.global.fixedDeltaTime;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void SetChronometer(this GameObject gameObject, ChronometerBase chronometer)
	{
		IUseChronometer component = gameObject.GetComponent<IUseChronometer>();
		if (component != null)
		{
			component.chronometer = chronometer;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void SetChronometer(this GameObject gameObject, GameObject owner)
	{
		IUseChronometer component = gameObject.GetComponent<IUseChronometer>();
		if (component != null)
		{
			component.chronometer = owner.GetComponent<ChronometerBase>();
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void SetChronometer(this GameObject gameObject, Component owner)
	{
		IUseChronometer component = gameObject.GetComponent<IUseChronometer>();
		if (component != null)
		{
			component.chronometer = owner.GetComponent<ChronometerBase>();
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void SetChronometer(this IUseChronometer chronometerUser, ChronometerBase chronometer)
	{
		chronometerUser.chronometer = chronometer;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void SetChronometer(this IUseChronometer chronometerUser, GameObject owner)
	{
		chronometerUser.chronometer = owner.GetComponent<ChronometerBase>();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void SetChronometer(this IUseChronometer chronometerUser, Component owner)
	{
		chronometerUser.chronometer = owner.GetComponent<ChronometerBase>();
	}
}
