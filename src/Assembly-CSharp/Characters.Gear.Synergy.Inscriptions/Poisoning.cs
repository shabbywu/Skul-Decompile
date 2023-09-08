using Characters.Abilities;
using Characters.Abilities.Upgrades;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions;

public sealed class Poisoning : SimpleStatBonusKeyword
{
	[SerializeField]
	private double[] _statBonusByStep = new double[4] { 0.0, 0.0, 0.5, 1.5 };

	[Header("1 Step 효과")]
	[SerializeField]
	private OperationByTriggerComponent _step1Ability;

	protected override double[] statBonusByStep => _statBonusByStep;

	protected override Stat.Category statCategory => Stat.Category.Constant;

	protected override Stat.Kind statKind => Stat.Kind.PoisonTickFrequency;

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
	}

	public override void Detach()
	{
		base.Detach();
		base.character.ability.Remove(_step1Ability.ability);
	}
}
