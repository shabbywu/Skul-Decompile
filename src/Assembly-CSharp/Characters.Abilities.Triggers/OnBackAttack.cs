using System;
using Characters.Movements;
using UnityEngine;

namespace Characters.Abilities.Triggers;

[Serializable]
public class OnBackAttack : Trigger
{
	private Character _character;

	public override void Attach(Character character)
	{
		_character = character;
		Character character2 = _character;
		character2.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character2.onGaveDamage, new GaveDamageDelegate(OnGaveDamage));
	}

	public override void Detach()
	{
		Character character = _character;
		character.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(character.onGaveDamage, new GaveDamageDelegate(OnGaveDamage));
	}

	private void OnGaveDamage(ITarget target, in Damage originalDamage, in Damage tookDamage, double damageDealt)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)target.character == (Object)null)
		{
			return;
		}
		Movement movement = target.character.movement;
		if (movement == null || movement.config.type != 0)
		{
			int num = Math.Sign(((Component)_character).transform.position.x - target.transform.position.x);
			if ((num == -1 && target.character.lookingDirection == Character.LookingDirection.Right) || (num == 1 && target.character.lookingDirection == Character.LookingDirection.Left))
			{
				Invoke();
			}
		}
	}
}
