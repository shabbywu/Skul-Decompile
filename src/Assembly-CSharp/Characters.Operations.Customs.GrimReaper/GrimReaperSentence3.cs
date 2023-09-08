using System;
using System.Collections;
using System.Collections.Generic;
using FX;
using UnityEngine;
using UnityEngine.Rendering;

namespace Characters.Operations.Customs.GrimReaper;

public sealed class GrimReaperSentence3 : TargetedCharacterOperation
{
	[Serializable]
	public class PositionInfo
	{
		public enum Pivot
		{
			Center,
			TopLeft,
			Top,
			TopRight,
			Left,
			Right,
			BottomLeft,
			Bottom,
			BottomRight,
			Custom
		}

		private static readonly EnumArray<Pivot, Vector2> _pivotValues = new EnumArray<Pivot, Vector2>(new Vector2(0f, 0f), new Vector2(-0.5f, 0.5f), new Vector2(0f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(-0.5f, 0f), new Vector2(0f, 0.5f), new Vector2(-0.5f, -0.5f), new Vector2(0f, -0.5f), new Vector2(0.5f, -0.5f), new Vector2(0f, 0f));

		[SerializeField]
		private Pivot _pivot;

		[SerializeField]
		[HideInInspector]
		private Vector2 _pivotValue;

		public Pivot pivot => _pivot;

		public Vector2 pivotValue => _pivotValue;

		public PositionInfo()
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			_pivot = Pivot.Center;
			_pivotValue = Vector2.zero;
		}

		public PositionInfo(bool attach, bool layerOnly, int layerOrderOffset, Pivot pivot)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			_pivot = pivot;
			_pivotValue = _pivotValues[pivot];
		}
	}

	private struct TargetData
	{
		internal EffectPoolInstance markEffect;

		internal float angle;

		internal TargetData(EffectPoolInstance markEffect, float angle)
		{
			this.markEffect = markEffect;
			this.angle = angle;
		}
	}

	private static short spriteLayer = short.MinValue;

	[SerializeField]
	[Header("마크")]
	private EffectInfo _markEffectInfo;

	[SerializeField]
	[Tooltip("오퍼레이션 프리팹")]
	private OperationRunner _operationRunner;

	[SerializeField]
	[Space]
	private PositionInfo _positionInfo;

	[SerializeField]
	private CustomFloat _scale = new CustomFloat(1f);

	[SerializeField]
	private CustomAngle _angle;

	[SerializeField]
	private PositionNoise _noise;

	[SerializeField]
	[Space]
	private bool _snapToGround;

	[Tooltip("땅을 찾기 위해 소환지점으로부터 아래로 탐색할 거리. 실패 시 그냥 소환 지점에 소환됨")]
	[SerializeField]
	private float _groundFindingDistance = 7f;

	[SerializeField]
	[Space]
	private bool _copyAttackDamage;

	[SerializeField]
	private float _duration;

	[SerializeField]
	private bool _rotationSyncWithMark;

	private AttackDamage _attackDamage;

	private Character _owner;

	private Dictionary<Character, TargetData> _targetDatas;

	public override void Initialize()
	{
		_attackDamage = ((Component)this).GetComponentInParent<AttackDamage>();
		_targetDatas = new Dictionary<Character, TargetData>(32);
	}

	public override void Run(Character owner, Character target)
	{
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		if (!_targetDatas.ContainsKey(target) && !((Object)(object)target == (Object)null) && target.liveAndActive)
		{
			float num = (_rotationSyncWithMark ? _angle.value : 0f);
			EffectPoolInstance markEffect = ((_markEffectInfo == null) ? null : _markEffectInfo.Spawn(((Component)target).transform.position, target, num));
			TargetData value = new TargetData(markEffect, num);
			_targetDatas.Add(target, value);
			target.health.onDiedTryCatch += SummonClosure;
			_owner = owner;
			if (_duration > 0f)
			{
				((MonoBehaviour)_owner).StartCoroutine(CWaitForDurationAndSummon(SummonClosure, target));
			}
		}
		void SummonClosure()
		{
			target.health.onDiedTryCatch -= SummonClosure;
			Summon(target);
		}
	}

	private IEnumerator CWaitForDurationAndSummon(Action summonClosure, Character target)
	{
		yield return target.chronometer.master.WaitForSeconds(_duration);
		if (!target.health.dead)
		{
			summonClosure?.Invoke();
		}
	}

	private void Summon(Character target)
	{
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)target == (Object)null) && _targetDatas.ContainsKey(target))
		{
			TargetData targetData = _targetDatas[target];
			_targetDatas.Remove(target);
			Vector3 val = (_rotationSyncWithMark ? new Vector3(0f, 0f, targetData.angle) : new Vector3(0f, 0f, _angle.value));
			if ((Object)(object)targetData.markEffect != (Object)null)
			{
				targetData.markEffect.Stop();
				targetData.markEffect = null;
			}
			Vector3 position = ((Component)target).transform.position;
			position.x += ((Collider2D)target.collider).offset.x;
			position.y += ((Collider2D)target.collider).offset.y;
			Bounds bounds = ((Collider2D)target.collider).bounds;
			Vector3 size = ((Bounds)(ref bounds)).size;
			size.x *= _positionInfo.pivotValue.x;
			size.y *= _positionInfo.pivotValue.y;
			Vector3 val2 = position + size + _noise.Evaluate();
			OperationRunner operationRunner = _operationRunner.Spawn();
			OperationInfos operationInfos = operationRunner.operationInfos;
			((Component)operationInfos).transform.SetPositionAndRotation(val2, Quaternion.Euler(val));
			if (_copyAttackDamage && (Object)(object)_attackDamage != (Object)null)
			{
				operationRunner.attackDamage.minAttackDamage = _attackDamage.minAttackDamage;
				operationRunner.attackDamage.maxAttackDamage = _attackDamage.maxAttackDamage;
			}
			SortingGroup component = ((Component)operationRunner).GetComponent<SortingGroup>();
			if ((Object)(object)component != (Object)null)
			{
				component.sortingOrder = spriteLayer++;
			}
			((Component)operationInfos).transform.localScale = Vector3.one * _scale.value;
			operationInfos.Run(_owner);
		}
	}
}
