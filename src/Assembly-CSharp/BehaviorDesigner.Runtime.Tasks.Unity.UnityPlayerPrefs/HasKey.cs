using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityPlayerPrefs;

[TaskDescription("Retruns success if the specified key exists.")]
[TaskCategory("Unity/PlayerPrefs")]
public class HasKey : Conditional
{
	[Tooltip("The key to check")]
	public SharedString key;

	public override TaskStatus OnUpdate()
	{
		if (PlayerPrefs.HasKey(((SharedVariable<string>)key).Value))
		{
			return (TaskStatus)2;
		}
		return (TaskStatus)1;
	}

	public override void OnReset()
	{
		key = "";
	}
}
