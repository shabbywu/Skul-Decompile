using Characters.Abilities;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions;

public sealed class Heritage : SimpleStatBonusKeyword
{
	[Header("1세트 효과")]
	[SerializeField]
	private double[] _statBonusByStep = new double[3] { 0.0, 0.4000000059604645, 0.4000000059604645 };

	[Header("3세트 효과 (Percent)")]
	[AbilityComponent.Subcomponent]
	[SerializeField]
	private AbilityComponent _step2ability;

	protected override double[] statBonusByStep => _statBonusByStep;

	protected override Stat.Category statCategory => Stat.Category.PercentPoint;

	protected override Stat.Kind statKind => Stat.Kind.EssenceCooldownSpeed;

	protected override void Initialize()
	{
		base.Initialize();
		_step2ability.Initialize();
	}

	public override void Attach()
	{
		base.Attach();
	}

	public override void Detach()
	{
		base.Detach();
		base.character.ability.Remove(_step2ability.ability);
	}

	public override void UpdateBonus(bool wasActive, bool wasOmen)
	{
		UpdateStat();
		if (keyword.isMaxStep)
		{
			base.character.ability.Add(_step2ability.ability);
		}
		else
		{
			base.character.ability.Remove(_step2ability.ability);
		}
	}
}
