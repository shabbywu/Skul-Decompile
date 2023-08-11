using UnityEngine;

namespace Characters.Operations;

public class DeactivateChildren : CharacterOperation
{
	[SerializeField]
	private ParentPool _parentPool;

	public override void Run(Character owner)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		foreach (Transform item in _parentPool.currentEffectParent)
		{
			((Component)item).gameObject.SetActive(false);
		}
	}
}
