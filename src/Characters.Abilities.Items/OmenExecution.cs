using System;
using Characters.Actions;
using Characters.Operations;
using FX;
using UnityEngine;

namespace Characters.Abilities.Items;

[Serializable]
public sealed class OmenExecution : Ability
{
	public sealed class Instance : AbilityInstance<OmenExecution>
	{
		private readonly int entryAnimation = Animator.StringToHash("OmenExecution_0");

		private const string key = "OmenExecution";

		private float _totalDamage;

		private int _currentStep;

		private readonly EffectInfo _effect;

		private EffectPoolInstance _effectInstance;

		public override int iconStacks => (int)_totalDamage;

		public Instance(Character owner, OmenExecution ability)
			: base(owner, ability)
		{
			_effect = ability._effect;
		}

		protected override void OnAttach()
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			_effectInstance = ((_effect == null) ? null : _effect.Spawn(((Component)owner).transform.position, owner));
			Character character = owner;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character.onGaveDamage, new GaveDamageDelegate(HandleOnGaveDamage));
			((PriorityList<GiveDamageDelegate>)owner.onGiveDamage).Add(int.MinValue, (GiveDamageDelegate)OnGiveDamage);
			owner.onStartAction += HandleOnStartAction;
		}

		private void HandleOnStartAction(Characters.Actions.Action characterAction)
		{
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			if (_currentStep >= ability._damageStep.Length - 1 && characterAction.type == Characters.Actions.Action.Type.Skill)
			{
				_totalDamage = 0f;
				UpdateStep();
				if ((Object)(object)_effectInstance != (Object)null)
				{
					((Component)ability._summonPoint).transform.position = ((Component)_effectInstance).transform.position;
				}
				ability._onSummon.Run(owner);
				_currentStep = 0;
				_effectInstance.animator.Play(entryAnimation, 0, 0f);
				_effectInstance.animator.SetInteger("Stacks", _currentStep);
			}
		}

		private bool OnGiveDamage(ITarget target, ref Damage damage)
		{
			if (!damage.key.Equals("OmenExecution", StringComparison.OrdinalIgnoreCase))
			{
				return false;
			}
			Character character = target.character;
			if ((Object)(object)character == (Object)null)
			{
				return false;
			}
			if ((character.type == Character.Type.Adventurer || character.type == Character.Type.Boss) && character.health.percent > (double)ability._enhancedDamageBossHealthPercent * 0.01)
			{
				return false;
			}
			if (character.health.percent > (double)ability._enhancedDamageHealthPercent * 0.01)
			{
				return false;
			}
			damage.multiplier += ability._enhancedDamageMultiplier;
			return false;
		}

		private void HandleOnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
		{
			Character character = target.character;
			if (!((Object)(object)character == (Object)null) && ((EnumArray<Character.Type, bool>)ability._targetType)[character.type])
			{
				_totalDamage += (float)damageDealt;
				UpdateStep();
			}
		}

		private void UpdateStep()
		{
			if (_currentStep >= ability._damageStep.Length - 1 && (float)ability._damageStep[_currentStep] <= _totalDamage)
			{
				return;
			}
			_currentStep = 0;
			for (int num = ability._damageStep.Length - 1; num >= 0; num--)
			{
				if ((float)ability._damageStep[num] <= _totalDamage)
				{
					_currentStep = num;
					break;
				}
			}
			_effectInstance.animator.SetInteger("Stacks", _currentStep);
		}

		protected override void OnDetach()
		{
			if ((Object)(object)_effectInstance != (Object)null)
			{
				_effectInstance.Stop();
				_effectInstance = null;
			}
			Character character = owner;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(character.onGaveDamage, new GaveDamageDelegate(HandleOnGaveDamage));
			((PriorityList<GiveDamageDelegate>)owner.onGiveDamage).Remove((GiveDamageDelegate)OnGiveDamage);
			owner.onStartAction -= HandleOnStartAction;
		}
	}

	[SerializeField]
	[Tooltip("혹시 단계별 이펙트를 바꾸고 싶을 경우 Duel 애니메이터의 Transition과 Parameter를 수정하면 됨")]
	[Header("연출")]
	private EffectInfo _effect = new EffectInfo
	{
		subordinated = true
	};

	[Header("필터")]
	[SerializeField]
	private CharacterTypeBoolArray _targetType;

	[Header("설정")]
	[SerializeField]
	private int[] _damageStep;

	[SerializeField]
	[Range(0f, 100f)]
	private int _enhancedDamageHealthPercent;

	[Range(0f, 100f)]
	[SerializeField]
	private int _enhancedDamageBossHealthPercent;

	[SerializeField]
	[Information(/*Could not decode attribute arguments.*/)]
	private float _enhancedDamageMultiplier;

	[Header("단두대")]
	[SerializeField]
	private Transform _summonPoint;

	[SerializeField]
	[CharacterOperation.Subcomponent]
	private CharacterOperation.Subcomponents _onSummon;

	public override void Initialize()
	{
		base.Initialize();
		_onSummon.Initialize();
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
