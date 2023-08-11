using Characters.Abilities;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions;

public sealed class Chase : SimpleStatBonusKeyword
{
	[SerializeField]
	[Header("2세트 효과")]
	private double[] _statBonusByStep = new double[3] { 0.0, 0.20000000298023224, 0.20000000298023224 };

	[SerializeField]
	[Header("4세트 효과")]
	private AbilityComponent _maxStepAbilityComponent;

	protected override double[] statBonusByStep => _statBonusByStep;

	protected override Stat.Category statCategory => Stat.Category.PercentPoint;

	protected override Stat.Kind statKind => Stat.Kind.DashCooldownSpeed;

	protected override void Initialize()
	{
		base.Initialize();
		_maxStepAbilityComponent.Initialize();
	}

	public override void UpdateBonus(bool wasActive, bool wasOmen)
	{
		UpdateStat();
		if (keyword.step == _statBonusByStep.Length - 1)
		{
			if (!base.character.ability.Contains(_maxStepAbilityComponent.ability))
			{
				base.character.ability.Add(_maxStepAbilityComponent.ability);
			}
		}
		else
		{
			base.character.ability.Remove(_maxStepAbilityComponent.ability);
		}
	}

	public override void Detach()
	{
		base.Detach();
		base.character.ability.Remove(_maxStepAbilityComponent.ability);
	}
}
