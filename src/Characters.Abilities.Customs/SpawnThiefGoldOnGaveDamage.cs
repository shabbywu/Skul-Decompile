using System;
using Data;
using Level;
using UnityEngine;

namespace Characters.Abilities.Customs;

[Serializable]
public class SpawnThiefGoldOnGaveDamage : Ability
{
	public class Instance : AbilityInstance<SpawnThiefGoldOnGaveDamage>
	{
		public Instance(Character owner, SpawnThiefGoldOnGaveDamage ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			Character character = owner;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character.onGaveDamage, new GaveDamageDelegate(OnCharacterGaveDamage));
		}

		protected override void OnDetach()
		{
			Character character = owner;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(character.onGaveDamage, new GaveDamageDelegate(OnCharacterGaveDamage));
		}

		private void OnCharacterGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
		{
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			if (!(gaveDamage.amount < ability._minDamage) && !((Object)(object)target.character == (Object)null) && ((EnumArray<Character.Type, bool>)ability._characterTypeFilter)[target.character.type] && ((EnumArray<Damage.MotionType, bool>)ability._motionTypeFilter)[gaveDamage.motionType] && ((EnumArray<Damage.AttackType, bool>)ability._attackTypeFilter)[gaveDamage.attackType])
			{
				SpawnGold(Vector2.op_Implicit(gaveDamage.hitPoint));
			}
		}

		private void SpawnGold(Vector3 position)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			CurrencyParticle component = ((Component)ability._thiefGold.Spawn(position, true)).GetComponent<CurrencyParticle>();
			component.currencyType = GameData.Currency.Type.Gold;
			component.currencyAmount = ability._goldAmount;
		}
	}

	[SerializeField]
	private PoolObject _thiefGold;

	[SerializeField]
	private int _goldAmount;

	[SerializeField]
	[Header("Filter")]
	private double _minDamage = 1.0;

	[SerializeField]
	private CharacterTypeBoolArray _characterTypeFilter = new CharacterTypeBoolArray(true, true, true, true, true, false, false, false);

	[SerializeField]
	private MotionTypeBoolArray _motionTypeFilter;

	[SerializeField]
	private AttackTypeBoolArray _attackTypeFilter;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
