using UnityEngine;

namespace Characters;

public sealed class SpriteLaser : Laser
{
	[SerializeField]
	private Transform _fireEffect;

	[SerializeField]
	private Transform _body;

	[SerializeField]
	private Transform _hitEffect;

	[SerializeField]
	private bool _flipByLookinDirection;

	[SerializeField]
	private float _minSize;

	[SerializeField]
	private bool _selfUpdate = true;

	private float _directionDegree;

	public override void Activate(Vector2 direction)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		_direction = direction;
		_directionDegree = Mathf.Atan2(_direction.y, _direction.x) * 57.29578f;
		UpdateLaser();
		((Component)this).gameObject.SetActive(true);
	}

	public override void Activate(float direction)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		_direction = Vector2.op_Implicit(Quaternion.Euler(0f, 0f, direction) * Vector2.op_Implicit(Vector2.right));
		_directionDegree = direction;
		UpdateLaser();
		((Component)this).gameObject.SetActive(true);
	}

	public override void Deactivate()
	{
		((Component)this).gameObject.SetActive(false);
		((Component)_fireEffect).gameObject.SetActive(false);
		if ((Object)(object)_hitEffect != (Object)null)
		{
			((Component)_hitEffect).gameObject.SetActive(false);
		}
	}

	private void Update()
	{
		if (_selfUpdate)
		{
			UpdateLaser();
		}
	}

	private void UpdateLaser()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		RaycastHit2D hit = default(RaycastHit2D);
		float num = _directionDegree;
		if (_flipByLookinDirection && _owner.lookingDirection == Character.LookingDirection.Left)
		{
			num = 180f - _directionDegree;
		}
		if (TargetFinder.RayCast(Vector2.op_Implicit(_originTransform.position), Vector2.op_Implicit(Quaternion.Euler(0f, 0f, num) * Vector2.op_Implicit(Vector2.right)), _maxLength, _terrainLayer, ref hit))
		{
			UpdateTransform(hit);
		}
		else
		{
			UpdateToDefault();
		}
	}

	private void UpdateTransform(RaycastHit2D hit)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		float num = Vector2.Distance(Vector2.op_Implicit(_originTransform.position), ((RaycastHit2D)(ref hit)).point);
		_fireEffect.localRotation = Quaternion.Euler(0f, 0f, _directionDegree);
		((Component)_fireEffect).gameObject.SetActive(true);
		if (num < _minSize)
		{
			num = _minSize;
		}
		_body.localScale = Vector2.op_Implicit(new Vector2(num, 1f));
		_body.localRotation = Quaternion.Euler(0f, 0f, _directionDegree);
		float num2 = Mathf.Atan2(((RaycastHit2D)(ref hit)).normal.y, ((RaycastHit2D)(ref hit)).normal.x) * 57.29578f;
		Vector2 val = ((RaycastHit2D)(ref hit)).point;
		if (num <= _minSize)
		{
			int num3 = ((!_flipByLookinDirection || _owner.lookingDirection != Character.LookingDirection.Left) ? 1 : (-1));
			Vector2 val2 = _direction * num;
			val2.x *= num3;
			val = Vector2.op_Implicit(_originTransform.position) + val2;
		}
		((Component)_hitEffect).transform.SetPositionAndRotation(Vector2.op_Implicit(val), Quaternion.Euler(0f, 0f, num2));
		((Component)_hitEffect).transform.localScale = Vector2.op_Implicit(Vector2.one);
		((Component)_hitEffect).gameObject.SetActive(true);
		((Component)this).transform.position = _originTransform.position;
	}

	private void UpdateToDefault()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = _direction;
		if (_flipByLookinDirection && _owner.lookingDirection == Character.LookingDirection.Left)
		{
			((Vector2)(ref val))._002Ector(0f - _direction.x, _direction.y);
		}
		_fireEffect.localRotation = Quaternion.Euler(0f, 0f, _directionDegree);
		((Component)_fireEffect).gameObject.SetActive(true);
		_body.localScale = Vector2.op_Implicit(new Vector2(_maxLength, 1f));
		_body.localRotation = Quaternion.Euler(0f, 0f, _directionDegree);
		((Component)_hitEffect).transform.SetPositionAndRotation(_originTransform.position + Vector2.op_Implicit(val) * _maxLength, Quaternion.identity);
		((Component)_hitEffect).transform.localScale = Vector2.op_Implicit(Vector2.one);
		((Component)_hitEffect).gameObject.SetActive(true);
		((Component)this).transform.position = _originTransform.position;
	}

	private void OnDestroy()
	{
		if ((Object)(object)_hitEffect != (Object)null)
		{
			Object.Destroy((Object)(object)((Component)_hitEffect).gameObject);
		}
	}
}
