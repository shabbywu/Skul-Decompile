using System.Collections;
using System.Collections.Generic;
using FX;
using UnityEngine;

namespace Characters.Operations.Customs;

public class MultiShadowBunshin : CharacterOperation
{
	[SerializeField]
	private EffectInfo _spawnEffect;

	[SerializeField]
	private OperationRunner _origin;

	[SerializeField]
	private OperationRunner _fake;

	[SerializeField]
	private int _totalCount;

	[SerializeField]
	private int _originCount;

	[SerializeField]
	private float _delay;

	private HashSet<int> _originIndics;

	private void Awake()
	{
		_originIndics = new HashSet<int>();
	}

	public override void Run(Character owner)
	{
		UpdateOriginIndics();
		((MonoBehaviour)this).StartCoroutine(CRun(owner));
	}

	private IEnumerator CRun(Character owner)
	{
		Bounds platform = owner.movement.controller.collisionState.lastStandingCollider.bounds;
		Vector2 val2 = default(Vector2);
		for (int i = 0; i < _totalCount; i++)
		{
			Vector3 val = Vector2.op_Implicit(Vector2.zero);
			bool num = i >= _totalCount / 2;
			if (num)
			{
				((Vector2)(ref val2))._002Ector(Random.Range(((Bounds)(ref platform)).center.x, ((Bounds)(ref platform)).max.x), ((Bounds)(ref platform)).max.y);
				val.z = (180f - val.z) % 360f;
			}
			else
			{
				((Vector2)(ref val2))._002Ector(Random.Range(((Bounds)(ref platform)).min.x, ((Bounds)(ref platform)).center.x), ((Bounds)(ref platform)).max.y);
			}
			OperationRunner operationRunner = ((!_originIndics.Contains(i)) ? _fake : _origin);
			OperationInfos operationInfos = operationRunner.Spawn().operationInfos;
			((Component)operationInfos).transform.SetPositionAndRotation(Vector2.op_Implicit(val2), Quaternion.Euler(val));
			if (num)
			{
				((Component)operationInfos).transform.localScale = new Vector3(1f, -1f, 1f);
			}
			else
			{
				((Component)operationInfos).transform.localScale = new Vector3(1f, 1f, 1f);
			}
			_spawnEffect.Spawn(Vector2.op_Implicit(val2));
			operationInfos.Run(owner);
			yield return owner.chronometer.master.WaitForSeconds(_delay);
		}
	}

	private void UpdateOriginIndics()
	{
		_originIndics.Clear();
		for (int i = 0; i < _originCount; i++)
		{
			int item = Random.Range(0, _totalCount);
			while (_originIndics.Contains(item))
			{
				item = Random.Range(0, _totalCount);
			}
			_originIndics.Add(item);
		}
	}
}
