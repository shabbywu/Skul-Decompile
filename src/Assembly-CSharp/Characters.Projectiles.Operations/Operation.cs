using System;
using Characters.Projectiles.Operations.Customs;
using Characters.Projectiles.Operations.Decorator;
using UnityEditor;
using UnityEngine;

namespace Characters.Projectiles.Operations;

public abstract class Operation : HitOperation
{
	public new class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
	{
		public SubcomponentAttribute()
			: base(allowCustom: true, Operation.types, Operation.names)
		{
		}
	}

	[Serializable]
	internal new class Subcomponents : SubcomponentArray<Operation>
	{
		public void Run(IProjectile projectile)
		{
			for (int i = 0; i < base.components.Length; i++)
			{
				base.components[i].Run(projectile);
			}
		}
	}

	public new static readonly Type[] types;

	public static readonly string[] names;

	static Operation()
	{
		types = new Type[25]
		{
			typeof(RunCharacterOperation),
			null,
			typeof(InstantAttack),
			typeof(FireProjectile),
			typeof(SummonOperationRunner),
			typeof(SummonOperationRunnersOnGround),
			null,
			typeof(CameraShake),
			typeof(ScreenFlash),
			typeof(SpawnEffect),
			typeof(PlaySound),
			null,
			typeof(MoveOwnerToProjectile),
			typeof(SpawnRandomEnemy),
			typeof(SpawnBDRandomCharacter),
			typeof(ClearBounceHistory),
			typeof(DropSkulHead),
			typeof(Chance),
			typeof(Characters.Projectiles.Operations.Decorator.Random),
			typeof(WeightedRandom),
			typeof(Repeater),
			typeof(Repeater2),
			typeof(Repeater3),
			typeof(ByProjectileSpeed),
			typeof(SummonClusterGrenades)
		};
		int length = typeof(Operation).Namespace.Length;
		names = new string[types.Length];
		for (int i = 0; i < names.Length; i++)
		{
			Type type = types[i];
			if (type == null)
			{
				string text = names[i - 1];
				int num = text.LastIndexOf('/');
				if (num == -1)
				{
					names[i] = string.Empty;
				}
				else
				{
					names[i] = text.Substring(0, num + 1);
				}
			}
			else
			{
				names[i] = type.FullName.Substring(length + 1, type.FullName.Length - length - 1).Replace('.', '/');
			}
		}
	}

	public abstract void Run(IProjectile projectile);

	public override void Run(IProjectile projectile, RaycastHit2D raycastHit)
	{
		Run(projectile);
	}

	public override void Run(IProjectile projectile, RaycastHit2D raycastHit, Character character)
	{
		Run(projectile);
	}
}
