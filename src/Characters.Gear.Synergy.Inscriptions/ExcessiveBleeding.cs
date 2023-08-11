using Characters.Abilities;
using Characters.Abilities.Upgrades;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions;

public class ExcessiveBleeding : SimpleStatBonusKeyword
{
	[SerializeField]
	private double[] _statBonusByLevel;

	[Header("1세트 효과")]
	[SerializeField]
	private OperationByTriggerComponent _step1Ability;

	protected override double[] statBonusByStep => _statBonusByLevel;

	protected override Stat.Category statCategory => Stat.Category.Percent;

	protected override Stat.Kind statKind => Stat.Kind.BleedDamage;

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
			base.character.status.canBleedCritical = true;
		}
		else
		{
			base.character.status.canBleedCritical = false;
		}
	}

	public override void Detach()
	{
		base.Detach();
		base.character.ability.Remove(_step1Ability.ability);
		base.character.status.wound.critical = false;
	}
}
