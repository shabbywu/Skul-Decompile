using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using InControl;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI;

public abstract class Dialogue : MonoBehaviour
{
	public static readonly List<Dialogue> opened = new List<Dialogue>();

	[SerializeField]
	protected Selectable _defaultFocus;

	private GameObject _lastValidFocus;

	public static bool anyDialogueOpened => opened.Count > 0;

	public abstract bool closeWithPauseKey { get; }

	public bool focused => Focused(this);

	public static Dialogue GetCurrent()
	{
		if (opened.Count <= 0)
		{
			return null;
		}
		return opened[opened.Count - 1];
	}

	private static bool Focused(Dialogue dialogue)
	{
		if (opened.Count == 0)
		{
			return false;
		}
		return (Object)(object)opened[opened.Count - 1] == (Object)(object)dialogue;
	}

	public void Toggle()
	{
		if (((Component)this).gameObject.activeSelf)
		{
			Close();
		}
		else
		{
			Open();
		}
	}

	public void Open()
	{
		if (!((Component)this).gameObject.activeSelf)
		{
			opened.Add(this);
			((Component)this).gameObject.SetActive(true);
		}
	}

	protected virtual void OnEnable()
	{
		Focus();
	}

	public void Close()
	{
		if (opened.Count >= 2 && focused)
		{
			opened[opened.Count - 2].Focus();
		}
		((Component)this).gameObject.SetActive(false);
	}

	protected virtual void OnDisable()
	{
		InputManager.ClearInputState();
		opened.Remove(this);
	}

	public void Focus()
	{
		if (!((Object)(object)_defaultFocus == (Object)null))
		{
			Focus(_defaultFocus);
		}
	}

	public void Focus(Selectable focus)
	{
		((MonoBehaviour)this).StartCoroutine(CFocus(focus));
	}

	private IEnumerator CFocus(Selectable focus)
	{
		EventSystem.current.SetSelectedGameObject((GameObject)null);
		yield return null;
		EventSystem.current.SetSelectedGameObject(((Component)focus).gameObject);
		focus.Select();
		typeof(Selectable).GetMethod("DoStateTransition", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(focus, new object[2] { 3, true });
	}

	protected virtual void Update()
	{
		if (!focused || (Object)(object)EventSystem.current == (Object)null)
		{
			return;
		}
		GameObject currentSelectedGameObject = EventSystem.current.currentSelectedGameObject;
		if ((Object)(object)currentSelectedGameObject == (Object)null || (Object)(object)currentSelectedGameObject.GetComponentInParent<Dialogue>(true) != (Object)(object)this)
		{
			if ((Object)(object)_lastValidFocus == (Object)null)
			{
				if ((Object)(object)_defaultFocus == (Object)null)
				{
					return;
				}
				_lastValidFocus = ((Component)_defaultFocus).gameObject;
			}
			EventSystem.current.SetSelectedGameObject(_lastValidFocus);
		}
		else
		{
			_lastValidFocus = currentSelectedGameObject;
		}
	}
}
