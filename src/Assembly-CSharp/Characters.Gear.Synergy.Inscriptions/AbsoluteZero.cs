using Characters.Abilities;
using Characters.Abilities.Upgrades;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions;

public class AbsoluteZero : SimpleStatBonusKeyword
{
	[SerializeField]
	private double[] _statBonusByStep;

	[Header("1세트 효과")]
	[SerializeField]
	private OperationByTriggerComponent _step1Ability;

	[AbilityComponent.Subcomponent]
	[SerializeField]
	[Header("3세트 효과")]
	private AbilityComponent _abilityComponent;

	protected override double[] statBonusByStep => _statBonusByStep;

	protected override Stat.Category statCategory => Stat.Category.Constant;

	protected override Stat.Kind statKind => Stat.Kind.FreezeDuration;

	protected override void Initialize()
	{
		base.Initialize();
		_step1Ability.Initialize();
	}

	public override void UpdateBonus(bool wasActive, bool wasOmen)
	{
		UpdateStat();
		if (keyword.step >= 1)
		{
			if (!base.character.ability.Contains(_step1Ability.ability) && base.character.ability.GetInstance<KettleOfSwampWitch>() == null)
			{
				base.character.ability.Add(_step1Ability.ability);
			}
		}
		else
		{
			base.character.ability.Remove(_step1Ability.ability);
		}
		if (keyword.isMaxStep)
		{
			base.character.status.freezeMaxHitStack = 3;
		}
		else
		{
			base.character.status.freezeMaxHitStack = 1;
		}
	}

	private void AttachAbility(Character attacker, Character target)
	{
		if (!keyword.isMaxStep)
		{
			target.ability.Add(_abilityComponent.ability);
		}
	}

	public override void Attach()
	{
		base.character.status.Register(CharacterStatus.Kind.Freeze, CharacterStatus.Timing.Release, AttachAbility);
		base.character.status.Register(CharacterStatus.Kind.Freeze, CharacterStatus.Timing.Refresh, AttachAbility);
	}

	public override void Detach()
	{
		base.character.ability.Remove(_statBonus);
		base.character.ability.Remove(_step1Ability.ability);
		base.character.status.freezeMaxHitStack = 1;
		base.character.status.Unregister(CharacterStatus.Kind.Freeze, CharacterStatus.Timing.Release, AttachAbility);
		base.character.status.Unregister(CharacterStatus.Kind.Freeze, CharacterStatus.Timing.Refresh, AttachAbility);
	}
}
