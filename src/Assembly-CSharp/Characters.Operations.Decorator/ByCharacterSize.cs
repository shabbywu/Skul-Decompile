using UnityEngine;

namespace Characters.Operations.Decorator;

public sealed class ByCharacterSize : CharacterOperation
{
	[SerializeField]
	[Subcomponent]
	private CharacterOperation _small;

	[Subcomponent]
	[SerializeField]
	private CharacterOperation _medium;

	[Subcomponent]
	[SerializeField]
	private CharacterOperation _large;

	[SerializeField]
	[Subcomponent]
	private CharacterOperation _extraLarge;

	[Subcomponent]
	[SerializeField]
	private CharacterOperation _none;

	public override void Initialize()
	{
		if ((Object)(object)_small != (Object)null)
		{
			_small.Initialize();
		}
		if ((Object)(object)_medium != (Object)null)
		{
			_medium.Initialize();
		}
		if ((Object)(object)_large != (Object)null)
		{
			_large.Initialize();
		}
		if ((Object)(object)_extraLarge != (Object)null)
		{
			_extraLarge.Initialize();
		}
		if ((Object)(object)_none != (Object)null)
		{
			_none.Initialize();
		}
	}

	public override void Run(Character owner)
	{
		if (owner.sizeForEffect == Character.SizeForEffect.Small && (Object)(object)_small != (Object)null)
		{
			_small.Run(owner);
		}
		else if (owner.sizeForEffect == Character.SizeForEffect.Medium && (Object)(object)_medium != (Object)null)
		{
			_medium.Run(owner);
		}
		else if (owner.sizeForEffect == Character.SizeForEffect.Large && (Object)(object)_large != (Object)null)
		{
			_large.Run(owner);
		}
		else if (owner.sizeForEffect == Character.SizeForEffect.ExtraLarge && (Object)(object)_extraLarge != (Object)null)
		{
			_extraLarge.Run(owner);
		}
		else if (owner.sizeForEffect == Character.SizeForEffect.None && (Object)(object)_none != (Object)null)
		{
			_none.Run(owner);
		}
	}

	public override void Stop()
	{
		if ((Object)(object)_small != (Object)null)
		{
			_small.Stop();
		}
		if ((Object)(object)_medium != (Object)null)
		{
			_medium.Stop();
		}
		if ((Object)(object)_large != (Object)null)
		{
			_large.Stop();
		}
		if ((Object)(object)_extraLarge != (Object)null)
		{
			_extraLarge.Stop();
		}
		if ((Object)(object)_none != (Object)null)
		{
			_none.Stop();
		}
	}
}
