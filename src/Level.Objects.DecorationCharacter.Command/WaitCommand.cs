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
		yield return _owner.chronometer.master.WaitForSeconds(_waitSeconds.value);
	}
}
