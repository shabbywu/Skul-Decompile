using Characters.Gear.Weapons;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions;

public class Mutation : SimpleStatBonusKeyword
{
	[SerializeField]
	[Header("2세트 효과")]
	private double[] _statBonusByStep;

	[Header("4세트 효과")]
	[SerializeField]
	private RarityPossibilities _statBonusByRarity;

	protected override double[] statBonusByStep => _statBonusByStep;

	protected override Stat.Category statCategory => Stat.Category.PercentPoint;

	protected override Stat.Kind statKind => Stat.Kind.SwapCooldownSpeed;

	public override void UpdateBonus(bool wasActive, bool wasOmen)
	{
		UpdateStat();
		if (keyword.step == _statBonusByStep.Length - 1 && !((PriorityList<GiveDamageDelegate>)base.character.onGiveDamage).Contains((GiveDamageDelegate)OnGiveDamage))
		{
			((PriorityList<GiveDamageDelegate>)base.character.onGiveDamage).Add(int.MaxValue, (GiveDamageDelegate)OnGiveDamage);
		}
		else
		{
			((PriorityList<GiveDamageDelegate>)base.character.onGiveDamage).Remove((GiveDamageDelegate)OnGiveDamage);
		}
	}

	private bool OnGiveDamage(ITarget target, ref Damage damage)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		if (damage.motionType != Damage.MotionType.Swap)
		{
			return false;
		}
		if ((Object)(object)base.character == (Object)null)
		{
			return false;
		}
		Weapon current = base.character.playerComponents.inventory.weapon.current;
		int num = _statBonusByRarity[current.rarity];
		damage.percentMultiplier *= 1f + (float)num * 0.01f;
		return false;
	}

	public override void Detach()
	{
		base.Detach();
		((PriorityList<GiveDamageDelegate>)base.character.onGiveDamage).Remove((GiveDamageDelegate)OnGiveDamage);
	}
}
