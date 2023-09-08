using Characters;
using Characters.Abilities;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

public sealed class SetAbilityComponent : Action
{
	[SerializeField]
	private SharedCharacter _owner;

	[SerializeField]
	private SharedAbilityComponent _abilityComponent;

	[SerializeField]
	private bool _isAttach = true;

	private Character _ownerValue;

	private IAbility _ability;

	public override void OnAwake()
	{
		_ownerValue = ((SharedVariable<Character>)_owner).Value;
		_ability = ((SharedVariable<AbilityComponent>)_abilityComponent).Value.ability;
	}

	public override void OnStart()
	{
		_ability.Initialize();
	}

	public override TaskStatus OnUpdate()
	{
		if (!((Object)(object)_ownerValue == (Object)null) && _ability != null)
		{
			if (_isAttach)
			{
				_ownerValue.ability.Add(_ability);
			}
			else
			{
				_ownerValue.ability.Remove(_ability);
			}
			return (TaskStatus)2;
		}
		return (TaskStatus)1;
	}
}
