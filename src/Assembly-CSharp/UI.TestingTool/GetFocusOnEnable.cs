using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.TestingTool;

[RequireComponent(typeof(Selectable))]
public class GetFocusOnEnable : MonoBehaviour
{
	private void OnEnable()
	{
		((MonoBehaviour)this).StartCoroutine(GetFocus());
	}

	private IEnumerator GetFocus()
	{
		EventSystem.current.SetSelectedGameObject((GameObject)null);
		yield return null;
		EventSystem.current.SetSelectedGameObject(((Component)this).gameObject);
		Selectable component = ((Component)this).GetComponent<Selectable>();
		typeof(Selectable).GetMethod("DoStateTransition", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(component, new object[2] { 3, true });
	}
}
