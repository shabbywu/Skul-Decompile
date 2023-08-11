using Hardmode;
using Singletons;
using UnityEngine;

namespace Characters.Operations.Decorator;

public sealed class InHardmode : CharacterOperation
{
	[Subcomponent]
	[SerializeField]
	private Subcomponents _inHardmode;

	[SerializeField]
	[Subcomponent]
	private Subcomponents _inNormal;

	public override void Initialize()
	{
		_inHardmode.Initialize();
		_inNormal.Initialize();
	}

	public override void Run(Character owner)
	{
		if (Singleton<HardmodeManager>.Instance.hardmode)
		{
			_inHardmode.Run(owner);
		}
		else
		{
			_inNormal.Run(owner);
		}
	}

	public override void Run(Character owner, Character target)
	{
		if (Singleton<HardmodeManager>.Instance.hardmode)
		{
			_inHardmode.Run(owner);
		}
		else
		{
			_inNormal.Run(owner);
		}
	}

	public override void Stop()
	{
		_inHardmode.Stop();
		_inNormal.Stop();
	}
}
