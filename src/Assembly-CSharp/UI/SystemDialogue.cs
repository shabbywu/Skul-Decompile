using System.Collections;
using InControl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UserInput;

namespace UI;

public sealed class SystemDialogue : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI _name;

	[SerializeField]
	private NpcConversationBody _body;

	[SerializeField]
	private Image _enter;

	public bool visible
	{
		get
		{
			return ((Component)this).gameObject.activeSelf;
		}
		set
		{
			if (((Component)this).gameObject.activeSelf != value)
			{
				((Component)this).gameObject.SetActive(value);
			}
		}
	}

	public IEnumerator CShow(string name, string body, bool skippable = true)
	{
		((TMP_Text)_name).text = name;
		_body.text = body;
		_body.skippable = skippable;
		((Component)this).gameObject.SetActive(true);
		yield return CShow();
	}

	public void Show(string name, string body, bool skippable = true)
	{
		((TMP_Text)_name).text = name;
		_body.text = body;
		_body.skippable = skippable;
		((Component)this).gameObject.SetActive(true);
		if (((Component)this).gameObject.activeInHierarchy)
		{
			((MonoBehaviour)this).StartCoroutine(CShow());
		}
	}

	private IEnumerator CShow()
	{
		yield return CType();
		yield return CWaitInput();
	}

	private IEnumerator CType()
	{
		if (_body.typing)
		{
			_body.StopType();
			yield return null;
		}
		yield return _body.CType();
	}

	private IEnumerator CWaitInput()
	{
		((Behaviour)_enter).enabled = true;
		do
		{
			yield return null;
		}
		while (!((OneAxisInputControl)KeyMapper.Map.Attack).WasPressed && !((OneAxisInputControl)KeyMapper.Map.Jump).WasPressed && !((OneAxisInputControl)KeyMapper.Map.Submit).WasPressed && !((OneAxisInputControl)KeyMapper.Map.Cancel).WasPressed);
		((Behaviour)_enter).enabled = false;
		Done();
	}

	private void Done()
	{
		visible = false;
	}
}
