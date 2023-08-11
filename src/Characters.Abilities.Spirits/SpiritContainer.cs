using Services;
using UnityEngine;

namespace Characters.Abilities.Spirits;

public class SpiritContainer : MonoBehaviour
{
	[SerializeField]
	private Spirit _spirit;

	private void Awake()
	{
		((Component)_spirit).transform.SetParent((Transform)null, false);
	}

	private void OnEnable()
	{
		((Component)_spirit).gameObject.SetActive(true);
	}

	private void OnDisable()
	{
		if (!Service.quitting)
		{
			((Component)_spirit).gameObject.SetActive(false);
		}
	}

	private void OnDestroy()
	{
		if (!Service.quitting)
		{
			Object.Destroy((Object)(object)((Component)_spirit).gameObject);
		}
	}
}
