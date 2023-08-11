using UnityEngine;

namespace Characters.Operations;

public class EnergyBombTakeAim : CharacterOperation
{
	[SerializeField]
	private Transform[] _centerAxisPositions;

	[SerializeField]
	private float _term = 2f;

	public override void Run(Character owner)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		Bounds bounds = GetBounds(owner);
		int num = _centerAxisPositions.Length;
		float num2 = (((Bounds)(ref bounds)).size.x - _term * (float)num) / (float)num;
		for (int i = 0; i < num; i++)
		{
			float num3 = ((i == 0) ? 0f : (num2 * (float)i + _term * (float)i));
			float num4 = num2 * (float)(i + 1) + _term * (float)i;
			float num5 = Random.Range(num3, num4);
			Vector3 val = new Vector3(((Bounds)(ref bounds)).min.x + num5, ((Bounds)(ref bounds)).max.y) - ((Component)_centerAxisPositions[i]).transform.position;
			float num6 = Mathf.Atan2(val.y, val.x) * 57.29578f;
			_centerAxisPositions[i].rotation = Quaternion.Euler(0f, 0f, num6);
		}
	}

	private Bounds GetBounds(Character owner)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		RaycastHit2D val = Physics2D.Raycast(Vector2.op_Implicit(((Component)owner).transform.position), Vector2.down, 20f, 262144);
		return ((RaycastHit2D)(ref val)).collider.bounds;
	}
}
