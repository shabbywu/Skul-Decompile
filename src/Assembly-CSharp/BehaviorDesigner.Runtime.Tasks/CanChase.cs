using Characters;
using Characters.AI;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

public class CanChase : Conditional
{
	[SerializeField]
	[Tooltip("행동 주체")]
	private SharedCharacter _character;

	[SerializeField]
	private SharedCharacter _target;

	private Character _characterValue;

	public override void OnAwake()
	{
		_characterValue = ((SharedVariable<Character>)_character).Value;
	}

	public override TaskStatus OnUpdate()
	{
		if (Precondition.CanChase(_characterValue, ((SharedVariable<Character>)_target).Value))
		{
			return (TaskStatus)2;
		}
		return (TaskStatus)1;
	}
}
