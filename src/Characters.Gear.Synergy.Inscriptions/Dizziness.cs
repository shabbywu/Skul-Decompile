using Characters.Abilities;
using Characters.Abilities.Upgrades;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions;

public class Dizziness : SimpleStatBonusKeyword
{
	[SerializeField]
	private double[] _statBonusByStep = new double[4] { 0.0, 0.0, 1.0, 1.0 };

	[SerializeField]
	[Header("1 Step 효과")]
	private OperationByTriggerComponent _step1Ability;

	[SerializeField]
	[Tooltip("Percent 효과")]
	[Header("3 Step 효과")]
	private float _attackDamageMultiplier = 0.3f;

	protected override double[] statBonusByStep => _statBonusByStep;

	protected override Stat.Category statCategory => Stat.Category.Constant;

	protected override Stat.Kind statKind => Stat.Kind.StunDuration;

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

	private bool OnGiveDamage(ITarget target, ref Damage damage)
	{
		if (keyword.step < keyword.steps.Count - 1)
		{
			return false;
		}
		if ((Object)(object)target.character == (Object)null)
		{
			return false;
		}
		if ((Object)(object)target.character.status == (Object)null)
		{
			return false;
		}
		if (!target.character.status.stuned)
		{
			return false;
		}
		damage.percentMultiplier *= _attackDamageMultiplier;
		return false;
	}

	public override void Attach()
	{
		base.Attach();
		((PriorityList<GiveDamageDelegate>)base.character.onGiveDamage).Add(int.MaxValue, (GiveDamageDelegate)OnGiveDamage);
	}

	public override void Detach()
	{
		base.Detach();
		base.character.ability.Remove(_statBonus);
		base.character.ability.Remove(_step1Ability.ability);
		((PriorityList<GiveDamageDelegate>)base.character.onGiveDamage).Remove((GiveDamageDelegate)OnGiveDamage);
	}
}
