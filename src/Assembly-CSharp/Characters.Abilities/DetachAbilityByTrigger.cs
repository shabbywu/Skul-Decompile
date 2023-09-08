using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Abilities.Triggers;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities;

[Serializable]
public class DetachAbilityByTrigger : Ability
{
	public class Instance : AbilityInstance<DetachAbilityByTrigger>
	{
		private float _remainDetachTime;

		private bool _detached;

		private int _index;

		public override Sprite icon
		{
			get
			{
				if (!_detached)
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
				if (ability._onOffFiilAmount)
				{
					return (_remainDetachTime > 0f) ? 1 : 0;
				}
				if (_remainDetachTime > 0f)
				{
					return 1f - _remainDetachTime / ability._detachTime;
				}
				return base.iconFillAmount;
			}
		}

		public Instance(Character owner, DetachAbilityByTrigger ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			_index = 0;
			owner.ability.Add(ability._abilityComponent.ability);
			ability.triggerComponents[_index].Attach(owner);
			ability.triggerComponents[_index].onTriggered += OnTriggered;
		}

		protected override void OnDetach()
		{
			((MonoBehaviour)owner).StartCoroutine(CDelayDetach());
			ability.triggerComponents[_index].Detach();
			ability.triggerComponents[_index].onTriggered -= OnTriggered;
		}

		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
			if (_detached)
			{
				_remainDetachTime -= deltaTime;
				if (_remainDetachTime < 0f)
				{
					owner.ability.Add(ability._abilityComponent.ability);
					((MonoBehaviour)owner).StartCoroutine(ability._onAttach.CRun(owner));
					_detached = false;
				}
			}
			ability.triggerComponents[_index].UpdateTime(deltaTime);
		}

		private void OnTriggered()
		{
			if (ability.triggerComponents.Count - 1 != _index || !(_remainDetachTime > 0f))
			{
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
				owner.ability.Remove(ability._abilityComponent.ability);
				((MonoBehaviour)owner).StartCoroutine(ability._onDetach.CRun(owner));
				_remainDetachTime = ability._detachTime;
				_detached = true;
			}
		}

		public void OnDestroy()
		{
			if ((Object)(object)owner != (Object)null)
			{
				owner.ability.Remove(ability._abilityComponent.ability);
			}
		}

		private IEnumerator CDelayDetach()
		{
			yield return null;
			if (!((Object)(object)owner == (Object)null))
			{
				owner.ability.Remove(ability._abilityComponent.ability);
			}
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
	[AbilityComponent.Subcomponent]
	private AbilityComponent _abilityComponent;

	[SerializeField]
	private Triggers[] _triggers;

	[SerializeField]
	private bool _onOffFiilAmount;

	[SerializeField]
	private float _detachTime;

	[SerializeField]
	[Subcomponent(typeof(OperationInfo))]
	private OperationInfo.Subcomponents _onAttach;

	[SerializeField]
	[Subcomponent(typeof(OperationInfo))]
	private OperationInfo.Subcomponents _onDetach;

	private List<TriggerComponent> triggerComponents;

	private Instance _instance;

	public override void Initialize()
	{
		base.Initialize();
		_onAttach.Initialize();
		_onDetach.Initialize();
		_abilityComponent.Initialize();
		triggerComponents = new List<TriggerComponent>();
		Triggers[] triggers = _triggers;
		foreach (Triggers triggers2 in triggers)
		{
			triggerComponents.Add(triggers2.component);
		}
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		_instance = new Instance(owner, this);
		return _instance;
	}

	public void Destroy()
	{
		if (_instance != null)
		{
			_instance.OnDestroy();
		}
	}
}
