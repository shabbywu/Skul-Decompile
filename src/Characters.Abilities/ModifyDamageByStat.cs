using System;
using UnityEngine;

namespace Characters.Abilities;

[Serializable]
public sealed class ModifyDamageByStat : Ability
{
	public class Instance : AbilityInstance<ModifyDamageByStat>
	{
		private float _remainCooldownTime;

		public override float iconFillAmount
		{
			get
			{
				if (ability._cooldownTime != 0f)
				{
					return _remainCooldownTime / ability._cooldownTime;
				}
				return base.iconFillAmount;
			}
		}

		internal Instance(Character owner, ModifyDamageByStat ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			((PriorityList<GiveDamageDelegate>)owner.onGiveDamage).Add(0, (GiveDamageDelegate)OnGiveDamage);
		}

		protected override void OnDetach()
		{
			((PriorityList<GiveDamageDelegate>)owner.onGiveDamage).Remove((GiveDamageDelegate)OnGiveDamage);
		}

		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
			_remainCooldownTime -= deltaTime;
		}

		private bool OnGiveDamage(ITarget target, ref Damage damage)
		{
			if (_remainCooldownTime > 0f)
			{
				return false;
			}
			if ((Object)(object)target.character != (Object)null && !((EnumArray<Character.Type, bool>)ability._characterTypes)[target.character.type])
			{
				return false;
			}
			if (!((EnumArray<Damage.MotionType, bool>)ability._attackTypes)[damage.motionType])
			{
				return false;
			}
			if (!((EnumArray<Damage.AttackType, bool>)ability._damageTypes)[damage.attackType])
			{
				return false;
			}
			if (!string.IsNullOrWhiteSpace(ability._attackKey) && !damage.key.Equals(ability._attackKey, StringComparison.OrdinalIgnoreCase))
			{
				return false;
			}
			double final = owner.stat.GetFinal(Stat.Kind.values[ability._sourceStat.kindIndex]);
			float num = Mathf.Min(ability._maxDamagePercent, (float)(final - 1.0) * ability._damagePercentByStatUnit);
			float num2 = Mathf.Min(ability._maxDamagePercentPoint, (float)(final - 1.0) * ability._damagePercentPointByStatUnit);
			damage.percentMultiplier *= 1f + num;
			damage.multiplier += num2;
			_remainCooldownTime = ability._cooldownTime;
			return false;
		}
	}

	[Header("발동 조건")]
	[Tooltip("비어있지 않을 경우, 해당 키를 가진 공격에만 발동됨")]
	[SerializeField]
	private string _attackKey;

	[SerializeField]
	private CharacterTypeBoolArray _characterTypes;

	[SerializeField]
	private MotionTypeBoolArray _attackTypes;

	[SerializeField]
	private AttackTypeBoolArray _damageTypes;

	[SerializeField]
	[Header("데미지 변화 설정")]
	private Stat.Value _sourceStat;

	[SerializeField]
	private float _cooldownTime;

	[Header("스텟 입력 1 -> 0.01%p")]
	[SerializeField]
	private float _damagePercentByStatUnit;

	[SerializeField]
	private float _damagePercentPointByStatUnit;

	[SerializeField]
	private float _maxDamagePercent = 10f;

	[SerializeField]
	private float _maxDamagePercentPoint;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
