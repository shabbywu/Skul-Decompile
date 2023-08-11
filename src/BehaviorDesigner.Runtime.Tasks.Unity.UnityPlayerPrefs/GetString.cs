using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityPlayerPrefs;

[TaskDescription("Stores the value with the specified key from the PlayerPrefs.")]
[TaskCategory("Unity/PlayerPrefs")]
public class GetString : Action
{
	[Tooltip("The key to store")]
	public SharedString key;

	[Tooltip("The default value")]
	public SharedString defaultValue;

	[RequiredField]
	[Tooltip("The value retrieved from the PlayerPrefs")]
	public SharedString storeResult;

	public override TaskStatus OnUpdate()
	{
		((SharedVariable<string>)storeResult).Value = PlayerPrefs.GetString(((SharedVariable<string>)key).Value, ((SharedVariable<string>)defaultValue).Value);
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		key = "";
		defaultValue = "";
		storeResult = "";
	}
}
