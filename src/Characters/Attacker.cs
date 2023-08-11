using Characters.Projectiles;
using Level.Traps;
using UnityEngine;

namespace Characters;

public struct Attacker
{
	public readonly Character character;

	public readonly IProjectile projectile;

	public readonly Trap trap;

	public readonly CharacterStatus characterStatus;

	public readonly Transform transform;

	public static implicit operator Attacker(Character character)
	{
		return new Attacker(character);
	}

	public static implicit operator Attacker(Trap trap)
	{
		return new Attacker(trap);
	}

	public static implicit operator Attacker(CharacterStatus characterStatus)
	{
		return new Attacker(characterStatus);
	}

	public Attacker(Character character)
	{
		this.character = character;
		projectile = null;
		trap = null;
		characterStatus = null;
		transform = ((Component)character).transform;
	}

	public Attacker(Character character, IProjectile projectile)
	{
		this.character = character;
		this.projectile = projectile;
		trap = null;
		characterStatus = null;
		transform = projectile.transform;
	}

	public Attacker(Trap trap)
	{
		character = null;
		projectile = null;
		this.trap = trap;
		characterStatus = null;
		transform = ((Component)trap).transform;
	}

	public Attacker(CharacterStatus characterStatus)
	{
		character = null;
		projectile = null;
		trap = null;
		this.characterStatus = characterStatus;
		transform = ((Component)characterStatus).transform;
	}
}
