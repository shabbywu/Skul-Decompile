using Characters;
using Characters.Operations;
using UnityEngine;

namespace Runnables;

public sealed class RunOperationInfos : Runnable
{
	[SerializeField]
	private Character _owner;

	[SerializeField]
	private OperationInfos _operationInfos;

	[SerializeField]
	private bool _reuseOperation;

	private void Awake()
	{
		_operationInfos.Initialize();
	}

	public override void Run()
	{
		if ((Object)(object)_owner == (Object)null)
		{
			_owner = ((Component)this).gameObject.GetComponentInParent<Character>();
		}
		if (!_owner.health.dead)
		{
			if (_reuseOperation)
			{
				((Component)_operationInfos).gameObject.SetActive(true);
			}
			if (((Component)_operationInfos).gameObject.activeSelf)
			{
				_operationInfos.Run(_owner);
			}
		}
	}
}
