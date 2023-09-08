using System;
using UnityEngine;

namespace Characters.Projectiles.Movements;

public class TrajectoryToPoint : Movement
{
	[SerializeField]
	private TargetFinder _finder;

	[SerializeField]
	private float _easingTime;

	[SerializeField]
	private float _gravity;

	private float _elapseTime;

	private float _targetDistance;

	private Target _target;

	private bool _isInitialized;

	private float _firingAngle = 45f;

	private Vector3 _targetPosition = Vector3.zero;

	public void OnEnable()
	{
		if (_isInitialized)
		{
			InitializedTrajectory();
		}
	}

	public override void Initialize(IProjectile projectile, float direction)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)_finder.range != (Object)null)
		{
			_finder.Initialize(projectile);
			InitializedTrajectory();
			Target target = _finder.Find();
			Bounds bounds = target.collider.bounds;
			Vector3 val = ((Bounds)(ref bounds)).center - ((Component)this).transform.position;
			Vector3 position = ((Component)this).transform.position;
			bounds = target.collider.bounds;
			_targetDistance = Vector3.Distance(position, ((Bounds)(ref bounds)).center);
			_firingAngle = ((((Component)this).transform.position.x < ((Component)target).transform.position.x) ? _firingAngle : (_firingAngle + 90f));
			Debug.Log((object)_firingAngle);
			_firingAngle = 135f;
			Debug.Log((object)_firingAngle);
			float num = _targetDistance / (Mathf.Sin(2f * _firingAngle * ((float)Math.PI / 180f)) / _gravity);
			float num2 = Mathf.Sqrt(num) * Mathf.Cos(_firingAngle * ((float)Math.PI / 180f));
			Mathf.Sqrt(num);
			Mathf.Sin(_firingAngle * ((float)Math.PI / 180f));
			_ = _targetDistance / num2;
			if ((Object)(object)target != (Object)null)
			{
				direction = Mathf.Atan2(val.y, val.x) * 57.29578f;
				Debug.Log((object)direction);
			}
			_elapseTime = 0f;
		}
		base.Initialize(projectile, direction);
	}

	public override (Vector2 direction, float speed) GetSpeed(float time, float deltaTime)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		_targetDistance = Vector3.Distance(((Component)this).transform.position, _targetPosition);
		float num = _targetDistance / (Mathf.Sin(2f * _firingAngle * ((float)Math.PI / 180f)) / _gravity);
		float num2 = Mathf.Sqrt(num) * Mathf.Cos(_firingAngle * ((float)Math.PI / 180f));
		float num3 = Mathf.Sqrt(num) * Mathf.Sin(_firingAngle * ((float)Math.PI / 180f));
		_elapseTime += Time.deltaTime;
		Vector2 val = default(Vector2);
		val.x = num2;
		val.y = num3 - _gravity * _elapseTime;
		Vector2 val2 = new Vector2(num2, num3 - _gravity * _elapseTime);
		_ = ((Vector2)(ref val2)).magnitude;
		return (((Vector2)(ref val)).normalized, ((Vector2)(ref val)).magnitude);
	}

	public bool InitializedTrajectory()
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		if (_finder == null || (Object)(object)_finder.range == (Object)null)
		{
			return false;
		}
		_target = _finder.Find();
		_elapseTime = 0f;
		Bounds bounds = _finder.Find().collider.bounds;
		_targetPosition = new Vector3((((Bounds)(ref bounds)).min.x + ((Bounds)(ref bounds)).max.x) / 2f, ((Bounds)(ref bounds)).min.y);
		_firingAngle = ((((Component)_target).transform.position.x > ((Component)this).transform.position.x) ? _firingAngle : (0f - _firingAngle));
		if (!((Object)(object)_target != (Object)null))
		{
			return false;
		}
		return true;
	}
}
