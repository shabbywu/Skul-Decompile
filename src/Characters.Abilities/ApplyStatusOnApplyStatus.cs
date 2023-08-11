using System;
using System.Collections;
using Characters.Operations;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Characters.Abilities;

[Serializable]
public sealed class ApplyStatusOnApplyStatus : Ability
{
	public class Instance : AbilityInstance<ApplyStatusOnApplyStatus>
	{
		private float _remainTime;

		private CharacterStatus.ApplyInfo _stun;

		private CharacterStatus.ApplyInfo _burn;

		private CharacterStatus.ApplyInfo _poison;

		private CharacterStatus.ApplyInfo _freeze;

		private CharacterStatus.ApplyInfo _wound;

		public override float iconFillAmount
		{
			get
			{
				if (ability._cooldownTime != 0f)
				{
					return _remainTime / ability._cooldownTime;
				}
				return 0f;
			}
		}

		internal Instance(Character owner, ApplyStatusOnApplyStatus ability)
			: base(owner, ability)
		{
			if (ability._self)
			{
				_stun = new CharacterStatus.ApplyInfo(CharacterStatus.Kind.Stun);
				_burn = new CharacterStatus.ApplyInfo(CharacterStatus.Kind.Burn);
				_poison = new CharacterStatus.ApplyInfo(CharacterStatus.Kind.Poison);
				_freeze = new CharacterStatus.ApplyInfo(CharacterStatus.Kind.Freeze);
				_wound = new CharacterStatus.ApplyInfo(CharacterStatus.Kind.Wound);
			}
		}

		protected override void OnAttach()
		{
			if (ability._self)
			{
				owner.status.Register(CharacterStatus.Kind.Stun, ability._timing, ApplyStun);
				owner.status.Register(CharacterStatus.Kind.Burn, ability._timing, ApplyBurn);
				owner.status.Register(CharacterStatus.Kind.Poison, ability._timing, ApplyPoison);
				owner.status.Register(CharacterStatus.Kind.Freeze, ability._timing, ApplyFreeze);
				owner.status.Register(CharacterStatus.Kind.Wound, ability._timing, ApplyWound);
			}
			else
			{
				CharacterStatus.Kind[] kinds = ability._kinds;
				foreach (CharacterStatus.Kind kind in kinds)
				{
					owner.status.Register(kind, ability._timing, HandleOnTimeDelegate);
				}
			}
		}

		private bool Pass()
		{
			if (_remainTime > 0f)
			{
				return false;
			}
			if (!MMMaths.PercentChance(ability._chance))
			{
				return false;
			}
			return true;
		}

		private void ApplyStun(Character attacker, Character target)
		{
			if (Pass())
			{
				_remainTime = ability._cooldownTime;
				((MonoBehaviour)attacker).StartCoroutine(CGiveStatus(attacker, target, _stun));
			}
		}

		private void ApplyBurn(Character attacker, Character target)
		{
			if (Pass())
			{
				_remainTime = ability._cooldownTime;
				((MonoBehaviour)attacker).StartCoroutine(CGiveStatus(attacker, target, _burn));
			}
		}

		private void ApplyPoison(Character attacker, Character target)
		{
			if (Pass())
			{
				_remainTime = ability._cooldownTime;
				((MonoBehaviour)attacker).StartCoroutine(CGiveStatus(attacker, target, _poison));
			}
		}

		private void ApplyFreeze(Character attacker, Character target)
		{
			if (Pass())
			{
				_remainTime = ability._cooldownTime;
				((MonoBehaviour)attacker).StartCoroutine(CGiveStatus(attacker, target, _freeze));
			}
		}

		private void ApplyWound(Character attacker, Character target)
		{
			if (Pass())
			{
				_remainTime = ability._cooldownTime;
				((MonoBehaviour)attacker).StartCoroutine(CGiveStatus(attacker, target, _wound));
			}
		}

		private void HandleOnTimeDelegate(Character attacker, Character target)
		{
			if (!(_remainTime > 0f) && MMMaths.PercentChance(ability._chance) && !ability._self)
			{
				_remainTime = ability._cooldownTime;
				((MonoBehaviour)attacker).StartCoroutine(CGiveStatus(attacker, target, ability._status));
			}
		}

		protected override void OnDetach()
		{
			if (ability._self)
			{
				owner.status.Unregister(CharacterStatus.Kind.Stun, ability._timing, ApplyStun);
				owner.status.Unregister(CharacterStatus.Kind.Burn, ability._timing, ApplyBurn);
				owner.status.Unregister(CharacterStatus.Kind.Poison, ability._timing, ApplyPoison);
				owner.status.Unregister(CharacterStatus.Kind.Freeze, ability._timing, ApplyFreeze);
				owner.status.Unregister(CharacterStatus.Kind.Wound, ability._timing, ApplyWound);
			}
			else
			{
				CharacterStatus.Kind[] kinds = ability._kinds;
				foreach (CharacterStatus.Kind kind in kinds)
				{
					owner.status.Unregister(kind, ability._timing, HandleOnTimeDelegate);
				}
			}
		}

		private IEnumerator CGiveStatus(Character giver, Character target, CharacterStatus.ApplyInfo info)
		{
			yield return Chronometer.global.WaitForSeconds(ability._delay);
			if ((Object)(object)ability._targetPoint != (Object)null)
			{
				ability._positionInfo.Attach(target, ability._targetPoint);
			}
			giver.GiveStatus(target, info);
			((MonoBehaviour)giver).StartCoroutine(ability._onGiveStatus.CRun(target));
		}

		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
			_remainTime -= deltaTime;
		}
	}

	[SerializeField]
	[Tooltip("default는 0초")]
	private float _cooldownTime;

	[SerializeField]
	[Range(1f, 100f)]
	private int _chance = 100;

	[SerializeField]
	private float _delay;

	[SerializeField]
	private CharacterStatus.Kind[] _kinds;

	[SerializeField]
	private CharacterStatus.Timing _timing;

	[Header("부여 할 것")]
	[Tooltip("부여 했던 것을 다시 부여할 경우 선택")]
	[SerializeField]
	private bool _self;

	[SerializeField]
	private CharacterStatus.ApplyInfo _status;

	[SerializeField]
	private Transform _targetPoint;

	[SerializeField]
	private PositionInfo _positionInfo;

	[Subcomponent(typeof(OperationInfo))]
	[SerializeField]
	private OperationInfo.Subcomponents _onGiveStatus;

	public override void Initialize()
	{
		base.Initialize();
		_onGiveStatus.Initialize();
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
