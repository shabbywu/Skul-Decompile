using System;
using System.Collections;
using Characters.Operations;
using FX;
using UnityEngine;

namespace Characters.Abilities.Darks;

[Serializable]
public sealed class LivingArmor : Ability
{
	public sealed class Instance : AbilityInstance<LivingArmor>
	{
		private float _remainRecoverWaitTime;

		private float _remainRecoverInterval;

		private EffectPoolInstance _loopEffectPoolInstance;

		private Characters.Shield.Instance _shieldInstance;

		private float _shieldAmount;

		private bool _active;

		public Instance(Character owner, LivingArmor ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			_shieldAmount = (float)owner.health.maximumHealth * ability._shieldPercent * 0.01f;
			_shieldInstance = owner.health.shield.Add(ability, _shieldAmount, OnShieldBroke);
			AttachShieldEffect();
			owner.health.onTookDamage += HandleOnTookDamage;
			_remainRecoverWaitTime = 2.1474836E+09f;
			_remainRecoverInterval = ability._recoverInterval;
		}

		private IEnumerator CLoad()
		{
			yield return null;
			ability._operationInfos.Initialize();
			Activate();
		}

		private void Activate()
		{
			if (!_active)
			{
				((Component)ability._operationInfos).gameObject.SetActive(true);
				if (((Component)ability._operationInfos).gameObject.activeSelf)
				{
					ability._operationInfos.Run(owner);
				}
				_active = true;
			}
		}

		private void Deactivate()
		{
			if (_active)
			{
				ability._operationInfos.Stop();
				_active = false;
			}
		}

		private void HandleOnTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			if (!(tookDamage.amount <= 0.0) && tookDamage.attackType != 0)
			{
				_remainRecoverWaitTime = ability._recoverWaitTime;
			}
		}

		protected override void OnDetach()
		{
			owner.health.onTookDamage -= HandleOnTookDamage;
			ability.onDetach?.Invoke(_shieldInstance);
			if (owner.health.shield.Remove(ability))
			{
				_shieldInstance = null;
			}
			if ((Object)(object)ability._operationInfos != (Object)null)
			{
				Deactivate();
			}
		}

		public override void UpdateTime(float deltaTime)
		{
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			base.UpdateTime(deltaTime);
			_remainRecoverWaitTime -= deltaTime;
			_remainRecoverInterval -= deltaTime;
			if (!(_remainRecoverWaitTime <= 0f) || _remainRecoverInterval > 0f)
			{
				return;
			}
			_remainRecoverInterval = ability._recoverInterval;
			float num = _shieldAmount * ability._recoverPercent * 0.01f;
			if (_shieldInstance != null)
			{
				if (_shieldInstance.amount >= (double)_shieldAmount)
				{
					return;
				}
				if ((Object)(object)_loopEffectPoolInstance == (Object)null)
				{
					float extraScale = ability._effectSize[owner.sizeForEffect];
					_loopEffectPoolInstance = ((ability._shieldEffectInfo != null) ? ability._shieldEffectInfo.Spawn(((Component)owner).transform.position, owner, 0f, extraScale) : null);
					if ((Object)(object)ability._operationInfos != (Object)null)
					{
						((MonoBehaviour)owner).StartCoroutine(CLoad());
					}
				}
				_shieldInstance.amount = Mathf.Min((float)_shieldInstance.amount + num, _shieldAmount);
				owner.health.shield.AddOrUpdate(ability, (float)_shieldInstance.amount, OnShieldBroke);
			}
			else
			{
				AttachShieldEffect();
				_shieldInstance = owner.health.shield.Add(ability, num, OnShieldBroke);
			}
		}

		private void AttachShieldEffect()
		{
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			float extraScale = ability._effectSize[owner.sizeForEffect];
			if ((Object)(object)_loopEffectPoolInstance != (Object)null)
			{
				_loopEffectPoolInstance.Stop();
				_loopEffectPoolInstance = null;
			}
			_loopEffectPoolInstance = ((ability._shieldEffectInfo != null) ? ability._shieldEffectInfo.Spawn(((Component)owner).transform.position, owner, 0f, extraScale) : null);
			if ((Object)(object)ability._operationInfos != (Object)null)
			{
				((MonoBehaviour)owner).StartCoroutine(CLoad());
			}
		}

		private void OnShieldBroke()
		{
			if (_shieldInstance != null)
			{
				if ((Object)(object)_loopEffectPoolInstance != (Object)null)
				{
					_loopEffectPoolInstance.Stop();
					_loopEffectPoolInstance = null;
				}
				if ((Object)(object)ability._operationInfos != (Object)null)
				{
					Deactivate();
				}
				owner.health.shield.Remove(ability);
				_shieldInstance = null;
				ability.onBroke?.Invoke(_shieldInstance);
				((Component)ability._onBreakShield).gameObject.SetActive(true);
				if (((Component)ability._onBreakShield).gameObject.activeSelf)
				{
					ability._onBreakShield.Run(owner);
				}
			}
		}
	}

	[SerializeField]
	private EffectInfo _shieldEffectInfo = new EffectInfo
	{
		subordinated = true
	};

	[SerializeField]
	private EnumArray<Character.SizeForEffect, float> _effectSize;

	[SerializeField]
	private OperationInfos _operationInfos;

	[SerializeField]
	private OperationInfos _onBreakShield;

	[SerializeField]
	[Range(0f, 100f)]
	private float _shieldPercent;

	[SerializeField]
	private float _recoverWaitTime;

	[SerializeField]
	[Range(0f, 100f)]
	private float _recoverPercent;

	[SerializeField]
	private float _recoverInterval;

	public event Action<Characters.Shield.Instance> onBroke;

	public event Action<Characters.Shield.Instance> onDetach;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
