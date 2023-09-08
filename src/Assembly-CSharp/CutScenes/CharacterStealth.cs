using Runnables;
using UnityEngine;

namespace CutScenes;

public class CharacterStealth : State
{
	[SerializeField]
	private Target _target;

	public override void Attach()
	{
		_target.character.stealth.Attach(State.key);
	}

	public override void Detach()
	{
		if (_target != null && !((Object)(object)_target.character == (Object)null))
		{
			_target.character.stealth.Detach(State.key);
		}
	}

	private void OnDisable()
	{
		Detach();
	}

	private void OnDestroy()
	{
		Detach();
	}
}
