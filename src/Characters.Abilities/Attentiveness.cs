using System;
using Characters.Operations;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Abilities;

[Serializable]
public class Attentiveness : Ability
{
	public class Instance : AbilityInstance<Attentiveness>
	{
		private int _count;

		private int _maxcount = 1;

		public Instance(Character owner, Attentiveness ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			base.remainTime = ability.duration;
			((ChronometerBase)owner.chronometer.animation).AttachTimeScale((object)this, 0f);
			owner.stat.AttachValues(ability._stat);
			owner.health.onTookDamage += OnTookDamage;
			_count = 0;
		}

		private void OnTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			if (((EnumArray<Damage.AttackType, bool>)ability._types)[originalDamage.attackType])
			{
				_count++;
				if (_count >= _maxcount)
				{
					owner.health.onTookDamage -= OnTookDamage;
					TakeDamage();
					owner.ability.Remove(this);
				}
			}
		}

		protected override void OnDetach()
		{
			owner.health.onTookDamage -= OnTookDamage;
			owner.stat.DetachValues(ability._stat);
			((ChronometerBase)owner.chronometer.animation).DetachTimeScale((object)this);
		}

		public override void Refresh()
		{
			_count = 0;
			base.remainTime = ability.duration;
		}

		private void TakeDamage()
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			Character player = Singleton<Service>.Instance.levelManager.player;
			Damage damage = player.stat.GetDamage(ability.attackDamage, Vector2.op_Implicit(((Component)owner).transform.position), ability.hitInfo);
			player.Attack(owner, ref damage);
		}
	}

	[SerializeField]
	private Stat.Values _stat;

	[SerializeField]
	private AttackTypeBoolArray _types;

	[SerializeField]
	private float attackDamage;

	[SerializeField]
	private HitInfo hitInfo;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
