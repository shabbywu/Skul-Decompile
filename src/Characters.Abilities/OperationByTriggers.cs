using System;
using System.Collections.Generic;
using Characters.Abilities.Triggers;
using Characters.Operations;
using UnityEngine;

namespace Characters.Abilities;

[Serializable]
public class OperationByTriggers : Ability
{
	public class Instance : AbilityInstance<OperationByTriggers>
	{
		private float _remainCooldownTime;

		private int _index;

		public override Sprite icon
		{
			get
			{
				if (_index < ability._iconActiveTriggerIndex)
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
				if (ability._cooldownTime > 0f)
				{
					return 1f - _remainCooldownTime / ability._cooldownTime;
				}
				return base.iconFillAmount;
			}
		}

		public Instance(Character owner, OperationByTriggers ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			_index = 0;
			ability.triggerComponents[_index].Attach(owner);
			ability.triggerComponents[_index].onTriggered += OnTriggered;
		}

		protected override void OnDetach()
		{
			ability.triggerComponents[_index].Detach();
			ability.triggerComponents[_index].onTriggered -= OnTriggered;
		}

		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
			_remainCooldownTime -= deltaTime;
			ability.triggerComponents[_index].UpdateTime(deltaTime);
		}

		private void OnTriggered()
		{
			if (ability.triggerComponents.Count - 1 == _index && _remainCooldownTime > 0f)
			{
				return;
			}
			ability.triggerComponents[_index].Detach();
			ability.triggerComponents[_index].onTriggered -= OnTriggered;
			if (_index < ability.triggerComponents.Count - 1)
			{
				_index++;
				ability.triggerComponents[_index].Attach(owner);
				ability.triggerComponents[_index].onTriggered += OnTriggered;
				return;
			}
			_index = 0;
			ability.triggerComponents[_index].Attach(owner);
			ability.triggerComponents[_index].onTriggered += OnTriggered;
			for (int i = 0; i < ability.operations.Length; i++)
			{
				if (ability._stopOperationOnRun)
				{
					ability.operations[i].Stop();
				}
				ability.operations[i].Run(owner);
			}
			_remainCooldownTime = ability._cooldownTime;
		}
	}

	[Serializable]
	private class Triggers
	{
		[SerializeField]
		[TriggerComponent.Subcomponent]
		private TriggerComponent _triggerComponent;

		public TriggerComponent component => _triggerComponent;
	}

	[SerializeField]
	private int _iconActiveTriggerIndex;

	[SerializeField]
	private Triggers[] _triggers;

	private List<TriggerComponent> triggerComponents;

	[NonSerialized]
	public CharacterOperation[] operations;

	[SerializeField]
	private float _cooldownTime;

	[SerializeField]
	private bool _stopOperationOnRun;

	public override void Initialize()
	{
		base.Initialize();
		for (int i = 0; i < operations.Length; i++)
		{
			operations[i].Initialize();
		}
		triggerComponents = new List<TriggerComponent>();
		Triggers[] triggers = _triggers;
		foreach (Triggers triggers2 in triggers)
		{
			triggerComponents.Add(triggers2.component);
		}
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
