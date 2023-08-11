using System.Collections.Generic;
using UnityEngine;

namespace Characters.Operations.Decorator;

public class OneByOne : CharacterOperation
{
	[SerializeField]
	[Subcomponent]
	private Subcomponents _operations;

	[Tooltip("제일 먼저 실행될 위치를 임의로 지정")]
	[SerializeField]
	private bool _randomizeEntry = true;

	private int _index;

	public override void Initialize()
	{
		_operations.Initialize();
		if (_randomizeEntry)
		{
			_index = ExtensionMethods.RandomIndex<CharacterOperation>((IEnumerable<CharacterOperation>)((SubcomponentArray<CharacterOperation>)_operations).components);
		}
	}

	public override void Run(Character owner)
	{
		CharacterOperation characterOperation = ((SubcomponentArray<CharacterOperation>)_operations).components[_index];
		_index = (_index + 1) % ((SubcomponentArray<CharacterOperation>)_operations).components.Length;
		if (!((Object)(object)characterOperation == (Object)null))
		{
			characterOperation.Run(owner);
		}
	}

	public override void Stop()
	{
		_operations.Stop();
	}
}
