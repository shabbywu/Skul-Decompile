using System;
using System.Collections;
using UnityEngine;

namespace Runnables;

public abstract class CRunnable : MonoBehaviour
{
	public static readonly Type[] types = new Type[9]
	{
		typeof(CharacterTranslateTo),
		typeof(FadeIn),
		typeof(FadeOut),
		typeof(GameFadeIn),
		typeof(GameFadeOut),
		typeof(TransformTranslateTo),
		typeof(WaitForTime),
		typeof(WaitForWeaponUpgrade),
		typeof(ShowHardmodeEndingCredit)
	};

	public abstract IEnumerator CRun();
}
