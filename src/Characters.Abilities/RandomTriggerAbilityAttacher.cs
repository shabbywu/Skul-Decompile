using System.Collections;
using System.Collections.Generic;
using Characters.Abilities.Triggers;
using UnityEngine;

namespace Characters.Abilities;

public class RandomTriggerAbilityAttacher : AbilityAttacher
{
	[SerializeField]
	[TriggerComponent.Subcomponent]
	private TriggerComponent _trigger;

	[SerializeField]
	[AbilityComponent.Subcomponent]
	private AbilityComponent.Subcomponents _abilityComponents;

	private CoroutineReference _cUpdateReference;

	private void Awake()
	{
		_trigger.onTriggered += OnTriggered;
	}

	private void OnTriggered()
	{
		base.owner.ability.Add(ExtensionMethods.Random<AbilityComponent>((IEnumerable<AbilityComponent>)((SubcomponentArray<AbilityComponent>)_abilityComponents).components).ability);
	}

	public override void OnIntialize()
	{
		_abilityComponents.Initialize();
	}

	public override void StartAttach()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		_trigger.Attach(base.owner);
		_cUpdateReference = CoroutineReferenceExtension.StartCoroutineWithReference((MonoBehaviour)(object)this, CUpdate());
	}

	public override void StopAttach()
	{
		if (!((Object)(object)base.owner == (Object)null))
		{
			_trigger.Detach();
			((CoroutineReference)(ref _cUpdateReference)).Stop();
			AbilityComponent[] components = ((SubcomponentArray<AbilityComponent>)_abilityComponents).components;
			foreach (AbilityComponent abilityComponent in components)
			{
				base.owner.ability.Remove(abilityComponent.ability);
			}
		}
	}

	private IEnumerator CUpdate()
	{
		while (true)
		{
			_trigger.UpdateTime(((ChronometerBase)Chronometer.global).deltaTime);
			yield return null;
		}
	}

	public override string ToString()
	{
		return ExtensionMethods.GetAutoName((object)this);
	}
}
