using System;
using Characters.Operations;
using FX.BoundsAttackVisualEffect;
using UnityEngine;

namespace Characters.Abilities.Essences;

[Serializable]
public class Kiriz : Ability
{
	public class Instance : AbilityInstance<Kiriz>
	{
		private double _accumulatedDamage;

		public Instance(Character owner, Kiriz ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			_accumulatedDamage = 0.0;
			owner.chronometer.animation.AttachTimeScale(this, 0.03f);
			owner.health.onTakeDamage.Add(int.MinValue, OnTakeDamage);
		}

		protected override void OnDetach()
		{
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			owner.chronometer.animation.DetachTimeScale(this);
			owner.health.onTakeDamage.Remove(OnTakeDamage);
			if (!owner.liveAndActive)
			{
				return;
			}
			TargetStruct targetStruct = new TargetStruct(owner);
			ability._chrono.ApplyTo(owner);
			Bounds bounds = ((Collider2D)owner.collider).bounds;
			Vector2 hitPoint = MMMaths.RandomPointWithinBounds(bounds);
			Damage damage = owner.stat.GetDamage(_accumulatedDamage, hitPoint, ability._hitInfo);
			if (owner.cinematic.value)
			{
				ability._effect.Spawn(owner, bounds, in damage, targetStruct);
				return;
			}
			ability._attacker.TryAttackCharacter(targetStruct, ref damage);
			if (damage.amount > 0.0)
			{
				ability._effect.Spawn(owner, bounds, in damage, targetStruct);
			}
		}

		private bool OnTakeDamage(ref Damage damage)
		{
			_accumulatedDamage += damage.amount;
			return true;
		}
	}

	private Character _attacker;

	[Header("Attack")]
	[SerializeField]
	private HitInfo _hitInfo;

	[SerializeField]
	private ChronoInfo _chrono;

	[BoundsAttackVisualEffect.Subcomponent]
	[SerializeField]
	private BoundsAttackVisualEffect.Subcomponents _effect;

	public void SetAttacker(Character attacker)
	{
		_attacker = attacker;
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
