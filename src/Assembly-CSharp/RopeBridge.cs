using System.Collections.Generic;
using PhysicsUtils;
using UnityEngine;

public class RopeBridge : MonoBehaviour
{
	public struct RopeSegment
	{
		public Vector2 posNow;

		public Vector2 posOld;

		public RopeSegment(Vector2 pos)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			posNow = pos;
			posOld = pos;
		}
	}

	[SerializeField]
	private TargetLayer _layers;

	[SerializeField]
	private Collider2D _findRange;

	public Transform StartPoint;

	public Transform EndPoint;

	private LineRenderer lineRenderer;

	private List<RopeSegment> ropeSegments = new List<RopeSegment>();

	private float ropeSegLen = 0.25f;

	[SerializeField]
	private int segmentLength = 35;

	[SerializeField]
	private float pointMoveSpeed = 3f;

	private float lineWidth = 0.1f;

	private static NonAllocOverlapper nonAllocOverlapper;

	static RopeBridge()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		nonAllocOverlapper = new NonAllocOverlapper(32);
	}

	public bool FindAndMove()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		((ContactFilter2D)(ref nonAllocOverlapper.contactFilter)).SetLayerMask(_layers.Evaluate(((Component)this).gameObject));
		if (nonAllocOverlapper.OverlapCollider(_findRange).results.Count == 0)
		{
			return false;
		}
		return true;
	}

	private void Start()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		lineRenderer = ((Component)this).GetComponent<LineRenderer>();
		Vector3 position = StartPoint.position;
		for (int i = 0; i < segmentLength; i++)
		{
			ropeSegments.Add(new RopeSegment(Vector2.op_Implicit(position)));
			position.y -= ropeSegLen;
		}
	}

	private void Update()
	{
		DrawRope();
	}

	private void FixedUpdate()
	{
		Simulate();
	}

	private void Simulate()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = default(Vector2);
		((Vector2)(ref val))._002Ector(0f, -1f);
		for (int i = 1; i < segmentLength; i++)
		{
			RopeSegment value = ropeSegments[i];
			Vector2 val2 = value.posNow - value.posOld;
			value.posOld = value.posNow;
			ref Vector2 posNow = ref value.posNow;
			posNow += val2;
			ref Vector2 posNow2 = ref value.posNow;
			posNow2 += val * Time.fixedDeltaTime;
			ropeSegments[i] = value;
		}
		for (int j = 0; j < 50; j++)
		{
			ApplyConstraint();
		}
	}

	private void ApplyConstraint()
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		RopeSegment value = ropeSegments[0];
		value.posNow = Vector2.op_Implicit(StartPoint.position);
		ropeSegments[0] = value;
		RopeSegment value2 = ropeSegments[ropeSegments.Count - 1];
		value2.posNow = Vector2.op_Implicit(EndPoint.position);
		ropeSegments[ropeSegments.Count - 1] = value2;
		for (int i = 0; i < segmentLength - 1; i++)
		{
			RopeSegment value3 = ropeSegments[i];
			RopeSegment value4 = ropeSegments[i + 1];
			Vector2 val = value3.posNow - value4.posNow;
			float magnitude = ((Vector2)(ref val)).magnitude;
			float num = Mathf.Abs(magnitude - ropeSegLen);
			Vector2 val2 = Vector2.zero;
			if (magnitude > ropeSegLen)
			{
				val = value3.posNow - value4.posNow;
				val2 = ((Vector2)(ref val)).normalized;
			}
			else if (magnitude < ropeSegLen)
			{
				val = value4.posNow - value3.posNow;
				val2 = ((Vector2)(ref val)).normalized;
			}
			Vector2 val3 = val2 * num;
			if (i != 0)
			{
				ref Vector2 posNow = ref value3.posNow;
				posNow -= val3 * 0.5f;
				ropeSegments[i] = value3;
				ref Vector2 posNow2 = ref value4.posNow;
				posNow2 += val3 * 0.5f;
				ropeSegments[i + 1] = value4;
			}
			else
			{
				ref Vector2 posNow3 = ref value4.posNow;
				posNow3 += val3;
				ropeSegments[i + 1] = value4;
			}
		}
	}

	private void DrawRope()
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		float num = lineWidth;
		lineRenderer.startWidth = num;
		lineRenderer.endWidth = num;
		Vector3[] array = (Vector3[])(object)new Vector3[segmentLength];
		for (int i = 0; i < segmentLength; i++)
		{
			array[i] = Vector2.op_Implicit(ropeSegments[i].posNow);
		}
		lineRenderer.positionCount = array.Length;
		lineRenderer.SetPositions(array);
	}
}
