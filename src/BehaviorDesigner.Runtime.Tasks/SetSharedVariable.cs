using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

public class SetSharedVariable : Action
{
	[SerializeField]
	private SharedVariable _to;

	[SerializeField]
	private SharedVariable _from;

	public override TaskStatus OnUpdate()
	{
		_to.SetValue(_from.GetValue());
		return (TaskStatus)2;
	}
}
