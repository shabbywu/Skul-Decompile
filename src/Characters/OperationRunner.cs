using Characters.Operations;
using UnityEngine;

namespace Characters;

[RequireComponent(typeof(PoolObject), typeof(OperationInfos))]
public class OperationRunner : MonoBehaviour
{
	[GetComponent]
	[SerializeField]
	private PoolObject _poolObject;

	[SerializeField]
	[Information(/*Could not decode attribute arguments.*/)]
	private AttackDamage _attackDamage;

	[GetComponent]
	[SerializeField]
	private OperationInfos _operationInfos;

	public OperationInfos operationInfos => _operationInfos;

	public PoolObject poolObject => _poolObject;

	public AttackDamage attackDamage => _attackDamage;

	private void Awake()
	{
		_operationInfos.Initialize();
		_operationInfos.onEnd += _poolObject.Despawn;
	}

	public OperationRunner Spawn()
	{
		return ((Component)_poolObject.Spawn(true)).GetComponent<OperationRunner>();
	}
}
