using System.Collections.Generic;
using UnityEngine;

namespace Characters.Operations.Decorator;

public class Random : CharacterOperation
{
	[SerializeField]
	[Subcomponent]
	private Subcomponents _toRandom;

	public override void Initialize()
	{
		_toRandom.Initialize();
	}

	public override void Run(Character owner)
	{
		CharacterOperation characterOperation = ExtensionMethods.Random<CharacterOperation>((IEnumerable<CharacterOperation>)((SubcomponentArray<CharacterOperation>)_toRandom).components);
		if (!((Object)(object)characterOperation == (Object)null))
		{
			characterOperation.Run(owner);
		}
	}

	public override void Stop()
	{
		_toRandom.Stop();
	}
}
