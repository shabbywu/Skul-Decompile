using System;
using Characters.Abilities.CharacterStat;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Customs;

public class GhoulConsumeStatAttacher : AbilityAttacher
{
	[Serializable]
	private class KeyMap
	{
		[Serializable]
		public class Reorderable : ReorderableArray<KeyMap>
		{
		}

		public string key;

		[UnityEditor.Subcomponent(typeof(StackableStatBonusComponent))]
		public StackableStatBonusComponent statBonus;
	}

	[SerializeField]
	private MotionTypeBoolArray _motionTypeFilter;

	[SerializeField]
	private AttackTypeBoolArray _attackTypeFilter;

	[SerializeField]
	private CharacterTypeBoolArray _characterTypeFilter = new CharacterTypeBoolArray(true, true, true, true, true, false, false, false);

	[Space]
	[SerializeField]
	private KeyMap.Reorderable _keyMaps;

	public override void OnIntialize()
	{
	}

	public override void StartAttach()
	{
		Character character = base.owner;
		character.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character.onGaveDamage, new GaveDamageDelegate(OnOwnerGaveDamage));
	}

	private void OnOwnerGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
	{
		if ((Object)(object)target.character == (Object)null || !_motionTypeFilter[gaveDamage.motionType] || !_attackTypeFilter[gaveDamage.attackType] || !_characterTypeFilter[target.character.type] || string.IsNullOrWhiteSpace(gaveDamage.key))
		{
			return;
		}
		KeyMap[] values = _keyMaps.values;
		foreach (KeyMap keyMap in values)
		{
			if (gaveDamage.key.Equals(keyMap.key, StringComparison.OrdinalIgnoreCase))
			{
				base.owner.ability.Add(keyMap.statBonus.ability);
				break;
			}
		}
	}

	public override void StopAttach()
	{
		if (!((Object)(object)base.owner == (Object)null))
		{
			Character character = base.owner;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(character.onGaveDamage, new GaveDamageDelegate(OnOwnerGaveDamage));
			KeyMap[] values = _keyMaps.values;
			foreach (KeyMap keyMap in values)
			{
				base.owner.ability.Remove(keyMap.statBonus.ability);
			}
		}
	}
}
