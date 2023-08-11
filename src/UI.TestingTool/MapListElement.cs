using GameResources;
using Level;
using Services;
using Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.TestingTool;

public class MapListElement : MonoBehaviour
{
	[SerializeField]
	private Button _button;

	[SerializeField]
	private TMP_Text _text;

	public Chapter.Type chapter { get; private set; }

	public void Set(Chapter.Type chapterType, string stageName, MapReference mapReference)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		chapter = chapterType;
		_text.text = stageName;
		PathNode pathNode = new PathNode(mapReference, MapReward.Type.None, Gate.Type.None);
		((UnityEvent)_button.onClick).AddListener((UnityAction)delegate
		{
			Singleton<Service>.Instance.levelManager.currentChapter.ChangeMap(pathNode);
		});
	}
}
