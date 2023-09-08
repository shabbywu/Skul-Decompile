using System.Collections;
using Characters.Operations;
using UnityEngine;

namespace Characters.Gear.Items.Customs;

public sealed class ArchbishopsBibleMovement : MonoBehaviour
{
	[SerializeField]
	private float _speed;

	[SerializeField]
	private Collider2D _collider;

	[SerializeField]
	private Collider2D _findRange;

	[SerializeField]
	private TargetLayer _targetLayer;

	private Character _owner;

	private Character _target;

	private CoroutineReference _chaseReference;

	private float _remainFindTime;

	private const float _findInterval = 1f;

	private Collider2D _platform;

	public void StartMove(OperationInfos operationInfos)
	{
		_owner = operationInfos.owner;
		FindTarget();
		FindPlatform();
		_chaseReference.Stop();
		_chaseReference = ((MonoBehaviour)(object)this).StartCoroutineWithReference(CChase());
	}

	private void FindTarget()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		_target = TargetFinder.FindClosestTarget(_findRange, _collider, _targetLayer.Evaluate(((Component)this).gameObject));
		_remainFindTime = 1f;
	}

	private void FindPlatform()
	{
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)_target != (Object)null)
		{
			_platform = _target.movement.controller.collisionState.lastStandingCollider;
			if ((Object)(object)_platform == (Object)null)
			{
				_target.movement.TryGetClosestBelowCollider(out _platform, Layers.footholdMask);
			}
		}
		else
		{
			_platform = _owner.movement.controller.collisionState.lastStandingCollider;
			if ((Object)(object)_platform == (Object)null)
			{
				_owner.movement.TryGetClosestBelowCollider(out _platform, Layers.footholdMask);
			}
		}
	}

	private void MoveToTarget()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		float deltaTime = Chronometer.global.deltaTime;
		float x = ((Component)_target).transform.position.x;
		Bounds bounds = _platform.bounds;
		Vector2 val = default(Vector2);
		((Vector2)(ref val))._002Ector(x, ((Bounds)(ref bounds)).max.y);
		float x2 = val.x;
		bounds = _platform.bounds;
		if (x2 >= ((Bounds)(ref bounds)).max.x)
		{
			bounds = _platform.bounds;
			float x3 = ((Bounds)(ref bounds)).max.x;
			bounds = _collider.bounds;
			val.x = x3 - ((Bounds)(ref bounds)).extents.x;
		}
		float x4 = val.x;
		bounds = _platform.bounds;
		if (x4 <= ((Bounds)(ref bounds)).min.x)
		{
			bounds = _platform.bounds;
			float x5 = ((Bounds)(ref bounds)).min.x;
			bounds = _collider.bounds;
			val.x = x5 + ((Bounds)(ref bounds)).extents.x;
		}
		((Component)this).transform.position = Vector3.Lerp(((Component)this).transform.position, Vector2.op_Implicit(val), deltaTime * _speed);
	}

	private IEnumerator CChase()
	{
		while (true)
		{
			yield return null;
			_remainFindTime -= Chronometer.global.deltaTime;
			if (_remainFindTime <= 0f)
			{
				FindTarget();
			}
			if (!((Object)(object)_target == (Object)null) && !_target.health.dead)
			{
				MoveToTarget();
			}
		}
	}
}
