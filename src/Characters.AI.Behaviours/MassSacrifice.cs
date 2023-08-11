using System.Collections;
using System.Collections.Generic;
using Characters.Actions;
using UnityEngine;

namespace Characters.AI.Behaviours;

public sealed class MassSacrifice : Behaviour
{
	[SerializeField]
	private Action _action;

	[SerializeField]
	private Collider2D _range;

	public override IEnumerator CRun(AIController controller)
	{
		base.result = Result.Doing;
		if (!_action.TryStart())
		{
			base.result = Result.Fail;
			yield break;
		}
		while (_action.running)
		{
			yield return null;
		}
		base.result = Result.Success;
	}

	public bool CanUse(AIController aiController)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		if (!_action.canUse)
		{
			return false;
		}
		((Component)_range).transform.position = ((Component)aiController.target).transform.position;
		List<Character> list = aiController.FindEnemiesInRange(_range);
		if (list == null || list.Count <= 0)
		{
			return false;
		}
		foreach (Character item in list)
		{
			if (!((Object)(object)((Component)item).GetComponent<SacrificeCharacter>() == (Object)null))
			{
				return true;
			}
		}
		return false;
	}
}
