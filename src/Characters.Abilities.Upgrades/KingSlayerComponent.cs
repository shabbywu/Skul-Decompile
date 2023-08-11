using UnityEngine;

namespace Characters.Abilities.Upgrades;

public sealed class KingSlayerComponent : AbilityComponent<KingSlayer>
{
	[Range(0f, 100f)]
	[SerializeField]
	private int _triggerPercent;

	public int triggerPercent => _triggerPercent;
}
