using System;
using Characters.Abilities;
using UnityEngine;

namespace Characters.Gear.Quintessences.Effects;

public sealed class AttachAbilityToTargetOnGaveStatus : QuintessenceEffect
{
	[SerializeField]
	[AbilityComponent.Subcomponent]
	private AbilityComponent _abilityComponent;

	[SerializeField]
	private CharacterStatusKindBoolArray _statusKind;

	[SerializeField]
	private Character _character;

	private Quintessence _quintessence;

	private Character _owner;

	private void Awake()
	{
		_abilityComponent.Initialize();
	}

	protected override void OnInvoke(Quintessence quintessence)
	{
		Character owner = quintessence.owner;
		_quintessence = quintessence;
		owner.onGaveStatus = (Character.OnGaveStatusDelegate)Delegate.Combine(owner.onGaveStatus, new Character.OnGaveStatusDelegate(OnGaveStatus));
		if ((Object)(object)owner != (Object)(object)_owner)
		{
			quintessence.onDiscard += Detach;
		}
		if ((Object)(object)_character != (Object)null)
		{
			Character character = _character;
			character.onGaveStatus = (Character.OnGaveStatusDelegate)Delegate.Combine(character.onGaveStatus, new Character.OnGaveStatusDelegate(OnGaveStatus));
		}
		_owner = owner;
	}

	private void Detach(Gear gear)
	{
		if ((Object)(object)_character != (Object)null)
		{
			Character character = _character;
			character.onGaveStatus = (Character.OnGaveStatusDelegate)Delegate.Remove(character.onGaveStatus, new Character.OnGaveStatusDelegate(OnGaveStatus));
		}
		Character owner = _owner;
		owner.onGaveStatus = (Character.OnGaveStatusDelegate)Delegate.Remove(owner.onGaveStatus, new Character.OnGaveStatusDelegate(OnGaveStatus));
		_quintessence.onDiscard -= Detach;
	}

	private void OnGaveStatus(Character target, CharacterStatus.ApplyInfo applyInfo, bool result)
	{
		if (_statusKind[applyInfo.kind] && result)
		{
			target.ability.Add(_abilityComponent.ability);
		}
	}
}
