using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

public sealed class WaitWhenTaskDone : Action
{
	public Task[] tasks;

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

	private float _waitDuration;

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
			_waitDuration = Random.Range(((SharedVariable<float>)randomWaitMin).Value, ((SharedVariable<float>)randomWaitMax).Value);
		}
		else
		{
			_waitDuration = ((SharedVariable<float>)waitTime).Value;
		}
		_remainTime = _waitDuration;
	}

	public override TaskStatus OnUpdate()
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Invalid comparison between Unknown and I4
		bool flag = true;
		Task[] array = tasks;
		for (int i = 0; i < array.Length; i++)
		{
			if ((int)array[i].NodeData.ExecutionStatus == 3)
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			_remainTime -= ((ChronometerBase)_choronometer).deltaTime;
		}
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
