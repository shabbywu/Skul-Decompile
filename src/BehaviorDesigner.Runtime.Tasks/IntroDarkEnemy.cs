using Characters;
using Characters.Abilities.Darks;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

public sealed class IntroDarkEnemy : Action
{
	[SerializeField]
	private SharedCharacter _owner;

	private Character _ownerValue;

	private DarkEnemy _darkEnemy;

	public override void OnAwake()
	{
		_ownerValue = ((SharedVariable<Character>)_owner).Value;
	}

	public override void OnStart()
	{
		_darkEnemy = ((Component)_ownerValue).GetComponent<DarkEnemy>();
		if ((Object)(object)_darkEnemy == (Object)null)
		{
			Debug.LogError((object)"Not has DarkEnemy Component");
		}
		else if (!((Behaviour)_darkEnemy).enabled)
		{
			Debug.LogError((object)"Disabled DarkEnemy Component");
		}
	}

	public override TaskStatus OnUpdate()
	{
		_darkEnemy.RunIntro();
		return (TaskStatus)2;
	}
}
