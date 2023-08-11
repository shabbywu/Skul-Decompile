using System;
using System.Collections;
using Characters.Operations.Attack;
using Characters.Utils;
using FX;
using UnityEngine;

namespace Characters.Operations.Summon;

public class SummonMultipleOperationRunners : CharacterOperation
{
	[Serializable]
	private class SummonOption
	{
		[Serializable]
		public class Reorderable : ReorderableArray<SummonOption>
		{
		}

		[Tooltip("오퍼레이션 프리팹")]
		public OperationRunner operationRunner;

		[Tooltip("오퍼레이션이 스폰되는 시간")]
		public float timeToSpawn;

		public Transform spawnPosition;

		public CustomFloat scale = new CustomFloat(1f);

		public CustomAngle angle;

		public PositionNoise noise;

		[Tooltip("주인이 바라보고 있는 방향에 따라 X축으로 플립")]
		public bool flipXByLookingDirection;

		[Tooltip("체크하면 주인에 부착되며, 같이 움직임")]
		public bool attachToOwner;

		[Tooltip("체크하면 캐릭터의 Attach오브젝트에 부착되며, 같이 움직임, 플립안됨")]
		public bool notFlipOnOwner;

		public bool copyAttackDamage;
	}

	[SerializeField]
	[Tooltip("발동 시점에 미리 위치를 받아옵니다. 캐릭터가 이동해도 위치가 바뀌지 않게해야할 때 유용합니다.")]
	private bool _preloadPosition;

	[SerializeField]
	private bool _timeIndependant;

	[SerializeField]
	private bool _extraAngleBySpawnPositionRotation;

	[SerializeField]
	[Header("Sweepattack만 가능")]
	private bool _attackGroup;

	[SerializeField]
	private SummonOption.Reorderable _summonOptions;

	private AttackDamage _attackDamage;

	private HitHistoryManager _hitHistoryManager;

	public override void Initialize()
	{
		_attackDamage = ((Component)this).GetComponentInParent<AttackDamage>();
		if (_attackGroup)
		{
			_hitHistoryManager = new HitHistoryManager(15);
		}
	}

	public override void Run(Character owner)
	{
		((MonoBehaviour)this).StartCoroutine(CRun(owner));
	}

	private IEnumerator CRun(Character owner)
	{
		Vector3[] preloadedPositions = (Vector3[])(object)new Vector3[((ReorderableArray<SummonOption>)_summonOptions).values.Length];
		if (_preloadPosition)
		{
			for (int i = 0; i < preloadedPositions.Length; i++)
			{
				Transform spawnPosition = ((ReorderableArray<SummonOption>)_summonOptions).values[i].spawnPosition;
				if ((Object)(object)spawnPosition == (Object)null)
				{
					preloadedPositions[i] = ((Component)this).transform.position;
				}
				else
				{
					preloadedPositions[i] = spawnPosition.position;
				}
			}
		}
		int optionIndex = 0;
		float time = 0f;
		SummonOption[] options = ((ReorderableArray<SummonOption>)_summonOptions).values;
		if (_attackGroup)
		{
			_hitHistoryManager.Clear();
		}
		Vector3 val2 = default(Vector3);
		while (optionIndex < options.Length)
		{
			for (; optionIndex < options.Length && time >= options[optionIndex].timeToSpawn; optionIndex++)
			{
				SummonOption summonOption = options[optionIndex];
				Vector3 val = ((!_preloadPosition) ? (((Object)(object)summonOption.spawnPosition == (Object)null) ? ((Component)this).transform.position : summonOption.spawnPosition.position) : preloadedPositions[optionIndex]);
				val += summonOption.noise.Evaluate();
				float num;
				if (!_extraAngleBySpawnPositionRotation)
				{
					num = summonOption.angle.value;
				}
				else
				{
					Quaternion rotation = summonOption.spawnPosition.rotation;
					num = ((Quaternion)(ref rotation)).eulerAngles.z + summonOption.angle.value;
				}
				((Vector3)(ref val2))._002Ector(0f, 0f, num);
				bool num2 = summonOption.flipXByLookingDirection && owner.lookingDirection == Character.LookingDirection.Left;
				if (num2)
				{
					val2.z = (180f - val2.z) % 360f;
				}
				OperationRunner operationRunner = summonOption.operationRunner.Spawn();
				OperationInfos operationInfos = operationRunner.operationInfos;
				((Component)operationInfos).transform.SetPositionAndRotation(val, Quaternion.Euler(val2));
				if (summonOption.copyAttackDamage && (Object)(object)_attackDamage != (Object)null)
				{
					operationRunner.attackDamage.minAttackDamage = _attackDamage.minAttackDamage;
					operationRunner.attackDamage.maxAttackDamage = _attackDamage.maxAttackDamage;
				}
				if (num2)
				{
					((Component)operationInfos).transform.localScale = new Vector3(1f, -1f, 1f);
				}
				else
				{
					((Component)operationInfos).transform.localScale = new Vector3(1f, 1f, 1f);
				}
				operationInfos.Run(owner);
				if (_attackGroup)
				{
					SweepAttack[] componentsInChildren = ((Component)operationInfos).GetComponentsInChildren<SweepAttack>();
					foreach (SweepAttack obj in componentsInChildren)
					{
						obj.collisionDetector.group = true;
						obj.collisionDetector.hits = _hitHistoryManager;
					}
				}
				if (summonOption.attachToOwner)
				{
					((Component)operationInfos).transform.parent = ((Component)this).transform;
				}
				if (summonOption.notFlipOnOwner && (Object)(object)owner.attach != (Object)null)
				{
					((Component)operationInfos).transform.parent = owner.attach.transform;
				}
			}
			yield return null;
			time = ((!_timeIndependant) ? (time + ((ChronometerBase)owner.chronometer.animation).deltaTime) : (time + ((ChronometerBase)Chronometer.global).deltaTime));
		}
	}

	public override void Stop()
	{
		base.Stop();
		((MonoBehaviour)this).StopAllCoroutines();
	}
}
