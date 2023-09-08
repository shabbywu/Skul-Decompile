using System;
using System.Collections;
using Characters;
using UnityEngine;

namespace TwoDLaserPack;

public class LineBasedLaser : MonoBehaviour
{
	public delegate void LaserHitTriggerHandler(RaycastHit2D hitInfo);

	[SerializeField]
	private Character _character;

	public LineRenderer laserLineRendererArc;

	public LineRenderer laserLineRenderer;

	public int laserArcSegments = 20;

	public bool laserActive;

	public bool ignoreCollisions;

	public GameObject targetGo;

	public float laserTexOffsetSpeed = 1f;

	public ParticleSystem hitSparkParticleSystem;

	[SerializeField]
	private PoolObject laserhitEffect;

	private PoolObject _hitEffect;

	public float laserArcMaxYDown;

	public float laserArcMaxYUp;

	public float maxLaserRaycastDistance = 20f;

	public bool laserRotationEnabled;

	public bool lerpLaserRotation;

	public float turningRate = 3f;

	public float collisionTriggerInterval = 0.25f;

	public LayerMask mask;

	public string sortLayer = "Default";

	public int sortOrder;

	public bool useArc;

	private GameObject gameObjectCached;

	private float laserAngle;

	private float laserTextureOffset;

	private float laserTextureXScale;

	private float startLaserTextureXScale;

	private int startLaserSegmentLength;

	private bool waitingForTriggerTime;

	private EmissionModule hitSparkEmission;

	public event LaserHitTriggerHandler OnLaserHitTriggered;

	private void Awake()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		hitSparkEmission = hitSparkParticleSystem.emission;
	}

	private void Start()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		startLaserTextureXScale = ((Renderer)laserLineRenderer).material.mainTextureScale.x;
		startLaserSegmentLength = laserArcSegments;
		((Renderer)laserLineRenderer).sortingLayerName = sortLayer;
		((Renderer)laserLineRenderer).sortingOrder = sortOrder;
		((Renderer)laserLineRendererArc).sortingLayerName = sortLayer;
		((Renderer)laserLineRendererArc).sortingOrder = sortOrder;
	}

	private void OnEnable()
	{
		gameObjectCached = ((Component)this).gameObject;
		if ((Object)(object)laserLineRendererArc != (Object)null)
		{
			laserLineRendererArc.SetVertexCount(laserArcSegments);
		}
	}

	private void Update()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)gameObjectCached != (Object)null) || !laserActive)
		{
			return;
		}
		((Renderer)laserLineRenderer).material.mainTextureOffset = new Vector2(laserTextureOffset, 0f);
		laserTextureOffset -= _character.chronometer.master.deltaTime * laserTexOffsetSpeed;
		RaycastHit2D val3;
		if (laserRotationEnabled && (Object)(object)targetGo != (Object)null)
		{
			Vector3 val = targetGo.transform.position - gameObjectCached.transform.position;
			laserAngle = Mathf.Atan2(val.y, val.x);
			if (laserAngle < 0f)
			{
				laserAngle = (float)Math.PI * 2f + laserAngle;
			}
			float num = laserAngle * 57.29578f;
			if (lerpLaserRotation)
			{
				((Component)this).transform.rotation = Quaternion.Slerp(((Component)this).transform.rotation, Quaternion.AngleAxis(num, ((Component)this).transform.forward), _character.chronometer.master.deltaTime * turningRate);
				Vector3 val2 = ((Component)this).transform.rotation * Vector3.right;
				val3 = Physics2D.Raycast(Vector2.op_Implicit(((Component)this).transform.position), Vector2.op_Implicit(val2), maxLaserRaycastDistance, LayerMask.op_Implicit(mask));
			}
			else
			{
				((Component)this).transform.rotation = Quaternion.AngleAxis(num, ((Component)this).transform.forward);
				val3 = Physics2D.Raycast(Vector2.op_Implicit(((Component)this).transform.position), Vector2.op_Implicit(val), maxLaserRaycastDistance, LayerMask.op_Implicit(mask));
			}
		}
		else
		{
			val3 = Physics2D.Raycast(Vector2.op_Implicit(((Component)this).transform.position), Vector2.op_Implicit(((Component)this).transform.right), maxLaserRaycastDistance, LayerMask.op_Implicit(mask));
		}
		if (!ignoreCollisions)
		{
			if ((Object)(object)((RaycastHit2D)(ref val3)).collider != (Object)null)
			{
				SetLaserEndToTargetLocation(val3);
				if (!waitingForTriggerTime)
				{
					((MonoBehaviour)this).StartCoroutine(HitTrigger(collisionTriggerInterval, val3));
				}
			}
			else
			{
				SetLaserToDefaultLength();
			}
		}
		else
		{
			SetLaserToDefaultLength();
		}
	}

	private IEnumerator HitTrigger(float triggerInterval, RaycastHit2D hit)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		waitingForTriggerTime = true;
		this.OnLaserHitTriggered?.Invoke(hit);
		yield return _character.chronometer.master.WaitForSeconds(triggerInterval);
		waitingForTriggerTime = false;
	}

	public void SetLaserState(bool enabledStatus)
	{
		laserActive = enabledStatus;
		((Renderer)laserLineRenderer).enabled = enabledStatus;
		if ((Object)(object)laserLineRendererArc != (Object)null)
		{
			((Renderer)laserLineRendererArc).enabled = enabledStatus;
		}
		if ((Object)(object)hitSparkParticleSystem != (Object)null)
		{
			((EmissionModule)(ref hitSparkEmission)).enabled = enabledStatus;
		}
	}

	private void SetLaserEndToTargetLocation(RaycastHit2D hit)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		float num = Vector2.Distance(((RaycastHit2D)(ref hit)).point, Vector2.op_Implicit(((Component)laserLineRenderer).transform.position));
		laserLineRenderer.SetPosition(1, Vector2.op_Implicit(new Vector2(num, 0f)));
		laserTextureXScale = startLaserTextureXScale * num;
		((Renderer)laserLineRenderer).material.mainTextureScale = new Vector2(laserTextureXScale, 1f);
		if (useArc)
		{
			if (!((Renderer)laserLineRendererArc).enabled)
			{
				((Renderer)laserLineRendererArc).enabled = true;
			}
			int vertexCount = Mathf.Abs((int)num);
			laserLineRendererArc.SetVertexCount(vertexCount);
			laserArcSegments = vertexCount;
			SetLaserArcVertices(num, useHitPoint: true);
		}
		else if (((Renderer)laserLineRendererArc).enabled)
		{
			((Renderer)laserLineRendererArc).enabled = false;
		}
		if ((Object)(object)hitSparkParticleSystem != (Object)null)
		{
			((Component)hitSparkParticleSystem).transform.position = Vector2.op_Implicit(((RaycastHit2D)(ref hit)).point);
			if ((Object)(object)_hitEffect == (Object)null)
			{
				laserhitEffect.Spawn(Vector2.op_Implicit(((RaycastHit2D)(ref hit)).point), true);
			}
			((Component)_hitEffect).transform.position = Vector2.op_Implicit(((RaycastHit2D)(ref hit)).point);
			((EmissionModule)(ref hitSparkEmission)).enabled = true;
		}
	}

	private void SetLaserToDefaultLength()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		laserLineRenderer.SetPosition(1, Vector2.op_Implicit(new Vector2((float)laserArcSegments, 0f)));
		laserTextureXScale = startLaserTextureXScale * (float)laserArcSegments;
		((Renderer)laserLineRenderer).material.mainTextureScale = new Vector2(laserTextureXScale, 1f);
		if (useArc)
		{
			if (!((Renderer)laserLineRendererArc).enabled)
			{
				((Renderer)laserLineRendererArc).enabled = true;
			}
			laserLineRendererArc.SetVertexCount(startLaserSegmentLength);
			laserArcSegments = startLaserSegmentLength;
			SetLaserArcVertices(0f, useHitPoint: false);
		}
		else
		{
			if (((Renderer)laserLineRendererArc).enabled)
			{
				((Renderer)laserLineRendererArc).enabled = false;
			}
			laserLineRendererArc.SetVertexCount(startLaserSegmentLength);
			laserArcSegments = startLaserSegmentLength;
		}
		if ((Object)(object)hitSparkParticleSystem != (Object)null)
		{
			((EmissionModule)(ref hitSparkEmission)).enabled = false;
			((Component)hitSparkParticleSystem).transform.position = Vector2.op_Implicit(new Vector2((float)laserArcSegments, ((Component)this).transform.position.y));
		}
	}

	private void SetLaserArcVertices(float distancePoint, bool useHitPoint)
	{
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = default(Vector2);
		for (int i = 1; i < laserArcSegments; i++)
		{
			float num = Mathf.Clamp(Mathf.Sin((float)i + Time.time * Random.Range(0.5f, 1.3f)), laserArcMaxYDown, laserArcMaxYUp);
			((Vector2)(ref val))._002Ector((float)i * 1.2f, num);
			if (useHitPoint && i == laserArcSegments - 1)
			{
				laserLineRendererArc.SetPosition(i, Vector2.op_Implicit(new Vector2(distancePoint, 0f)));
			}
			else
			{
				laserLineRendererArc.SetPosition(i, Vector2.op_Implicit(val));
			}
		}
	}
}
