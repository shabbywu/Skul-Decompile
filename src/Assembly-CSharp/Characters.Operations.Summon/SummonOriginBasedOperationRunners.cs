using System;
using System.Collections;
using FX;
using UnityEngine;

namespace Characters.Operations.Summon;

public class SummonOriginBasedOperationRunners : CharacterOperation
{
	[Serializable]
	private class SummonOption
	{
		[Serializable]
		public class Reorderable : ReorderableArray<SummonOption>
		{
			public void Dispose()
			{
				for (int i = 0; i < values.Length; i++)
				{
					values[i].Dispose();
					values[i] = null;
				}
			}
		}

		[Tooltip("오퍼레이션 프리팹")]
		public OperationRunner operationRunner;

		[Tooltip("오퍼레이션이 스폰되는 시간")]
		public float timeToSpawn;

		public Transform originPosition;

		[SerializeField]
		private CustomAngle _moveAngleFromOrigin;

		[SerializeField]
		private CustomFloat _moveAmountFromOrigin;

		[SerializeField]
		private bool _extraMoveAngleByOrigin;

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

		public bool extraMoveAngleByOrigin => _extraMoveAngleByOrigin;

		public float moveAmount => _moveAmountFromOrigin.value;

		public void Dispose()
		{
			operationRunner = null;
		}
	}

	[Tooltip("발동 시점에 미리 위치를 받아옵니다. 캐릭터가 이동해도 위치가 바뀌지 않게해야할 때 유용합니다.")]
	[SerializeField]
	private bool _preloadPosition;

	[SerializeField]
	private bool _timeIndependant;

	[SerializeField]
	private bool _extraAngleBySpawnPositionRotation;

	[SerializeField]
	private SummonOption.Reorderable _summonOptions;

	private AttackDamage _attackDamage;

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_summonOptions.Dispose();
	}

	public override void Initialize()
	{
		_attackDamage = ((Component)this).GetComponentInParent<AttackDamage>();
	}

	public override void Run(Character owner)
	{
		((MonoBehaviour)this).StartCoroutine(CRun(owner));
	}

	private IEnumerator CRun(Character owner)
	{
		Vector3[] preloadedPositions = (Vector3[])(object)new Vector3[_summonOptions.values.Length];
		if (_preloadPosition)
		{
			for (int i = 0; i < preloadedPositions.Length; i++)
			{
				Transform originPosition = _summonOptions.values[i].originPosition;
				if ((Object)(object)originPosition == (Object)null)
				{
					preloadedPositions[i] = ((Component)this).transform.position;
				}
				else
				{
					preloadedPositions[i] = originPosition.position;
				}
			}
		}
		int optionIndex = 0;
		float time = 0f;
		SummonOption[] options = _summonOptions.values;
		Vector3 val2 = default(Vector3);
		while (optionIndex < options.Length)
		{
			for (; optionIndex < options.Length && time >= options[optionIndex].timeToSpawn; optionIndex++)
			{
				SummonOption summonOption = options[optionIndex];
				Vector3 val = ((!_preloadPosition) ? (((Object)(object)summonOption.originPosition == (Object)null) ? ((Component)this).transform.position : summonOption.originPosition.position) : preloadedPositions[optionIndex]);
				float value = summonOption.angle.value;
				float num = value;
				float num2;
				Quaternion rotation;
				if (!summonOption.extraMoveAngleByOrigin)
				{
					num2 = 0f;
				}
				else
				{
					rotation = summonOption.originPosition.rotation;
					num2 = ((Quaternion)(ref rotation)).eulerAngles.z;
				}
				value = num + num2;
				value *= (float)Math.PI / 180f;
				val += new Vector3(Mathf.Cos(value), Mathf.Sin(value), 0f) * summonOption.moveAmount;
				val += summonOption.noise.Evaluate();
				float num3;
				if (!_extraAngleBySpawnPositionRotation)
				{
					num3 = summonOption.angle.value;
				}
				else
				{
					rotation = summonOption.originPosition.rotation;
					num3 = ((Quaternion)(ref rotation)).eulerAngles.z + summonOption.angle.value;
				}
				((Vector3)(ref val2))._002Ector(0f, 0f, num3);
				bool flag = summonOption.flipXByLookingDirection && owner.lookingDirection == Character.LookingDirection.Left;
				if (summonOption.flipXByLookingDirection)
				{
					flag = summonOption.originPosition.localScale.x > 0f;
				}
				if (flag)
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
				if (flag)
				{
					((Component)operationInfos).transform.localScale = new Vector3(1f, -1f, 1f);
				}
				else
				{
					((Component)operationInfos).transform.localScale = new Vector3(1f, 1f, 1f);
				}
				operationInfos.Run(owner);
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
			time = ((!_timeIndependant) ? (time + owner.chronometer.animation.deltaTime) : (time + Chronometer.global.deltaTime));
		}
	}

	public override void Stop()
	{
		base.Stop();
		((MonoBehaviour)this).StopAllCoroutines();
	}
}
