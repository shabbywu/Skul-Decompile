using System;
using System.Collections;
using UnityEngine;

namespace Level.Objects.DecorationCharacter.Command;

[Serializable]
public class WaitCommand : ICommand
{
	[SerializeField]
	private DecorationCharacter _owner;

	[SerializeField]
	private CustomFloat _waitSeconds;

	public IEnumerator CRun()
	{
		yield return ChronometerExtension.WaitForSeconds((ChronometerBase)(object)_owner.chronometer.master, _waitSeconds.value);
	}
}
