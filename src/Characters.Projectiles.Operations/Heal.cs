using UnityEngine;

namespace Characters.Projectiles.Operations;

public sealed class Heal : CharacterHitOperation
{
	private enum Type
	{
		Percent,
		Constnat
	}

	[SerializeField]
	private Type _type;

	[SerializeField]
	private CustomFloat _amount;

	public override void Run(IProjectile projectile, RaycastHit2D raycastHit, Character character)
	{
		if (!((Object)(object)character == (Object)null))
		{
			character.health.Heal(GetAmount(character.health));
		}
	}

	private double GetAmount(Health health)
	{
		return _type switch
		{
			Type.Percent => (double)_amount.value * health.maximumHealth * 0.009999999776482582, 
			Type.Constnat => _amount.value, 
			_ => 0.0, 
		};
	}
}
