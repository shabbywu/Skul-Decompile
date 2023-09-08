using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using Hardmode;
using Level.Adventurer;
using Singletons;
using UnityEngine;

namespace Characters.AI.Adventurer;

public class Commander : MonoBehaviour
{
	private readonly string _IsMainKey = "IsMain";

	private readonly string _IsPartyKey = "IsParty";

	private readonly string _supportingCharacterEscapePointKey = "EscapeDestination";

	private readonly string _IntroSkipKey = "IntroSkip";

	private readonly string _cutSceneWarrior = "CutSceneWarrior";

	[SerializeField]
	private PartyRandomizer _partyRandomizer;

	[SerializeField]
	private float _roleChangeTime = 10f;

	private bool _inCombat;

	private List<Character> _adventurers;

	private List<Character> _subAdventurers;

	[SerializeField]
	[Header("Hardmode")]
	private Transform _supportingCharacterEscapePoint;

	private Character _supportingCharacter;

	private void Awake()
	{
		_partyRandomizer.Initialize();
		_adventurers = _partyRandomizer.SpawnCharacters();
		if (_adventurers.Count > 1)
		{
			_subAdventurers = new List<Character>();
		}
		if (Singleton<HardmodeManager>.Instance.hardmode)
		{
			_supportingCharacter = _partyRandomizer.SpawnSupportingCharacter();
		}
	}

	public void StartIntro()
	{
		if (_inCombat)
		{
			return;
		}
		_inCombat = true;
		InitializeAdventurers();
		((MonoBehaviour)this).StartCoroutine(CCheckRoleChange());
		if (!_partyRandomizer.CanPlayHardmodeCutScene())
		{
			return;
		}
		GameObject variableValue = null;
		foreach (Character adventurer in _adventurers)
		{
			((Component)adventurer).gameObject.SetActive(false);
			((Component)adventurer).GetComponent<BehaviorDesignerCommunicator>().SetVariable<SharedBool>(_IntroSkipKey, (object)true);
			variableValue = ((Component)adventurer).gameObject;
		}
		((Component)_supportingCharacter).GetComponent<BehaviorDesignerCommunicator>().SetVariable<SharedGameObject>(_cutSceneWarrior, (object)variableValue);
	}

	private void InitializeAdventurers()
	{
		foreach (Character adventurer in _adventurers)
		{
			((Component)adventurer).GetComponent<EnemyCharacterBehaviorOption>().SetTargetToPlayer();
			adventurer.health.onDied += delegate
			{
				_adventurers.Remove(adventurer);
				if (_adventurers.Count > 0)
				{
					ChangeAdventurerRole();
				}
			};
			((Component)adventurer).GetComponent<BehaviorDesignerCommunicator>().SetVariable<SharedBool>(_IsPartyKey, (object)(_adventurers.Count > 1));
		}
		if (Singleton<HardmodeManager>.Instance.hardmode && (Object)(object)_supportingCharacter != (Object)null)
		{
			((Component)_supportingCharacter).GetComponent<EnemyCharacterBehaviorOption>().SetTargetToPlayer();
			((Component)_supportingCharacter).GetComponent<BehaviorDesignerCommunicator>().SetVariable<SharedTransform>(_supportingCharacterEscapePointKey, (object)_supportingCharacterEscapePoint);
		}
	}

	private IEnumerator CCheckRoleChange()
	{
		while (_adventurers.Count > 0)
		{
			ChangeAdventurerRole();
			yield return (object)new WaitForSeconds(_roleChangeTime);
		}
	}

	private void ChangeAdventurerRole()
	{
		Character character = _adventurers.Random();
		if (_subAdventurers != null)
		{
			_subAdventurers.Clear();
		}
		foreach (Character adventurer in _adventurers)
		{
			BehaviorDesignerCommunicator component = ((Component)adventurer).GetComponent<BehaviorDesignerCommunicator>();
			if ((Object)(object)adventurer == (Object)(object)character)
			{
				component.SetVariable<SharedBool>(_IsMainKey, (object)true);
				continue;
			}
			_subAdventurers.Add(adventurer);
			component.SetVariable<SharedBool>(_IsMainKey, (object)false);
		}
	}
}
