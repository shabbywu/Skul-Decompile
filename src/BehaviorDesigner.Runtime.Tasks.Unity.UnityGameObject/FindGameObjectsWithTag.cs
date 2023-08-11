using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;

[TaskCategory("Unity/GameObject")]
[TaskDescription("Finds a GameObject by tag. Returns Success.")]
public class FindGameObjectsWithTag : Action
{
	[Tooltip("The tag of the GameObject to find")]
	public SharedString tag;

	[RequiredField]
	[Tooltip("The objects found by name")]
	public SharedGameObjectList storeValue;

	public override TaskStatus OnUpdate()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag(((SharedVariable<string>)tag).Value);
		for (int i = 0; i < array.Length; i++)
		{
			((SharedVariable<List<GameObject>>)storeValue).Value.Add(array[i]);
		}
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		((SharedVariable<string>)tag).Value = null;
		((SharedVariable<List<GameObject>>)storeValue).Value = null;
	}
}
