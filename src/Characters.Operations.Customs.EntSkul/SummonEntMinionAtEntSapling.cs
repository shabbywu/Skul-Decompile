using Level;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Operations.Customs.EntSkul;

public class SummonEntMinionAtEntSapling : CharacterOperation
{
	[SerializeField]
	private Collider2D _range;

	[SerializeField]
	private float _lifeTime;

	private static readonly NonAllocOverlapper _overlapper;

	static SummonEntMinionAtEntSapling()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		_overlapper = new NonAllocOverlapper(32);
		((ContactFilter2D)(ref _overlapper.contactFilter)).SetLayerMask(LayerMask.op_Implicit(512));
	}

	public override void Run(Character owner)
	{
		if (owner.playerComponents == null)
		{
			return;
		}
		foreach (SaplingTarget component in _overlapper.OverlapCollider(_range).GetComponents<SaplingTarget>(true))
		{
			if (!component.spawnable)
			{
				break;
			}
			component.SummonEntMinion(owner, _lifeTime);
		}
	}
}
