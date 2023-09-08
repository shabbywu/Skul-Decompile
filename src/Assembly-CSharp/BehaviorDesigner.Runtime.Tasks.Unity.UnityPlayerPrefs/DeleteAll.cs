using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityPlayerPrefs;

[TaskDescription("Deletes all entries from the PlayerPrefs.")]
[TaskCategory("Unity/PlayerPrefs")]
public class DeleteAll : Action
{
	public override TaskStatus OnUpdate()
	{
		PlayerPrefs.DeleteAll();
		return (TaskStatus)2;
	}
}
