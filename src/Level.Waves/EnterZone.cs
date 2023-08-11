using Characters;
using PhysicsUtils;
using UnityEngine;

namespace Level.Waves;

public sealed class EnterZone : Leaf
{
	[SerializeField]
	private Collider2D _zone;

	private static readonly NonAllocOverlapper _overlapper;

	static EnterZone()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		_overlapper = new NonAllocOverlapper(1);
		((ContactFilter2D)(ref _overlapper.contactFilter)).SetLayerMask(LayerMask.op_Implicit(512));
	}

	protected override bool Check(EnemyWave wave)
	{
		return (Object)(object)_overlapper.OverlapCollider(_zone).GetComponent<Target>() != (Object)null;
	}
}
