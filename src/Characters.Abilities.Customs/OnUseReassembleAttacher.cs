using System.Collections;
using Data;
using UnityEngine;

namespace Characters.Abilities.Customs;

public sealed class OnUseReassembleAttacher : AbilityAttacher
{
	private const float _checkInterval = 0.2f;

	[AbilityComponent.Subcomponent]
	[SerializeField]
	private AbilityComponent _abilityComponent;

	private CoroutineReference _cUpdateReference;

	public override void OnIntialize()
	{
		_abilityComponent.Initialize();
	}

	public override void StartAttach()
	{
		_cUpdateReference = ((MonoBehaviour)(object)this).StartCoroutineWithReference(CCheckLoop());
	}

	private IEnumerator CCheckLoop()
	{
		while (!GameData.Progress.reassembleUsed)
		{
			yield return Chronometer.global.WaitForSeconds(0.2f);
		}
		Attach();
	}

	private void Attach()
	{
		base.owner.ability.Add(_abilityComponent.ability);
	}

	public override void StopAttach()
	{
		_cUpdateReference.Stop();
		if ((Object)(object)base.owner != (Object)null)
		{
			base.owner.ability.Remove(_abilityComponent.ability);
		}
	}
}
