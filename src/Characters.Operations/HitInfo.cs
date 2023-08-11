using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Characters.Operations;

[Serializable]
public class HitInfo
{
	[SerializeField]
	private Damage.Attribute _attribute;

	[SerializeField]
	private Damage.AttackType _type;

	[FormerlySerializedAs("_attackType")]
	[SerializeField]
	private Damage.MotionType _motionType;

	[SerializeField]
	private float _damageMultiplier = 1f;

	[SerializeField]
	private short _priority;

	[SerializeField]
	private float _stoppingPower;

	[SerializeField]
	private float _extraCriticalChance;

	[SerializeField]
	private float _extraCriticalDamage;

	[SerializeField]
	private string _key;

	public Damage.Attribute attribute => _attribute;

	public Damage.AttackType attackType => _type;

	public Damage.MotionType motionType => _motionType;

	public float damageMultiplier
	{
		get
		{
			return _damageMultiplier;
		}
		set
		{
			_damageMultiplier = value;
		}
	}

	public short priority => _priority;

	public float stoppingPower => _stoppingPower;

	public float extraCriticalChance => _extraCriticalChance;

	public float extraCriticalDamage => _extraCriticalDamage;

	public string key => _key;

	public HitInfo(Damage.AttackType type)
	{
		_type = type;
	}

	public void ChangeAdaptiveDamageAttribute(Character owner)
	{
		_attribute = owner.stat.GetAdaptiveForceAttribute();
	}
}
