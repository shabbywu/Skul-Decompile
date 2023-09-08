using Characters.Abilities;
using Platforms;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions;

public sealed class Omen : SimpleStatBonusKeyword
{
	[SerializeField]
	private double[] _statBonusByStep;

	[Header("1세트 효과")]
	[AbilityComponent.Subcomponent]
	[SerializeField]
	private AbilityComponent _firstAbilityComponent;

	[SerializeField]
	[AbilityComponent.Subcomponent]
	[Header("3세트 효과")]
	private AbilityComponent _maxAbilityComponent;

	protected override double[] statBonusByStep => _statBonusByStep;

	protected override Stat.Category statCategory => Stat.Category.Percent;

	protected override Stat.Kind statKind => Stat.Kind.Health;

	protected override void Initialize()
	{
		base.Initialize();
		_firstAbilityComponent.Initialize();
		_maxAbilityComponent.Initialize();
	}

	public override void UpdateBonus(bool wasActive, bool wasOmen)
	{
		UpdateStat();
		if (keyword.step >= 1)
		{
			if (!base.character.ability.Contains(_firstAbilityComponent.ability))
			{
				base.character.ability.Add(_firstAbilityComponent.ability);
			}
		}
		else
		{
			base.character.ability.Remove(_firstAbilityComponent.ability);
		}
		if (keyword.isMaxStep)
		{
			if (!base.character.ability.Contains(_maxAbilityComponent.ability))
			{
				base.character.ability.Add(_maxAbilityComponent.ability);
			}
		}
		else
		{
			base.character.ability.Remove(_maxAbilityComponent.ability);
		}
	}

	public override void Attach()
	{
		base.Attach();
		ExtensionMethods.Set((Type)68);
	}

	public override void Detach()
	{
		base.Detach();
		base.character.ability.Remove(_firstAbilityComponent.ability);
		base.character.ability.Remove(_maxAbilityComponent.ability);
	}
}
