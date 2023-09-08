using Hardmode.Darktech;
using Singletons;
using UnityEngine;

namespace Characters.Gear.Upgrades;

public sealed class AdamantiumSkeleton : UpgradeAbility
{
	[SerializeField]
	[Range(0f, 100f)]
	private int _statValueMultiplier;

	public override void Attach(Character target)
	{
		Singleton<DarktechManager>.Instance.setting.건강보조장치스탯증폭량 += _statValueMultiplier;
	}

	public override void Detach()
	{
		Singleton<DarktechManager>.Instance.setting.건강보조장치스탯증폭량 -= _statValueMultiplier;
	}
}
