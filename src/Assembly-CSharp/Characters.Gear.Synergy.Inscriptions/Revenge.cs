using System;
using Characters.Abilities;
using Characters.Operations;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Characters.Gear.Synergy.Inscriptions;

public class Revenge : InscriptionInstance
{
	[Serializable]
	public sealed class RevengeAbility : Ability
	{
		public sealed class Instance : AbilityInstance<RevengeAbility>
		{
			public Instance(Character owner, RevengeAbility ability)
				: base(owner, ability)
			{
			}

			protected override void OnAttach()
			{
				Character character = owner;
				character.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character.onGaveDamage, new GaveDamageDelegate(HandleOnGaveDamage));
			}

			protected override void OnDetach()
			{
				Character character = owner;
				character.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(character.onGaveDamage, new GaveDamageDelegate(HandleOnGaveDamage));
			}

			private void HandleOnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
			{
				AttackRevengeTarget(target);
			}

			private void AttackRevengeTarget(ITarget target)
			{
				//IL_0080: Unknown result type (might be due to invalid IL or missing references)
				//IL_0085: Unknown result type (might be due to invalid IL or missing references)
				Character character = owner;
				character.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(character.onGaveDamage, new GaveDamageDelegate(HandleOnGaveDamage));
				ability._targetPositionInfo.Attach(target.character, ability._targetPoint);
				ability._additionalHitInfo.ChangeAdaptiveDamageAttribute(owner);
				Damage damage = owner.stat.GetDamage(ability._attackDamage.amount, MMMaths.RandomPointWithinBounds(target.collider.bounds), ability._additionalHitInfo);
				((MonoBehaviour)owner).StartCoroutine(ability._onHitOwner.CRun(owner));
				((MonoBehaviour)owner).StartCoroutine(ability._onHitTarget.CRun(target.character));
				owner.TryAttackCharacter(target, ref damage);
				Recover();
				if ((Object)(object)ability._revengeReward != (Object)null)
				{
					owner.ability.Add(ability._revengeReward.ability);
				}
				owner.ability.Remove(this);
			}

			private void Recover()
			{
				double num = owner.health.lastTakenDamage * (double)ability._recoveryPercent;
				owner.health.Heal(num, num > 0.0);
			}
		}

		[SerializeField]
		private HitInfo _additionalHitInfo;

		[SerializeField]
		private AttackDamage _attackDamage;

		[SerializeField]
		private PositionInfo _targetPositionInfo;

		[SerializeField]
		private Transform _targetPoint;

		[Range(0f, 1f)]
		[SerializeField]
		private float _recoveryPercent;

		[SerializeField]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _onHitOwner;

		[SerializeField]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _onHitTarget;

		[SerializeField]
		private AbilityComponent _revengeReward;

		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new Instance(owner, this);
		}
	}

	[SerializeField]
	private float _cooldownTime;

	[SerializeField]
	[Header("2세트")]
	private RevengeAbility _revengeAbility2;

	[Header("4세트")]
	[SerializeField]
	private RevengeAbility _revengeAbility4;

	private float _lastTakeDamageTime;

	private double _beforeHealth;

	protected override void Initialize()
	{
	}

	public override void UpdateBonus(bool wasActive, bool wasOmen)
	{
	}

	public override void Attach()
	{
		base.character.health.onTakeDamage.Add(int.MaxValue, HandleOnTakeDamage);
		base.character.health.onTookDamage += HandleOnTookDamage;
	}

	public override void Detach()
	{
		base.character.health.onTakeDamage.Remove(HandleOnTakeDamage);
		base.character.health.onTookDamage -= HandleOnTookDamage;
	}

	private bool HandleOnTakeDamage(ref Damage damage)
	{
		_beforeHealth = base.character.health.currentHealth;
		return false;
	}

	private void HandleOnTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
	{
		if (keyword.step >= 1 && !(Time.time - _lastTakeDamageTime < _cooldownTime) && !(_beforeHealth - base.character.health.currentHealth < 1.0))
		{
			_lastTakeDamageTime = Time.time;
			if (keyword.isMaxStep)
			{
				base.character.ability.Add(_revengeAbility4);
			}
			else
			{
				base.character.ability.Add(_revengeAbility2);
			}
		}
	}
}
