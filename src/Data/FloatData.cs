namespace Data;

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
