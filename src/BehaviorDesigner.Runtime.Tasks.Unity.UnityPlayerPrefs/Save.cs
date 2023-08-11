using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityPlayerPrefs;

[TaskDescription("Saves the PlayerPrefs.")]
[TaskCategory("Unity/PlayerPrefs")]
public class Save : Action
{
	public override TaskStatus OnUpdate()
	{
		PlayerPrefs.Save();
		return (TaskStatus)2;
	}
}
