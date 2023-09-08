using System;
using System.Collections;
using UnityEngine;

namespace Characters.Abilities.Darks;

[Serializable]
public sealed class Protection : Ability
{
	public sealed class Instance : AbilityInstance<Protection>
	{
		private float _remainCooldownTime;

		private float _lastGuardDurability;

		private bool _raising;

		public Instance(Character owner, Protection ability)
			: base(owner, ability)
		{
			ability._guard.Initialize(owner);
			float num = (float)owner.health.maximumHealth * ability._guardDurabilityMultiplier;
			ability._guard.durability = num;
			ability._gauge.Set(num, num);
		}

		protected override void OnAttach()
		{
			((MonoBehaviour)owner).StartCoroutine(CGuardUp());
		}

		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
			float num = ability._guard.currentDurability;
			if (!Mathf.Approximately(num, _lastGuardDurability))
			{
				_lastGuardDurability = num;
				ability._gauge.Set(num);
			}
			if (!ability._guard.active && !_raising)
			{
				_remainCooldownTime -= deltaTime;
				if (_remainCooldownTime <= 0f)
				{
					((MonoBehaviour)owner).StartCoroutine(CGuardUp());
				}
			}
		}

		private IEnumerator CGuardUp()
		{
			_raising = true;
			float elapsed = 0f;
			float length = 0.5f;
			while (elapsed <= length)
			{
				ability._guard.currentDurability = Mathf.Lerp(0f, ability._guard.durability, elapsed / length);
				elapsed += owner.chronometer.master.deltaTime;
				yield return null;
			}
			_raising = false;
			GuardUp();
		}

		private void GuardUp()
		{
			((Component)ability._guard).gameObject.SetActive(true);
			ability._guard.GuardUp();
			_remainCooldownTime = ability._guardCooldownTime;
		}

		private void GuardDown()
		{
			((Component)ability._guard).gameObject.SetActive(false);
			ability._guard.GuardDown();
		}

		protected override void OnDetach()
		{
			GuardDown();
		}
	}

	[SerializeField]
	private Guard _guard;

	[SerializeField]
	private float _guardDurabilityMultiplier;

	[SerializeField]
	private DarkAbilityGauge _gauge;

	[SerializeField]
	private float _guardCooldownTime;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
