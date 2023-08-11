using GameResources;
using UnityEngine;

namespace CutScenes.Shots;

public class TalkerRandomInfo : TalkerInfo
{
	[MinMaxSlider(0f, 10f)]
	private Vector2Int _range;

	private int _seleted;

	private void Awake()
	{
		_seleted = Random.Range(((Vector2Int)(ref _range)).x, ((Vector2Int)(ref _range)).y);
	}

	public override string[] GetNextText()
	{
		return Localization.GetLocalizedStringArray($"{_textKey}/{_seleted}/{_currentIndex++}");
	}
}
