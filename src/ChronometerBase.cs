using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChronometerBase
{
	protected static readonly List<IUseChronometer> _useChronometersCache = new List<IUseChronometer>();

	protected readonly ProductFloat _timeScales = new ProductFloat(1f);

	protected readonly Dictionary<object, Coroutine> _attachTimeScaleCoroutines = new Dictionary<object, Coroutine>();

	public abstract ChronometerBase parent { get; set; }

	public abstract float localTimeScale { get; }

	public abstract float timeScale { get; }

	public abstract float deltaTime { get; }

	public abstract float smoothDeltaTime { get; }

	public abstract float fixedDeltaTime { get; }

	protected abstract void Update();

	public void AttachTimeScale(object key, float timeScale)
	{
		if (_attachTimeScaleCoroutines.TryGetValue(key, out var value))
		{
			((MonoBehaviour)CoroutineProxy.instance).StopCoroutine(value);
			_attachTimeScaleCoroutines.Remove(key);
		}
		_timeScales[key] = timeScale;
		Update();
	}

	public void AttachTimeScale(object key, float timeScale, float duration)
	{
		if (_attachTimeScaleCoroutines.TryGetValue(key, out var value))
		{
			((MonoBehaviour)CoroutineProxy.instance).StopCoroutine(value);
			_attachTimeScaleCoroutines[key] = ((MonoBehaviour)CoroutineProxy.instance).StartCoroutine(CAttachTimeScale());
		}
		else
		{
			_attachTimeScaleCoroutines.Add(key, ((MonoBehaviour)CoroutineProxy.instance).StartCoroutine(CAttachTimeScale()));
		}
		IEnumerator CAttachTimeScale()
		{
			float remainTime = duration;
			_timeScales[key] = timeScale;
			Update();
			while (remainTime > 0f)
			{
				if (Time.timeScale > 0f)
				{
					remainTime -= Time.unscaledDeltaTime;
				}
				yield return null;
			}
			_timeScales.Remove(key);
			Update();
		}
	}

	public bool DetachTimeScale(object key)
	{
		if (_timeScales.Remove(key))
		{
			Update();
			return true;
		}
		return false;
	}

	public void AttachTo(Component component, bool includeChildren)
	{
		if (includeChildren)
		{
			component.GetComponentsInChildren<IUseChronometer>(true, _useChronometersCache);
		}
		else
		{
			component.GetComponents<IUseChronometer>(_useChronometersCache);
		}
		for (int i = 0; i < _useChronometersCache.Count; i++)
		{
			_useChronometersCache[i].chronometer = this;
		}
	}

	public void AttachTo(GameObject gameObject, bool includeChildren)
	{
		if (includeChildren)
		{
			gameObject.GetComponentsInChildren<IUseChronometer>(true, _useChronometersCache);
		}
		else
		{
			gameObject.GetComponents<IUseChronometer>(_useChronometersCache);
		}
		for (int i = 0; i < _useChronometersCache.Count; i++)
		{
			_useChronometersCache[i].chronometer = this;
		}
	}
}
