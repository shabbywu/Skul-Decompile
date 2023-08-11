using Characters;
using UnityEngine;

namespace Runnables;

public sealed class SetCurrentHealth : Runnable
{
	public enum HealthType
	{
		Percent,
		Constant
	}

	[SerializeField]
	private Character _target;

	[SerializeField]
	private HealthType _healthType = HealthType.Constant;

	[SerializeField]
	private CustomFloat _amount;

	public override void Run()
	{
		if (_amount.value <= 0f)
		{
			_target.health.Kill();
		}
		else if (_healthType == HealthType.Constant)
		{
			_target.health.SetCurrentHealth(_amount.value);
		}
		else
		{
			_target.health.SetCurrentHealth(_target.health.maximumHealth * (double)_amount.value * 0.009999999776482582);
		}
	}
}
