using System;
using Characters.Gear.Weapons.Gauges;
using Characters.Movements;
using UnityEngine;

namespace Characters.Abilities;

[Serializable]
public sealed class ModifyDamageByAirTIme : Ability
{
	public sealed class Instance : AbilityInstance<ModifyDamageByAirTIme>
	{
		private const float _updateInterval = 0.25f;

		private float _remainUpdateTime;

		private float _remainBuffTime;

		private bool _wasGrounded;

		private float _airTime;

		private float _cachedMultiplier;

		public override Sprite icon => null;

		public override float iconFillAmount => 1f - _airTime / ability._timeToMaxDamageMultiplier;

		public Instance(Character owner, ModifyDamageByAirTIme ability)
			: base(owner, ability)
		{
			if ((Object)(object)ability._gauge != (Object)null)
			{
				ability._gauge.maxValue = ability._timeToMaxDamageMultiplier;
			}
		}

		protected override void OnAttach()
		{
			_wasGrounded = owner.movement.controller.isGrounded;
			owner.movement.onJump += OnJump;
			owner.movement.onGrounded += OnGrounded;
			((PriorityList<GiveDamageDelegate>)owner.onGiveDamage).Add(0, (GiveDamageDelegate)OnGiveDamage);
		}

		private bool OnGiveDamage(ITarget target, ref Damage damage)
		{
			if (!((EnumArray<Damage.MotionType, bool>)ability._motionType)[damage.motionType] || !((EnumArray<Damage.AttackType, bool>)ability._attachkType)[damage.attackType])
			{
				return false;
			}
			if (!damage.key.Equals(ability._attackKey, StringComparison.OrdinalIgnoreCase))
			{
				return false;
			}
			damage.multiplier *= _cachedMultiplier;
			return false;
		}

		protected override void OnDetach()
		{
			owner.movement.onJump -= OnJump;
			owner.movement.onGrounded -= OnGrounded;
			((PriorityList<GiveDamageDelegate>)owner.onGiveDamage).Remove((GiveDamageDelegate)OnGiveDamage);
			if ((Object)(object)ability._gauge != (Object)null)
			{
				ability._gauge.Set(0f);
			}
		}

		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
			_remainUpdateTime -= deltaTime;
			if (_wasGrounded)
			{
				_remainBuffTime -= deltaTime;
			}
			else
			{
				_airTime += deltaTime;
			}
			if (_airTime > ability._timeToMaxDamageMultiplier)
			{
				_airTime = ability._timeToMaxDamageMultiplier;
			}
			if (_remainUpdateTime < 0f)
			{
				_remainUpdateTime = 0.25f;
				UpdateStat();
			}
		}

		public void UpdateStat()
		{
			float num = 0f;
			if (_remainBuffTime > 0f)
			{
				num = ability._maxDamageMultiplier * _airTime / ability._timeToMaxDamageMultiplier;
			}
			if (num == _cachedMultiplier)
			{
				return;
			}
			if ((Object)(object)ability._gauge != (Object)null)
			{
				if (!_wasGrounded)
				{
					ability._gauge.Set(_airTime);
				}
				else
				{
					ability._gauge.Set(0f);
				}
			}
			_cachedMultiplier = num;
		}

		private void OnJump(Movement.JumpType jumpType, float jumpHeight)
		{
			if (_wasGrounded)
			{
				_wasGrounded = false;
				_airTime = 0f;
			}
			_remainBuffTime = float.PositiveInfinity;
			UpdateStat();
		}

		private void OnGrounded()
		{
			_wasGrounded = true;
			_remainBuffTime = ability._remainTimeOnGround;
		}
	}

	[SerializeField]
	private ValueGauge _gauge;

	[SerializeField]
	private MotionTypeBoolArray _motionType;

	[SerializeField]
	private AttackTypeBoolArray _attachkType;

	[SerializeField]
	private string _attackKey;

	[Header("= DamageMultiplier * MaxMultiplier * (AireTime / MaxTime)")]
	[Space(5f)]
	[SerializeField]
	private float _maxDamageMultiplier = 3f;

	[SerializeField]
	private float _timeToMaxDamageMultiplier;

	[Tooltip("바닥에 착지할 경우 이 시간 후에 버프가 사라짐")]
	[SerializeField]
	private float _remainTimeOnGround = 1f;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
