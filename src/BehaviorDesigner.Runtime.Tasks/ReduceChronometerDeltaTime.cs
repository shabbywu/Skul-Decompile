using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

[TaskDescription("Time에서 Owner의 Master Chronometer의 DeltaTime만큼 감소시킴")]
[TaskIcon("Assets/Behavior Designer/Icon/ReduceChronometerDeltaTime.png")]
public sealed class ReduceChronometerDeltaTime : Action
{
	[SerializeField]
	private SharedCharacter _owner;

	[SerializeField]
	private SharedFloat _time;

	public override TaskStatus OnUpdate()
	{
		Character value = ((SharedVariable<Character>)_owner).Value;
		((SharedVariable)_time).SetValue((object)(((SharedVariable<float>)_time).Value - ((ChronometerBase)value.chronometer.master).deltaTime));
		return (TaskStatus)2;
	}
}
