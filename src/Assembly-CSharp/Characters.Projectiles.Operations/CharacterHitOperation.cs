using System;
using UnityEditor;
using UnityEngine;

namespace Characters.Projectiles.Operations;

public abstract class CharacterHitOperation : MonoBehaviour
{
	public class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
	{
		public SubcomponentAttribute()
			: base(allowCustom: true, CharacterHitOperation.types)
		{
		}
	}

	[Serializable]
	internal class Subcomponents : SubcomponentArray<CharacterHitOperation>
	{
	}

	public static readonly Type[] types = new Type[18]
	{
		typeof(AddMarkStack),
		typeof(AttachAbility),
		typeof(AttachAbilityToOwner),
		typeof(AttachCurseOfLight),
		typeof(Attack),
		typeof(Knockback),
		typeof(KnockbackTo),
		typeof(GrabTo),
		typeof(ShaderEffect),
		typeof(ApplyStatus),
		typeof(SummonOperationRunner),
		typeof(Smash),
		typeof(StuckToCharacter),
		typeof(Heal),
		typeof(InstantAttack),
		typeof(PlaySound),
		typeof(Despawn),
		typeof(MoveOwnerToProjectile)
	};

	public abstract void Run(IProjectile projectile, RaycastHit2D raycastHit, Character character);
}
