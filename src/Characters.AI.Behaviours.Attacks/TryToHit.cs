using System;
using System.Collections;
using Characters.Actions;
using UnityEngine;

namespace Characters.AI.Behaviours.Attacks;

public class TryToHit : Attack
{
	[SerializeField]
	private Characters.Actions.Action _attack;

	[SerializeField]
	private string _key;

	public override IEnumerator CRun(AIController controller)
	{
		base.result = Result.Doing;
		gaveDamage = false;
		Character character = controller.character;
		character.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character.onGaveDamage, new GaveDamageDelegate(Character_onGaveDamage));
		if (_attack.TryStart())
		{
			while (_attack.running && !gaveDamage)
			{
				yield return null;
			}
		}
		Character character2 = controller.character;
		character2.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(character2.onGaveDamage, new GaveDamageDelegate(Character_onGaveDamage));
		if (gaveDamage)
		{
			base.result = Result.Success;
		}
		else
		{
			base.result = Result.Fail;
		}
	}

	private void Character_onGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
	{
		if (string.IsNullOrEmpty(_key))
		{
			base.gaveDamage = true;
		}
		else if (originalDamage.key == _key)
		{
			base.gaveDamage = true;
		}
	}
}
