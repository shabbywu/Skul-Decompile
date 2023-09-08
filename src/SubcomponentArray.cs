using System;
using System.Text;
using UnityEditor;
using UnityEngine;

[Serializable]
public class SubcomponentArray : SubcomponentArray<Component>
{
}
[Serializable]
public class SubcomponentArray<T> where T : Component
{
	public const string nameofContainer = "_container";

	public const string nameofComponents = "_components";

	[Subcomponent(typeof(GameObject))]
	[SerializeField]
	protected GameObject _container;

	[SerializeField]
	protected T[] _components;

	public T[] components => _components;

	public SubcomponentArray()
	{
		_components = new T[0];
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (_components == null || _components.Length == 0)
		{
			return "[]";
		}
		stringBuilder.Append('[');
		stringBuilder.Append(((object)_components[0]).ToString());
		for (int i = 1; i < _components.Length; i++)
		{
			stringBuilder.Append(", ");
			stringBuilder.Append(((object)_components[i]).ToString());
		}
		stringBuilder.Append(']');
		return stringBuilder.ToString();
	}
}
