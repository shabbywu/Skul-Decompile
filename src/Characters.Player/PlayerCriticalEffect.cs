using System;
using Characters.Operations.Fx;
using UnityEditor;
using UnityEngine;

namespace Characters.Player;

public class PlayerCriticalEffect : MonoBehaviour
{
	[SerializeField]
	[GetComponent]
	private Character _character;

	[Subcomponent(typeof(Vignette))]
	[SerializeField]
	private Vignette _vignette;

	private void Awake()
	{
		Character character = _character;
		character.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character.onGaveDamage, new GaveDamageDelegate(OnGaveDamage));
	}

	private void OnGaveDamage(ITarget target, in Damage originalDamage, in Damage tookDamage, double damageDealt)
	{
		if (!((Object)(object)target.character == (Object)null) && !(tookDamage.amount <= 0.0) && tookDamage.critical)
		{
			((ChronometerBase)Chronometer.global).AttachTimeScale((object)this, 0.2f, 0.1f);
			((ChronometerBase)_character.chronometer.master).AttachTimeScale((object)this, 5f, 0.1f);
			_vignette.Run(_character);
		}
	}
}
