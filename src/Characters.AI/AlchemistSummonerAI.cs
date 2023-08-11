using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.Actions;
using UnityEditor;
using UnityEngine;

namespace Characters.AI;

public sealed class AlchemistSummonerAI : AIController
{
	[SerializeField]
	private AlchemistSummonerAI _anotherSummonerAI;

	[SerializeField]
	private Collider2D _sightRange;

	[SerializeField]
	private Collider2D _anotherSummonerSightRange;

	[Subcomponent(typeof(CheckWithinSight))]
	[SerializeField]
	private CheckWithinSight _checkWithinSight;

	[SerializeField]
	[Header("PrepareSummon")]
	private ChainAction _prepareSummon;

	[SerializeField]
	private RepeatPlaySound _takeNotesAudioSource;

	private void Awake()
	{
		base.behaviours = new List<Behaviour> { _checkWithinSight };
		character.status.unstoppable.Attach((object)this);
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		((MonoBehaviour)this).StartCoroutine(CProcess());
		((MonoBehaviour)this).StartCoroutine(_checkWithinSight.CRun(this));
	}

	protected override IEnumerator CProcess()
	{
		yield return CPlayStartOption();
		while (!base.dead)
		{
			bool num = Object.op_Implicit((Object)(object)FindClosestPlayerBody(_sightRange)) || Object.op_Implicit((Object)(object)FindClosestPlayerBody(_anotherSummonerSightRange));
			bool flag = (Object)(object)base.lastAttacker != (Object)null;
			if (num || flag)
			{
				StartSummon();
				_anotherSummonerAI.StartSummon();
				break;
			}
			yield return null;
		}
	}

	public void StartSummon()
	{
		_takeNotesAudioSource.Stop();
		if (((Component)this).gameObject.activeSelf)
		{
			_prepareSummon.TryStart();
		}
	}
}
