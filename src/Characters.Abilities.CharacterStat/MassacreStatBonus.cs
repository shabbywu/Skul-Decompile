using System;
using UnityEngine;

namespace Characters.Abilities.CharacterStat;

[Serializable]
public sealed class MassacreStatBonus : Ability
{
	public class Instance : AbilityInstance<MassacreStatBonus>
	{
		public Instance(Character owner, MassacreStatBonus ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			owner.stat.AttachValues(ability._statValues);
			Character character = owner;
			character.onKilled = (Character.OnKilledDelegate)Delegate.Combine(character.onKilled, new Character.OnKilledDelegate(HandleOnKilled));
		}

		private void HandleOnKilled(ITarget target, ref Damage damage)
		{
			Character character = target.character;
			if (!((Object)(object)character == (Object)null) && ability._targetFilter[character.type])
			{
				base.remainTime += ability._recoverTime;
			}
		}

		protected override void OnDetach()
		{
			owner.stat.DetachValues(ability._statValues);
			Character character = owner;
			character.onKilled = (Character.OnKilledDelegate)Delegate.Remove(character.onKilled, new Character.OnKilledDelegate(HandleOnKilled));
		}
	}

	[SerializeField]
	private Stat.Values _statValues;

	[SerializeField]
	private CharacterTypeBoolArray _targetFilter;

	[SerializeField]
	private float _recoverTime;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
