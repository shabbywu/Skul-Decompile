using TMPro;
using UnityEngine;

namespace EndingCredit;

public class CreditText : MonoBehaviour
{
	[SerializeField]
	private TextAsset _textAsset;

	[SerializeField]
	private GameObject _background;

	public void Initialize()
	{
		string[] array = _textAsset.text.Split(new char[1] { '\n' });
		TextMeshProUGUI[] componentsInChildren = _background.GetComponentsInChildren<TextMeshProUGUI>(true);
		if (componentsInChildren.Length != array.Length)
		{
			Debug.LogError((object)$"텍스트가 들어갈 자리가 많거나 부족합니다. 확인해주세요. {componentsInChildren.Length} : {array.Length}");
			return;
		}
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			((TMP_Text)componentsInChildren[i]).text = array[i];
		}
	}
}
