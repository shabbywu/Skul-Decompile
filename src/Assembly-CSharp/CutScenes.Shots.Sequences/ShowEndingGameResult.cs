using System.Collections;
using InControl;
using Scenes;
using UserInput;

namespace CutScenes.Shots.Sequences;

public class ShowEndingGameResult : Sequence
{
	public override IEnumerator CRun()
	{
		GameBase gameBase = Scene<GameBase>.instance;
		gameBase.uiManager.gameResult.ShowEndingResult();
		while (!gameBase.uiManager.gameResult.animationFinished || (!((OneAxisInputControl)KeyMapper.Map.Attack).WasPressed && !((OneAxisInputControl)KeyMapper.Map.Submit).WasPressed))
		{
			yield return null;
		}
		Hide();
	}

	private void Hide()
	{
		Scene<GameBase>.instance.uiManager.gameResult.Hide();
	}

	private void OnDisable()
	{
		Hide();
	}
}
