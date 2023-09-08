using System;
using Characters.Gear.Quintessences;
using FX;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Essences;

[Serializable]
public class Dwarf : Ability
{
	public class Instance : AbilityInstance<Dwarf>
	{
		public override int iconStacks => ability.attackCount;

		public Instance(Character owner, Dwarf ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			Character character = ability._dwarfEssence.owner;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character.onGaveDamage, new GaveDamageDelegate(OnGaveDamage));
		}

		protected override void OnDetach()
		{
			Character character = ability._dwarfEssence.owner;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(character.onGaveDamage, new GaveDamageDelegate(OnGaveDamage));
		}

		private void OnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
		{
			if (gaveDamage.motionType == Damage.MotionType.Quintessence && !((Object)(object)target.character == (Object)null) && target.character.type != Character.Type.Dummy && gaveDamage.key.Equals(ability._attackKey, StringComparison.OrdinalIgnoreCase))
			{
				ability.AddAttackCount(owner);
			}
		}
	}

	[SerializeField]
	private int _attackCountToPromote;

	[SerializeField]
	private Quintessence _dwarfEssence;

	[SerializeField]
	private Quintessence _essenceToPromote;

	[SerializeField]
	private EffectInfo _awakeningEffect;

	[SerializeField]
	private SoundInfo _awakeningSound;

	public string _attackKey;

	public int attackCount { get; set; }

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}

	public void AddAttackCount(Character owner)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		attackCount++;
		if (attackCount >= _attackCountToPromote)
		{
			_dwarfEssence.ChangeOnInventory(_essenceToPromote.Instantiate());
			_awakeningEffect.Spawn(((Component)owner).transform.position, owner);
			PersistentSingleton<SoundManager>.Instance.PlaySound(_awakeningSound, ((Component)owner).transform.position);
		}
	}
}
