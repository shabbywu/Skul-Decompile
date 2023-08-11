using System;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Items;

[Serializable]
public sealed class AccelerationSword : Ability
{
	public class Instance : AbilityInstance<AccelerationSword>
	{
		private float _newCooldownTime;

		private float _remainTime;

		public override float iconFillAmount => _remainTime / _newCooldownTime;

		public Instance(Character owner, AccelerationSword ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			_newCooldownTime = ability._cooldownTime / (float)((double)ability._multiplier * owner.stat.GetFinal(Stat.Kind.BasicAttackSpeed));
			_remainTime = Mathf.Max(ability._minCooldownTime, _newCooldownTime);
		}

		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
			_remainTime -= deltaTime;
			if (_remainTime < 0f)
			{
				Attack();
			}
		}

		protected override void OnDetach()
		{
		}

		public void Attack()
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)TargetFinder.GetRandomTarget(ability._detectCollider, LayerMask.op_Implicit(1024)) == (Object)null)
			{
				_remainTime = 0.5f;
			}
			_newCooldownTime = ability._cooldownTime / (float)((double)ability._multiplier * owner.stat.GetFinal(Stat.Kind.BasicAttackSpeed));
			_remainTime = Mathf.Max(ability._minCooldownTime, _newCooldownTime);
			((MonoBehaviour)owner).StartCoroutine(ability._operations.CRun(owner));
		}
	}

	[SerializeField]
	private Collider2D _detectCollider;

	[Header("쿨타임 = 기본 쿨타임 / (multiplier * 공격속도 스텟[기본값1])")]
	[SerializeField]
	private float _multiplier;

	[SerializeField]
	private float _cooldownTime;

	[SerializeField]
	private float _minCooldownTime;

	[SerializeField]
	private CustomFloat _attackDamage;

	[SerializeField]
	[Subcomponent(typeof(OperationInfo))]
	private OperationInfo.Subcomponents _operations;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
