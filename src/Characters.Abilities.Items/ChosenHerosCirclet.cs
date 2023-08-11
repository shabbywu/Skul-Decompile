using System;
using Characters.Operations;
using FX;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Items;

[Serializable]
public sealed class ChosenHerosCirclet : Ability
{
	public class Instance : AbilityInstance<ChosenHerosCirclet>
	{
		private float _remainCooldownTime;

		private float _remainBuffTime;

		private double _totalAttackDamage;

		private bool _fortitude;

		private CoroutineReference _cooldownReference;

		private EffectPoolInstance _loopEffect;

		public override Sprite icon
		{
			get
			{
				if (_fortitude)
				{
					return null;
				}
				return base.icon;
			}
		}

		public override float iconFillAmount => _remainCooldownTime / ability._fortitudeCooldownTime;

		public bool canUse
		{
			get
			{
				if (!_fortitude)
				{
					return _remainCooldownTime <= 0f;
				}
				return false;
			}
		}

		public Instance(Character owner, ChosenHerosCirclet ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			base.remainTime = 2.1474836E+09f;
			owner.health.onDie += HandleOnDie;
		}

		private void HandleOnDie()
		{
			if (canUse)
			{
				owner.health.SetCurrentHealth(1.0);
				AttachBuff();
			}
		}

		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
			_remainBuffTime -= deltaTime;
			_remainCooldownTime -= deltaTime;
			if (_fortitude)
			{
				if (_remainBuffTime <= 0f)
				{
					_remainCooldownTime = ability._fortitudeCooldownTime;
					DetachBuff();
					Heal();
				}
			}
			else if (owner.health.percent <= (double)ability._triggerHealthPercent && _remainCooldownTime <= 0f)
			{
				AttachBuff();
			}
		}

		private void AttachBuff()
		{
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			_remainBuffTime = ability._fortitudeTime;
			_fortitude = true;
			owner.invulnerable.Attach((object)this);
			Character character = owner;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character.onGaveDamage, new GaveDamageDelegate(HandleOnGaveDamage));
			owner.stat.AttachValues(ability._bonusStat);
			((CoroutineReference)(ref _cooldownReference)).Stop();
			_cooldownReference = CoroutineReferenceExtension.StartCoroutineWithReference((MonoBehaviour)(object)owner, ability._onFortitudeStart.CRun(owner));
			_loopEffect = ability._fortitudeLoopEffect.Spawn(((Component)owner).transform.position, owner);
		}

		private void DetachBuff()
		{
			if (_fortitude)
			{
				((MonoBehaviour)owner).StartCoroutine(ability._onFortitudeEnd.CRun(owner));
			}
			_fortitude = false;
			owner.invulnerable.Detach((object)this);
			Character character = owner;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(character.onGaveDamage, new GaveDamageDelegate(HandleOnGaveDamage));
			owner.stat.DetachValues(ability._bonusStat);
			if ((Object)(object)_loopEffect != (Object)null)
			{
				_loopEffect.Stop();
				_loopEffect = null;
			}
		}

		private void Heal()
		{
			float num = (float)owner.health.maximumHealth * ability._recoverMaxPercent;
			float num2 = (float)owner.health.maximumHealth * (float)_totalAttackDamage / ability._recoverDamageUnit;
			float num3 = Mathf.Min(num, num2);
			owner.health.Heal(num3);
			_totalAttackDamage = 0.0;
		}

		private void HandleOnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
		{
			if (gaveDamage.attribute == Damage.Attribute.Physical)
			{
				_totalAttackDamage += gaveDamage.amount;
			}
		}

		protected override void OnDetach()
		{
			owner.health.onDie -= HandleOnDie;
			DetachBuff();
		}
	}

	[Range(0f, 1f)]
	[SerializeField]
	private float _triggerHealthPercent;

	[SerializeField]
	private float _fortitudeTime;

	[SerializeField]
	private float _fortitudeCooldownTime;

	[SerializeField]
	private float _recoverDamageUnit;

	[Range(0f, 1f)]
	[SerializeField]
	private float _recoverMaxPercent;

	[SerializeField]
	private Stat.Values _bonusStat;

	[SerializeField]
	private EffectInfo _fortitudeLoopEffect = new EffectInfo
	{
		subordinated = true
	};

	[SerializeField]
	[Subcomponent(typeof(OperationInfo))]
	private OperationInfo.Subcomponents _onFortitudeStart;

	[SerializeField]
	[Subcomponent(typeof(OperationInfo))]
	private OperationInfo.Subcomponents _onFortitudeEnd;

	public override void Initialize()
	{
		_onFortitudeStart.Initialize();
		_onFortitudeEnd.Initialize();
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
