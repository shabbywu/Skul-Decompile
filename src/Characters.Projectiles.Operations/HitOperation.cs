using System;
using UnityEditor;
using UnityEngine;

namespace Characters.Projectiles.Operations;

public abstract class HitOperation : CharacterHitOperation
{
	public new class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
	{
		public SubcomponentAttribute()
			: base(allowCustom: true, HitOperation.types)
		{
		}
	}

	[Serializable]
	internal new class Subcomponents : SubcomponentArray<HitOperation>
	{
	}

	public new static readonly Type[] types = new Type[16]
	{
		typeof(Despawn),
		typeof(PlaySound),
		typeof(DropSkulHead),
		typeof(Stuck),
		typeof(SummonOperationRunner),
		typeof(InstantAttack),
		typeof(MoveOwnerToProjectile),
		typeof(SummonOperationRunner),
		typeof(SummonOperationRunnerOnHitPoint),
		typeof(SpreadOperationRunner),
		typeof(ActivateObject),
		typeof(Bounce),
		typeof(SpawnObject),
		typeof(CameraShake),
		typeof(DropParts),
		typeof(InHardmode)
	};

	public abstract void Run(IProjectile projectile, RaycastHit2D raycastHit);

	public override void Run(IProjectile projectile, RaycastHit2D raycastHit, Character character)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		Run(projectile, raycastHit);
	}
}
