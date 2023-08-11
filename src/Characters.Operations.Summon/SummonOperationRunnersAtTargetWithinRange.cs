using System;
using System.Collections.Generic;
using FX;
using PhysicsUtils;
using UnityEngine;
using UnityEngine.Rendering;

namespace Characters.Operations.Summon;

public class SummonOperationRunnersAtTargetWithinRange : CharacterOperation
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

		private static readonly EnumArray<Pivot, Vector2> _pivotValues = new EnumArray<Pivot, Vector2>((Vector2[])(object)new Vector2[10]
		{
			new Vector2(0f, 0f),
			new Vector2(-0.5f, 0.5f),
			new Vector2(0f, 0.5f),
			new Vector2(0.5f, 0.5f),
			new Vector2(-0.5f, 0f),
			new Vector2(0f, 0.5f),
			new Vector2(-0.5f, -0.5f),
			new Vector2(0f, -0.5f),
			new Vector2(0.5f, -0.5f),
			new Vector2(0f, 0f)
		});

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

	public enum FindingMethod
	{
		Random,
		CloseToFar,
		FarToClose
	}

	private static short spriteLayer = short.MinValue;

	private NonAllocOverlapper _overlapper;

	[SerializeField]
	[Tooltip("오퍼레이션 프리팹")]
	private OperationRunner _operationRunner;

	[SerializeField]
	private PositionInfo _positionInfo;

	[SerializeField]
	private bool _attachToTarget;

	[SerializeField]
	private CustomFloat _scale = new CustomFloat(1f);

	[SerializeField]
	private CustomAngle _angle;

	[SerializeField]
	private PositionNoise _noise;

	[SerializeField]
	private bool _snapToGround;

	[SerializeField]
	[Tooltip("땅을 찾기 위해 소환지점으로부터 아래로 탐색할 거리. 실패 시 그냥 소환 지점에 소환됨")]
	private float _groundFindingDistance = 7f;

	[SerializeField]
	[Tooltip("주인이 바라보고 있는 방향에 따라 X축으로 플립")]
	private bool _flipXByLookingDirection;

	[Tooltip("X축 플립")]
	[SerializeField]
	private bool _flipX;

	[Header("Special Settings")]
	[SerializeField]
	private Collider2D _collider;

	[Tooltip("콜라이더 최적화 여부, Composite Collider등 특별한 경우가 아니면 true로 유지")]
	[SerializeField]
	private bool _optimizedCollider = true;

	[SerializeField]
	private TargetLayer _layer = new TargetLayer(LayerMask.op_Implicit(0), allyBody: false, foeBody: true, allyProjectile: false, foeProjectile: false);

	[SerializeField]
	[Tooltip("범위 내 감지가능한 최대 적의 수, 프롭을 포함하지 않으므로 128로 충분")]
	private int _maxCount = 128;

	[Tooltip("스폰될 오퍼레이션러너의 최대 개수")]
	[SerializeField]
	private int _totalOperationCount;

	[Tooltip("하나의 적에게 중첩되어 스폰될 수 있는 최대 개수")]
	[SerializeField]
	private int _maxCountPerUnit = 1;

	[SerializeField]
	private FindingMethod _method;

	[SerializeField]
	[Tooltip("Close To Far, Far To Close 계산 시 기준점이 될 위치, 비워둘 경우 콜라이더의 중심점을 기준으로 함")]
	private Transform _sortOrigin;

	[SerializeField]
	[Space]
	private bool _copyAttackDamage;

	private AttackDamage _attackDamage;

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_operationRunner = null;
	}

	public override void Initialize()
	{
		_attackDamage = ((Component)this).GetComponentInParent<AttackDamage>();
	}

	private void Awake()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		_overlapper = new NonAllocOverlapper(_maxCount);
		if (_optimizedCollider)
		{
			((Behaviour)_collider).enabled = false;
		}
	}

	public override void Run(Character owner)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		((ContactFilter2D)(ref _overlapper.contactFilter)).SetLayerMask(_layer.Evaluate(((Component)owner).gameObject));
		((Behaviour)_collider).enabled = true;
		_overlapper.OverlapCollider(_collider);
		Vector3 val;
		if (!((Object)(object)_sortOrigin != (Object)null))
		{
			Bounds bounds = _collider.bounds;
			val = ((Bounds)(ref bounds)).center;
		}
		else
		{
			val = _sortOrigin.position;
		}
		Vector3 origin = val;
		if (_optimizedCollider)
		{
			((Behaviour)_collider).enabled = false;
		}
		if (_overlapper.results.Count == 0)
		{
			return;
		}
		List<Character> list = new List<Character>(_overlapper.results.Count);
		for (int i = 0; i < _overlapper.results.Count; i++)
		{
			Target component = ((Component)_overlapper.results[i]).GetComponent<Target>();
			if (!((Object)(object)component == (Object)null) && !((Object)(object)component.character == (Object)null) && component.character.liveAndActive && !((Object)(object)component.character == (Object)(object)owner))
			{
				list.Add(component.character);
			}
		}
		if (list.Count == 0)
		{
			return;
		}
		switch (_method)
		{
		case FindingMethod.Random:
			ExtensionMethods.PseudoShuffle<Character>((IList<Character>)list);
			break;
		case FindingMethod.CloseToFar:
			list.Sort(delegate(Character x, Character y)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Unknown result type (might be due to invalid IL or missing references)
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0030: Unknown result type (might be due to invalid IL or missing references)
				//IL_0033: Unknown result type (might be due to invalid IL or missing references)
				//IL_0038: Unknown result type (might be due to invalid IL or missing references)
				//IL_003d: Unknown result type (might be due to invalid IL or missing references)
				Vector3 val2 = origin;
				Bounds bounds2 = ((Collider2D)x.collider).bounds;
				Vector3 val3 = val2 - ((Bounds)(ref bounds2)).center;
				Vector3 val4 = origin;
				bounds2 = ((Collider2D)y.collider).bounds;
				Vector3 val5 = val4 - ((Bounds)(ref bounds2)).center;
				return ((Vector3)(ref val3)).sqrMagnitude.CompareTo(((Vector3)(ref val5)).sqrMagnitude);
			});
			break;
		case FindingMethod.FarToClose:
			list.Sort(delegate(Character x, Character y)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Unknown result type (might be due to invalid IL or missing references)
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0030: Unknown result type (might be due to invalid IL or missing references)
				//IL_0033: Unknown result type (might be due to invalid IL or missing references)
				//IL_0038: Unknown result type (might be due to invalid IL or missing references)
				//IL_003d: Unknown result type (might be due to invalid IL or missing references)
				Vector3 val6 = origin;
				Bounds bounds3 = ((Collider2D)x.collider).bounds;
				Vector3 val7 = val6 - ((Bounds)(ref bounds3)).center;
				Vector3 val8 = origin;
				bounds3 = ((Collider2D)y.collider).bounds;
				Vector3 val9 = val8 - ((Bounds)(ref bounds3)).center;
				return ((Vector3)(ref val9)).sqrMagnitude.CompareTo(((Vector3)(ref val7)).sqrMagnitude);
			});
			break;
		}
		int num = _totalOperationCount;
		for (int j = 0; j < _maxCountPerUnit; j++)
		{
			for (int k = 0; k < list.Count; k++)
			{
				SpawnTo(owner, list[k]);
				num--;
				if (num == 0)
				{
					return;
				}
			}
		}
	}

	public void SpawnTo(Character owner, Character target)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = default(Vector3);
		((Vector3)(ref val))._002Ector(0f, 0f, _angle.value);
		int num;
		if (_flipXByLookingDirection)
		{
			num = ((owner.lookingDirection == Character.LookingDirection.Left) ? 1 : 0);
			if (num != 0)
			{
				val.z = (180f - val.z) % 360f;
			}
		}
		else
		{
			num = 0;
		}
		if (_flipX)
		{
			val.z = (180f - val.z) % 360f;
		}
		OperationRunner operationRunner = _operationRunner.Spawn();
		OperationInfos operationInfos = operationRunner.operationInfos;
		if (_copyAttackDamage && (Object)(object)_attackDamage != (Object)null)
		{
			operationRunner.attackDamage.minAttackDamage = _attackDamage.minAttackDamage;
			operationRunner.attackDamage.maxAttackDamage = _attackDamage.maxAttackDamage;
		}
		Vector3 position = ((Component)target).transform.position;
		position.x += ((Collider2D)target.collider).offset.x;
		position.y += ((Collider2D)target.collider).offset.y;
		Bounds bounds = ((Collider2D)target.collider).bounds;
		Vector3 size = ((Bounds)(ref bounds)).size;
		size.x *= _positionInfo.pivotValue.x;
		size.y *= _positionInfo.pivotValue.y;
		Vector3 val2 = position + size;
		if (_snapToGround)
		{
			RaycastHit2D val3 = Physics2D.Raycast(Vector2.op_Implicit(val2), Vector2.down, _groundFindingDistance, LayerMask.op_Implicit(Layers.groundMask));
			if (RaycastHit2D.op_Implicit(val3))
			{
				val2 = Vector2.op_Implicit(((RaycastHit2D)(ref val3)).point);
			}
		}
		val2 += _noise.Evaluate();
		((Component)operationInfos).transform.SetPositionAndRotation(val2, Quaternion.Euler(val));
		SortingGroup component = ((Component)operationRunner).GetComponent<SortingGroup>();
		if ((Object)(object)component != (Object)null)
		{
			component.sortingOrder = spriteLayer++;
		}
		if (num != 0)
		{
			((Component)operationInfos).transform.localScale = new Vector3(1f, -1f, 1f) * _scale.value;
		}
		else
		{
			((Component)operationInfos).transform.localScale = new Vector3(1f, 1f, 1f) * _scale.value;
		}
		if (_flipX)
		{
			((Component)operationInfos).transform.localScale = new Vector3(1f, -1f, 1f) * _scale.value;
		}
		if (_attachToTarget)
		{
			((Component)operationInfos).transform.parent = ((Component)target).transform;
		}
		operationInfos.Run(owner);
	}
}
