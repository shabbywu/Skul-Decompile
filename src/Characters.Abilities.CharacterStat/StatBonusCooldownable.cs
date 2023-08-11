using System;
using Characters.Gear.Synergy.Inscriptions;
using UnityEngine;

namespace Characters.Abilities.CharacterStat;

[Serializable]
public sealed class StatBonusCooldownable : CooldownAbility
{
	public sealed class Instance : CooldownAbilityInstance
	{
		private StatBonusCooldownable _ability;

		public Instance(Character owner, StatBonusCooldownable ability)
			: base(owner, ability)
		{
			_ability = ability;
		}

		protected override void OnAttach()
		{
		}

		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
			if (_remainCooldownTime < 0f)
			{
				OnAttachBuff();
			}
		}

		protected override void OnAttachBuff()
		{
			base.OnAttachBuff();
			owner.stat.AttachValues(_ability._stat);
		}

		protected override void OnDetachBuff()
		{
			base.OnDetachBuff();
			owner.stat.DetachValues(_ability._stat);
		}

		protected override void OnDetach()
		{
			OnDetachBuff();
		}
	}

	[SerializeField]
	private Stat.Values _stat;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
