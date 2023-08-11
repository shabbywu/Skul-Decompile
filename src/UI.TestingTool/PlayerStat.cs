using Characters;
using Services;
using Singletons;
using UnityEngine;

namespace UI.TestingTool;

public sealed class PlayerStat : MonoBehaviour
{
	[SerializeField]
	private PlayerStatElement _statElement;

	[SerializeField]
	private Transform _parent;

	private Character _player;

	private Stat.Kind[] _statList = new Stat.Kind[13]
	{
		Stat.Kind.Health,
		Stat.Kind.TakingDamage,
		Stat.Kind.PhysicalAttackDamage,
		Stat.Kind.MagicAttackDamage,
		Stat.Kind.BasicAttackSpeed,
		Stat.Kind.SkillAttackSpeed,
		Stat.Kind.MovementSpeed,
		Stat.Kind.ChargingSpeed,
		Stat.Kind.SkillCooldownSpeed,
		Stat.Kind.SwapCooldownSpeed,
		Stat.Kind.EssenceCooldownSpeed,
		Stat.Kind.CriticalChance,
		Stat.Kind.CriticalDamage
	};

	private void Awake()
	{
		_player = Singleton<Service>.Instance.levelManager.player;
		Stat.Kind[] statList = _statList;
		foreach (Stat.Kind kind in statList)
		{
			Object.Instantiate<PlayerStatElement>(_statElement, _parent).Set(kind);
		}
	}
}
