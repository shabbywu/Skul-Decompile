using UnityEngine;

namespace Characters.Operations.Decorator;

public class Sequencer : CharacterOperation
{
	[Subcomponent]
	[SerializeField]
	private Subcomponents _operations;

	public override void Initialize()
	{
		_operations.Initialize();
	}

	public override void Run(Character owner)
	{
		CharacterOperation[] components = ((SubcomponentArray<CharacterOperation>)_operations).components;
		for (int i = 0; i < components.Length; i++)
		{
			components[i].Run(owner);
		}
	}

	public override void Run(Character owner, Character target)
	{
		CharacterOperation[] components = ((SubcomponentArray<CharacterOperation>)_operations).components;
		for (int i = 0; i < components.Length; i++)
		{
			components[i].Run(owner);
		}
	}

	public override void Stop()
	{
		_operations.Stop();
	}
}
