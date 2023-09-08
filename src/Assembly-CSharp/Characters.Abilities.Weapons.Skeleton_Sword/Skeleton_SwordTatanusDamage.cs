using System;
using Characters.Operations;
using FX;
using UnityEngine;

namespace Characters.Abilities.Weapons.Skeleton_Sword;

[Serializable]
public class Skeleton_SwordTatanusDamage : Ability
{
	public class Instance : AbilityInstance<Skeleton_SwordTatanusDamage>
	{
		private double _remainTimeToNextTick;

		public Instance(Character owner, Skeleton_SwordTatanusDamage ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			_remainTimeToNextTick = ability._delay;
			base.remainTime += ability._delay;
			if (ability._stat.values.Length != 0)
			{
				owner.stat.AttachValues(ability._stat);
			}
		}

		protected override void OnDetach()
		{
			if (ability._stat.values.Length != 0)
			{
				owner.stat.DetachValues(ability._stat);
			}
		}

		public override void UpdateTime(float deltaTime)
		{
			base.remainTime -= deltaTime;
			_remainTimeToNextTick -= deltaTime;
			if (_remainTimeToNextTick < 0.0)
			{
				_remainTimeToNextTick += ability._damageCycleTime;
				GiveDamage();
			}
		}

		private void GiveDamage()
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			if (!((Object)(object)ability.attacker == (Object)null) && !((Object)(object)owner == (Object)null))
			{
				Vector2 val = MMMaths.RandomPointWithinBounds(((Collider2D)owner.collider).bounds);
				Damage damage = ability.attacker.stat.GetDamage(ability._damage, val, ability._hitInfo);
				ability.attacker.Attack(owner, ref damage);
				ability._hitEffect.Spawn(Vector2.op_Implicit(val), (float)Random.Range(0, 360), 1f);
			}
		}
	}

	[SerializeField]
	private float _delay;

	[SerializeField]
	private int _damage;

	[SerializeField]
	private float _damageCycleTime;

	[SerializeField]
	private HitInfo _hitInfo = new HitInfo(Damage.AttackType.Additional);

	[SerializeField]
	private EffectInfo _hitEffect;

	[Space]
	[SerializeField]
	private Stat.Values _stat;

	public Character attacker { get; set; }

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
