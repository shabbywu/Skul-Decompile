using System.Collections;
using Level;
using UnityEngine;

namespace Characters.Operations;

public class SummonGolemOnEnable : MonoBehaviour
{
	[SerializeField]
	private Character _leftAlchemist;

	[SerializeField]
	private Character _rightAlchemist;

	[SerializeField]
	private Character _golemCharacter;

	[SerializeField]
	private GameObject _sign;

	[SerializeField]
	private float _despawnTimeOfSign;

	[SerializeField]
	private Transform _position;

	private Character _spawned;

	private void OnEnable()
	{
		if (!((Object)(object)_spawned != (Object)null))
		{
			_leftAlchemist.health.Kill();
			_rightAlchemist.health.Kill();
			_spawned = Object.Instantiate<Character>(_golemCharacter, _position);
			Map.Instance.waveContainer.Attach(_spawned);
			((MonoBehaviour)this).StartCoroutine(CDisableSign());
		}
	}

	private IEnumerator CDisableSign()
	{
		yield return Chronometer.global.WaitForSeconds(_despawnTimeOfSign);
		_sign.SetActive(false);
	}
}
