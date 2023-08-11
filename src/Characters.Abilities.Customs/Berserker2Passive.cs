using System;
using Characters.Gear.Weapons;
using Characters.Gear.Weapons.Gauges;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Customs;

[Serializable]
public class Berserker2Passive : Ability
{
	public class Instance : AbilityInstance<Berserker2Passive>
	{
		public Instance(Character owner, Berserker2Passive ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			Character character = owner;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character.onGaveDamage, new GaveDamageDelegate(OnOwnerGaveDamage));
			owner.health.onTookDamage += OnOwnerTookDamage;
		}

		protected override void OnDetach()
		{
			Character character = owner;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(character.onGaveDamage, new GaveDamageDelegate(OnOwnerGaveDamage));
			owner.health.onTookDamage -= OnOwnerTookDamage;
		}

		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
			if (!owner.playerComponents.combatDetector.inCombat)
			{
				ability._gauge.Add((0f - ability._losingGaugeAmountPerSecond) * deltaTime);
			}
			CheckGaugeAndPolymorph();
		}

		private void CheckGaugeAndPolymorph()
		{
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			if (!(ability._gauge.currentValue < ability._gauge.maxValue) && (!((Object)(object)owner.motion != (Object)null) || !owner.motion.running))
			{
				ability._gauge.Clear();
				double amount = owner.health.currentHealth * (double)ability._losingHealthPercentOnPolymorph * 0.01;
				owner.health.TakeHealth(amount);
				Singleton<Service>.Instance.floatingTextSpawner.SpawnPlayerTakingDamage(amount, Vector2.op_Implicit(((Component)owner).transform.position));
				owner.playerComponents.inventory.weapon.Polymorph(ability._polymorphWeapon);
			}
		}

		private void OnOwnerGaveDamage(ITarget target, in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			if (!((Object)(object)target.character == (Object)null) && ((EnumArray<Damage.AttackType, bool>)ability._attackTypeFilter)[tookDamage.attackType] && ((EnumArray<Damage.MotionType, bool>)ability._motionTypeFilter)[tookDamage.motionType])
			{
				ability._gauge.Add(ability._gettingGaugeAmountByGaveDamage);
			}
		}

		private void OnOwnerTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			if (((EnumArray<Damage.AttackType, bool>)ability._attackTypeFilter)[tookDamage.attackType] && ((EnumArray<Damage.MotionType, bool>)ability._motionTypeFilter)[tookDamage.motionType])
			{
				ability._gauge.Add(ability._gettingGaugeAmountByTookDamage);
			}
		}
	}

	[Range(0f, 99f)]
	[SerializeField]
	[Space]
	private int _losingHealthPercentOnPolymorph;

	[SerializeField]
	[Header("Gauge")]
	private ValueGauge _gauge;

	[SerializeField]
	[Space]
	private float _gettingGaugeAmountByGaveDamage;

	[SerializeField]
	private float _gettingGaugeAmountByTookDamage;

	[SerializeField]
	[Space]
	private float _losingGaugeAmountPerSecond;

	[SerializeField]
	[Header("Filter")]
	private AttackTypeBoolArray _attackTypeFilter;

	[SerializeField]
	private MotionTypeBoolArray _motionTypeFilter;

	[NonSerialized]
	public Weapon _polymorphWeapon;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
