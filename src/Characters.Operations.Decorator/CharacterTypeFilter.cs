using UnityEditor;
using UnityEngine;

namespace Characters.Operations.Decorator;

public sealed class CharacterTypeFilter : CharacterOperation
{
	[SerializeField]
	private CharacterTypeBoolArray _characterType;

	[SerializeField]
	[Subcomponent(typeof(TargetedOperationInfo))]
	private TargetedOperationInfo.Subcomponents _operations;

	public override void Initialize()
	{
		_operations.Initialize();
	}

	public override void Run(Character owner)
	{
		if (((EnumArray<Character.Type, bool>)_characterType)[owner.type])
		{
			Run(owner, owner);
		}
	}

	public override void Run(Character owner, Character target)
	{
		if (((EnumArray<Character.Type, bool>)_characterType)[target.type])
		{
			((MonoBehaviour)this).StartCoroutine(_operations.CRun(owner, target));
		}
	}

	public override void Stop()
	{
		((MonoBehaviour)this).StopAllCoroutines();
	}
}
