using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

[TaskIcon("{SkinColor}WaitIcon.png")]
[TaskDescription("Wait a specified amount of time. The task will return running until the task is done waiting. It will return success after the wait time has elapsed.")]
public class Wait : Action
{
	[Tooltip("The amount of time to wait")]
	public SharedFloat waitTime = 1f;

	[Tooltip("Should the wait be randomized?")]
	public SharedBool randomWait = false;

	[Tooltip("The minimum wait time if random wait is enabled")]
	public SharedFloat randomWaitMin = 1f;

	[Tooltip("The maximum wait time if random wait is enabled")]
	public SharedFloat randomWaitMax = 1f;

	[SerializeField]
	private SharedCharacter _chronometerReference;

	private float waitDuration;

	private float startTime;

	private float pauseTime;

	private float _remainTime;

	private Chronometer _choronometer;

	public override void OnStart()
	{
		if (_chronometerReference != null && (Object)(object)((SharedVariable<Character>)_chronometerReference).Value != (Object)null)
		{
			_choronometer = ((SharedVariable<Character>)_chronometerReference).Value.chronometer.master;
		}
		if (((SharedVariable<bool>)randomWait).Value)
		{
			waitDuration = Random.Range(((SharedVariable<float>)randomWaitMin).Value, ((SharedVariable<float>)randomWaitMax).Value);
		}
		else
		{
			waitDuration = ((SharedVariable<float>)waitTime).Value;
		}
		_remainTime = waitDuration;
	}

	public override TaskStatus OnUpdate()
	{
		_remainTime -= ((ChronometerBase)_choronometer).deltaTime;
		if (!(_remainTime <= 0f))
		{
			return (TaskStatus)3;
		}
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		waitTime = 1f;
		randomWait = false;
		randomWaitMin = 1f;
		randomWaitMax = 1f;
	}
}
