using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions;

public sealed class ManaCycle : SimpleStatBonusKeyword
{
	[SerializeField]
	[Header("2세트 효과")]
	private double[] _statBonusByStep = new double[3] { 0.0, 0.4000000059604645, 0.4000000059604645 };

	[Header("4세트 효과 (Percent)")]
	[SerializeField]
	private float _skillAttackMultiplier = 1.3f;

	protected override double[] statBonusByStep => _statBonusByStep;

	protected override Stat.Category statCategory => Stat.Category.PercentPoint;

	protected override Stat.Kind statKind => Stat.Kind.SkillCooldownSpeed;

	public override void Attach()
	{
		base.Attach();
		((PriorityList<GiveDamageDelegate>)base.character.onGiveDamage).Add(int.MaxValue, (GiveDamageDelegate)OnGiveDamage);
	}

	public override void Detach()
	{
		base.Detach();
		((PriorityList<GiveDamageDelegate>)base.character.onGiveDamage).Remove((GiveDamageDelegate)OnGiveDamage);
	}

	public override void UpdateBonus(bool wasActive, bool wasOmen)
	{
		UpdateStat();
	}

	private bool OnGiveDamage(ITarget target, ref Damage damage)
	{
		if (!keyword.isMaxStep)
		{
			return false;
		}
		if (damage.motionType != Damage.MotionType.Skill)
		{
			return false;
		}
		damage.percentMultiplier *= _skillAttackMultiplier;
		return false;
	}
}
