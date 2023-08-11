using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimation;

[TaskCategory("Unity/Animation")]
[TaskDescription("Fades the animation over a period of time and fades other animations out. Returns Success.")]
public class CrossFade : Action
{
	[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
	public SharedGameObject targetGameObject;

	[Tooltip("The name of the animation")]
	public SharedString animationName;

	[Tooltip("The speed of the animation. Use a negative value to play the animation backwards")]
	public SharedFloat animationSpeed = 1f;

	[Tooltip("The amount of time it takes to blend")]
	public SharedFloat fadeLength = 0.3f;

	[Tooltip("The play mode of the animation")]
	public PlayMode playMode;

	private Animation animation;

	private GameObject prevGameObject;

	public override void OnStart()
	{
		GameObject defaultGameObject = ((Task)this).GetDefaultGameObject(((SharedVariable<GameObject>)targetGameObject).Value);
		if ((Object)(object)defaultGameObject != (Object)(object)prevGameObject)
		{
			animation = defaultGameObject.GetComponent<Animation>();
			prevGameObject = defaultGameObject;
		}
	}

	public override TaskStatus OnUpdate()
	{
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)animation == (Object)null)
		{
			Debug.LogWarning((object)"Animation is null");
			return (TaskStatus)1;
		}
		animation[((SharedVariable<string>)animationName).Value].speed = ((SharedVariable<float>)animationSpeed).Value;
		if (animation[((SharedVariable<string>)animationName).Value].speed < 0f)
		{
			animation[((SharedVariable<string>)animationName).Value].time = animation[((SharedVariable<string>)animationName).Value].length;
		}
		animation.CrossFade(((SharedVariable<string>)animationName).Value, ((SharedVariable<float>)fadeLength).Value, playMode);
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		targetGameObject = null;
		((SharedVariable<string>)animationName).Value = "";
		animationSpeed = 1f;
		fadeLength = 0.3f;
		playMode = (PlayMode)0;
	}
}
