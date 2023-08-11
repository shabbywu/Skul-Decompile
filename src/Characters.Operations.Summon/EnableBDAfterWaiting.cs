using System;
using System.Collections;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Characters.Operations.Summon;

[Serializable]
public class EnableBDAfterWaiting : IBDCharacterSetting
{
	[SerializeField]
	private CustomFloat _waitSeconds;

	public void ApplyTo(Character character)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		BehaviorTree component = ((Component)character).GetComponent<BehaviorTree>();
		((Behaviour)component).enabled = false;
		CoroutineReferenceExtension.StartCoroutineWithReference((MonoBehaviour)(object)character, CWaitAndThenEnable(component));
	}

	private IEnumerator CWaitAndThenEnable(BehaviorTree behaviorDesigner)
	{
		yield return (object)new WaitForSeconds(_waitSeconds.value);
		((Behaviour)behaviorDesigner).enabled = true;
	}
}
