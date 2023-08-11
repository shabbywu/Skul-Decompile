using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityPlayerPrefs;

[TaskDescription("Deletes the specified key from the PlayerPrefs.")]
[TaskCategory("Unity/PlayerPrefs")]
public class DeleteKey : Action
{
	[Tooltip("The key to delete")]
	public SharedString key;

	public override TaskStatus OnUpdate()
	{
		PlayerPrefs.DeleteKey(((SharedVariable<string>)key).Value);
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		key = "";
	}
}
