using UnityEngine;
using Utils;

namespace Characters.Operations.Movement;

public sealed class ClearGrabBoard : Operation
{
	[SerializeField]
	private GrabBoard _grabBoard;

	public override void Run()
	{
		_grabBoard.Clear();
	}
}
