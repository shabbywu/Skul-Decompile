using System.Collections.Generic;
using UnityEngine;

public abstract class ReorderableList<T> : ReorderableListParent
{
	[SerializeField]
	public List<T> values;
}
