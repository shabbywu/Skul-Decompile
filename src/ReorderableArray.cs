using UnityEngine;

public abstract class ReorderableArray<T> : ReorderableListParent
{
	[SerializeField]
	public T[] values;
}
