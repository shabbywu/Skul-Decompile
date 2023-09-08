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
		BehaviorTree component = ((Component)character).GetComponent<BehaviorTree>();
		((Behaviour)component).enabled = false;
		((MonoBehaviour)(object)character).StartCoroutineWithReference(CWaitAndThenEnable(component));
	}

	private IEnumerator CWaitAndThenEnable(BehaviorTree behaviorDesigner)
	{
		yield return (object)new WaitForSeconds(_waitSeconds.value);
		((Behaviour)behaviorDesigner).enabled = true;
	}
}
