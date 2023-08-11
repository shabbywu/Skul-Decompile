using System.Runtime.CompilerServices;
using Characters.Abilities;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations.Customs;

public class AttachSilence : TargetedCharacterOperation
{
	[Subcomponent(typeof(GetSilenceComponent))]
	[SerializeField]
	private GetSilenceComponent _abilityComponent;

	private IAbilityInstance _cache;

	public override void Initialize()
	{
		_abilityComponent.Initialize();
	}

	public override void Run(Character owner, Character target)
	{
		if ((Object)(object)target == (Object)null || !target.liveAndActive)
		{
			return;
		}
		if (_cache != null && _cache.attached)
		{
			RunOnCached();
			return;
		}
		_cache = target.ability.GetInstance<GetSilence>();
		if (_cache != null)
		{
			_cache.Refresh();
		}
		else
		{
			target.ability.Add(_abilityComponent.ability);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void RunOnCached()
	{
		_cache.Refresh();
	}

	public override string ToString()
	{
		return ExtensionMethods.GetAutoName((object)this);
	}
}
