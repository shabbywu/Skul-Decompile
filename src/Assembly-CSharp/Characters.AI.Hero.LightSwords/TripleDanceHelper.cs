using System.Collections.Generic;
using UnityEngine;

namespace Characters.AI.Hero.LightSwords;

public class TripleDanceHelper : MonoBehaviour
{
	private class TripleDanceSword
	{
		internal LightSword left;

		internal LightSword center;

		internal LightSword right;

		internal TripleDanceSword(List<LightSword> swords)
		{
			if (swords != null && swords.Count >= 3)
			{
				left = swords[0];
				center = swords[1];
				right = swords[2];
			}
		}
	}

	[SerializeField]
	private Character _owner;

	[SerializeField]
	private LightSwordPool _pool;

	[Range(180f, 360f)]
	[SerializeField]
	private float _left;

	[Range(180f, 360f)]
	[SerializeField]
	private float _center = 270f;

	[Range(180f, 360f)]
	[SerializeField]
	private float _right;

	private TripleDanceSword _sword;

	private void Start()
	{
		_sword = new TripleDanceSword(_pool.Get());
	}

	public void Fire(Transform source)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		Vector2 destination = CalculateDestination(Vector2.op_Implicit(source.position), _left);
		_sword.left.Fire(_owner, Vector2.op_Implicit(source.position), destination);
		destination = CalculateDestination(Vector2.op_Implicit(source.position), _center);
		_sword.center.Fire(_owner, Vector2.op_Implicit(source.position), destination);
		destination = CalculateDestination(Vector2.op_Implicit(source.position), _right);
		_sword.right.Fire(_owner, Vector2.op_Implicit(source.position), destination);
	}

	public (LightSword, LightSword, LightSword) GetStuck()
	{
		return (_sword.left, _sword.center, _sword.right);
	}

	private Vector2 CalculateDestination(Vector2 source, float degree)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = Quaternion.Euler(0f, 0f, degree) * Vector2.op_Implicit(Vector2.right);
		RaycastHit2D val2 = Physics2D.Raycast(source, Vector2.op_Implicit(val), float.PositiveInfinity, LayerMask.op_Implicit(Layers.groundMask));
		return ((RaycastHit2D)(ref val2)).point;
	}
}
