using System.Collections;
using Characters.Movements;
using GameResources;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Player;

public class PlayerCameraController : MonoBehaviour
{
	[SerializeField]
	[GetComponent]
	private Character _character;

	[SerializeField]
	private Camera _deathCamera;

	private readonly LineSequenceNonAllocCaster _lineCaster = new LineSequenceNonAllocCaster(1, 2);

	private Collider2D _ground;

	private CharacterController2D.CollisionState _collisionState;

	private Vector3 _trackPosition;

	public Vector3 trackPosition => _trackPosition;

	public float trackSpeed => 7f;

	private void Awake()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		_collisionState = _character.movement.controller.collisionState;
		RayCaster val = new RayCaster
		{
			direction = Vector2.down,
			distance = 2.75f
		};
		((ContactFilter2D)(ref ((Caster)val).contactFilter)).SetLayerMask(LayerMask.op_Implicit(393216));
		_lineCaster.caster = (Caster)(object)val;
		_character.health.onDied += OnDie;
		_deathCamera.targetTexture = CommonResource.instance.deathCamRenderTexture;
	}

	private void OnDestroy()
	{
		_deathCamera.targetTexture = null;
	}

	private void OnDie()
	{
		((MonoBehaviour)CoroutineProxy.instance).StartCoroutine(CRenderDeathCamera());
	}

	private IEnumerator CRenderDeathCamera()
	{
		yield return (object)new WaitForSecondsRealtime(1.6f);
		RenderDeathCamera();
	}

	public void RenderDeathCamera()
	{
		_deathCamera.Render();
	}

	private void Update()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		Bounds bounds = ((Collider2D)_character.collider).bounds;
		_lineCaster.start = new Vector2(((Bounds)(ref bounds)).min.x, ((Bounds)(ref bounds)).min.y);
		_lineCaster.end = new Vector2(((Bounds)(ref bounds)).max.x, ((Bounds)(ref bounds)).min.y);
		_lineCaster.Cast();
		_trackPosition = ((Component)this).transform.position;
		_trackPosition.y += 1f;
		RaycastHit2D? val = null;
		ReadonlyBoundedList<RaycastHit2D> results = _lineCaster.nonAllocCasters[0].results;
		ReadonlyBoundedList<RaycastHit2D> results2 = _lineCaster.nonAllocCasters[1].results;
		RaycastHit2D val2 = results[0];
		RaycastHit2D val3 = results2[0];
		bool flag = results.Count > 0;
		bool flag2 = results2.Count > 0;
		if (flag && flag2)
		{
			val = ((((RaycastHit2D)(ref val2)).distance < ((RaycastHit2D)(ref val3)).distance) ? val2 : val3);
		}
		else if (flag && !flag2)
		{
			val = val2;
		}
		else if (!flag && flag2)
		{
			val = val3;
		}
		if (val.HasValue)
		{
			RaycastHit2D value = val.Value;
			_ground = ((RaycastHit2D)(ref value)).collider;
			Bounds bounds2 = _ground.bounds;
			if (((Bounds)(ref bounds2)).size.x > 6f)
			{
				ref Vector3 reference = ref _trackPosition;
				bounds2 = _ground.bounds;
				reference.y = ((Bounds)(ref bounds2)).max.y + 2.5f;
			}
		}
	}
}
