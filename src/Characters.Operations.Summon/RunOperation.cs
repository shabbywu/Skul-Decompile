using System;
using UnityEngine;

namespace Characters.Operations.Summon;

[Serializable]
public class RunOperation : IBDCharacterSetting
{
	[SerializeField]
	private OperationInfos _operation;

	public void ApplyTo(Character character)
	{
		_operation.Initialize();
		if (!((Component)_operation).gameObject.activeSelf)
		{
			((Component)_operation).gameObject.SetActive(true);
		}
		_operation.Run(character);
	}
}
