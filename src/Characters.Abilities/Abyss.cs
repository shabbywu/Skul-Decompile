using System;
using Characters.Gear;
using Characters.Gear.Weapons;
using Characters.Operations;
using FX;
using UnityEngine;

namespace Characters.Abilities;

[Serializable]
public class Abyss : Ability
{
	public class Instance : AbilityInstance<Abyss>
	{
		private Weapon _from;

		private float _remainHitTime;

		public Instance(Character owner, Abyss ability, Weapon from)
			: base(owner, ability)
		{
			_from = from;
		}

		protected override void OnAttach()
		{
			base.remainTime = ability.duration;
			_remainHitTime = ability._hitInterval;
			owner.stat.AttachValues(ability._stat);
		}

		protected override void OnDetach()
		{
			owner.stat.DetachValues(ability._stat);
		}

		public override void Refresh()
		{
			base.remainTime = ability.duration;
		}

		public override void UpdateTime(float deltaTime)
		{
			if ((Object)(object)_from == (Object)null || _from.state != Characters.Gear.Gear.State.Equipped)
			{
				base.remainTime = 0f;
				return;
			}
			base.remainTime -= deltaTime;
			_remainHitTime -= deltaTime;
			if (_remainHitTime < 0f)
			{
				_remainHitTime += ability._hitInterval;
				Hit();
			}
		}

		private void Hit()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			Vector2 val = MMMaths.RandomPointWithinBounds(((Collider2D)owner.collider).bounds);
			ability._hitEffect.Spawn(Vector2.op_Implicit(val));
			if (!owner.cinematic.value)
			{
				Damage damage = owner.stat.GetDamage(ability._attackDamage.amount, val, ability._hitInfo);
				_from.owner.Attack(owner, ref damage);
				ability._operationOnHit.Run(_from.owner, owner);
			}
		}
	}

	[SerializeField]
	private Weapon _weapon;

	[SerializeField]
	private Stat.Values _stat;

	[SerializeField]
	private float _hitInterval;

	[SerializeField]
	private AttackDamage _attackDamage;

	[SerializeField]
	private HitInfo _hitInfo = new HitInfo(Damage.AttackType.Additional);

	[SerializeField]
	private EffectInfo _hitEffect;

	[CharacterOperation.Subcomponent]
	[SerializeField]
	private CharacterOperation.Subcomponents _operationOnHit;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this, _weapon);
	}
}
