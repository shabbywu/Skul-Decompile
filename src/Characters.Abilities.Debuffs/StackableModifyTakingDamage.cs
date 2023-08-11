using System;
using UnityEngine;

namespace Characters.Abilities.Debuffs;

[Serializable]
public sealed class StackableModifyTakingDamage : Ability
{
	public class Instance : AbilityInstance<StackableModifyTakingDamage>
	{
		private float _remainCooldownTime;

		private float _remainRefreshCooldownTime;

		private int _remainCount;

		public Instance(Character owner, StackableModifyTakingDamage ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			ability._stack = 1;
			if (ability._applyCount == 0)
			{
				_remainCount = int.MaxValue;
			}
			else
			{
				_remainCount = ability._applyCount;
			}
			owner.health.onTakeDamage.Add(0, (TakeDamageDelegate)HandleOnTakeDamage);
			_remainRefreshCooldownTime = ability._refreshCooldownTime;
		}

		public override void Refresh()
		{
			if (!(_remainRefreshCooldownTime > 0f))
			{
				base.Refresh();
				ability._stack = Mathf.Min(ability._stack + 1, ability._maxStack);
				_remainRefreshCooldownTime = ability._refreshCooldownTime;
			}
		}

		private bool HandleOnTakeDamage(ref Damage damage)
		{
			if (_remainCooldownTime > 0f)
			{
				return false;
			}
			if ((Object)(object)damage.attacker.character != (Object)null && !((EnumArray<Character.Type, bool>)ability._characterTypes)[damage.attacker.character.type])
			{
				return false;
			}
			if (!((EnumArray<Damage.MotionType, bool>)ability._attackTypes)[damage.motionType])
			{
				return false;
			}
			if (!((EnumArray<Damage.AttackType, bool>)ability._damageTypes)[damage.attackType])
			{
				return false;
			}
			if (!string.IsNullOrWhiteSpace(ability._attackKey) && !damage.key.Equals(ability._attackKey, StringComparison.OrdinalIgnoreCase))
			{
				return false;
			}
			damage.percentMultiplier *= 1f + ability._damagePercent * (float)ability._stack;
			damage.multiplier += ability._damagePercentPoint * (float)ability._stack;
			damage.criticalChance += ability._extraCriticalChance * (float)ability._stack;
			damage.criticalDamageMultiplier += ability._extraCriticalDamageMultiplier * (float)ability._stack;
			damage.criticalDamagePercentMultiplier += ability._extraCriticalDamagePercentMultiplier * (float)ability._stack;
			_remainCooldownTime = ability._cooldownTime;
			_remainCount--;
			if (_remainCount == 0)
			{
				owner.ability.Remove(this);
			}
			return false;
		}

		protected override void OnDetach()
		{
			owner.health.onTakeDamage.Remove((TakeDamageDelegate)HandleOnTakeDamage);
		}

		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
			_remainCooldownTime -= deltaTime;
			_remainRefreshCooldownTime -= deltaTime;
		}
	}

	[SerializeField]
	private int _maxStack;

	[SerializeField]
	[Tooltip("비어있지 않을 경우, 해당 키를 가진 공격에만 발동됨")]
	private string _attackKey;

	[SerializeField]
	private CharacterTypeBoolArray _characterTypes = new CharacterTypeBoolArray(true, true, true, true, true, true, true, true, true);

	[SerializeField]
	private MotionTypeBoolArray _attackTypes;

	[SerializeField]
	private AttackTypeBoolArray _damageTypes;

	[SerializeField]
	private float _cooldownTime;

	[SerializeField]
	private int _applyCount;

	[SerializeField]
	private float _refreshCooldownTime;

	[Header("스택당 설정")]
	[SerializeField]
	private float _damagePercent = 1f;

	[SerializeField]
	private float _damagePercentPoint;

	[SerializeField]
	private float _extraCriticalChance;

	[SerializeField]
	private float _extraCriticalDamagePercentMultiplier;

	[SerializeField]
	private float _extraCriticalDamageMultiplier;

	private int _stack;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
