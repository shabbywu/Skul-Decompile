using System.Collections;
using Services;
using Singletons;
using TMPro;
using UnityEngine;

namespace UI.Adventurer;

public class AdventurerLevelSetter : MonoBehaviour
{
	[SerializeField]
	private TMP_Text _levelText;

	private float _delay;

	private void OnEnable()
	{
		((MonoBehaviour)this).StartCoroutine(CAnimateLevel());
	}

	private IEnumerator CAnimateLevel()
	{
		int currentLevel = 1;
		Vector2Int adventurerLevel = Singleton<Service>.Instance.levelManager.currentChapter.currentStage.adventurerLevel;
		int targetLevel = Random.Range(((Vector2Int)(ref adventurerLevel)).x, ((Vector2Int)(ref adventurerLevel)).y);
		float duration = (float)targetLevel * 0.01f;
		float time = 0f;
		while (time < duration)
		{
			time += ((ChronometerBase)Chronometer.global).deltaTime;
			_levelText.text = Mathf.Lerp((float)currentLevel, (float)targetLevel, time).ToString("0");
			yield return null;
		}
		_levelText.text = targetLevel.ToString();
	}
}
