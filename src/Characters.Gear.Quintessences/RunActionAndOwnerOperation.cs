using System;
using System.Collections;
using Characters.Actions;
using Characters.Operations;
using Services;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Characters.Gear.Quintessences;

public class RunActionAndOwnerOperation : UseQuintessence
{
	[SerializeField]
	private Character _character;

	[SerializeField]
	[Subcomponent(typeof(OperationInfo))]
	private OperationInfo.Subcomponents _operations;

	[SerializeField]
	private Characters.Actions.Action _action;

	protected override void Awake()
	{
		base.Awake();
		_quintessence.onEquipped += _operations.Initialize;
		((Component)_character).gameObject.SetActive(false);
		((Component)_character).transform.parent = null;
	}

	private void AttachEvents()
	{
		Character character = _character;
		character.onKilled = (Character.OnKilledDelegate)Delegate.Combine(character.onKilled, new Character.OnKilledDelegate(OnKilled));
		((PriorityList<GiveDamageDelegate>)_character.onGiveDamage).Add(int.MinValue, (GiveDamageDelegate)OnGiveDamage);
		Character character2 = _character;
		character2.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character2.onGaveDamage, new GaveDamageDelegate(OnGaveDamage));
		Character character3 = _character;
		character3.onGaveStatus = (Character.OnGaveStatusDelegate)Delegate.Combine(character3.onGaveStatus, new Character.OnGaveStatusDelegate(OnGaveStatus));
	}

	private void DetachEvents()
	{
		Character character = _character;
		character.onKilled = (Character.OnKilledDelegate)Delegate.Remove(character.onKilled, new Character.OnKilledDelegate(OnKilled));
		((PriorityList<GiveDamageDelegate>)_character.onGiveDamage).Remove((GiveDamageDelegate)OnGiveDamage);
		Character character2 = _character;
		character2.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(character2.onGaveDamage, new GaveDamageDelegate(OnGaveDamage));
		Character character3 = _character;
		character3.onGaveStatus = (Character.OnGaveStatusDelegate)Delegate.Remove(character3.onGaveStatus, new Character.OnGaveStatusDelegate(OnGaveStatus));
	}

	private void Disable()
	{
		if (!((Object)(object)_character == (Object)null))
		{
			DetachEvents();
			_operations.StopAll();
			((Component)_character).gameObject.SetActive(false);
		}
	}

	private void OnEnable()
	{
		Singleton<Service>.Instance.levelManager.onMapLoadedAndFadedIn += Disable;
	}

	private void OnDisable()
	{
		Disable();
		Singleton<Service>.Instance.levelManager.onMapLoadedAndFadedIn -= Disable;
	}

	protected override void OnUse()
	{
		if (!((Component)_character).gameObject.activeSelf)
		{
			((MonoBehaviour)_quintessence.owner).StartCoroutine(CUse());
		}
	}

	private IEnumerator CUse()
	{
		_character.stat.getDamageOverridingStat = _quintessence.owner.stat;
		AttachEvents();
		_character.ForceToLookAt(_quintessence.owner.desiringLookingDirection);
		((Component)_character).transform.position = ((Component)this).transform.position;
		((Component)_character).gameObject.SetActive(true);
		((MonoBehaviour)this).StartCoroutine(_operations.CRun(_quintessence.owner));
		_action.TryStart();
		while (_action.running)
		{
			yield return null;
		}
		Disable();
	}

	private void OnKilled(ITarget target, ref Damage damage)
	{
		_quintessence.owner.onKilled?.Invoke(target, ref damage);
	}

	private bool OnGiveDamage(ITarget target, ref Damage damage)
	{
		return _quintessence.owner.onGiveDamage.Invoke(target, ref damage);
	}

	private void OnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
	{
		_quintessence.owner.onGaveDamage?.Invoke(target, in originalDamage, in gaveDamage, damageDealt);
	}

	private void OnGaveStatus(Character target, CharacterStatus.ApplyInfo applyInfo, bool result)
	{
		_quintessence.owner.onGaveStatus?.Invoke(target, applyInfo, result);
	}

	private void OnDestroy()
	{
		if (!Service.quitting)
		{
			Object.Destroy((Object)(object)((Component)_character).gameObject);
		}
	}
}
