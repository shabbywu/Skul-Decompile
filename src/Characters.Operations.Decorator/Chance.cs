using UnityEngine;

namespace Characters.Operations.Decorator;

public class Chance : CharacterOperation
{
	[SerializeField]
	[Range(0f, 1f)]
	private float _successChance = 0.5f;

	[Subcomponent]
	[SerializeField]
	private CharacterOperation _onSuccess;

	[Subcomponent]
	[SerializeField]
	private CharacterOperation _onFail;

	public override void Initialize()
	{
		if ((Object)(object)_onSuccess != (Object)null)
		{
			_onSuccess.Initialize();
		}
		if ((Object)(object)_onFail != (Object)null)
		{
			_onFail.Initialize();
		}
	}

	public override void Run(Character owner)
	{
		if (MMMaths.Chance(_successChance))
		{
			if (!((Object)(object)_onSuccess == (Object)null))
			{
				_onSuccess.Stop();
				_onSuccess.Run(owner);
			}
		}
		else if (!((Object)(object)_onFail == (Object)null))
		{
			_onFail.Stop();
			_onFail.Run(owner);
		}
	}

	public override void Stop()
	{
		if ((Object)(object)_onSuccess != (Object)null)
		{
			_onSuccess.Stop();
		}
		if ((Object)(object)_onFail != (Object)null)
		{
			_onFail.Stop();
		}
	}
}
