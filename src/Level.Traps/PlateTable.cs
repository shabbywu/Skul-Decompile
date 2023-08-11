using Characters;
using Characters.Projectiles;
using UnityEngine;

namespace Level.Traps;

public class PlateTable : MonoBehaviour
{
	[SerializeField]
	private Prop _prop;

	[SerializeField]
	private Projectile _projectile;

	[SerializeField]
	private float _damage;

	[SerializeField]
	private int _quantity;

	[SerializeField]
	private Collider2D _fireRange;

	private void Awake()
	{
		_prop.onDidHit += OnPropDidHit;
	}

	private void OnPropDidHit(Character from, in Damage damage, Vector2 force)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		if (_prop.phase != 0)
		{
			_prop.onDidHit -= OnPropDidHit;
			float num = Mathf.Atan2(force.y, force.x) * 57.29578f;
			for (int i = 0; i < _quantity; i++)
			{
				float speedMultiplier = Mathf.Clamp(((Vector2)(ref force)).magnitude, 2f, 6f);
				Vector2 val = MMMaths.RandomPointWithinBounds(_fireRange.bounds);
				((Component)_projectile.reusable.Spawn(Vector2.op_Implicit(val), true)).GetComponent<Projectile>().Fire(from, _damage, num + Random.Range(-10f, 10f), flipX: false, flipY: false, speedMultiplier);
			}
		}
	}
}
