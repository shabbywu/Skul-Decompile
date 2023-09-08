using System.Collections;
using FX;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Operations.Customs.DarkHero;

public sealed class BrutalCharge : CharacterOperation
{
	private enum Point
	{
		Ground,
		LeftWall,
		RightWall
	}

	[SerializeField]
	private CustomFloat _heightRange;

	[SerializeField]
	private CustomFloat _count;

	[SerializeField]
	private Transform[] _points;

	[SerializeField]
	private EffectInfo _sign;

	[SerializeField]
	private EffectInfo _smoke;

	[SerializeField]
	private EffectInfo _attack;

	[SerializeField]
	private EffectInfo _flame;

	private int _lowCount;

	private bool _lookRight;

	private Point _last;

	private Character _owner;

	private const float _effectXLength = 23f;

	private const float _extents = 1.35f;

	public override void Run(Character owner)
	{
		_owner = owner;
		((MonoBehaviour)this).StartCoroutine(CAttack(owner));
	}

	private IEnumerator CAttack(Character owner)
	{
		int count = (int)_count.value;
		if (count >= _points.Length)
		{
			count = _points.Length;
		}
		Collider2D platform = owner.movement.controller.collisionState.lastStandingCollider;
		if ((Object)(object)platform == (Object)null)
		{
			owner.movement.TryGetClosestBelowCollider(out platform, LayerMask.op_Implicit(262144));
		}
		_lowCount = 2;
		_lookRight = owner.lookingDirection == Character.LookingDirection.Right;
		_points[0].position = ((Component)owner).transform.position;
		float wallToWallCount;
		if (owner.movement.isGrounded)
		{
			wallToWallCount = ((count % 2 != 1) ? 2f : 3f);
			_last = Point.Ground;
		}
		else
		{
			float x = ((Component)owner).transform.position.x;
			Bounds bounds = platform.bounds;
			if (x <= ((Bounds)(ref bounds)).center.x)
			{
				wallToWallCount = ((count % 2 != 0) ? 2f : 3f);
				_last = Point.LeftWall;
			}
			else
			{
				wallToWallCount = ((count % 2 != 0) ? 2f : 3f);
				_last = Point.RightWall;
			}
		}
		Vector2 beforePoint = Vector2.op_Implicit(_points[0].position);
		Vector2 val = default(Vector2);
		for (int j = 1; j < count; j++)
		{
			float x2 = 0f;
			float y = 0f;
			switch (_last)
			{
			case Point.Ground:
				if (_lookRight)
				{
					MoveToRightWall(platform, out x2, out y);
				}
				else
				{
					MoveToLeftWall(platform, out x2, out y);
				}
				break;
			case Point.LeftWall:
				_lookRight = true;
				if (wallToWallCount > 0f)
				{
					wallToWallCount -= 1f;
					MoveToRightWall(platform, out x2, out y);
				}
				else
				{
					MoveToGround(platform, out x2, out y);
				}
				break;
			case Point.RightWall:
				_lookRight = false;
				if (wallToWallCount > 0f)
				{
					wallToWallCount -= 1f;
					MoveToLeftWall(platform, out x2, out y);
				}
				else
				{
					MoveToGround(platform, out x2, out y);
				}
				break;
			}
			((Vector2)(ref val))._002Ector(x2, y);
			_points[j].position = Vector2.op_Implicit(val);
			Vector2 val2 = val - beforePoint;
			float num = Mathf.Atan2(val2.y, val2.x) * 57.29578f;
			if (num < 0f)
			{
				num += 360f;
			}
			_points[j - 1].rotation = Quaternion.Euler(0f, 0f, num);
			_sign.Spawn(_points[j - 1].position, owner, num);
			beforePoint = val;
			yield return owner.chronometer.master.WaitForSeconds(0.1f);
		}
		yield return owner.chronometer.master.WaitForSeconds(0.5f);
		for (int j = 0; j < count - 1; j++)
		{
			float num2 = Vector2.Distance(MMMaths.Vector3ToVector2(_points[j].position), MMMaths.Vector3ToVector2(_points[j + 1].position));
			_attack.scaleX = new CustomFloat(num2 / 23f);
			EffectInfo attack = _attack;
			Vector3 position = _points[j].position;
			Quaternion rotation = _points[j].rotation;
			attack.Spawn(position, owner, ((Quaternion)(ref rotation)).eulerAngles.z);
			EffectInfo smoke = _smoke;
			Vector3 position2 = _points[j].position;
			rotation = _points[j].rotation;
			smoke.Spawn(position2, owner, ((Quaternion)(ref rotation)).eulerAngles.z);
			EffectInfo flame = _flame;
			Vector3 position3 = _points[j].position;
			rotation = _points[j].rotation;
			flame.Spawn(position3, owner, ((Quaternion)(ref rotation)).eulerAngles.z);
			yield return owner.chronometer.master.WaitForSeconds(0.1f);
		}
	}

	private void MoveToGround(Collider2D platform, out float x, out float y)
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		Bounds bounds;
		if (_lookRight)
		{
			bounds = platform.bounds;
			float num = ((Bounds)(ref bounds)).center.x + 2.5f;
			bounds = platform.bounds;
			x = Random.Range(num, ((Bounds)(ref bounds)).max.x - 1f);
		}
		else
		{
			bounds = platform.bounds;
			float num2 = ((Bounds)(ref bounds)).min.x + 1f;
			bounds = platform.bounds;
			x = Random.Range(num2, ((Bounds)(ref bounds)).center.x - 2.5f);
		}
		bounds = platform.bounds;
		y = ((Bounds)(ref bounds)).max.y;
		_last = Point.Ground;
	}

	private void MoveToLeftWall(Collider2D platform, out float x, out float y)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		Bounds bounds = platform.bounds;
		x = ((Bounds)(ref bounds)).min.x + 1.35f;
		if (_lowCount > 0)
		{
			bounds = ((Collider2D)Singleton<Service>.Instance.levelManager.player.collider).bounds;
			y = ((Bounds)(ref bounds)).center.y;
			_lowCount--;
		}
		else
		{
			bounds = platform.bounds;
			y = ((Bounds)(ref bounds)).max.y + _heightRange.value;
		}
		_last = Point.LeftWall;
	}

	private void MoveToRightWall(Collider2D platform, out float x, out float y)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		Bounds bounds = platform.bounds;
		x = ((Bounds)(ref bounds)).max.x - 1.35f;
		if (_lowCount > 0)
		{
			bounds = ((Collider2D)Singleton<Service>.Instance.levelManager.player.collider).bounds;
			y = ((Bounds)(ref bounds)).center.y;
			_lowCount--;
		}
		else
		{
			bounds = platform.bounds;
			y = ((Bounds)(ref bounds)).max.y + _heightRange.value;
		}
		_last = Point.RightWall;
	}
}
