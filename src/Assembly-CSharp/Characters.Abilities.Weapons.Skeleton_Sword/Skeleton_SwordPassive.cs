using System;
using UnityEngine;

namespace Characters.Abilities.Weapons.Skeleton_Sword;

[Serializable]
public class Skeleton_SwordPassive : Ability
{
	public class Instance : AbilityInstance<Skeleton_SwordPassive>
	{
		private float _remainBonusTime;

		public override float iconFillAmount => 1f - _remainBonusTime / ability._bonusDuration;

		public override Sprite icon
		{
			get
			{
				if (!(_remainBonusTime > 0f))
				{
					return null;
				}
				return base.icon;
			}
		}

		public Instance(Character owner, Skeleton_SwordPassive ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			Character character = owner;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character.onGaveDamage, new GaveDamageDelegate(OnGaveDamage));
			_remainBonusTime = ability._bonusDuration;
		}

		protected override void OnDetach()
		{
			Character character = owner;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(character.onGaveDamage, new GaveDamageDelegate(OnGaveDamage));
		}

		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
			_remainBonusTime -= deltaTime;
		}

		private void OnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
		{
			if (!((Object)(object)target.character == (Object)null) && !target.character.health.dead && ability._motionTypeFilter[gaveDamage.motionType] && ability._damageTypeFilter[gaveDamage.attackType] && MMMaths.PercentChance((_remainBonusTime < 0f) ? ability._chance : ability._chanceWithBonus))
			{
				owner.GiveStatus(target.character, ability._status);
			}
		}
	}

	[Tooltip("기본 상처 부여 확률")]
	[SerializeField]
	[Range(0f, 100f)]
	private int _chance;

	[Tooltip("교대 보너스 지속 시간")]
	[SerializeField]
	private float _bonusDuration;

	[SerializeField]
	[Tooltip("교대 보너스가 있을 때 상처 부여 확률")]
	[Range(0f, 100f)]
	private int _chanceWithBonus;

	[SerializeField]
	private MotionTypeBoolArray _motionTypeFilter;

	[SerializeField]
	private AttackTypeBoolArray _damageTypeFilter;

	[SerializeField]
	private CharacterStatus.ApplyInfo _status;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
