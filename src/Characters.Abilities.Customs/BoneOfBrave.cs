using System;
using Characters.Gear.Weapons;
using Characters.Player;
using FX;
using UnityEngine;

namespace Characters.Abilities.Customs;

[Serializable]
public class BoneOfBrave : Ability
{
	public class Instance : AbilityInstance<BoneOfBrave>
	{
		private WeaponInventory _weaponInventory;

		private float _remainCooldownTime;

		private float _remainAbilityTime;

		private int _powerHeadCount;

		private bool _canUse;

		private bool _attached;

		public override float iconFillAmount => _remainCooldownTime / ability._cooldownTime;

		public Instance(Character owner, BoneOfBrave ability)
			: base(owner, ability)
		{
			_weaponInventory = owner.playerComponents.inventory.weapon;
			UpdatePowerHeadCount();
		}

		protected override void OnAttach()
		{
			owner.onGiveDamage.Add(0, OnOwnerGiveDamage);
			_weaponInventory.onChanged += OnWeaponChanged;
		}

		protected override void OnDetach()
		{
			owner.onGiveDamage.Remove(OnOwnerGiveDamage);
			_weaponInventory.onChanged -= OnWeaponChanged;
		}

		public override void UpdateTime(float deltaTime)
		{
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			base.UpdateTime(deltaTime);
			_remainCooldownTime -= deltaTime;
			if (_attached)
			{
				_remainAbilityTime -= deltaTime;
				if (_remainAbilityTime <= 0f)
				{
					_attached = false;
				}
			}
			if (!_canUse && _remainCooldownTime < 0f)
			{
				ability._effectOnCanUse.Spawn(((Component)owner).transform.position, owner);
				_canUse = true;
			}
		}

		private bool OnOwnerGiveDamage(ITarget target, ref Damage damage)
		{
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			if (!ability._motionTypeFilter[damage.motionType])
			{
				return false;
			}
			if (!ability._damageTypeFilter[damage.attackType])
			{
				return false;
			}
			if (!ability._attributeFilter[damage.attribute])
			{
				return false;
			}
			if (_attached)
			{
				if (_remainAbilityTime < 0f)
				{
					return false;
				}
			}
			else
			{
				if (_remainCooldownTime > 0f)
				{
					return false;
				}
				ability._effectOnStart.Spawn(((Component)owner).transform.position, owner);
				_attached = true;
				_canUse = false;
				_remainAbilityTime = ability._abilityTime;
				_remainCooldownTime = ability._cooldownTime;
			}
			damage.percentMultiplier *= ability.damagePercents[_powerHeadCount];
			return false;
		}

		private void OnWeaponChanged(Weapon old, Weapon @new)
		{
			UpdatePowerHeadCount();
		}

		private void UpdatePowerHeadCount()
		{
			_powerHeadCount = _weaponInventory.GetCountByCategory(Weapon.Category.Power);
		}
	}

	[SerializeField]
	private MotionTypeBoolArray _motionTypeFilter;

	[SerializeField]
	private AttackTypeBoolArray _damageTypeFilter;

	[SerializeField]
	private DamageAttributeBoolArray _attributeFilter;

	[SerializeField]
	private EffectInfo _effectOnCanUse;

	[SerializeField]
	private EffectInfo _effectOnStart;

	[SerializeField]
	private float _abilityTime;

	[SerializeField]
	private float _cooldownTime;

	[SerializeField]
	[Tooltip("파워 타입 스컬 개수가 0개일 때 피해량 증폭")]
	private double _damagePercent0;

	[Tooltip("파워 타입 스컬 개수가 1개일 때 피해량 증폭")]
	[SerializeField]
	private double _damagePercent1;

	[Tooltip("파워 타입 스컬 개수가 2개일 때 피해량 증폭")]
	[SerializeField]
	private double _damagePercent2;

	private double[] damagePercents;

	public override void Initialize()
	{
		base.Initialize();
		damagePercents = new double[3] { _damagePercent0, _damagePercent1, _damagePercent2 };
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
