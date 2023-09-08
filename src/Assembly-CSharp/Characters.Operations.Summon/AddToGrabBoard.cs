using System;
using UnityEngine;
using Utils;

namespace Characters.Operations.Summon;

[Serializable]
public class AddToGrabBoard : IBDCharacterSetting
{
	[SerializeField]
	private GrabBoard _grabBoard;

	public void ApplyTo(Character character)
	{
		Target component = ((Component)character).GetComponent<Target>();
		if ((Object)(object)component != (Object)null)
		{
			_grabBoard.Add(component);
		}
	}
}
