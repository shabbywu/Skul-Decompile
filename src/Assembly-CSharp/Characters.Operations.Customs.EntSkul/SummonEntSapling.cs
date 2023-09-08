using Level;
using UnityEngine;

namespace Characters.Operations.Customs.EntSkul;

public class SummonEntSapling : CharacterOperation
{
	[SerializeField]
	private bool _intro = true;

	[SerializeField]
	private EntSapling _ent;

	[SerializeField]
	private LayerMask _terrainLayer;

	[SerializeField]
	private int _preloadCount = 5;

	private const float _groundFindingRayDistance = 9f;

	public override void Run(Character owner)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		if (owner.playerComponents != null)
		{
			Vector3 position = ((Component)owner).transform.position;
			_ent.Spawn(position, _intro);
		}
	}
}
