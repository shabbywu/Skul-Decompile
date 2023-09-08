using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

[TaskDescription("Target과의 수평거리가 distance 보다 작은가")]
public class HorizontalCloseness : Conditional
{
	[SerializeField]
	[Tooltip("행동 주체")]
	private SharedCharacter _owner;

	[Tooltip("타겟")]
	[SerializeField]
	private SharedCharacter _target;

	[Tooltip("거리")]
	[SerializeField]
	private SharedFloat _distance;

	private Character _ownerValue;

	private float _distanceValue;

	public override void OnAwake()
	{
		_ownerValue = ((SharedVariable<Character>)_owner).Value;
		_distanceValue = ((SharedVariable<float>)_distance).Value;
	}

	public override TaskStatus OnUpdate()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		Character value = ((SharedVariable<Character>)_target).Value;
		if (!((Object)(object)value == (Object)null))
		{
			if (!(Mathf.Abs(((Component)_ownerValue).transform.position.x - ((Component)value).transform.position.x) < _distanceValue))
			{
				return (TaskStatus)1;
			}
			return (TaskStatus)2;
		}
		return (TaskStatus)1;
	}
}
