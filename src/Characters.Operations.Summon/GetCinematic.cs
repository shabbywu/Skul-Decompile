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
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		CoroutineReferenceExtension.StartCoroutineWithReference((MonoBehaviour)(object)character, CAttachCinematic(character));
	}

	private IEnumerator CAttachCinematic(Character character)
	{
		character.cinematic.Attach((object)this);
		yield return (object)new WaitForSeconds(_duration.value);
		character.cinematic.Detach((object)this);
	}
}
