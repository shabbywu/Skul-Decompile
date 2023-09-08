using System;
using UnityEngine;

namespace Characters.Abilities;

[Serializable]
public class ApplyStatusOnGaveDamage : Ability
{
	public class Instance : AbilityInstance<ApplyStatusOnGaveDamage>
	{
		private float _remainTime;

		public override float iconFillAmount
		{
			get
			{
				if (ability._cooldownTime != 0f)
				{
					return _remainTime / ability._cooldownTime;
				}
				return 0f;
			}
		}

		internal Instance(Character owner, ApplyStatusOnGaveDamage ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			Character character = owner;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character.onGaveDamage, new GaveDamageDelegate(OnGaveDamage));
		}

		protected override void OnDetach()
		{
			Character character = owner;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(character.onGaveDamage, new GaveDamageDelegate(OnGaveDamage));
		}

		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
			_remainTime -= deltaTime;
		}

		private void OnGaveDamage(ITarget target, in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			if (!(_remainTime > 0f) && !((Object)(object)target.character == (Object)null) && !((Object)(object)target.character == (Object)(object)owner) && (!ability._onCritical || tookDamage.critical) && ability._attackTypes[tookDamage.motionType] && ability._types[tookDamage.attackType] && MMMaths.PercentChance(ability._chance) && (string.IsNullOrWhiteSpace(ability._attackKey) || tookDamage.key.Equals(ability._attackKey, StringComparison.OrdinalIgnoreCase)) && owner.GiveStatus(target.character, ability._status))
			{
				_remainTime = ability._cooldownTime;
			}
		}
	}

	[Serializable]
	private class AttackTypeBoolArray : EnumArray<Damage.MotionType, bool>
	{
	}

	[Serializable]
	private class DamageTypeBoolArray : EnumArray<Damage.AttackType, bool>
	{
	}

	[Tooltip("default는 0초")]
	[SerializeField]
	private float _cooldownTime;

	[SerializeField]
	private CharacterStatus.ApplyInfo _status;

	[Range(1f, 100f)]
	[SerializeField]
	private int _chance = 100;

	[SerializeField]
	private bool _onCritical;

	[SerializeField]
	[Tooltip("비어있지 않을 경우, 해당 키를 가진 공격에만 발동됨")]
	private string _attackKey;

	[SerializeField]
	private AttackTypeBoolArray _attackTypes;

	[SerializeField]
	private DamageTypeBoolArray _types;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
