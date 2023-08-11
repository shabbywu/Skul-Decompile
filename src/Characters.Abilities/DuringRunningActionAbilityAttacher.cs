using System.Collections;
using Characters.Actions;
using UnityEngine;

namespace Characters.Abilities;

public sealed class DuringRunningActionAbilityAttacher : AbilityAttacher
{
	[AbilityComponent.Subcomponent]
	[SerializeField]
	private AbilityComponent _abilityComponent;

	[SerializeField]
	private Action[] _actions;

	public override void OnIntialize()
	{
		_abilityComponent.Initialize();
	}

	public override void StartAttach()
	{
		Action[] actions = _actions;
		for (int i = 0; i < actions.Length; i++)
		{
			actions[i].onStart += AttachAbility;
		}
	}

	private void AttachAbility()
	{
		base.owner.ability.Add(_abilityComponent.ability);
		((MonoBehaviour)this).StopCoroutine("CCheckRunningAction");
		((MonoBehaviour)this).StartCoroutine("CCheckRunningAction");
	}

	private IEnumerator CCheckRunningAction()
	{
		while (true)
		{
			bool flag = false;
			Action[] actions = _actions;
			for (int i = 0; i < actions.Length; i++)
			{
				if (actions[i].running)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				break;
			}
			yield return null;
		}
		RemoveAbility();
	}

	private void RemoveAbility()
	{
		base.owner.ability.Remove(_abilityComponent.ability);
	}

	public override void StopAttach()
	{
		if (!((Object)(object)base.owner == (Object)null))
		{
			Action[] actions = _actions;
			for (int i = 0; i < actions.Length; i++)
			{
				actions[i].onStart -= AttachAbility;
			}
			RemoveAbility();
		}
	}
}
