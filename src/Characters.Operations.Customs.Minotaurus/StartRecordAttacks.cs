using System.Collections;
using Characters.Abilities.Weapons.Minotaurus;
using UnityEngine;

namespace Characters.Operations.Customs.Minotaurus;

public sealed class StartRecordAttacks : CharacterOperation
{
	[SerializeField]
	private MinotaurusPassiveComponent _passive;

	[SerializeField]
	private float _duration;

	public override void Run(Character owner)
	{
		((MonoBehaviour)this).StopCoroutine("CRun");
		_passive.StartRecordingAttacks();
		((MonoBehaviour)this).StartCoroutine(CRun(owner));
	}

	private IEnumerator CRun(Character owner)
	{
		yield return ChronometerExtension.WaitForSeconds((ChronometerBase)(object)owner.chronometer.master, _duration);
		_passive.StopRecodingAttacks();
	}
}
