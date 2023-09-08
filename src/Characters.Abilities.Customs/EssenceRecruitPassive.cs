using System;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Customs;

[Serializable]
public class EssenceRecruitPassive : Ability
{
	public class Instance : AbilityInstance<EssenceRecruitPassive>
	{
		private float _remainCooldownTime;

		public Instance(Character owner, EssenceRecruitPassive ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			owner.health.onTakeDamage.Add(int.MaxValue, OnTakeDamage);
			owner.stat.AttachValues(ability._stat);
		}

		protected override void OnDetach()
		{
			owner.health.onTakeDamage.Remove(OnTakeDamage);
			owner.stat.DetachValues(ability._stat);
		}

		private bool OnTakeDamage(ref Damage damage)
		{
			if (_remainCooldownTime > 0f)
			{
				return true;
			}
			_remainCooldownTime = ability._cooldownTime;
			((MonoBehaviour)owner).StartCoroutine(ability._onHits.CRun(owner));
			return true;
		}

		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
			if (!(_remainCooldownTime < 0f))
			{
				_remainCooldownTime -= deltaTime;
			}
		}
	}

	[SerializeField]
	private float _cooldownTime;

	[Subcomponent(typeof(OperationInfo))]
	[SerializeField]
	private OperationInfo.Subcomponents _onHits;

	[SerializeField]
	private Stat.Values _stat;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
