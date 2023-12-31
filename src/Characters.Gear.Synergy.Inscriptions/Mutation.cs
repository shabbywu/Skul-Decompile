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
		if (keyword.step == _statBonusByStep.Length - 1 && !base.character.onGiveDamage.Contains(OnGiveDamage))
		{
			base.character.onGiveDamage.Add(int.MaxValue, OnGiveDamage);
		}
		else
		{
			base.character.onGiveDamage.Remove(OnGiveDamage);
		}
	}

	private bool OnGiveDamage(ITarget target, ref Damage damage)
	{
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
		base.character.onGiveDamage.Remove(OnGiveDamage);
	}
}
