using System;
using Characters.Abilities;
using Characters.Actions;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions;

public class Arms : SimpleStatBonusKeyword
{
	[Serializable]
	protected class AdditionalHit : IAbility, IAbilityInstance
	{
		[Header("4세트 효과")]
		[SerializeField]
		private float _cooldownTime;

		[SerializeField]
		private MotionTypeBoolArray _motionType;

		[SerializeField]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _additionalAttackOnGround;

		[SerializeField]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _additionalAttackOnAir;

		[Header("6 세트 효과")]
		[Tooltip("cycle번째 마다 강화된 추가공격")]
		[SerializeField]
		private Sprite _icon;

		[SerializeField]
		private int _cycle;

		[Subcomponent(typeof(OperationInfo))]
		[SerializeField]
		private OperationInfo.Subcomponents _additionalEnhancedAttackOnGround;

		[SerializeField]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _additionalEnhancedAttackOnAir;

		private int _count;

		private Character _owner;

		private float _remainCooldownTime;

		Character IAbilityInstance.owner => _owner;

		public IAbility ability => this;

		public float remainTime { get; set; }

		public bool attached => true;

		public Sprite icon
		{
			get
			{
				if (!(_remainCooldownTime > 0f))
				{
					return null;
				}
				return _icon;
			}
		}

		public float iconFillAmount => _remainCooldownTime / _cooldownTime;

		public bool iconFillInversed => false;

		public bool iconFillFlipped => true;

		public int iconStacks
		{
			get
			{
				if (!enhanced)
				{
					return 0;
				}
				return _count;
			}
		}

		public bool expired => false;

		public float duration { get; set; }

		public int iconPriority => 0;

		public bool removeOnSwapWeapon => false;

		public bool enhanced { get; set; }

		public Arms arms { get; set; }

		public IAbilityInstance CreateInstance(Character owner)
		{
			_owner = owner;
			_additionalAttackOnGround.Initialize();
			_additionalAttackOnAir.Initialize();
			_additionalEnhancedAttackOnAir.Initialize();
			_additionalEnhancedAttackOnGround.Initialize();
			return this;
		}

		public void Initialize()
		{
		}

		public void UpdateTime(float deltaTime)
		{
			_remainCooldownTime -= deltaTime;
		}

		public void Refresh()
		{
		}

		void IAbilityInstance.Attach()
		{
			_count = 0;
			_owner.onStartAction += HandleOnStartAction;
		}

		private void HandleOnStartAction(Characters.Actions.Action action)
		{
			if ((action.type != Characters.Actions.Action.Type.BasicAttack && action.type != Characters.Actions.Action.Type.JumpAttack) || _remainCooldownTime > 0f)
			{
				return;
			}
			OperationInfo.Subcomponents subcomponents = (_owner.movement.isGrounded ? _additionalAttackOnGround : _additionalAttackOnAir);
			if (enhanced && _count == _cycle - 1)
			{
				subcomponents = (_owner.movement.isGrounded ? _additionalEnhancedAttackOnGround : _additionalEnhancedAttackOnAir);
			}
			((MonoBehaviour)_owner).StartCoroutine(subcomponents.CRun(_owner));
			_remainCooldownTime = _cooldownTime;
			if (enhanced)
			{
				_count++;
				if (_count == _cycle)
				{
					_count = 0;
				}
			}
		}

		void IAbilityInstance.Detach()
		{
			_owner.onStartAction -= HandleOnStartAction;
		}
	}

	[Header("2 세트 효과")]
	[SerializeField]
	private double[] _statBonusByStep;

	[Header("4 세트 효과")]
	[SerializeField]
	private AdditionalHit _additionalHit;

	protected override double[] statBonusByStep => _statBonusByStep;

	protected override Stat.Category statCategory => Stat.Category.PercentPoint;

	protected override Stat.Kind statKind => Stat.Kind.PhysicalAttackDamage;

	protected override void Initialize()
	{
		base.Initialize();
		_additionalHit.arms = this;
	}

	public override void UpdateBonus(bool wasActive, bool wasOmen)
	{
		UpdateStat();
		if (keyword.step >= 2)
		{
			base.character.ability.Add(_additionalHit.ability);
		}
		else
		{
			base.character.ability.Remove(_additionalHit.ability);
		}
		if (keyword.isMaxStep)
		{
			_additionalHit.enhanced = true;
		}
		else
		{
			_additionalHit.enhanced = false;
		}
	}
}
