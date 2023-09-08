using UnityEngine;
using UnityEngine.UI;

namespace UI;

public class NpcConfirmText : MonoBehaviour
{
	[SerializeField]
	[GetComponent]
	private Text _text;

	private bool _focus;

	public string text
	{
		get
		{
			return _text.text;
		}
		set
		{
			_text.text = value;
		}
	}

	public bool focus
	{
		get
		{
			return _focus;
		}
		set
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			_focus = value;
			((Graphic)_text).color = (_focus ? Color.white : Color.gray);
		}
	}
}
