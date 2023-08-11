using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Gear.Upgrades;

public sealed class RunOperations : UpgradeAbility
{
	[Subcomponent(typeof(OperationInfo))]
	[SerializeField]
	private OperationInfo.Subcomponents _operations;

	public override void Attach(Character target)
	{
		if ((Object)(object)target == (Object)null)
		{
			Debug.LogError((object)"Player is null");
		}
		else
		{
			((MonoBehaviour)this).StartCoroutine(_operations.CRun(target));
		}
	}

	public override void Detach()
	{
	}
}
