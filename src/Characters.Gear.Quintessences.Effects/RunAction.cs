using System.Collections;
using Characters.Actions;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Gear.Quintessences.Effects;

public sealed class RunAction : QuintessenceEffect
{
	[SerializeField]
	private Character _character;

	[SerializeField]
	private Action _action;

	[SerializeField]
	private CharacterSynchronization _synchronization;

	private void Awake()
	{
		Initialize();
	}

	protected override void OnInvoke(Quintessence quintessence)
	{
		Character owner = quintessence.owner;
		if (!((Object)(object)owner == (Object)null))
		{
			ActivateCharacter();
			_synchronization.Synchronize(_character, owner);
			((MonoBehaviour)this).StartCoroutine(CRun());
		}
	}

	private void Initialize()
	{
		DeactivateCharacter();
		((Component)_character).transform.parent = null;
		Singleton<Service>.Instance.levelManager.onMapLoaded += DeactivateCharacter;
	}

	private void ActivateCharacter()
	{
		((Component)_character).gameObject.SetActive(true);
	}

	private void DeactivateCharacter()
	{
		((Component)_character).gameObject.SetActive(false);
	}

	private void Dispose()
	{
		Singleton<Service>.Instance.levelManager.onMapLoaded -= DeactivateCharacter;
		Object.Destroy((Object)(object)((Component)_character).gameObject);
	}

	private IEnumerator CRun()
	{
		_action.TryStart();
		while (_action.running)
		{
			yield return null;
		}
		DeactivateCharacter();
	}

	private void OnDestroy()
	{
		if (!Service.quitting)
		{
			Dispose();
		}
	}
}
