using System.Collections;
using Characters.Actions;
using PhysicsUtils;
using Services;
using UnityEngine;

namespace Characters.Gear.Quintessences.Effects;

public sealed class Vampire : QuintessenceEffect
{
	[SerializeField]
	private Collider2D _area;

	[SerializeField]
	private Character _character;

	[SerializeField]
	private Action _ritual;

	[SerializeField]
	private Action _fail;

	[SerializeField]
	private Action _success;

	[SerializeField]
	private CharacterSynchronization _sync;

	private static readonly NonAllocOverlapper _sharedOverlapper = new NonAllocOverlapper(1);

	protected override void OnInvoke(Quintessence quintessence)
	{
		ActivateCharacter();
		_sync.Synchronize(_character, quintessence.owner);
		((MonoBehaviour)this).StartCoroutine(CRitual());
	}

	private IEnumerator CRitual()
	{
		_ritual.TryStart();
		bool success = false;
		while (_ritual.running)
		{
			yield return null;
			if (!CheckPlayerInRange())
			{
				yield return CFailInRitual();
				success = false;
			}
		}
		if (success)
		{
			yield return CSucceedInRitual();
		}
		DeactivateCharacter();
	}

	private void ActivateCharacter()
	{
		((Component)_character).gameObject.SetActive(true);
	}

	private void DeactivateCharacter()
	{
		((Component)_character).gameObject.SetActive(false);
	}

	private bool CheckPlayerInRange()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		((ContactFilter2D)(ref _sharedOverlapper.contactFilter)).SetLayerMask(LayerMask.op_Implicit(512));
		foreach (Target component in _sharedOverlapper.OverlapCollider(_area).GetComponents<Target>(true))
		{
			if (!((Object)(object)component.character == (Object)null) && component.character.type == Character.Type.Player)
			{
				return true;
			}
		}
		return false;
	}

	private IEnumerator CSucceedInRitual()
	{
		_success.TryStart();
		while (_success.running)
		{
			yield return null;
		}
	}

	private IEnumerator CFailInRitual()
	{
		_fail.TryStart();
		while (_fail.running)
		{
			yield return null;
		}
	}

	private void OnDestroy()
	{
		if (!Service.quitting)
		{
			Object.Destroy((Object)(object)((Component)_character).gameObject);
		}
	}
}
