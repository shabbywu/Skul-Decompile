using UnityEngine;
using Utils;

namespace BehaviorDesigner.Runtime.Tasks;

[TaskDescription("Grab Board에 성공한 대상이 있는지 확인합니다.")]
public sealed class Grabbed : Conditional
{
	[SerializeField]
	private SharedGrabBoard _grabBoard;

	public override TaskStatus OnUpdate()
	{
		if (_grabBoard != null)
		{
			if (((SharedVariable<GrabBoard>)_grabBoard).Value.targets.Count <= 0)
			{
				return (TaskStatus)1;
			}
			return (TaskStatus)2;
		}
		return (TaskStatus)1;
	}
}
