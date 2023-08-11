using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Characters;

[Serializable]
public class CharacterSynchronization
{
	[SerializeField]
	private bool _posistion = true;

	[SerializeField]
	private bool _lookingDirection = true;

	[SerializeField]
	private bool _overrideDamageStat;

	[SerializeField]
	private bool _copyDamageStat = true;

	[SerializeField]
	private bool _copyAttackSpeedStat;

	[SerializeField]
	private Stat.Values _statsToCopy;

	private Stat.Values _attachedStats;

	public void Synchronize(Character source, Character target)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		if (_lookingDirection)
		{
			source.ForceToLookAt(target.lookingDirection);
		}
		if (_posistion)
		{
			((Component)source).transform.position = ((Component)target).transform.position;
		}
		if (_overrideDamageStat)
		{
			source.stat.getDamageOverridingStat = target.stat;
		}
		if (_attachedStats != null)
		{
			source.stat.DetachValues(_attachedStats);
		}
		source.stat.adaptiveAttribute = target.stat.adaptiveAttribute;
		List<Stat.Value> list = ((((ReorderableArray<Stat.Value>)_statsToCopy).values.Length == 0) ? new List<Stat.Value>() : ((ReorderableArray<Stat.Value>)_statsToCopy).values.ToList());
		if (_copyDamageStat)
		{
			list.Add(new Stat.Value(Stat.Category.Percent, Stat.Kind.AttackDamage, 0.0));
			list.Add(new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.AttackDamage, 0.0));
			list.Add(new Stat.Value(Stat.Category.Percent, Stat.Kind.BasicAttackDamage, 0.0));
			list.Add(new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.BasicAttackDamage, 0.0));
			list.Add(new Stat.Value(Stat.Category.Percent, Stat.Kind.SkillAttackDamage, 0.0));
			list.Add(new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.SkillAttackDamage, 0.0));
			list.Add(new Stat.Value(Stat.Category.Percent, Stat.Kind.PhysicalAttackDamage, 0.0));
			list.Add(new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, 0.0));
			list.Add(new Stat.Value(Stat.Category.Percent, Stat.Kind.MagicAttackDamage, 0.0));
			list.Add(new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 0.0));
			list.Add(new Stat.Value(Stat.Category.Percent, Stat.Kind.ProjectileAttackDamage, 0.0));
			list.Add(new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.ProjectileAttackDamage, 0.0));
			list.Add(new Stat.Value(Stat.Category.Percent, Stat.Kind.CriticalDamage, 0.0));
			list.Add(new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.CriticalDamage, 0.0));
			list.Add(new Stat.Value(Stat.Category.Percent, Stat.Kind.CriticalChance, 0.0));
			list.Add(new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.CriticalChance, 0.0));
		}
		if (_copyAttackSpeedStat)
		{
			list.Add(new Stat.Value(Stat.Category.Percent, Stat.Kind.AttackSpeed, 0.0));
			list.Add(new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.AttackSpeed, 0.0));
			list.Add(new Stat.Value(Stat.Category.Percent, Stat.Kind.BasicAttackSpeed, 0.0));
			list.Add(new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.BasicAttackSpeed, 0.0));
			list.Add(new Stat.Value(Stat.Category.Percent, Stat.Kind.SkillAttackSpeed, 0.0));
			list.Add(new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.SkillAttackSpeed, 0.0));
		}
		_attachedStats = new Stat.Values(list.ToArray());
		if (((ReorderableArray<Stat.Value>)_attachedStats).values.Length != 0)
		{
			for (int i = 0; i < ((ReorderableArray<Stat.Value>)_attachedStats).values.Length; i++)
			{
				Stat.Value value = ((ReorderableArray<Stat.Value>)_attachedStats).values[i];
				value.value = target.stat.Get(value.categoryIndex, value.kindIndex);
			}
			source.stat.AttachOrUpdateValues(_attachedStats);
			source.stat.SetNeedUpdate();
		}
	}
}
