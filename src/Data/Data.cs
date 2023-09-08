namespace Data;

public abstract class Data
{
	protected string _key;

	public abstract bool isDefaultValue { get; }

	public bool localOnly { get; set; }

	public string key
	{
		get
		{
			return _key;
		}
		set
		{
			_key = value;
		}
	}

	public abstract void Save();

	public abstract void Save(string key);

	public abstract void Load();

	public abstract void Load(string key);

	public abstract void Reset();
}
public abstract class Data<T> : Data, ISavable, ILoadable
{
	private T _value;

	private Getter<T> _getter;

	private Setter<T> _setter;

	private bool _isRealtime;

	private bool _isSaveNeeded = true;

	protected T _defaultValue;

	public override bool isDefaultValue => _value.Equals(_defaultValue);

	public T value
	{
		get
		{
			return _value;
		}
		set
		{
			if (!_value.Equals(value))
			{
				if (_isRealtime)
				{
					_setter(_key, value);
					_isSaveNeeded = false;
				}
				else
				{
					_isSaveNeeded = true;
				}
				_value = value;
			}
		}
	}

	public Data(string key, Getter<T> getter, Setter<T> setter, T defaultValue = default(T), bool isRealtime = false)
	{
		_key = key;
		_getter = getter;
		_setter = setter;
		_defaultValue = defaultValue;
		_value = getter(key, defaultValue);
		_isRealtime = isRealtime;
	}

	public Data(string key, Getter<T> getter, Setter<T> setter, bool isRealtime)
	{
		_key = key;
		_getter = getter;
		_setter = setter;
		_value = getter(key);
		_defaultValue = default(T);
		_isRealtime = isRealtime;
	}

	public Data(Getter<T> getter, Setter<T> setter, T defaultValue = default(T), bool isRealtime = false)
	{
		_getter = getter;
		_setter = setter;
		_defaultValue = default(T);
		_value = getter(base.key, defaultValue);
		_isRealtime = isRealtime;
	}

	public Data(Getter<T> getter, Setter<T> setter, bool isRealtime)
	{
		_getter = getter;
		_setter = setter;
		_value = getter(base.key);
		_isRealtime = isRealtime;
	}

	public override void Save()
	{
		Save(_key);
	}

	public override void Save(string key)
	{
		if (_isSaveNeeded)
		{
			_setter(key, value);
		}
	}

	public override void Load()
	{
		_isSaveNeeded = false;
		_value = _getter(_key);
	}

	public override void Load(string key)
	{
		_isSaveNeeded = false;
		_value = _getter(key);
	}

	public override string ToString()
	{
		if (value == null)
		{
			return string.Empty;
		}
		return value.ToString();
	}

	public override void Reset()
	{
		value = _defaultValue;
	}
}
