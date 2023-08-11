using System;
using UnityEngine;

namespace Characters.Operations;

public class StopAnotherOperation : Operation
{
	[Serializable]
	private class OperationsToStop : ReorderableArray<CharacterOperation>
	{
	}

	[SerializeField]
	private OperationsToStop _operationsToStop;

	public override void Run()
	{
		for (int i = 0; i < ((ReorderableArray<CharacterOperation>)_operationsToStop).values.Length; i++)
		{
			((ReorderableArray<CharacterOperation>)_operationsToStop).values[i].Stop();
		}
	}
}
