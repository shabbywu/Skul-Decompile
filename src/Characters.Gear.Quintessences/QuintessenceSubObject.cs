using System.Collections;
using Level;
using Services;
using Singletons;
using UnityEngine;
using UnityEngine.Events;

namespace Characters.Gear.Quintessences;

public sealed class QuintessenceSubObject : MonoBehaviour
{
	[SerializeField]
	private float _lifeTime;

	[SerializeField]
	private Quintessence _quintessence;

	[SerializeField]
	private UnityEvent _onUse;

	[SerializeField]
	private UnityEvent _onDiscard;

	private void Start()
	{
		RegisterEvents();
		Deactive();
	}

	private void RegisterEvents()
	{
		Singleton<Service>.Instance.levelManager.onMapLoaded += Deactive;
		_quintessence.onDiscard += OnDiscard;
	}

	public void Use()
	{
		if ((Object)(object)((Component)this).transform.parent != (Object)null)
		{
			((Component)this).transform.SetParent((Transform)null);
		}
		UnityEvent onUse = _onUse;
		if (onUse != null)
		{
			onUse.Invoke();
		}
		((MonoBehaviour)this).StartCoroutine(CLifeSpan());
	}

	private void OnUse()
	{
		UnityEvent onUse = _onUse;
		if (onUse != null)
		{
			onUse.Invoke();
		}
	}

	private void OnDiscard(Gear gear)
	{
		((Component)this).transform.SetParent(((Component)Map.Instance).transform);
		UnityEvent onDiscard = _onDiscard;
		if (onDiscard != null)
		{
			onDiscard.Invoke();
		}
		Deactive();
		Singleton<Service>.Instance.levelManager.onMapLoaded -= Deactive;
	}

	private void Deactive()
	{
		((Component)this).gameObject.SetActive(false);
	}

	private IEnumerator CLifeSpan()
	{
		for (float elapsed = 0f; elapsed < _lifeTime; elapsed += ((ChronometerBase)Chronometer.global).deltaTime)
		{
			yield return null;
		}
		Deactive();
	}
}
