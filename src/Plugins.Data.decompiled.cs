using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: CompilationRelaxations(8)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: AssemblyVersion("0.0.0.0")]
namespace Data;

public delegate T Getter<T>(string key, T defaultValue = default(T));
public delegate void Setter<T>(string key, T Value);
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
public class IntData : Data<int>
{
	public static Getter<int> getter;

	public static Setter<int> setter;

	public IntData(string key, bool isRealtime = false)
		: base(key, getter, setter, isRealtime)
	{
	}

	public IntData(string key, int defaultValue, bool isRealtime = false)
		: base(key, getter, setter, defaultValue, isRealtime)
	{
	}
}
public class BoolData : Data<bool>
{
	public static Getter<bool> getter;

	public static Setter<bool> setter;

	public BoolData(string key, bool isRealtime = false)
		: base(key, getter, setter, isRealtime)
	{
	}

	public BoolData(string key, bool defaultValue, bool isRealtime)
		: base(key, getter, setter, defaultValue, isRealtime)
	{
	}
}
public class FloatData : Data<float>
{
	public static Getter<float> getter;

	public static Setter<float> setter;

	public FloatData(string key, bool isRealtime = false)
		: base(key, getter, setter, isRealtime)
	{
	}

	public FloatData(string key, float defaultValue, bool isRealtime = false)
		: base(key, getter, setter, defaultValue, isRealtime)
	{
	}
}
public class IntDataArray
{
	public readonly bool isRealtime;

	public readonly string key;

	public readonly int length;

	private readonly IntData[] _dataArray;

	public int this[int