using UnityEngine;

namespace Characters.Operations.Movement;

public class OverrideTerrainLayer : CharacterOperation
{
	[SerializeField]
	private LayerMask _terrainLayer;

	private LayerMask _cached;

	private Character _owner;

	public override void Run(Character owner)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		_owner = owner;
		_cached = _owner.movement.controller.terrainMask;
		_owner.movement.controller.terrainMask = _terrainLayer;
	}

	private void Pop()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)_owner == (Object)null))
		{
			_owner.movement.controller.terrainMask = _cached;
		}
	}

	public override void Stop()
	{
		base.Stop();
		Pop();
	}
}
