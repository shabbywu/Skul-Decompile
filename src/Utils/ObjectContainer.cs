using Services;
using UnityEngine;

namespace Utils;

public class ObjectContainer : MonoBehaviour
{
	[SerializeField]
	private GameObject _element;

	private void Awake()
	{
		_element.transform.SetParent((Transform)null, false);
	}

	private void OnEnable()
	{
		_element.SetActive(true);
	}

	private void OnDisable()
	{
		_element.SetActive(false);
	}

	private void OnDestroy()
	{
		if (!Service.quitting)
		{
			Object.Destroy((Object)(object)_element);
		}
	}
}
