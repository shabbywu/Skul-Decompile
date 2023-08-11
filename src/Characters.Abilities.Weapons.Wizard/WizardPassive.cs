using System;
using Characters.Actions;
using Characters.Gear.Weapons;
using Characters.Gear.Weapons.Gauges;
using UnityEngine;

namespace Characters.Abilities.Weapons.Wizard;

[Serializable]
public sealed class WizardPassive : Ability
{
	public class Instance : AbilityInstance<WizardPassive>
	{
		private float _finalManaChargingSpeed;

		public override Sprite icon
		{
			get
			{
				if (!((Object)(object)ability._transcendenceAction == (Object)null))
				{
					return base.icon;
				}
				return null;
			}
		}

		public override float iconFillAmount
		{
			get
			{
				if ((Object)(object)ability._transcendenceAction == (Object)null)
				{
					return 0f;
				}
				if (ability._transcendenceAction.cooldown.canUse)
				{
					return 0f;
				}
				return ability._transcendenceAction.cooldown.time.remainTime / ability._transcendenceAction.cooldown.time.cooldownTime;
			}
		}

		public Instance(Character owner, WizardPassive ability)
			: base(owner, ability)
		{
		}

		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
			if (!((Object)(object)owner.playerComponents.inventory.weapon.polymorphWeapon != (Object)null) || !((Object)(object)ability._polymorphWeapon != (Object)null) || !((Object)owner.playerComponents.inventory.weapon.polymorphWeapon).name.Equals(((Object)ability._polymorphWeapon).name))
			{
				float num = (owner.stat.GetSkillCooldownSpeed() - 1f) * 100f * ability._manaChargingSpeedBonusBySkillCooldown;
				_finalManaChargingSpeed = ability._baseManaChargingSpeed * ability.manaChargingSpeedMultiplier + num;
				if (ability.정신집중영향받기)
				{
					_finalManaChargingSpeed *= (float)owner.stat.GetFinal(Stat.Kind.ChargingSpeed);
				}
				float amount = _finalManaChargingSpeed * deltaTime;
				ability._gauge.Add(amount);
			}
		}

		protected override void OnAttach()
		{
		}

		protected override void OnDetach()
		{
		}

		public void AddGauge(float value)
		{
			ability._gauge.Add(value);
		}

		public void FillUp()
		{
			ability._gauge.FillUp();
		}
	}

	[Header("For Test")]
	[SerializeField]
	private bool 정신집중영향받기;

	[SerializeField]
	private bool 초월상태;

	[Header("마나 관련")]
	[SerializeField]
	private ValueGauge _gauge;

	[Tooltip("초당 마나 획득량")]
	[SerializeField]
	private float _baseManaChargingSpeed;

	[SerializeField]
	private float _maxManaChargingSpeed;

	[Tooltip("스킬 쿨다운 속도 1%p당 추가 증가량")]
	[SerializeField]
	private float _manaChargingSpeedBonusBySkillCooldown;

	[Header("레전더리 초월")]
	[SerializeField]
	private Characters.Actions.Action _transcendenceAction;

	[SerializeField]
	private Weapon _polymorphWeapon;

	[NonSerialized]
	public float manaChargingSpeedMultiplier = 1f;

	[NonSerialized]
	public bool transcendence;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}

	public bool IsMaxGauge()
	{
		return _gauge.maxValue <= _gauge.currentValue;
	}

	public bool TryReduceMana(float value)
	{
		if (transcendence || 초월상태)
		{
			return false;
		}
		_gauge.Consume(value);
		return true;
	}
}
