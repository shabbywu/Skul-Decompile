using System;
using System.Collections;
using UnityEngine;

namespace TwoDLaserPack;

public class SpriteBasedLaser : MonoBehaviour
{
	public delegate void LaserHitTriggerHandler(RaycastHit2D hitInfo);

	public GameObject laserStartPiece;

	public GameObject laserMiddlePiece;

	public GameObject laserEndPiece;

	public LineRenderer laserLineRendererArc;

	public int laserArcSegments = 20;

	public RandomPositionMover laserOscillationPositionerScript;

	public bool oscillateLaser;

	public float maxLaserLength = 20f;

	public float oscillationSpeed = 1f;

	public bool laserActive;

	public bool ignoreCollisions;

	public GameObject targetGo;

	public ParticleSystem hitSparkParticleSystem;

	public float laserArcMaxYDown;

	public float laserArcMaxYUp;

	public float maxLaserRaycastDistance;

	public bool laserRotationEnabled;

	public bool lerpLaserRotation;

	public float turningRate = 3f;

	public float collisionTriggerInterval = 0.25f;

	public LayerMask mask;

	public bool useArc;

	public float oscillationThreshold = 0.2f;

	private GameObject gameObjectCached;

	private float laserAngle;

	private float lerpYValue;

	private float startLaserLength;

	private GameObject startGoPiece;

	private GameObject middleGoPiece;

	private GameObject endGoPiece;

	private float startSpriteWidth;

	private bool waitingForTriggerTime;

	private EmissionModule hitSparkEmission;

	public event LaserHitTriggerHandler OnLaserHitTriggered;

	private void Awake()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		hitSparkEmission = hitSparkParticleSystem.emission;
	}

	private void OnEnable()
	{
		gameObjectCached = ((Component)this).gameObject;
		if ((Object)(object)laserLineRendererArc != (Object)null)
		{
			laserLineRendererArc.SetVertexCount(laserArcSegments);
		}
	}

	private void Start()
	{
		startLaserLength = maxLaserLength;
		if ((Object)(object)laserOscillationPositionerScript != (Object)null)
		{
			laserOscillationPositionerScript.radius = oscillationThreshold;
		}
	}

	private void OscillateLaserParts(float currentLaserDistance)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)laserOscillationPositionerScript == (Object)null))
		{
			lerpYValue = Mathf.Lerp(middleGoPiece.transform.localPosition.y, laserOscillationPositionerScript.randomPointInCircle.y, Time.deltaTime * oscillationSpeed);
			if ((Object)(object)startGoPiece != (Object)null && (Object)(object)middleGoPiece != (Object)null)
			{
				Vector2 val = default(Vector2);
				((Vector2)(ref val))._002Ector(startGoPiece.transform.localPosition.x, laserOscillationPositionerScript.randomPointInCircle.y);
				Vector2 val2 = Vector2.Lerp(Vector2.op_Implicit(startGoPiece.transform.localPosition), val, Time.deltaTime * oscillationSpeed);
				startGoPiece.transform.localPosition = Vector2.op_Implicit(val2);
				Vector2 val3 = default(Vector2);
				((Vector2)(ref val3))._002Ector(currentLaserDistance / 2f + startSpriteWidth / 4f, lerpYValue);
				middleGoPiece.transform.localPosition = Vector2.op_Implicit(val3);
			}
			if ((Object)(object)endGoPiece != (Object)null)
			{
				Vector2 val4 = default(Vector2);
				((Vector2)(ref val4))._002Ector(currentLaserDistance + startSpriteWidth / 2f, lerpYValue);
				endGoPiece.transform.localPosition = Vector2.op_Implicit(val4);
			}
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

	private void Update()
	{
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0334: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Unknown result type (might be due to invalid IL or missing references)
		//IL_0349: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_035f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0364: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0385: Unknown result type (might be due to invalid IL or missing references)
		//IL_0390: Unknown result type (might be due to invalid IL or missing references)
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0305: Unknown result type (might be due to invalid IL or missing references)
		//IL_030a: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0310: Unknown result type (might be due to invalid IL or missing references)
		//IL_031c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0326: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03de: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_045b: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)gameObjectCached != (Object)null) || !laserActive)
		{
			return;
		}
		if ((Object)(object)startGoPiece == (Object)null)
		{
			InstantiateLaserPart(ref startGoPiece, laserStartPiece);
			startGoPiece.transform.parent = ((Component)this).transform;
			startGoPiece.transform.localPosition = Vector2.op_Implicit(Vector2.zero);
			Bounds bounds = laserStartPiece.GetComponent<Renderer>().bounds;
			startSpriteWidth = ((Bounds)(ref bounds)).size.x;
		}
		if ((Object)(object)middleGoPiece == (Object)null)
		{
			InstantiateLaserPart(ref middleGoPiece, laserMiddlePiece);
			middleGoPiece.transform.parent = ((Component)this).transform;
			middleGoPiece.transform.localPosition = Vector2.op_Implicit(Vector2.zero);
		}
		middleGoPiece.transform.localScale = new Vector3(maxLaserLength - startSpriteWidth + 0.2f, middleGoPiece.transform.localScale.y, middleGoPiece.transform.localScale.z);
		if (oscillateLaser)
		{
			OscillateLaserParts(maxLaserLength);
		}
		else
		{
			if ((Object)(object)middleGoPiece != (Object)null)
			{
				middleGoPiece.transform.localPosition = Vector2.op_Implicit(new Vector2(maxLaserLength / 2f + startSpriteWidth / 4f, lerpYValue));
			}
			if ((Object)(object)endGoPiece != (Object)null)
			{
				endGoPiece.transform.localPosition = Vector2.op_Implicit(new Vector2(maxLaserLength + startSpriteWidth / 2f, 0f));
			}
		}
		RaycastHit2D hit;
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
				((Component)this).transform.rotation = Quaternion.Slerp(((Component)this).transform.rotation, Quaternion.AngleAxis(num, ((Component)this).transform.forward), Time.deltaTime * turningRate);
				Vector3 val2 = ((Component)this).transform.rotation * Vector3.right;
				hit = Physics2D.Raycast(Vector2.op_Implicit(((Component)this).transform.position), Vector2.op_Implicit(val2), maxLaserRaycastDistance, LayerMask.op_Implicit(mask));
			}
			else
			{
				((Component)this).transform.rotation = Quaternion.AngleAxis(num, ((Component)this).transform.forward);
				hit = Physics2D.Raycast(Vector2.op_Implicit(((Component)this).transform.position), Vector2.op_Implicit(val), maxLaserRaycastDistance, LayerMask.op_Implicit(mask));
			}
		}
		else
		{
			hit = Physics2D.Raycast(Vector2.op_Implicit(((Component)this).transform.position), Vector2.op_Implicit(((Component)this).transform.right), maxLaserRaycastDistance, LayerMask.op_Implicit(mask));
		}
		if (!ignoreCollisions)
		{
			if ((Object)(object)((RaycastHit2D)(ref hit)).collider != (Object)null)
			{
				maxLaserLength = Vector2.Distance(((RaycastHit2D)(ref hit)).point, Vector2.op_Implicit(((Component)this).transform.position)) + startSpriteWidth / 4f;
				InstantiateLaserPart(ref endGoPiece, laserEndPiece);
				if ((Object)(object)hitSparkParticleSystem != (Object)null)
				{
					((Component)hitSparkParticleSystem).transform.position = Vector2.op_Implicit(((RaycastHit2D)(ref hit)).point);
					((EmissionModule)(ref hitSparkEmission)).enabled = true;
				}
				if (useArc)
				{
					if (!((Renderer)laserLineRendererArc).enabled)
					{
						((Renderer)laserLineRendererArc).enabled = true;
					}
					SetLaserArcVertices(maxLaserLength, useHitPoint: true);
					SetLaserArcSegmentLength();
				}
				else if (((Renderer)laserLineRendererArc).enabled)
				{
					((Renderer)laserLineRendererArc).enabled = false;
				}
				if (!waitingForTriggerTime)
				{
					((MonoBehaviour)this).StartCoroutine(HitTrigger(collisionTriggerInterval, hit));
				}
				return;
			}
			SetLaserBackToDefaults();
			if (useArc)
			{
				if (!((Renderer)laserLineRendererArc).enabled)
				{
					((Renderer)laserLineRendererArc).enabled = true;
				}
				SetLaserArcSegmentLength();
				SetLaserArcVertices(0f, useHitPoint: false);
			}
			else if (((Renderer)laserLineRendererArc).enabled)
			{
				((Renderer)laserLineRendererArc).enabled = false;
			}
		}
		else
		{
			SetLaserBackToDefaults();
			SetLaserArcVertices(0f, useHitPoint: false);
			SetLaserArcSegmentLength();
		}
	}

	private IEnumerator HitTrigger(float triggerInterval, RaycastHit2D hit)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		waitingForTriggerTime = true;
		if (this.OnLaserHitTriggered != null)
		{
			this.OnLaserHitTriggered(hit);
		}
		yield return (object)new WaitForSeconds(triggerInterval);
		waitingForTriggerTime = false;
	}

	public void SetLaserState(bool enabledStatus)
	{
		laserActive = enabledStatus;
		if ((Object)(object)startGoPiece != (Object)null)
		{
			startGoPiece.SetActive(enabledStatus);
		}
		if ((Object)(object)middleGoPiece != (Object)null)
		{
			middleGoPiece.SetActive(enabledStatus);
		}
		if ((Object)(object)endGoPiece != (Object)null)
		{
			endGoPiece.SetActive(enabledStatus);
		}
		if ((Object)(object)laserLineRendererArc != (Object)null)
		{
			((Renderer)laserLineRendererArc).enabled = enabledStatus;
		}
		if ((Object)(object)hitSparkParticleSystem != (Object)null)
		{
			((EmissionModule)(ref hitSparkEmission)).enabled = enabledStatus;
		}
	}

	private void SetLaserArcSegmentLength()
	{
		int vertexCount = Mathf.Abs((int)maxLaserLength);
		laserLineRendererArc.SetVertexCount(vertexCount);
		laserArcSegments = vertexCount;
	}

	private void SetLaserBackToDefaults()
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		Object.Destroy((Object)(object)endGoPiece);
		maxLaserLength = startLaserLength;
		if ((Object)(object)hitSparkParticleSystem != (Object)null)
		{
			((EmissionModule)(ref hitSparkEmission)).enabled = false;
			((Component)hitSparkParticleSystem).transform.position = Vector2.op_Implicit(new Vector2(maxLaserLength, ((Component)this).transform.position.y));
		}
	}

	private void InstantiateLaserPart(ref GameObject laserComponent, GameObject laserPart)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)laserComponent == (Object)null)
		{
			laserComponent = Object.Instantiate<GameObject>(laserPart);
			laserComponent.transform.parent = ((Component)this).gameObject.transform;
			laserComponent.transform.localPosition = Vector2.op_Implicit(Vector2.zero);
			laserComponent.transform.localEulerAngles = Vector2.op_Implicit(Vector2.zero);
		}
	}

	public void DisableLaserGameObjectComponents()
	{
		Object.Destroy((Object)(object)startGoPiece);
		Object.Destroy((Object)(object)middleGoPiece);
		Object.Destroy((Object)(object)endGoPiece);
	}
}
