using System;
using Characters.Operations;
using UnityEngine;

namespace Characters.Abilities;

[Serializable]
public sealed class DamageAttributeChange : Ability
{
	public sealed class Instance : AbilityInstance<DamageAttributeChange>
	{
		public Instance(Character owner, DamageAttributeChange ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			Stat stat = owner.stat;
			stat.IsChangeAttribute = (Func<HitInfo, bool>)Delegate.Combine(stat.IsChangeAttribute, new Func<HitInfo, bool>(CheckMotion));
		}

		private bool CheckMotion(HitInfo hitInfo)
		{
			if (!((EnumArray<Damage.MotionType, bool>)ability._motionType)[hitInfo.motionType])
			{
				return false;
			}
			return true;
		}

		protected override void OnDetach()
		{
			Stat stat = owner.stat;
			stat.IsChangeAttribute = (Func<HitInfo, bool>)Delegate.Remove(stat.IsChangeAttribute, new Func<HitInfo, bool>(CheckMotion));
		}
	}

	[SerializeField]
	private MotionTypeBoolArray _motionType;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
