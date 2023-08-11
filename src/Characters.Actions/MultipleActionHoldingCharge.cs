using System.Linq;
using UnityEngine;

namespace Characters.Actions;

public class MultipleActionHoldingCharge : MultipleAction
{
	[SerializeField]
	private Motion[] _chargingMotions;

	public override bool TryStart()
	{
		if (!base.cooldown.canUse)
		{
			return false;
		}
		for (int i = 0; i < ((SubcomponentArray<Motion>)_motions).components.Length; i++)
		{
			if (PassAllConstraints(((SubcomponentArray<Motion>)_motions).components[i]) && ConsumeCooldownIfNeeded())
			{
				if (_chargingMotions.Contains(base.owner.motion))
				{
					DoActionNonBlock(((SubcomponentArray<Motion>)_motions).components[i]);
				}
				else
				{
					DoAction(((SubcomponentArray<Motion>)_motions).components[i]);
				}
				return true;
			}
		}
		return false;
	}
}
