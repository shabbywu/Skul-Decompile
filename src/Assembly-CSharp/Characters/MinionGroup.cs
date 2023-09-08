using System.Collections;
using System.Collections.Generic;

namespace Characters;

public sealed class MinionGroup : IEnumerable<Minion>, IEnumerable
{
	private LinkedList<Minion> _group;

	public int Count => _group.Count;

	public MinionGroup()
	{
		_group = new LinkedList<Minion>();
	}

	public void Join(Minion minion)
	{
		_group.AddLast(minion);
	}

	public void Leave(Minion minion)
	{
		_group.Remove(minion);
	}

	public void DespawnOldest()
	{
		if (_group.Count != 0)
		{
			_group.First.Value.Despawn();
		}
	}

	public void DespawnAll()
	{
		for (int num = _group.Count - 1; num >= 0; num--)
		{
			_group.First.Value.Despawn();
		}
	}

	public IEnumerator<Minion> GetEnumerator()
	{
		return _group.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		yield return _group.GetEnumerator();
	}
}
