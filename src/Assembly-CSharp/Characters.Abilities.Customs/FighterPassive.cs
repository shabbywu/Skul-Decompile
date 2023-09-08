using System;
using Characters.Actions;
using Characters.Gear.Weapons;
using Characters.Gear.Weapons.Gauges;
using UnityEngine;

namespace Characters.Abilities.Customs;

[Serializable]
public class FighterPassive : Ability, IAbilityInstance
{
	[Header("교대기")]
	[SerializeField]
	[AbilityComponent.Subcomponent]
	private AbilityComponent _swapAttackAbility;

	[SerializeField]
	private Weapon _weapon;

	[SerializeField]
	[Space]
	private SkillInfo[] _skills;

	[SerializeField]
	private SkillInfo[] _rageSkills;

	[Space]
	[SerializeField]
	private Characters.Actions.Action[] _actions;

	[SerializeField]
	private Characters.Actions.Action[] _rageActions;

	[SerializeField]
	[Space]
	private CharacterAnimation _characterAnimation;

	[SerializeField]
	private RuntimeAnimatorController _rageBaseAnimator;

	[SerializeField]
	private AnimationClip _rageIdleClip;

	[SerializeField]
	private AnimationClip _rageWalkClip;

	[SerializeField]
	private AnimationClip _rageJumpClip;

	[SerializeField]
	private AnimationClip _rageFallClip;

	[SerializeField]
	private AnimationClip _rageFallRepeatClip;

	private AnimationClipOverrider _overrider;

	private EffectPoolInstance _loopEffectInstance;

	private bool _buffAttached;

	private float _gaugeAnimationTime;

	[SerializeField]
	private float _cooldownTime;

	[SerializeField]
	private Sprite _cooldownIcon;

	[SerializeField]
	private Sprite _readyIcon;

	[SerializeField]
	private Color _defaultBarColor;

	[SerializeField]
	private Color _buffBarColor;

	[SerializeField]
	private ValueGauge _gauge;

	[SerializeField]
	private float _timeScale;

	[SerializeField]
	private Stat.Values _stat;

	public bool buffAttached => _buffAttached;

	public Character owner { get; private set; }

	IAbility IAbilityInstance.ability => this;

	public Sprite icon
	{
		get
		{
			if (_buffAttached)
			{
				return _defaultIcon;
			}
			if (!(remainTime > 0f))
			{
				return _readyIcon;
			}
			return _cooldownIcon;
		}
	}

	public float iconFillAmount
	{
		get
		{
			if (_buffAttached)
			{
				return remainTime / base.duration;
			}
			if (remainTime > 0f)
			{
				return 1f - remainTime / _cooldownTime;
			}
			return 0f;
		}
	}

	public float remainTime { get; set; }

	public bool attached => true;

	public int iconStacks => 0;

	public bool expired => false;

	public bool rageReady
	{
		get
		{
			if (!_buffAttached)
			{
				return remainTime <= 0f;
			}
			return false;
		}
	}

	public float timeScale => _timeScale;

	~FighterPassive()
	{
		_defaultIcon = null;
		_cooldownIcon = null;
		_readyIcon = null;
		_rageIdleClip = null;
		_rageWalkClip = null;
		_rageJumpClip = null;
		_rageFallClip = null;
		_rageFallRepeatClip = null;
	}

	public override void Initialize()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		base.Initialize();
		_swapAttackAbility.Initialize();
		if (_overrider == null)
		{
			_overrider = new AnimationClipOverrider(_rageBaseAnimator);
			_overrider.Override("EmptyIdle", _rageIdleClip);
			_overrider.Override("EmptyWalk", _rageWalkClip);
			_overrider.Override("EmptyJumpUp", _rageJumpClip);
			_overrider.Override("EmptyJumpDown", _rageFallClip);
			_overrider.Override("EmptyJumpDownLoop", _rageFallRepeatClip);
		}
	}

	public void Attach()
	{
		owner.onGiveDamage.Add(int.MaxValue, HandleOnGiveDamage);
	}

	private bool HandleOnGiveDamage(ITarget target, ref Damage damage)
	{
		if (damage.motionType != Damage.MotionType.Swap)
		{
			return false;
		}
		if (target == null)
		{
			return false;
		}
		if ((Object)(object)target.character == (Object)null)
		{
			return false;
		}
		target.character.ability.Add(_swapAttackAbility.ability);
		return false;
	}

	public void Detach()
	{
		owner.onGiveDamage.Remove(HandleOnGiveDamage);
		DetachRage();
	}

	public void AttachRage()
	{
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		if (!_buffAttached && !(remainTime > 0f))
		{
			remainTime = base.duration;
			_buffAttached = true;
			owner.stat.AttachValues(_stat);
			Chronometer.global.AttachTimeScale(this, _timeScale);
			owner.chronometer.master.AttachTimeScale(this, 1f / _timeScale);
			_loopEffectInstance = ((base.loopEffect == null) ? null : base.loopEffect.Spawn(((Component)owner).transform.position, owner));
			base.effectOnAttach.Spawn(((Component)owner).transform.position);
			for (int i = 0; i < _actions.Length; i++)
			{
				_weapon.ChangeAction(_actions[i], _rageActions[i]);
				_rageActions[i].cooldown.CopyCooldown(_actions[i].cooldown);
			}
			_weapon.AttachSkillChanges(_skills, _rageSkills, copyCooldown: true);
			_characterAnimation.AttachOverrider(_overrider);
			owner.animationController.ForceUpdate();
		}
	}

	private void DetachRage()
	{
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		if (_buffAttached)
		{
			remainTime = _cooldownTime;
			_buffAttached = false;
			owner.stat.DetachValues(_stat);
			Chronometer.global.DetachTimeScale(this);
			owner.chronometer.master.DetachTimeScale(this);
			if ((Object)(object)_loopEffectInstance != (Object)null)
			{
				_loopEffectInstance.Stop();
				_loopEffectInstance = null;
			}
			base.effectOnDetach.Spawn(((Component)owner).transform.position);
			for (int i = 0; i < _actions.Length; i++)
			{
				_weapon.ChangeAction(_rageActions[i], _actions[i]);
				_actions[i].cooldown.CopyCooldown(_rageActions[i].cooldown);
			}
			_weapon.DetachSkillChanges(_skills, _rageSkills, copyCooldown: true);
			_characterAnimation.DetachOverrider(_overrider);
			owner.animationController.ForceUpdate();
		}
	}

	public void UpdateTime(float deltaTime)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		remainTime -= deltaTime;
		if (_buffAttached)
		{
			_gauge.defaultBarGaugeColor.baseColor = _buffBarColor;
			if (remainTime < 0f)
			{
				DetachRage();
			}
			return;
		}
		if (remainTime > 0f)
		{
			_gauge.defaultBarGaugeColor.baseColor = _defaultBarColor;
			return;
		}
		remainTime = 0f;
		_gaugeAnimationTime += deltaTime * 2f;
		if (_gaugeAnimationTime > 2f)
		{
			_gaugeAnimationTime = 0f;
		}
		_gauge.defaultBarGaugeColor.baseColor = Color.LerpUnclamped(_defaultBarColor, _buffBarColor, (_gaugeAnimationTime < 1f) ? _gaugeAnimationTime : (2f - _gaugeAnimationTime));
	}

	public void Refresh()
	{
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		this.owner = owner;
		return this;
	}
}
