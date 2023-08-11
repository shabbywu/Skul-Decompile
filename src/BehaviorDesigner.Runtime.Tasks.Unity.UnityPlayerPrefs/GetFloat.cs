using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityPlayerPrefs;

[TaskDescription("Stores the value with the specified key from the PlayerPrefs.")]
[TaskCategory("Unity/PlayerPrefs")]
public class GetFloat : Action
{
	[Tooltip("The key to store")]
	public SharedString key;

	[Tooltip("The default value")]
	public SharedFloat defaultValue;

	[RequiredField]
	[Tooltip("The value retrieved from the PlayerPrefs")]
	public SharedFloat storeResult;

	public override TaskStatus OnUpdate()
	{
		((SharedVariable<float>)storeResult).Value = PlayerPrefs.GetFloat(((SharedVariable<string>)key).Value, ((SharedVariable<float>)defaultValue).Value);
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		key = "";
		defaultValue = 0f;
		storeResult = 0f;
	}
}
