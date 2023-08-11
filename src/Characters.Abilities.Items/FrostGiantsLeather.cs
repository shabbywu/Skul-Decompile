using System;
using Characters.Actions;
using FX;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Items;

[Serializable]
public sealed class FrostGiantsLeather : Ability
{
	public class Instance : AbilityInstance<FrostGiantsLeather>
	{
		private Characters.Shield.Instance _shieldInstance;

		private float _remainCooldownTime;

		private float _remainBuffTime;

		private float _remainShieldTime;

		private bool _cooldownStart;

		private bool _buffAttached;

		private EffectPoolInstance _shieldEffectInstance;

		private EffectPoolInstance _buffEffectInstance;

		public override float iconFillAmount
		{
			get
			{
				if (_buffAttached)
				{
					return _remainBuffTime / ability._buffTime;
				}
				return _remainCooldownTime / ability._cooldownTime;
			}
		}

		public Instance(Character owner, FrostGiantsLeather ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			owner.onStartAction += HandleOnStartAction;
		}

		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
			if (_cooldownStart)
			{
				_remainCooldownTime -= deltaTime;
			}
			_remainBuffTime -= deltaTime;
			_remainShieldTime -= deltaTime;
			if (_remainShieldTime < 0f && _shieldInstance != null)
			{
				BreakShield();
			}
			if (_remainBuffTime < 0f && _buffAttached)
			{
				DetachBuff();
			}
		}

		private void HandleOnStartAction(Characters.Actions.Action action)
		{
			if (action.type == Characters.Actions.Action.Type.Skill && !(_remainCooldownTime > 0f))
			{
				_remainCooldownTime = ability._cooldownTime;
				_cooldownStart = false;
				AttachBuff();
			}
		}

		private void AttachBuff()
		{
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			owner.stat.AttachValues(ability._statBonus);
			owner.status.onApplyFreeze += UpdateShieldAmount;
			owner.status.onRefreshFreeze += UpdateShieldAmount;
			_buffAttached = true;
			base.iconFillInversed = true;
			base.iconFillFlipped = false;
			_remainBuffTime = ability._buffTime;
			PersistentSingleton<SoundManager>.Instance.PlaySound(ability._attachBuffSound, ((Component)owner).transform.position);
			_buffEffectInstance = ((ability._buffAura == null) ? null : ability._buffAura.Spawn(((Component)owner).transform.position, owner));
		}

		private void DetachBuff()
		{
			owner.stat.DetachValues(ability._statBonus);
			owner.status.onApplyFreeze -= UpdateShieldAmount;
			owner.status.onRefreshFreeze -= UpdateShieldAmount;
			_buffAttached = false;
			_cooldownStart = true;
			base.iconFillInversed = false;
			base.iconFillFlipped = true;
			if ((Object)(object)_buffEffectInstance != (Object)null)
			{
				_buffEffectInstance.Stop();
				_buffEffectInstance = null;
			}
		}

		private void UpdateShieldAmount(Character attacker, Character target)
		{
			UpdateShieldAmount();
		}

		private void UpdateShieldAmount()
		{
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			if (_shieldInstance != null)
			{
				int num = Mathf.Min((int)_shieldInstance.amount + ability._shieldAmount, ability._maxShieldAmount);
				_shieldInstance.amount = num;
			}
			else
			{
				_shieldInstance = owner.health.shield.Add(this, ability._shieldAmount, BreakShield);
				_shieldEffectInstance = ability._shieldEffect?.Spawn(((Component)owner).transform.position, owner);
			}
			_remainShieldTime = ability._shieldTime;
		}

		private void BreakShield()
		{
			if (owner.health.shield.Remove(this))
			{
				_shieldInstance = null;
			}
			if ((Object)(object)_shieldEffectInstance != (Object)null)
			{
				_shieldEffectInstance.Stop();
				_shieldEffectInstance = null;
			}
		}

		protected override void OnDetach()
		{
			owner.onStartAction -= HandleOnStartAction;
			_remainBuffTime = ability._cooldownTime;
			DetachBuff();
			BreakShield();
		}
	}

	[SerializeField]
	private float _buffTime;

	[SerializeField]
	private float _cooldownTime;

	[SerializeField]
	private float _shieldTime;

	[SerializeField]
	private Stat.Values _statBonus;

	[SerializeField]
	private int _shieldAmount;

	[SerializeField]
	private int _maxShieldAmount;

	[SerializeField]
	private EffectInfo _shieldEffect;

	[SerializeField]
	private EffectInfo _buffAura;

	[SerializeField]
	private SoundInfo _attachBuffSound;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
