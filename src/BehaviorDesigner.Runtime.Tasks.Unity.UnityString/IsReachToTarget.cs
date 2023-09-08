using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityString;

[TaskDescription("Target이 가까운지 체크")]
public class IsReachToTarget : Conditional
{
	[SerializeField]
	private SharedCharacter _owner;

	[SerializeField]
	private SharedCharacter _target;

	private Character _ownerValue;

	public override void OnAwake()
	{
		_ownerValue = ((SharedVariable<Character>)_owner).Value;
	}

	public override TaskStatus OnUpdate()
	{
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		Character value = ((SharedVariable<Character>)_target).Value;
		float num = _ownerValue.stat.GetInterpolatedMovementSpeed() * _ownerValue.chronometer.animation.deltaTime;
		if (!((Object)(object)_ownerValue == (Object)null) && !((Object)(object)value == (Object)null))
		{
			if (((Component)_ownerValue).transform.position.x + num >= ((Component)value).transform.position.x && ((Component)_ownerValue).transform.position.x - num <= ((Component)value).transform.position.x)
			{
				return (TaskStatus)2;
			}
			return (TaskStatus)1;
		}
		return (TaskStatus)1;
	}
}
