namespace Data;

public class StringData : Data<string>
{
	public static Getter<string> getter;

	public static Setter<string> setter;

	public StringData(string key, bool isRealtime = false)
		: base(key, getter, setter, "", isRealtime)
	{
	}

	public StringData(string key, string defaultValue, bool isRealtime = false)
		: base(key, getter, setter, defaultValue, isRealtime)
	{
	}
}
