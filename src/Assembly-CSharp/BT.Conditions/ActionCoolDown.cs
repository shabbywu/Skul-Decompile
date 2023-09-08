using Characters.Actions;
using UnityEngine;

namespace BT.Conditions;

public sealed class ActionCoolDown : Condition
{
	[SerializeField]
	private Action _action;

	protected override bool Check(Context context)
	{
		return _action.cooldown.canUse;
	}
}
