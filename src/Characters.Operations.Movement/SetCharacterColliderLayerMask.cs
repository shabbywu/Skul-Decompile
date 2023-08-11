using UnityEngine;

namespace Characters.Operations.Movement;

public class SetCharacterColliderLayerMask : CharacterOperation
{
	public enum TargetCollider
	{
		Terrain,
		Trigger,
		OneWayPlatformMask
	}

	public enum ApplyLayerMask
	{
		Default,
		Foothold,
		Ground,
		TerrainMask,
		TerrainMaskForProjectile
	}

	[SerializeField]
	private TargetCollider _targetCollider;

	[SerializeField]
	private LayerMask layerMask;

	private Character character;

	public override void Run(Character owner)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		character = owner;
		switch (_targetCollider)
		{
		case TargetCollider.Terrain:
			character.movement.controller.terrainMask = layerMask;
			break;
		case TargetCollider.Trigger:
			character.movement.controller.triggerMask = layerMask;
			break;
		case TargetCollider.OneWayPlatformMask:
			character.movement.controller.oneWayPlatformMask = layerMask;
			break;
		}
	}

	public override void Stop()
	{
	}

	public void SetLayerMask(ApplyLayerMask _layerMask)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		switch (_layerMask)
		{
		case ApplyLayerMask.Default:
			layerMask = LayerMask.op_Implicit(0);
			break;
		case ApplyLayerMask.Foothold:
			layerMask = Layers.footholdMask;
			break;
		case ApplyLayerMask.Ground:
			layerMask = Layers.groundMask;
			break;
		case ApplyLayerMask.TerrainMask:
			layerMask = Layers.terrainMask;
			break;
		case ApplyLayerMask.TerrainMaskForProjectile:
			layerMask = Layers.terrainMaskForProjectile;
			break;
		}
	}
}
