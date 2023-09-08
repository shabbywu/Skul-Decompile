using Characters;
using GameResources;
using Services;
using Singletons;
using TMPro;
using UnityEngine;

namespace UI.Inventory;

public sealed class StatOption : MonoBehaviour
{
	[Header("Title")]
	[SerializeField]
	private TMP_Text _titleLabel;

	[Header("Labels")]
	[SerializeField]
	private TMP_Text _healthLabel;

	[SerializeField]
	private TMP_Text _takeDamageLabel;

	[SerializeField]
	private TMP_Text _physicalDamageLabel;

	[SerializeField]
	private TMP_Text _magicalDamageLabel;

	[SerializeField]
	private TMP_Text _attackSpeedLabel;

	[SerializeField]
	private TMP_Text _movementSpeedLabel;

	[SerializeField]
	private TMP_Text _chargingSpeedLabel;

	[SerializeField]
	private TMP_Text _skillCooldownLabel;

	[SerializeField]
	private TMP_Text _swapCooldownLabel;

	[SerializeField]
	private TMP_Text _essenceCooldownLabel;

	[SerializeField]
	private TMP_Text _criticalChanceLabel;

	[SerializeField]
	private TMP_Text _criticalDamageLabel;

	[Header("Values")]
	[SerializeField]
	private StatValue _health;

	[SerializeField]
	private StatValue _takingDamage;

	[SerializeField]
	private StatValue _physicalDamage;

	[SerializeField]
	private StatValue _magicalDamage;

	[SerializeField]
	private StatValue _attackSpeed;

	[SerializeField]
	private StatValue _movementSpeed;

	[SerializeField]
	private StatValue _chargingSpeed;

	[SerializeField]
	private StatValue _skillCooldown;

	[SerializeField]
	private StatValue _swapCooldown;

	[SerializeField]
	private StatValue _essenceCooldown;

	[SerializeField]
	private StatValue _criticalChance;

	[SerializeField]
	private StatValue _criticalDamage;

	public string titleText => Localization.GetLocalizedString("stat/title");

	public string healthText => Localization.GetLocalizedString("stat/health/name");

	public string takingDamageText => Localization.GetLocalizedString("stat/takingDamage/name");

	public string physicalAttackDamageText => Localization.GetLocalizedString("stat/physicalAttackDamage/name");

	public string magicalAttackDamageText => Localization.GetLocalizedString("stat/magicalAttackDamage/name");

	public string attackSpeedText => Localization.GetLocalizedString("stat/attackSpeed/name");

	public string movementSpeedText => Localization.GetLocalizedString("stat/movementSpeed/name");

	public string chargingSpeedText => Localization.GetLocalizedString("stat/chargingSpeed/name");

	public string skillCooldownSpeedText => Localization.GetLocalizedString("stat/skillCooldownSpeed/name");

	public string swapCooldownSpeedText => Localization.GetLocalizedString("stat/swapCooldownSpeed/name");

	public string essenceCooldownSpeedText => Localization.GetLocalizedString("stat/essenceCooldownSpeed/name");

	public string criticalChanceText => Localization.GetLocalizedString("stat/criticalChance/name");

	public string criticalDamageText => Localization.GetLocalizedString("stat/criticalDamage/name");

	private void OnEnable()
	{
		Character player = Singleton<Service>.Instance.levelManager.player;
		if (!((Object)(object)player == (Object)null))
		{
			_titleLabel.text = titleText;
			_healthLabel.text = healthText ?? "";
			_takeDamageLabel.text = takingDamageText ?? "";
			_physicalDamageLabel.text = physicalAttackDamageText ?? "";
			_magicalDamageLabel.text = magicalAttackDamageText ?? "";
			_attackSpeedLabel.text = attackSpeedText ?? "";
			_movementSpeedLabel.text = movementSpeedText ?? "";
			_chargingSpeedLabel.text = chargingSpeedText ?? "";
			_skillCooldownLabel.text = skillCooldownSpeedText ?? "";
			_swapCooldownLabel.text = swapCooldownSpeedText ?? "";
			_essenceCooldownLabel.text = essenceCooldownSpeedText ?? "";
			_criticalChanceLabel.text = criticalChanceText ?? "";
			_criticalDamageLabel.text = criticalDamageText ?? "";
			Stat stat = player.stat;
			double final = stat.GetFinal(Stat.Kind.Health);
			double final2 = stat.GetFinal(Stat.Kind.TakingDamage);
			_health.Set($"{final}");
			_takingDamage.Set($"x{final2:0.00}", positive: false);
			double final3 = player.stat.GetFinal(Stat.Kind.PhysicalAttackDamage);
			double final4 = player.stat.GetFinal(Stat.Kind.MagicAttackDamage);
			_physicalDamage.Set($"{final3 * 100.0:0}", positive: false, "%");
			_magicalDamage.Set($"{final4 * 100.0:0}", positive: false, "%");
			double final5 = stat.GetFinal(Stat.Kind.BasicAttackSpeed);
			double num = stat.GetFinal(Stat.Kind.MovementSpeed) / 5.0;
			double final6 = stat.GetFinal(Stat.Kind.ChargingSpeed);
			_attackSpeed.Set($"{final5 * 100.0:0}", positive: false, "%");
			_movementSpeed.Set($"{num * 100.0:0}", positive: false, "%");
			_chargingSpeed.Set($"{final6 * 100.0:0}", positive: false, "%");
			double final7 = stat.GetFinal(Stat.Kind.SkillCooldownSpeed);
			double final8 = stat.GetFinal(Stat.Kind.SwapCooldownSpeed);
			double final9 = stat.GetFinal(Stat.Kind.EssenceCooldownSpeed);
			_skillCooldown.Set($"{final7 * 100.0:0}", positive: false, "%");
			_swapCooldown.Set($"{final8 * 100.0:0}", positive: false, "%");
			_essenceCooldown.Set($"{final9 * 100.0:0}", positive: false, "%");
			double num2 = stat.GetFinal(Stat.Kind.CriticalChance) - 1.0;
			double final10 = stat.GetFinal(Stat.Kind.CriticalDamage);
			_criticalChance.Set($"{num2 * 100.0:0}", num2 >= 0.0, "%");
			_criticalDamage.Set($"x{final10:0.00}", positive: false);
		}
	}
}
