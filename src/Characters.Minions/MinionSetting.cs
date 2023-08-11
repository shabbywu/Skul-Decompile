using UnityEngine;

namespace Characters.Minions;

[CreateAssetMenu(fileName = "MinionSetting", menuName = "ScriptableObjects/MinionSetting", order = 1)]
public sealed class MinionSetting : ScriptableObject
{
	public int maxCount = int.MaxValue;

	public float lifeTime = float.MaxValue;

	[Space]
	public bool despawnOnMapChanged = true;

	public bool despawnOnSwap;

	public bool despawnOnWeaponDropped;

	public bool despawnOnEssenceChanged;

	[Space]
	public bool triggerOnKilled = true;

	public bool triggerOnGiveDamage;

	public bool triggerOnGaveDamage;

	public bool triggerOnGaveStatus;
}
