using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityString;

public class CheckCharacterType : Conditional
{
	[SerializeField]
	private SharedCharacter _character;

	[SerializeField]
	private Character.Type _type;

	private Character _ownerValue;

	public override void OnAwake()
	{
		_ownerValue = ((SharedVariable<Character>)_character).Value;
	}

	public override TaskStatus OnUpdate()
	{
		if (_ownerValue.type != _type)
		{
			return (TaskStatus)1;
		}
		return (TaskStatus)2;
	}
}
