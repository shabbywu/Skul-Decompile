using System;
using Characters.Gear.Weapons.Gauges;
using Characters.Operations;
using UnityEngine;

namespace Characters.Abilities.Customs;

[Serializable]
public class RecruitPassive : Ability
{
	public class Instance : AbilityInstance<RecruitPassive>
	{
		private Color _defaultGaugeColor;

		private float _remainSummoningTime;

		private float _remainSummoningInterval;

		public Instance(Character owner, RecruitPassive ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			Character character = owner;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character.onGaveDamage, new GaveDamageDelegate(OnOwnerGaveDamage));
			ability._gauge.onChanged += OnGaugeChanged;
		}

		protected override void OnDetach()
		{
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			Character character = owner;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(character.onGaveDamage, new GaveDamageDelegate(OnOwnerGaveDamage));
			if (_remainSummoningTime > 0f)
			{
				ability._gauge.defaultBarColor = _defaultGaugeColor;
				ability._gauge.Clear();
			}
		}

		public override void UpdateTime(float deltaTime)
		{
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			base.UpdateTime(deltaTime);
			if (_remainSummoningTime <= 0f)
			{
				if (ability._gauge.currentValue == ability._gauge.maxValue)
				{
					ability._gauge.defaultBarColor = _defaultGaugeColor;
					ability._gauge.Clear();
				}
				return;
			}
			_remainSummoningInterval -= deltaTime;
			_remainSummoningTime -= deltaTime;
			if (_remainSummoningInterval < 0f)
			{
				ability._summoningOperation.Run(owner);
				_remainSummoningInterval += ability._summoningInterval;
			}
		}

		private void OnOwnerGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
		{
			if (string.IsNullOrEmpty(gaveDamage.key))
			{
				return;
			}
			for (int i = 0; i < ability._attackKeyAndGaugeAmounts.Length; i++)
			{
				GaugeAmountByAttackKey gaugeAmountByAttackKey = ability._attackKeyAndGaugeAmounts[i];
				if (string.Equals(gaugeAmountByAttackKey.attackKey, gaveDamage.key, StringComparison.OrdinalIgnoreCase))
				{
					ability._gauge.Add(gaugeAmountByAttackKey.gaugeAmountByAttack);
					break;
				}
			}
		}

		private void OnGaugeChanged(float oldValue, float newValue)
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			if (!(oldValue >= newValue) && !(newValue < ability._gauge.maxValue))
			{
				_defaultGaugeColor = ability._gauge.defaultBarColor;
				ability._gauge.defaultBarColor = ability._fullGaugeColor;
				_remainSummoningTime = ability._summoningDuration;
				_remainSummoningInterval = 0f;
			}
		}
	}

	[Serializable]
	private class GaugeAmountByAttackKey
	{
		public string attackKey;

		public int gaugeAmountByAttack;
	}

	[SerializeField]
	[Header("Gauge")]
	private ValueGauge _gauge;

	[SerializeField]
	private Color _fullGaugeColor;

	[SerializeField]
	private GaugeAmountByAttackKey[] _attackKeyAndGaugeAmounts;

	[SerializeField]
	[Header("Summon")]
	private float _summoningDuration;

	[SerializeField]
	private float _summoningInterval;

	[SerializeField]
	[CharacterOperation.Subcomponent]
	private CharacterOperation.Subcomponents _summoningOperation;

	public override void Initialize()
	{
		base.Initialize();
		_summoningOperation.Initialize();
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
