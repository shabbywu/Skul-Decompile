using System.Collections;
using Characters.Operations;
using Characters.Operations.Attack;
using Level;
using Services;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Hero;

public class KilivanFinish : MonoBehaviour
{
	[SerializeField]
	private Character _owner;

	[SerializeField]
	private float _speed;

	[SerializeField]
	private LayerMask _collision;

	[SerializeField]
	private Transform _firePosition;

	[SerializeField]
	private Transform _projectile;

	[Subcomponent(typeof(OperationInfos))]
	[SerializeField]
	private OperationInfos _fireOperations;

	[Subcomponent(typeof(OperationInfos))]
	[SerializeField]
	private OperationInfos _hitOperations;

	[Subcomponent(typeof(SweepAttack))]
	[SerializeField]
	private SweepAttack _sweepAttack;

	private Vector2 _direction;

	public bool running { get; set; }

	private void Awake()
	{
		_fireOperations.Initialize();
		_hitOperations.Initialize();
		_sweepAttack.Initialize();
	}

	public IEnumerator CFire()
	{
		running = true;
		_direction = ((_owner.lookingDirection == Character.LookingDirection.Right) ? Vector2.right : Vector2.left);
		yield return CMove(Vector2.op_Implicit(((Component)Singleton<Service>.Instance.levelManager.player).transform.position));
	}

	public Vector2 Fire(Vector2 direction)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		_direction = direction;
		((Component)_projectile).transform.position = _firePosition.position;
		float num = Mathf.Atan2(direction.y, direction.x) * 57.29578f;
		if (_owner.lookingDirection == Character.LookingDirection.Left)
		{
			num += 180f;
		}
		((Component)_projectile).transform.rotation = Quaternion.Euler(0f, 0f, num);
		_sweepAttack.Run(_owner);
		Show();
		Singleton<Service>.Instance.levelManager.player.movement.TryBelowRayCast(LayerMask.op_Implicit(262144), out var point, 100f);
		return ((RaycastHit2D)(ref point)).point;
	}

	public bool UpdateMove(float deltaTime, Vector2 destination)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		float num = _speed * deltaTime;
		if (!DetectCollision(destination, num))
		{
			_projectile.Translate(Vector2.op_Implicit(_direction * num), (Space)0);
			return true;
		}
		_sweepAttack.Stop();
		Hide();
		running = false;
		return false;
	}

	private IEnumerator CMove(Vector2 destination)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		((Component)_projectile).transform.position = _firePosition.position;
		_sweepAttack.Run(_owner);
		Show();
		float num = _speed * Chronometer.global.deltaTime;
		while (!DetectCollision(destination, num))
		{
			yield return null;
			num = _speed * Chronometer.global.deltaTime;
			_projectile.Translate(Vector2.op_Implicit(_direction * num), (Space)0);
		}
		_sweepAttack.Stop();
		Hide();
		running = false;
	}

	private bool DetectCollision(Vector2 destination, float speed)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		float x = ((Component)_projectile).transform.position.x;
		if ((_direction.x >= 0f && x > destination.x) || (_direction.x <= 0f && x < destination.x))
		{
			OnCollision(destination);
			return true;
		}
		RaycastHit2D val = Physics2D.Raycast(Vector2.op_Implicit(((Component)_projectile).transform.position), _direction, speed, LayerMask.op_Implicit(_collision));
		if (RaycastHit2D.op_Implicit(val))
		{
			OnCollision(((RaycastHit2D)(ref val)).point);
			return true;
		}
		return false;
	}

	private void OnCollision(Vector2 hitPoint)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		((Component)_projectile).transform.position = Vector2.op_Implicit(hitPoint);
		((Component)_hitOperations).gameObject.SetActive(true);
		_hitOperations.Run(_owner);
		float x = hitPoint.x;
		Evaluate(ref x);
		((Vector2)(ref hitPoint))._002Ector(x, hitPoint.y);
		_owner.movement.controller.TeleportUponGround(hitPoint, 3f);
	}

	private void Evaluate(ref float x)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		Bounds bounds = Map.Instance.bounds;
		float x2 = ((Bounds)(ref bounds)).max.x;
		bounds = ((Collider2D)_owner.collider).bounds;
		float num = x2 - ((Bounds)(ref bounds)).size.x;
		bounds = Map.Instance.bounds;
		float x3 = ((Bounds)(ref bounds)).min.x;
		bounds = ((Collider2D)_owner.collider).bounds;
		float num2 = x3 + ((Bounds)(ref bounds)).size.x;
		if (x > num)
		{
			x = num;
		}
		if (x < num2)
		{
			x = num2;
		}
	}

	private void Show()
	{
		((Component)_projectile).gameObject.SetActive(true);
	}

	private void Hide()
	{
		((Component)_projectile).gameObject.SetActive(false);
	}
}
