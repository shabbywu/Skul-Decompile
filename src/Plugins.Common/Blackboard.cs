using System.Collections;
using System.Collections.Generic;

public class Blackboard : Dictionary<string, object>, IBlackboard, IDictionary<string, object>, ICollection<KeyValuePair<string, object>>, IEnumerable<KeyValuePair<string, object>>, IEnumerable
{
	public virtual T Get<T>(string name)
	{
		if (TryGetValue(name, out var value))
		{
			return (T)value;
		}
		return default(T);
	}

	public virtual void Set<T>(string name, T value)
	{
		if (!ContainsKey(name))
		{
			Add(name, value);
		}
		else
		{
			base[name] = value;
		}
	}
}
