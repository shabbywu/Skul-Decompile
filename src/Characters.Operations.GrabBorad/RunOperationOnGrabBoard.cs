using UnityEditor;
using UnityEngine;
using Utils;

namespace Characters.Operations.GrabBorad;

public class RunOperationOnGrabBoard : CharacterOperation
{
	[SerializeField]
	private GrabBoard _grabBoard;

	[SerializeField]
	[Subcomponent(typeof(OperationInfo))]
	private OperationInfo.Subcomponents _operations;

	public override void Run(Character owner)
	{
		_operations.Initialize();
		foreach (Target target in _grabBoard.targets)
		{
			((MonoBehaviour)this).StartCoroutine(_operations.CRun(target.character));
		}
	}
}
