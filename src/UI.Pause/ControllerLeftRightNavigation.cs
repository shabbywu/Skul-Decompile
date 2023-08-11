using System.Reflection;
using InControl;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Pause;

public class ControllerLeftRightNavigation : MonoBehaviour, ISelectHandler, IEventSystemHandler, IDeselectHandler
{
	[SerializeField]
	private Selectable[] _lefts;

	[SerializeField]
	private Selectable[] _rights;

	private bool _selected;

	public void OnSelect(BaseEventData eventData)
	{
		_selected = true;
	}

	public void OnDeselect(BaseEventData eventData)
	{
		_selected = false;
	}

	private void Update()
	{
		if (!_selected)
		{
			return;
		}
		InputDevice activeDevice = InputManager.ActiveDevice;
		if (activeDevice == null)
		{
			return;
		}
		Selectable[] lefts;
		if (_lefts.Length != 0 && (((OneAxisInputControl)activeDevice.LeftBumper).WasPressed || ((OneAxisInputControl)activeDevice.LeftTrigger).WasPressed || ((OneAxisInputControl)activeDevice.RightStickLeft).WasPressed))
		{
			lefts = _lefts;
			foreach (Selectable val in lefts)
			{
				if (!((Object)(object)val == (Object)null) && ((Component)val).gameObject.activeSelf)
				{
					Focus(val);
					return;
				}
			}
		}
		if (_rights.Length == 0 || (!((OneAxisInputControl)activeDevice.RightBumper).WasPressed && !((OneAxisInputControl)activeDevice.RightTrigger).WasPressed && !((OneAxisInputControl)activeDevice.RightStickRight).WasPressed))
		{
			return;
		}
		lefts = _rights;
		foreach (Selectable val2 in lefts)
		{
			if (!((Object)(object)val2 == (Object)null) && ((Component)val2).gameObject.activeSelf)
			{
				Focus(val2);
				break;
			}
		}
	}

	private void Focus(Selectable selectable)
	{
		EventSystem.current.SetSelectedGameObject(((Component)selectable).gameObject);
		selectable.Select();
		if (selectable.interactable)
		{
			typeof(Selectable).GetMethod("DoStateTransition", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(selectable, new object[2] { 3, true });
		}
	}
}
