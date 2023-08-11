using System;
using System.Collections;
using Characters.Actions;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Gear.Quintessences;

public class RunAction : UseQuintessence
{
	[SerializeField]
	private Character _character;

	[SerializeField]
	private Characters.Actions.Action _action;

	[SerializeField]
	private Transform _flipObject;

	protected override void Awake()
	{
		base.Awake();
		((Component)_character).gameObject.SetActive(false);
		((Component)_character).transform.parent = null;
	}

	private void OnEnable()
	{
		Singleton<Service>.Instance.levelManager.onMapLoaded += Disable;
	}

	private void OnDisable()
	{
		DetachEvents();
		Singleton<Service>.Instance.levelManager.onMapLoaded -= Disable;
	}

	private void Disable()
	{
		DetachEvents();
		((Component)_character).gameObject.SetActive(false);
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

	protected override void OnUse()
	{
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		if (((Component)_character).gameObject.activeSelf)
		{
			return;
		}
		if ((Object)(object)_flipObject != (Object)null)
		{
			if (_quintessence.owner.lookingDirection == Character.LookingDirection.Right)
			{
				_flipObject.localScale = Vector2.op_Implicit(new Vector2(1f, 1f));
			}
			else
			{
				_flipObject.localScale = Vector2.op_Implicit(new Vector2(-1f, 1f));
			}
		}
		((MonoBehaviour)_quintessence.owner).StartCoroutine(CUse());
	}

	private IEnumerator CUse()
	{
		_character.stat.getDamageOverridingStat = _quintessence.owner.stat;
		AttachEvents();
		_character.ForceToLookAt(_quintessence.owner.desiringLookingDirection);
		((Component)_character).transform.position = ((Component)this).transform.position;
		((Component)_character).gameObject.SetActive(true);
		_action.TryStart();
		while (_action.running)
		{
			yield return null;
		}
		Disable();
	}

	private void OnDestroy()
	{
		if (!Service.quitting)
		{
			Object.Destroy((Object)(object)((Component)_character).gameObject);
		}
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
}
