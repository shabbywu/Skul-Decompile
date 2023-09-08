using System.Collections;
using System.Collections.Generic;
using Characters;
using Characters.Abilities.Darks;
using Scenes;
using UnityEngine;

namespace CutScenes.Shots.Events.Customs;

public sealed class OpenDarkEnemyHealthBar : Event
{
	[SerializeField]
	private Character _character;

	[SerializeField]
	private DarkAbilityAttacher _attcher;

	private void Awake()
	{
		if ((Object)(object)_character == (Object)null)
		{
			_character = ((Component)this).GetComponentInParent<Character>();
		}
	}

	public override void Run()
	{
		((MonoBehaviour)this).StartCoroutine(CRun());
	}

	private IEnumerator CRun()
	{
		while (!_attcher.attached)
		{
			yield return null;
		}
		Scene<GameBase>.instance.uiManager.headupDisplay.darkEnemyHealthBar.Open(_character, _attcher.displayName);
		Scene<GameBase>.instance.uiManager.headupDisplay.darkEnemyHealthBar.AddTarget(_character, _attcher.displayName);
		_character.health.onDiedTryCatch += delegate
		{
			Scene<GameBase>.instance.uiManager.headupDisplay.darkEnemyHealthBar.RemoveTarget(_character);
			if (Scene<GameBase>.instance.uiManager.headupDisplay.darkEnemyHealthBar.attached.Count == 0)
			{
				Scene<GameBase>.instance.uiManager.headupDisplay.darkEnemyHealthBar.Close();
			}
			else
			{
				IDictionary<Character, string> attached = Scene<GameBase>.instance.uiManager.headupDisplay.darkEnemyHealthBar.attached;
				if (attached.Count > 0)
				{
					Scene<GameBase>.instance.uiManager.headupDisplay.darkEnemyHealthBar.Set(attached.Random().Key);
				}
			}
		};
	}

	private void OnDestroy()
	{
		Scene<GameBase>.instance.uiManager.headupDisplay.darkEnemyHealthBar.Close();
		Scene<GameBase>.instance.uiManager.headupDisplay.darkEnemyHealthBar.RemoveTarget(_character);
	}
}
