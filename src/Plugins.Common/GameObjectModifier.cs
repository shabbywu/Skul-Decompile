using System.Collections;
using UnityEngine;

public static class GameObjectModifier
{
	public delegate void Delegate(MonoBehaviour component);

	public static readonly Delegate None = delegate
	{
	};

	public static readonly Delegate OneScale = delegate(MonoBehaviour component)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		((Component)component).transform.localScale = Vector3.one;
	};

	public static readonly Delegate IdentityRotation = delegate(MonoBehaviour component)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		((Component)component).transform.rotation = Quaternion.identity;
	};

	public static readonly Delegate RandomRotation = delegate(MonoBehaviour component)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		((Component)component).transform.rotation = MMMaths.RandomRotation2D();
	};

	public static void Modify(this MonoBehaviour @this, Delegate @delegate)
	{
		@delegate(@this);
	}

	public static Delegate Scale(float scale)
	{
		return delegate(MonoBehaviour component)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			((Component)component).transform.localScale = Vector3.one * scale;
		};
	}

	public static Delegate Scale(float x, float y)
	{
		return delegate(MonoBehaviour component)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			((Component)component).transform.localScale = new Vector3(x, y, 1f);
		};
	}

	public static Delegate RandomScale(float min, float max)
	{
		return delegate(MonoBehaviour component)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			((Component)component).transform.localScale = Vector3.one * Random.Range(min, max);
		};
	}

	public static Delegate LerpScale(float from, float to, float time)
	{
		return delegate(MonoBehaviour component)
		{
			component.StartCoroutine(CLerpScale(component, from, to, time));
		};
	}

	public static IEnumerator CLerpScale(MonoBehaviour component, float from, float to, float time)
	{
		float elpasedTime = 0f;
		Vector3 fromScale = Vector3.one * from;
		Vector3 toScale = Vector3.one * to;
		for (; elpasedTime < time; elpasedTime += Chronometer.global.deltaTime)
		{
			((Component)component).transform.localScale = Vector3.Lerp(fromScale, toScale, elpasedTime / time);
			yield return null;
		}
	}

	public static Delegate TranslateBySpeedAndAcc(float speed, float acc, float accMultiplier)
	{
		return delegate(MonoBehaviour component)
		{
			component.StartCoroutine(CTranslateBySpeedAndAcc(component, speed, acc, accMultiplier));
		};
	}

	public static IEnumerator CTranslateBySpeedAndAcc(MonoBehaviour component, float speed, float acc, float accMultiflier)
	{
		while (true)
		{
			speed += acc * Time.deltaTime;
			acc += acc * accMultiflier * Time.deltaTime;
			((Component)component).transform.Translate(0f, speed * Time.deltaTime, 0f);
			yield return null;
		}
	}

	public static Delegate ScaleByAcc(float scale, float acc, float accMultiplier)
	{
		return delegate(MonoBehaviour component)
		{
			component.StartCoroutine(CScaleByAcc(component, scale, acc, accMultiplier));
		};
	}

	public static IEnumerator CScaleByAcc(MonoBehaviour component, float scale, float acc, float accMultiflier)
	{
		while (true)
		{
			scale += acc * Time.deltaTime;
			acc += acc * accMultiflier * Time.deltaTime;
			((Component)component).transform.localScale = new Vector3(scale, scale, 0f);
			yield return null;
		}
	}

	public static Delegate TranslateUniformMotion(float x, float y, float z)
	{
		return delegate(MonoBehaviour component)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			component.StartCoroutine(CTranslateUniformMotion(component, new Vector3(x, y, z)));
		};
	}

	public static Delegate TranslateUniformMotion(Vector3 speed)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return delegate(MonoBehaviour component)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			component.StartCoroutine(CTranslateUniformMotion(component, speed));
		};
	}

	public static IEnumerator CTranslateUniformMotion(MonoBehaviour component, Vector3 speed)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		while (true)
		{
			((Component)component).transform.Translate(speed * Time.deltaTime);
			yield return null;
		}
	}
}
