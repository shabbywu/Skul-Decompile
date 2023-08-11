using UnityEngine;

namespace Characters.Abilities.Customs;

public sealed class CharacterAliveAbilityAttacher : AbilityAttacher
{
	[SerializeField]
	private Character _target;

	[SerializeField]
	[AbilityComponent.Subcomponent]
	private AbilityComponent _abilityComponent;

	public override void OnIntialize()
	{
		_abilityComponent.Initialize();
	}

	public override void StartAttach()
	{
		if (!_target.health.dead)
		{
			base.owner.ability.Add(_abilityComponent.ability);
			_target.health.onDiedTryCatch += StopAttach;
		}
	}

	public override void StopAttach()
	{
		base.owner.ability.Remove(_abilityComponent.ability);
	}
}
