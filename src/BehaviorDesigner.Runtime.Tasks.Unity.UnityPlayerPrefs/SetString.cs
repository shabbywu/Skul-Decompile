using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityPlayerPrefs;

[TaskDescription("Sets the value with the specified key from the PlayerPrefs.")]
[TaskCategory("Unity/PlayerPrefs")]
public class SetString : Action
{
	[Tooltip("The key to store")]
	public SharedString key;

	[Tooltip("The value to set")]
	public SharedString value;

	public override TaskStatus OnUpdate()
	{
		PlayerPrefs.SetString(((SharedVariable<string>)key).Value, ((SharedVariable<string>)value).Value);
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		key = "";
		value = "";
	}
}
