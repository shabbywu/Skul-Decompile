using System;
using System.Collections.Generic;
using Characters.Abilities;
using Characters.Actions;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions;

public sealed class Rapidity : InscriptionInstance
{
	[Serializable]
	private sealed class RapidityAttack : Ability
	{
		private sealed class Instance : AbilityInstance<RapidityAttack>
		{
			private List<float> _attackTimeHistory;

			private int _topPoint = -1;

			private int _bottomPoint = -1;

			private float _remainCooldownTime;

			public override float iconFillAmount => _remainCooldownTime / ability._cooldownTime;

			public Instance(Character owner, RapidityAttack ability)
				: base(owner, ability)
			{
				_attackTimeHistory = new List<float>(ability._triggerAttackCount);
			}

			protected override void OnAttach()
			{
				owner.onStartAction += HandleOnStartAction;
			}

			protected override void OnDetach()
			{
				owner.onStartAction -= HandleOnStartAction;
			}

			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				_remainCooldownTime -= deltaTime;
			}

			private void HandleOnStartAction(Characters.Actions.Action action)
			{
				if (!((EnumArray<Characters.Actions.Action.Type, bool>)ability._triggerActionFilter)[action.type] || _remainCooldownTime > 0f)
				{
					return;
				}
				_topPoint = (_topPoint + 1) % _attackTimeHistory.Capacity;
				if (_attackTimeHistory.Count < _attackTimeHistory.Capacity)
				{
					_attackTimeHistory.Add(Time.time);
				}
				if (_attackTimeHistory.Count >= _attackTimeHistory.Capacity)
				{
					_bottomPoint = (_bottomPoint + 1) % _attackTimeHistory.Capacity;
					_attackTimeHistory[_topPoint] = Time.time;
					if (_topPoint != _bottomPoint && !(_attackTimeHistory[_topPoint] - _attackTimeHistory[_bottomPoint] > ability._triggerInterval))
					{
						_remainCooldownTime = ability._cooldownTime;
						((MonoBehaviour)owner).StartCoroutine(ability._onStep2Invoke.CRun(owner));
					}
				}
			}
		}

		[SerializeField]
		private ActionTypeBoolArray _triggerActionFilter;

		[SerializeField]
		private float _triggerInterval;

		[SerializeField]
		private int _triggerAttackCount;

		[SerializeField]
		private float _cooldownTime;

		[SerializeField]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _onStep2Invoke;

		public override void Initialize()
		{
			base.Initialize();
			_onStep2Invoke.Initialize();
		}

		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new Instance(owner, this);
		}
	}

	[Header("2세트 효과")]
	[SerializeField]
	[Subcomponent(typeof(TriggerAbilityAttacher))]
	private TriggerAbilityAttacher _step1Ability;

	[Header("4세트 효과")]
	[SerializeField]
	private RapidityAttack _rapidityAttack;

	protected override void Initialize()
	{
		_step1Ability.Initialize(base.character);
		_rapidityAttack.Initialize();
	}

	public override void UpdateBonus(bool wasActive, bool wasOmen)
	{
		if (keyword.isMaxStep)
		{
			if (!base.character.ability.Contains(_rapidityAttack))
			{
				base.character.ability.Add(_rapidityAttack);
			}
		}
		else if (base.character.ability.Contains(_rapidityAttack))
		{
			base.character.ability.Remove(_rapidityAttack);
		}
	}

	public override void Attach()
	{
		_step1Ability.StartAttach();
	}

	public override void Detach()
	{
		_step1Ability.StopAttach();
	}
}
