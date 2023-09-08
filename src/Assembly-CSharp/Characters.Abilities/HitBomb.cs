using System;
using Characters.Operations;
using Services;
using Singletons;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Characters.Abilities;

[Serializable]
public sealed class HitBomb : Ability
{
	public sealed class Instance : AbilityInstance<HitBomb>
	{
		private double _baseDamage;

		private float _startTime;

		public Instance(Character owner, HitBomb ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			owner.health.onTookDamage += HandleOnTookDamage;
			_startTime = Time.time;
		}

		private void HandleOnTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			if (!((Object)(object)tookDamage.attacker.character == (Object)null) && ability._attackerFilter[tookDamage.attacker.character.type] && ability._motionTypeFilter[tookDamage.motionType] && ability._attackTypeFilter[tookDamage.attackType])
			{
				_baseDamage += damageDealt;
			}
		}

		private void Bomb()
		{
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			float num = Mathf.Clamp((float)_baseDamage * ability._hitDamageMultiplier, 1f, ability._maxDamage);
			Damage damage = owner.stat.GetDamage(num, MMMaths.RandomPointWithinBounds(((Collider2D)owner.collider).bounds), ability._bombHitInfo);
			Character player = Singleton<Service>.Instance.levelManager.player;
			player.Attack(owner, ref damage);
			ability._positionInfo.Attach(owner, ability._targetPoint);
			((MonoBehaviour)player).StartCoroutine(ability._onBomb.CRun(player));
		}

		protected override void OnDetach()
		{
			owner.health.onTookDamage -= HandleOnTookDamage;
			float num = Time.time - _startTime;
			if (ability._bombOnDetached || !(num < ability.duration))
			{
				Bomb();
			}
		}
	}

	[Header("받는 공격 설정")]
	[SerializeField]
	private CharacterTypeBoolArray _attackerFilter;

	[SerializeField]
	private MotionTypeBoolArray _motionTypeFilter;

	[SerializeField]
	private AttackTypeBoolArray _attackTypeFilter;

	[Header("터지는 공격 설정")]
	[SerializeField]
	private bool _bombOnDetached;

	[SerializeField]
	private float _hitDamageMultiplier;

	[SerializeField]
	private float _maxDamage;

	[SerializeField]
	private HitInfo _bombHitInfo;

	[SerializeField]
	private PositionInfo _positionInfo;

	[SerializeField]
	private Transform _targetPoint;

	[Subcomponent(typeof(OperationInfo))]
	[SerializeField]
	private OperationInfo.Subcomponents _onBomb;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
