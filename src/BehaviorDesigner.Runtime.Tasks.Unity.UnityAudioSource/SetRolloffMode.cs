using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAudioSource;

[TaskDescription("Sets the rolloff mode of the AudioSource. Returns Success.")]
[TaskCategory("Unity/AudioSource")]
public class SetRolloffMode : Action
{
	[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
	public SharedGameObject targetGameObject;

	[Tooltip("The rolloff mode of the AudioSource")]
	public AudioRolloffMode rolloffMode;

	private AudioSource audioSource;

	private GameObject prevGameObject;

	public override void OnStart()
	{
		GameObject defaultGameObject = ((Task)this).GetDefaultGameObject(((SharedVariable<GameObject>)targetGameObject).Value);
		if ((Object)(object)defaultGameObject != (Object)(object)prevGameObject)
		{
			audioSource = defaultGameObject.GetComponent<AudioSource>();
			prevGameObject = defaultGameObject;
		}
	}

	public override TaskStatus OnUpdate()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)audioSource == (Object)null)
		{
			Debug.LogWarning((object)"AudioSource is null");
			return (TaskStatus)1;
		}
		audioSource.rolloffMode = rolloffMode;
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		targetGameObject = null;
		rolloffMode = (AudioRolloffMode)0;
	}
}
