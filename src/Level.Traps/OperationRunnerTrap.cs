using Characters;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Level.Traps;

public class OperationRunnerTrap : Trap
{
	[SerializeField]
	private Character _character;

	[Subcomponent(typeof(OperationInfo))]
	[SerializeField]
	private OperationInfo.Subcomponents _operations;

	private void Awake()
	{
		_operations.Initialize();
	}

	private void OnEnable()
	{
		((MonoBehaviour)this).StartCoroutine(_operations.CRun(_character));
	}
}
