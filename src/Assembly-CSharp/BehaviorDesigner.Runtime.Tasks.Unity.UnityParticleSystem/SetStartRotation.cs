using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityParticleSystem;

[TaskDescription("Sets the start rotation of the Particle System.")]
[TaskCategory("Unity/ParticleSystem")]
public class SetStartRotation : Action
{
	[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
	public SharedGameObject targetGameObject;

	[Tooltip("The start rotation of the ParticleSystem")]
	public SharedFloat startRotation;

	private ParticleSystem particleSystem;

	private GameObject prevGameObject;

	public override void OnStart()
	{
		GameObject defaultGameObject = ((Task)this).GetDefaultGameObject(((SharedVariable<GameObject>)targetGameObject).Value);
		if ((Object)(object)defaultGameObject != (Object)(object)prevGameObject)
		{
			particleSystem = defaultGameObject.GetComponent<ParticleSystem>();
			prevGameObject = defaultGameObject;
		}
	}

	public override TaskStatus OnUpdate()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)particleSystem == (Object)null)
		{
			Debug.LogWarning((object)"ParticleSystem is null");
			return (TaskStatus)1;
		}
		MainModule main = particleSystem.main;
		((MainModule)(ref main)).startRotation = MinMaxCurve.op_Implicit(((SharedVariable<float>)startRotation).Value);
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		targetGameObject = null;
		startRotation = 0f;
	}
}
