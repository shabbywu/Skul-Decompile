using Characters.Abilities;
using Characters.Operations;
using FX;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions;

public abstract class CooldownAbility : Ability
{
	public abstract class CooldownAbilityInstance : AbilityInstance<CooldownAbility>
	{
		protected float _remainCooldownTime;

		protected float _remainBuffTime;

		protected bool _buffAttached;

		protected EffectPoolInstance _loopEffect;

		public override Sprite icon
		{
			get
			{
				if (!(_remainBuffTime > 0f) && !(_remainCooldownTime > 0f))
				{
					return null;
				}
				return base.icon;
			}
		}

		public override float iconFillAmount
		{
			get
			{
				if (_buffAttached)
				{
					return 1f - _remainBuffTime / ability._buffDuration;
				}
				if (_remainCooldownTime > 0f)
				{
					return 1f - _remainCooldownTime / ability._cooldownTime;
				}
				return base.iconFillAmount;
			}
		}

		public CooldownAbilityInstance(Character owner, CooldownAbility ability)
			: base(owner, ability)
		{
		}

		protected void ChangeIconFillToBuffTime()
		{
			base.iconFillInversed = false;
			base.iconFillFlipped = false;
		}

		protected void ChangeIconFillToCooldownTime()
		{
			base.iconFillInversed = true;
			base.iconFillFlipped = true;
		}

		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
			if (_remainBuffTime < 0f)
			{
				_remainCooldownTime -= deltaTime;
				if (_remainCooldownTime < 0f)
				{
					ChangeIconFillToBuffTime();
				}
			}
			_remainBuffTime -= deltaTime;
			if (_buffAttached && _remainBuffTime < 0f)
			{
				OnDetachBuff();
			}
		}

		protected virtual void OnAttachBuff()
		{
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			_remainBuffTime = ability._buffDuration;
			_remainCooldownTime = ability._cooldownTime;
			ChangeIconFillToBuffTime();
			_buffAttached = true;
			_loopEffect = ((ability._buffLoopEffect == null) ? null : ability._buffLoopEffect.Spawn(((Component)owner).transform.position, owner));
		}

		protected virtual void OnDetachBuff()
		{
			ChangeIconFillToCooldownTime();
			if ((Object)(object)_loopEffect != (Object)null)
			{
				_loopEffect.Stop();
				_loopEffect = null;
			}
			_buffAttached = false;
		}
	}

	[SerializeField]
	internal float _buffDuration;

	[SerializeField]
	internal float _cooldownTime;

	[SerializeField]
	[CharacterOperation.Subcomponent]
	internal CharacterOperation.Subcomponents _onAttached;

	[CharacterOperation.Subcomponent]
	internal CharacterOperation.Subcomponents _onDetached;

	[SerializeField]
	internal EffectInfo _buffLoopEffect = new EffectInfo
	{
		subordinated = true
	};
}
