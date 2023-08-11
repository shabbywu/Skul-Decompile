using System.Collections;
using Characters.Abilities.Triggers;
using UnityEngine;

namespace Characters.Abilities;

public class TriggerAbilityAttacher : AbilityAttacher
{
	[SerializeField]
	[TriggerComponent.Subcomponent]
	private TriggerComponent _trigger;

	[SerializeField]
	[AbilityComponent.Subcomponent]
	private AbilityComponent _abilityComponent;

	private CoroutineReference _cUpdateReference;

	public bool attached { get; private set; }

	private void Awake()
	{
		_trigger.onTriggered += OnTriggered;
	}

	private void OnTriggered()
	{
		base.owner.ability.Add(_abilityComponent.ability);
	}

	public override void OnIntialize()
	{
		_abilityComponent.Initialize();
	}

	public override void StartAttach()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		attached = true;
		_trigger.Attach(base.owner);
		_cUpdateReference = CoroutineReferenceExtension.StartCoroutineWithReference((MonoBehaviour)(object)this, CUpdate());
	}

	public override void StopAttach()
	{
		attached = false;
		if (!((Object)(object)base.owner == (Object)null))
		{
			_trigger.Detach();
			((CoroutineReference)(ref _cUpdateReference)).Stop();
			base.owner.ability.Remove(_abilityComponent.ability);
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
