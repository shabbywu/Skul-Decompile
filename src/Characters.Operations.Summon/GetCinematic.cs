using System;
using System.Collections;
using UnityEngine;

namespace Characters.Operations.Summon;

[Serializable]
public class GetCinematic : IBDCharacterSetting
{
	[SerializeField]
	private CustomFloat _duration;

	public void ApplyTo(Character character)
	{
		((MonoBehaviour)(object)character).StartCoroutineWithReference(CAttachCinematic(character));
	}

	private IEnumerator CAttachCinematic(Character character)
	{
		character.cinematic.Attach(this);
		yield return (object)new WaitForSeconds(_duration.value);
		character.cinematic.Detach(this);
	}
}
