using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Gear.Quintessences.Effects;

public sealed class RunOperations : QuintessenceEffect
{
	[SerializeField]
	[Subcomponent(typeof(OperationInfo))]
	private OperationInfo.Subcomponents _operations;

	private void Awake()
	{
		_operations.Initialize();
	}

	protected override void OnInvoke(Quintessence quintessence)
	{
		Character owner = quintessence.owner;
		if (!((Object)(object)owner == (Object)null))
		{
			_operations.StopAll();
			((MonoBehaviour)this).StartCoroutine(_operations.CRun(owner));
		}
	}
}
