using UnityEngine;

namespace Characters;

public class OnDieDeactivation : MonoBehaviour
{
	[SerializeField]
	private Character _character;

	private void Start()
	{
		if ((Object)(object)_character == (Object)null)
		{
			_character = ((Component)this).GetComponentInParent<Character>();
		}
		_character.health.onDiedTryCatch += OnDie;
	}

	private void Update()
	{
		if (_character.health.dead)
		{
			((Component)this).gameObject.SetActive(false);
		}
	}

	private void OnDestroy()
	{
		if ((Object)(object)_character != (Object)null)
		{
			_character.health.onDiedTryCatch -= OnDie;
		}
	}

	private void OnDie()
	{
		_character.health.onDiedTryCatch -= OnDie;
		((Component)this).gameObject.SetActive(false);
	}
}
