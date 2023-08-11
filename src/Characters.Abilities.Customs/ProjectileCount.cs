using System;
using Characters.Projectiles;
using UnityEngine;

namespace Characters.Abilities.Customs;

[Serializable]
public sealed class ProjectileCount : Ability
{
	public sealed class Instance : AbilityInstance<ProjectileCount>
	{
		private int _totalCache;

		public override Sprite icon
		{
			get
			{
				int num = 0;
				Projectile[] projectilesToCount = ability._projectilesToCount;
				foreach (Projectile projectile in projectilesToCount)
				{
					num += projectile.reusable.spawnedCount;
				}
				if (num <= 0)
				{
					return null;
				}
				return ability.defaultIcon;
			}
		}

		public override int iconStacks
		{
			get
			{
				int num = 0;
				Projectile[] projectilesToCount = ability._projectilesToCount;
				foreach (Projectile projectile in projectilesToCount)
				{
					num += projectile.reusable.spawnedCount;
				}
				_totalCache = num;
				return num;
			}
		}

		public Instance(Character owner, ProjectileCount ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
		}

		protected override void OnDetach()
		{
		}
	}

	[SerializeField]
	private Projectile[] _projectilesToCount;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
