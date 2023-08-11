using System;
using UnityEngine;

namespace Characters;

public struct Damage
{
	public enum AttackType
	{
		None,
		Melee,
		Ranged,
		Projectile,
		Additional
	}

	public enum MotionType
	{
		Basic,
		Skill,
		Item,
		Quintessence,
		Status,
		Dash,
		Swap,
		DarkAbility,
		None
	}

	public enum Attribute
	{
		Physical,
		Magic,
		Fixed
	}

	public static int evasionPriority = 0;

	public static int invulnerablePriority = 0;

	public static int cinematicPriority = int.MaxValue;

	public readonly Attacker attacker;

	public double @base;

	public Attribute attribute;

	public readonly AttackType attackType;

	public readonly MotionType motionType;

	public readonly string key;

	public bool @null;

	public bool canCritical;

	public bool critical;

	public double percentMultiplier;

	public double multiplier;

	public float stoppingPower;

	public PriorityList<bool> guaranteedCritical;

	public double criticalChance;

	public double criticalDamageMultiplier;

	public double criticalDamagePercentMultiplier;

	public double extraFixedDamage;

	public double ignoreDamageReduction;

	public short priority;

	public readonly Vector2 hitPoint;

	public double amount
	{
		get
		{
			if (attackType == AttackType.None)
			{
				return 0.0;
			}
			if (attribute == Attribute.Fixed)
			{
				return Math.Ceiling(@base);
			}
			double num = @base * multiplier * percentMultiplier;
			if (critical)
			{
				double num2 = criticalDamagePercentMultiplier * criticalDamageMultiplier;
				num *= num2;
			}
			num += extraFixedDamage;
			return Math.Ceiling(num);
		}
	}

	public Damage(Attacker attacker, double @base, Vector2 hitPoint, Attribute attribute, AttackType attackType, MotionType motionType, double multiplier = 1.0, float stoppingPower = 0f, double criticalChance = 0.0, double criticalDamageMultiplier = 1.0, double criticalDamagePercentMultiplier = 1.0, bool canCritical = true, bool @null = false, double extraFixedDamage = 0.0, double ignoreDamageReduction = 0.0, short priority = 0, PriorityList<bool> guaranteedCritical = null, double percentMultiplier = 1.0)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		this.attacker = attacker;
		this.@base = @base;
		this.multiplier = multiplier;
		this.attribute = attribute;
		this.attackType = attackType;
		this.motionType = motionType;
		key = string.Empty;
		this.hitPoint = hitPoint;
		critical = false;
		this.stoppingPower = stoppingPower;
		this.criticalChance = criticalChance;
		this.criticalDamageMultiplier = criticalDamageMultiplier;
		this.criticalDamagePercentMultiplier = criticalDamagePercentMultiplier;
		this.canCritical = canCritical;
		this.@null = @null;
		this.extraFixedDamage = extraFixedDamage;
		this.ignoreDamageReduction = ignoreDamageReduction;
		this.priority = priority;
		this.guaranteedCritical = guaranteedCritical;
		this.percentMultiplier = percentMultiplier;
	}

	public Damage(Attacker attacker, double @base, Vector2 hitPoint, Attribute attribute, AttackType attackType, MotionType motionType, string key, double multiplier = 1.0, float stoppingPower = 0f, double criticalChance = 0.0, double criticalDamageMultiplier = 1.0, double criticalDamagePercentMultiplier = 1.0, bool canCritical = true, bool @null = false, double extraFixedDamage = 0.0, double ignoreDamageReduction = 0.0, short priority = 0, PriorityList<bool> guaranteedCritical = null, double percentMultiplier = 1.0)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		this.attacker = attacker;
		this.@base = @base;
		this.multiplier = multiplier;
		this.attribute = attribute;
		this.attackType = attackType;
		this.motionType = motionType;
		this.key = key;
		this.hitPoint = hitPoint;
		critical = false;
		this.stoppingPower = stoppingPower;
		this.criticalChance = criticalChance;
		this.criticalDamageMultiplier = criticalDamageMultiplier;
		this.criticalDamagePercentMultiplier = criticalDamagePercentMultiplier;
		this.canCritical = canCritical;
		this.@null = @null;
		this.extraFixedDamage = extraFixedDamage;
		this.ignoreDamageReduction = ignoreDamageReduction;
		this.priority = priority;
		this.guaranteedCritical = guaranteedCritical;
		this.percentMultiplier = percentMultiplier;
	}

	public void SetGuaranteedCritical(int priority, bool critical)
	{
		if (guaranteedCritical == null)
		{
			guaranteedCritical = new PriorityList<bool>();
		}
		guaranteedCritical.Add(priority, critical);
	}

	public void Evaluate(bool immuneToCritical)
	{
		if (!immuneToCritical && motionType != MotionType.Item && motionType != MotionType.Quintessence && canCritical && !@null)
		{
			if (guaranteedCritical != null && guaranteedCritical.Count > 0)
			{
				critical = guaranteedCritical[0];
			}
			else
			{
				critical = MMMaths.Chance(criticalChance);
			}
		}
	}

	public override string ToString()
	{
		return amount.ToString("0");
	}
}
