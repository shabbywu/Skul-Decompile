using PhysicsUtils;
using UnityEngine;

namespace Level.ReactiveProps;

public class EnterZoneFly : ReactiveProp
{
	[SerializeField]
	[GetComponent]
	private Collider2D _collider;

	private static readonly NonAllocOverlapper _playerOverlapper;

	protected static readonly NonAllocOverlapper _enemyOverlapper;

	static EnterZoneFly()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		_playerOverlapper = new NonAllocOverlapper(15);
		((ContactFilter2D)(ref _playerOverlapper.contactFilter)).SetLayerMask(LayerMask.op_Implicit(512));
		_enemyOverlapper = new NonAllocOverlapper(31);
		((ContactFilter2D)(ref _enemyOverlapper.contactFilter)).SetLayerMask(LayerMask.op_Implicit(1024));
	}

	private void Update()
	{
		if (CheckWithinSight() && !_flying)
		{
			Activate();
			Object.Destroy((Object)(object)_collider);
			_collider = null;
		}
	}

	private bool CheckWithinSight()
	{
		if ((Object)(object)_collider == (Object)null)
		{
			return false;
		}
		NonAllocOverlapper obj = _playerOverlapper.OverlapCollider(_collider);
		NonAllocOverlapper val = _enemyOverlapper.OverlapCollider(_collider);
		if (obj.results.Count > 0 || val.results.Count > 0)
		{
			return true;
		}
		return false;
	}
}
