using System;
using Characters.Operations;
using UnityEngine;

namespace Characters.Abilities;

[Serializable]
public class OperationByTrigger : Ability
{
	public class Instance : AbilityInstance<OperationByTrigger>
	{
		private int _count;

		private float _remainCooldownTime;

		public override Sprite icon
		{
			get
			{
				Sprite result = base.icon;
				if (ability._triggerCount > 0)
				{
					if (_count == 0)
					{
						result = null;
					}
				}
				else
				{
					if (!(ability._cooldownTime > 0f))
					{
						return result;
					}
					if (_remainCooldownTime <= 0f)
					{
						result = null;
					}
				}
				return result;
			}
		}

		public override int iconStacks
		{
			get
			{
				if (_count <= 0)
				{
					return base.iconStacks;
				}
				return _count;
			}
		}

		public override float iconFillAmount
		{
			get
			{
				if (ability._cooldownTime > 0f)
				{
					return 1f - _remainCooldownTime / ability._cooldownTime;
				}
				return base.iconFillAmount;
			}
		}

		public Instance(Character owner, OperationByTrigger ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			if (!ability._onAwake)
			{
				_remainCooldownTime = ability._cooldownTime;
			}
			ability.trigger.Attach(owner);
			ability.trigger.onTriggered += OnTriggered;
		}

		protected override void OnDetach()
		{
			ability.trigger.Detach();
			ability.trigger.onTriggered -= OnTriggered;
		}

		public override void Refresh()
		{
			base.Refresh();
			ability.trigger.Refresh();
		}

		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
			_remainCooldownTime -= deltaTime;
			ability.trigger.UpdateTime(deltaTime);
		}

		private void OnTriggered()
		{
			if (_remainCooldownTime > 0f)
			{
				return;
			}
			if (_count < ability._triggerCount)
			{
				_count++;
				return;
			}
			_count = 0;
			_remainCooldownTime = ability._cooldownTime;
			for (int i = 0; i < ability.operations.Length; i++)
			{
				if (ability._stopOperationOnRun)
				{
					ability.operations[i].Stop();
				}
				ability.operations[i].Run(owner);
			}
		}
	}

	public ITrigger trigger;

	[NonSerialized]
	public CharacterOperation[] operations;

	[SerializeField]
	private bool _onAwake = true;

	[SerializeField]
	private int _triggerCount;

	[SerializeField]
	private float _cooldownTime;

	[SerializeField]
	private bool _stopOperationOnRun;

	public OperationByTrigger()
	{
	}

	public OperationByTrigger(ITrigger trigger, CharacterOperation[] operations)
	{
		this.trigger = trigger;
		this.operations = operations;
	}

	public override void Initialize()
	{
		base.Initialize();
		for (int i = 0; i < operations.Length; i++)
		{
			operations[i].Initialize();
		}
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
