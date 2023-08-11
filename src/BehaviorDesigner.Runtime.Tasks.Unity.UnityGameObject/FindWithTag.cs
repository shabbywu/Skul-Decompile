using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;

[TaskCategory("Unity/GameObject")]
[TaskDescription("Finds a GameObject by tag. Returns success if an object is found.")]
public class FindWithTag : Action
{
	[Tooltip("The tag of the GameObject to find")]
	public SharedString tag;

	[Tooltip("Should a random GameObject be found?")]
	public SharedBool random;

	[RequiredField]
	[Tooltip("The object found by name")]
	public SharedGameObject storeValue;

	public override TaskStatus OnUpdate()
	{
		if (((SharedVariable<bool>)random).Value)
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag(((SharedVariable<string>)tag).Value);
			if (array == null || array.Length == 0)
			{
				return (TaskStatus)1;
			}
			((SharedVariable<GameObject>)storeValue).Value = array[Random.Range(0, array.Length)];
		}
		else
		{
			((SharedVariable<GameObject>)storeValue).Value = GameObject.FindWithTag(((SharedVariable<string>)tag).Value);
		}
		if ((Object)(object)((SharedVariable<GameObject>)storeValue).Value != (Object)null)
		{
			return (TaskStatus)2;
		}
		return (TaskStatus)1;
	}

	public override void OnReset()
	{
		((SharedVariable<string>)tag).Value = null;
		((SharedVariable<GameObject>)storeValue).Value = null;
	}
}
