using System.Collections;
using Characters.Operations;
using Runnables;
using UnityEditor;
using UnityEngine;

namespace SkulStories;

public class RunOperation : Sequence
{
	[Subcomponent(typeof(OperationInfo))]
	[SerializeField]
	private OperationInfo.Subcomponents _operations;

	[SerializeField]
	private Target _owner;

	private void Awake()
	{
		_operations.Initialize();
	}

	public override IEnumerator CRun()
	{
		yield return _operations.CRun(_owner.character);
	}
}
