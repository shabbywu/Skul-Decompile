using System.Collections;
using System.Collections.Generic;
using Characters.AI;
using Characters.Projectiles;
using UnityEngine;

namespace Characters.Operations.Customs;

public sealed class FlameStorm : CharacterOperation
{
	[SerializeField]
	private AIController _ai;

	[Tooltip("오퍼레이션 프리팹")]
	[SerializeField]
	private OperationRunner _operationRunner;

	[SerializeField]
	private Projectile _projectile;

	[SerializeField]
	private Transform _spawnPointContainer;

	[SerializeField]
	private int _emptyCount = 2;

	[SerializeField]
	private float _fireDelay = 1.5f;

	private int[] _numbers;

	private IAttackDamage _attackDamage;

	private void Awake()
	{
		_numbers = new int[_spawnPointContainer.childCount];
		for (int i = 0; i < _spawnPointContainer.childCount; i++)
		{
			_numbers[i] = i;
		}
	}

	public override void Initialize()
	{
		_attackDamage = ((Component)this).GetComponentInParent<IAttackDamage>();
	}

	public override void Run(Character owner)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		ExtensionMethods.Shuffle<int>((IList<int>)_numbers);
		for (int i = 0; i < _spawnPointContainer.childCount - _emptyCount; i++)
		{
			int num = _numbers[i];
			Vector3 position = _spawnPointContainer.GetChild(num).position;
			OperationInfos operationInfos = _operationRunner.Spawn().operationInfos;
			((Component)operationInfos).transform.position = position;
			operationInfos.Run(owner);
		}
		int num2 = _numbers[Random.Range(0, _spawnPointContainer.childCount - _emptyCount)];
		((MonoBehaviour)this).StartCoroutine(CFire(owner, _spawnPointContainer.GetChild(num2)));
	}

	private IEnumerator CFire(Character owner, Transform fireTransform)
	{
		yield return Chronometer.global.WaitForSeconds(_fireDelay);
		float direction = ((!(((Component)_ai.target).transform.position.x > fireTransform.position.x)) ? 180f : 0f);
		((Component)_projectile.reusable.Spawn(fireTransform.position, true)).GetComponent<Projectile>().Fire(owner, _attackDamage.amount, direction);
	}
}
