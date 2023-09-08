namespace Data;

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
