namespace Characters;

public sealed class GrayHealth
{
	private double _maximum;

	private double _canHeal;

	private Health _health;

	public double maximum
	{
		get
		{
			return _maximum;
		}
		set
		{
			_maximum = value;
			if (_health.currentHealth + _maximum > _health.maximumHealth)
			{
				_maximum = _health.maximumHealth - _health.currentHealth;
			}
			if (_maximum < _canHeal)
			{
				canHeal = _maximum;
			}
			if (_maximum < 0.0)
			{
				_maximum = 0.0;
			}
		}
	}

	public double canHeal
	{
		get
		{
			return _canHeal;
		}
		set
		{
			if (value > _maximum)
			{
				_canHeal = _maximum;
			}
			else
			{
				_canHeal = value;
			}
			if (_canHeal < 0.0)
			{
				_canHeal = 0.0;
			}
		}
	}

	public GrayHealth(Health health)
	{
		_health = health;
	}
}
