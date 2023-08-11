using UnityEngine;

namespace Characters.Operations.ObjectTransform;

public sealed class SetParentToCharacterAttach : CharacterOperation
{
	[SerializeField]
	private Transform _transform;

	[SerializeField]
	private bool _flipByOwnerDirection;

	public override void Run(Character owner)
	{
		if (!((Object)(object)owner.attach == (Object)null))
		{
			if ((Object)(object)_transform == (Object)null)
			{
				_transform = ((Component)this).transform;
			}
			if (_flipByOwnerDirection)
			{
				_transform.SetParent(owner.attachWithFlip.transform);
			}
			else
			{
				_transform.SetParent(owner.attach.transform);
			}
		}
	}
}
