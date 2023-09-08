using UnityEngine;
using Utils;

namespace Characters.Operations.GrabBorad.Custom;

public class CopyGrabBoard : CharacterOperation
{
	[SerializeField]
	private GrabBoard _to;

	[SerializeField]
	private GrabBoard _from;

	public override void Run(Character owner)
	{
		_to.targets.AddRange(_from.targets);
	}
}
