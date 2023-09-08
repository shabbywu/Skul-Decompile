using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Runnables;

public sealed class RunOperations : Runnable
{
	[UnityEditor.Subcomponent(typeof(OperationInfo))]
	[SerializeField]
	private OperationInfo.Subcomponents _operations;

	[SerializeField]
	private Target _owner;

	private void Awake()
	{
		_operations.Initialize();
	}

	public override void Run()
	{
		if (((Component)_owner.character).gameObject.activeSelf)
		{
			((MonoBehaviour)this).StartCoroutine(_operations.CRun(_owner.character));
		}
	}
}
