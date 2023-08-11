using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityPlayerPrefs;

[TaskDescription("Stores the value with the specified key from the PlayerPrefs.")]
[TaskCategory("Unity/PlayerPrefs")]
public class GetInt : Action
{
	[Tooltip("The key to store")]
	public SharedString key;

	[Tooltip("The default value")]
	public SharedInt defaultValue;

	[RequiredField]
	[Tooltip("The value retrieved from the PlayerPrefs")]
	public SharedInt storeResult;

	public override TaskStatus OnUpdate()
	{
		((SharedVariable<int>)storeResult).Value = PlayerPrefs.GetInt(((SharedVariable<string>)key).Value, ((SharedVariable<int>)defaultValue).Value);
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		key = "";
		defaultValue = 0;
		storeResult = 0;
	}
}
