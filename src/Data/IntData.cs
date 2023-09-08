namespace Data;

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
