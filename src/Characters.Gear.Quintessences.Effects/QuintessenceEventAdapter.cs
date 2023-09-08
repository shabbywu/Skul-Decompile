using System;
using Services;
using UnityEngine;

namespace Characters.Gear.Quintessences.Effects;

public sealed class QuintessenceEventAdapter : MonoBehaviour
{
	[SerializeField]
	private Quintessence _quintessence;

	[SerializeField]
	private Character _quintessenceCharacter;

	private void Awake()
	{
		Connect();
	}

	private void OnDestroy()
	{
		if (!Service.quitting && !((Object)(object)_quintessence == (Object)null))
		{
			Disconnect();
		}
	}

	private void Connect()
	{
		_quintessence.onEquipped += AttachEvents;
		_quintessence.onDropped += DetachEvents;
		_quintessence.onDiscard += DetachEvents;
	}

	private void Disconnect()
	{
		_quintessence.onEquipped -= AttachEvents;
		_quintessence.onDropped -= DetachEvents;
		_quintessence.onDiscard -= DetachEvents;
	}

	private void AttachEvents()
	{
		Character quintessenceCharacter = _quintessenceCharacter;
		quintessenceCharacter.onKilled = (Character.OnKilledDelegate)Delegate.Combine(quintessenceCharacter.onKilled, new Character.OnKilledDelegate(OnKilled));
		_quintessenceCharacter.onGiveDamage.Add(int.MinValue, OnGiveDamage);
		Character quintessenceCharacter2 = _quintessenceCharacter;
		quintessenceCharacter2.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(quintessenceCharacter2.onGaveDamage, new GaveDamageDelegate(OnGaveDamage));
		Character quintessenceCharacter3 = _quintessenceCharacter;
		quintessenceCharacter3.onGaveStatus = (Character.OnGaveStatusDelegate)Delegate.Combine(quintessenceCharacter3.onGaveStatus, new Character.OnGaveStatusDelegate(OnGaveStatus));
	}

	private void DetachEvents(Gear gear)
	{
		DetachEvents();
	}

	private void DetachEvents()
	{
		Character quintessenceCharacter = _quintessenceCharacter;
		quintessenceCharacter.onKilled = (Character.OnKilledDelegate)Delegate.Remove(quintessenceCharacter.onKilled, new Character.OnKilledDelegate(OnKilled));
		_quintessenceCharacter.onGiveDamage.Remove(OnGiveDamage);
		Character quintessenceCharacter2 = _quintessenceCharacter;
		quintessenceCharacter2.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(quintessenceCharacter2.onGaveDamage, new GaveDamageDelegate(OnGaveDamage));
		Character quintessenceCharacter3 = _quintessenceCharacter;
		quintessenceCharacter3.onGaveStatus = (Character.OnGaveStatusDelegate)Delegate.Remove(quintessenceCharacter3.onGaveStatus, new Character.OnGaveStatusDelegate(OnGaveStatus));
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
