using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

public class LiveAndActive : Conditional
{
	[SerializeField]
	private SharedCharacter _owner;

	private Character _ownerValue;

	public override void OnStart()
	{
		_ownerValue = ((SharedVariable<Character>)_owner).Value;
	}

	public override TaskStatus OnUpdate()
	{
		if (!_ownerValue.liveAndActive)
		{
			return (TaskStatus)1;
		}
		return (TaskStatus)2;
	}
}
