namespace Data;

public class FloatDataArray
{
	public readonly bool isRealtime;

	public readonly string key;

	public readonly int length;

	private readonly FloatData[] _dataArray;

	public float this[int index]
	{
		get
		{
			return _dataArray[index].value;
		}
		set
		{
			_dataArray[index].value = value;
		}
	}

	public FloatDataArray(string key, int length, bool isRealtime = false)
	{
		this.key = key;
		this.length = length;
		this.isRealtime = isRealtime;
		_dataArray = new FloatData[length];
		for (int i = 0; i < length; i++)
		{
			_dataArray[i] = new FloatData($"{key}/{i}", isRealtime);
		}
	}

	public void Save()
	{
		for (int i = 0; i < length; i++)
		{
			_dataArray[i].Save();
		}
	}

	public void Reset()
	{
		for (int i = 0; i < length; i++)
		{
			_dataArray[i].Reset();
		}
	}
}
