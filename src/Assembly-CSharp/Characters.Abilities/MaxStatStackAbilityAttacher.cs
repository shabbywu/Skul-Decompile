using System.Collections;
using Characters.Abilities.CharacterStat;
using UnityEngine;

namespace Characters.Abilities;

public sealed class MaxStatStackAbilityAttacher : AbilityAttacher
{
	[SerializeField]
	private StackableStatBonusComponent _stackableStatBonusComponent;

	[AbilityComponent.Subcomponent]
	[SerializeField]
	private AbilityComponent _abilityComponent;

	private CoroutineReference _cUpdateReference;

	private bool _attached;

	public override void OnIntialize()
	{
		_abilityComponent.Initialize();
	}

	public override void StartAttach()
	{
		_cUpdateReference = ((MonoBehaviour)(object)this).StartCoroutineWithReference(CUpdate());
	}

	public override void StopAttach()
	{
		if (!((Object)(object)base.owner == (Object)null))
		{
			_cUpdateReference.Stop();
			base.owner.ability.Remove(_abilityComponent.ability);
		}
	}

	private IEnumerator CUpdate()
	{
		while (true)
		{
			if (base.owner.ability.Contains(_stackableStatBonusComponent.ability) && _stackableStatBonusComponent.isMax)
			{
				if (!_attached)
				{
					_attached = true;
					base.owner.ability.Add(_abilityComponent.ability);
				}
			}
			else
			{
				_attached = false;
				base.owner.ability.Remove(_abilityComponent.ability);
			}
			yield return null;
		}
	}

	public override string ToString()
	{
		return this.GetAutoName();
	}
}
