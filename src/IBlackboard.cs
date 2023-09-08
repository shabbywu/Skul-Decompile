using System.Collections;
using System.Collections.Generic;

public interface IBlackboard : IDictionary<string, object>, ICollection<KeyValuePair<string, object>>, IEnumerable<KeyValuePair<string, object>>, IEnumerable
{
	T Get<T>(string name);

	void Set<T>(string name, T value);
}
