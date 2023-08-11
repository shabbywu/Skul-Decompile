using System.Collections;
using UnityEngine;

namespace BT.Conditions;

public class CoolDown : Condition
{
	[SerializeField]
	private CustomFloat _timeOverrideValue;

	[SerializeField]
	private float _time;

	private bool _success;

	private void OnEnable()
	{
		_success = true;
	}

	protected override bool Check(Context context)
	{
		if (_success)
		{
			((MonoBehaviour)this).StartCoroutine(CCoolDown());
			return true;
		}
		return false;
	}

	private IEnumerator CCoolDown()
	{
		_success = false;
		float num = ((_time == 0f) ? _timeOverrideValue.value : _time);
		yield return Chronometer.global.WaitForSeconds(num);
		_success = true;
	}
}
