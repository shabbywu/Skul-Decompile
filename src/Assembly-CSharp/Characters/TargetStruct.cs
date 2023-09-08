using Level;
using UnityEngine;

namespace Characters;

public readonly struct TargetStruct : ITarget
{
	public readonly Collider2D collider;

	public readonly Transform transform;

	public readonly Character character;

	public readonly DestructibleObject damageable;

	Collider2D ITarget.collider => collider;

	Character ITarget.character => character;

	DestructibleObject ITarget.damageable => damageable;

	Transform ITarget.transform => transform;

	public TargetStruct(Character character)
	{
		this.character = character;
		damageable = null;
		collider = (Collider2D)(object)character.collider;
		transform = ((Component)character).transform;
	}

	public TargetStruct(DestructibleObject damageable)
	{
		character = null;
		this.damageable = damageable;
		collider = damageable.collider;
		transform = ((Component)damageable).transform;
	}
}
