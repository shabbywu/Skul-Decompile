using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

[Serializable]
public class SubcomponentList : SubcomponentArray<Component>
{
}
[Serializable]
public class SubcomponentList<T> where T : Component
{
	public const string nameofContainer = "_container";

	public const string nameofComponents = "_components";

	[SerializeField]
	[Subcomponent(typeof(GameObject))]
	protected GameObject _container;

	[SerializeField]
	protected List<T> _components;

	public List<T> components => _components;

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (_components == null || _components.Count == 0)
		{
			return "[]";
		}
		stringBuilder.Append('[');
		stringBuilder.Append(((object)_components[0]).ToString());
		for (int i = 1; i < _components.Count; i++)
		{
			stringBuilder.Append(", ");
			stringBuilder.Append(((object)_components[i]).ToString());
		}
		stringBuilder.Append(']');
		return stringBuilder.ToString();
	}
}
