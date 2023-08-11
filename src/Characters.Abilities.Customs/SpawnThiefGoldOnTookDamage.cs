using System;
using Data;
using Level;
using UnityEngine;

namespace Characters.Abilities.Customs;

[Serializable]
public class SpawnThiefGoldOnTookDamage : Ability
{
	public class Instance : AbilityInstance<SpawnThiefGoldOnTookDamage>
	{
		public Instance(Character owner, SpawnThiefGoldOnTookDamage ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			if (((EnumArray<Character.Type, bool>)ability._characterTypeFilter)[owner.type])
			{
				owner.health.onTookDamage += OnCharacterTookDamage;
			}
		}

		protected override void OnDetach()
		{
			if (((EnumArray<Character.Type, bool>)ability._characterTypeFilter)[owner.type])
			{
				owner.health.onTookDamage -= OnCharacterTookDamage;
			}
		}

		private void OnCharacterTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			if (!(tookDamage.amount < ability._minDamage) && ((EnumArray<Damage.MotionType, bool>)ability._motionTypeFilter)[tookDamage.motionType] && ((EnumArray<Damage.AttackType, bool>)ability._attackTypeFilter)[tookDamage.attackType])
			{
				SpawnGold(Vector2.op_Implicit(tookDamage.hitPoint));
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
