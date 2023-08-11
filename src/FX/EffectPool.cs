using System.Collections;
using Singletons;
using UnityEngine;

namespace FX;

public class EffectPool : Singleton<EffectPool>
{
	private const int _limit = 300;

	[SerializeField]
	private EffectPoolInstance _originalPoolObject;

	private EffectPoolInstance[] _poolObjects;

	private int _border;

	protected override void Awake()
	{
		base.Awake();
		_poolObjects = new EffectPoolInstance[300];
		((MonoBehaviour)this).StartCoroutine(CPrepareObjects());
	}

	private IEnumerator CUpdate()
	{
		while (true)
		{
			for (int i = 0; i < _border; i++)
			{
				EffectPoolInstance effectPoolInstance = _poolObjects[i];
				if (!((Object)(object)effectPoolInstance == (Object)null))
				{
					effectPoolInstance.UpdateEffect();
					if (effectPoolInstance.state == EffectPoolInstance.State.Stopped)
					{
						_border--;
						_poolObjects[i] = _poolObjects[_border];
						_poolObjects[_border] = effectPoolInstance;
					}
				}
			}
			yield return null;
		}
	}

	private IEnumerator CPrepareObjects()
	{
		for (int i = 0; i < 300; i++)
		{
			yield return null;
			InstantiatePoolObject(i);
		}
		((MonoBehaviour)this).StartCoroutine(CUpdate());
	}

	private EffectPoolInstance GetPoolObjectOrInstantiate(int index)
	{
		EffectPoolInstance effectPoolInstance = _poolObjects[index];
		if ((Object)(object)effectPoolInstance == (Object)null || effectPoolInstance.state == EffectPoolInstance.State.Destroyed)
		{
			effectPoolInstance = InstantiatePoolObject(index);
		}
		return effectPoolInstance;
	}

	private EffectPoolInstance InstantiatePoolObject(int index)
	{
		if ((Object)(object)_originalPoolObject != (Object)null)
		{
			_poolObjects[index] = Object.Instantiate<EffectPoolInstance>(_originalPoolObject);
			return _poolObjects[index];
		}
		return null;
	}

	public EffectPoolInstance Play(RuntimeAnimatorController animation, float delay, float duration, bool loop, AnimationCurve fadeOutCurve, float fadeOutDuration)
	{
		EffectPoolInstance poolObjectOrInstantiate = GetPoolObjectOrInstantiate(_border);
		if ((Object)(object)poolObjectOrInstantiate == (Object)null)
		{
			return null;
		}
		poolObjectOrInstantiate.Play(animation, delay, duration, loop, fadeOutCurve, fadeOutDuration);
		_border++;
		if (_border == 300)
		{
			_border--;
			EffectPoolInstance poolObjectOrInstantiate2 = GetPoolObjectOrInstantiate(0);
			if ((Object)(object)poolObjectOrInstantiate2 == (Object)null)
			{
				return null;
			}
			poolObjectOrInstantiate2.Stop();
			_poolObjects[_border] = poolObjectOrInstantiate2;
			_poolObjects[0] = poolObjectOrInstantiate;
		}
		return poolObjectOrInstantiate;
	}

	public void Clear()
	{
		for (int i = 0; i < _border; i++)
		{
			EffectPoolInstance poolObjectOrInstantiate = GetPoolObjectOrInstantiate(i);
			if (poolObjectOrInstantiate.state != 0)
			{
				poolObjectOrInstantiate.Stop();
			}
		}
	}

	public void ClearNonAttached()
	{
		for (int i = 0; i < _border; i++)
		{
			EffectPoolInstance poolObjectOrInstantiate = GetPoolObjectOrInstantiate(i);
			if (poolObjectOrInstantiate.state != 0 && !((Object)(object)((Component)poolObjectOrInstantiate).transform.parent != (Object)null))
			{
				poolObjectOrInstantiate.Stop();
			}
		}
	}
}
