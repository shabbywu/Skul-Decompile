using System;
using Data;
using UnityEngine;

namespace Characters.Player;

public sealed class PlayerGameResultDataTracker : MonoBehaviour
{
	[SerializeField]
	private Character _character;

	private void Awake()
	{
		Character character = _character;
		character.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character.onGaveDamage, new GaveDamageDelegate(HandleOnGaveDamage));
		_character.health.onTookDamage += HandleOnTookDamage;
		_character.health.onHealed += HandleOnHealed;
	}

	private void HandleOnHealed(double healed, double overHealed)
	{
		GameData.Progress.totalHeal += (int)healed;
	}

	private void HandleOnTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
	{
		int num = (int)tookDamage.amount;
		GameData.Progress.totalTakingDamage += num;
	}

	private void HandleOnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
	{
		if (!((Object)(object)target.character == (Object)null))
		{
			int num = (int)gaveDamage.amount;
			GameData.Progress.totalDamage += num;
			GameData.Progress.bestDamage = Mathf.Max(num, GameData.Progress.bestDamage);
		}
	}
}
