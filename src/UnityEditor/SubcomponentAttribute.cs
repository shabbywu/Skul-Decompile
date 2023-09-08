using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor;

[AttributeUsage(AttributeTargets.Field)]
public class SubcomponentAttribute : PropertyAttribute
{
	private const string custom = "Custom";

	public readonly Type[] types;

	public readonly string[] names;

	public readonly bool allowCustom;

	public bool foldout = true;

	public Type defaultType => types[0];

	public SubcomponentAttribute(Type defaultType)
	{
		types = new Type[1] { defaultType };
		names = new string[1] { defaultType.Name };
	}

	public SubcomponentAttribute(bool allowCustom, Type defaultType)
	{
		types = new Type[1] { defaultType };
		if (allowCustom)
		{
			names = new string[2] { "Custom", defaultType.Name };
		}
		else
		{
			names = new string[1] { defaultType.Name };
		}
		this.allowCustom = allowCustom;
	}

	public SubcomponentAttribute(Type defaultType, params Type[] types)
	{
		this.types = types.Prepend(defaultType).ToArray();
		names = types.Select((Type t) => t.Name).ToArray();
	}

	public SubcomponentAttribute(Type defaultType, Type[] types, string[] names)
	{
		this.types = types.Prepend(defaultType).ToArray();
		this.names = names;
	}

	public SubcomponentAttribute(bool allowCustom, Type defaultType, params Type[] types)
	{
		this.types = types.Prepend(defaultType).ToArray();
		IEnumerable<string> source = types.Select((Type t) => t.Name);
		if (allowCustom)
		{
			source = source.Prepend("Custom");
		}
		names = source.ToArray();
		this.allowCustom = allowCustom;
	}

	public SubcomponentAttribute(bool allowCustom, Type defaultType, Type[] types, string[] names)
	{
		this.types = types.Prepend(defaultType).ToArray();
		if (allowCustom)
		{
			names = names.Prepend("Custom").ToArray();
		}
		this.names = names;
		this.allowCustom = allowCustom;
	}

	public SubcomponentAttribute(Type[] types)
	{
		this.types = types.ToArray();
		names = types.Select((Type t) => t.Name).ToArray();
	}

	public SubcomponentAttribute(Type[] types, string[] names)
	{
		this.types = types.ToArray();
		this.names = names;
	}

	public SubcomponentAttribute(bool allowCustom, Type[] types)
	{
		this.types = types.ToArray();
		IEnumerable<string> source = types.Select((Type t) => t.Name);
		if (allowCustom)
		{
			source = source.Prepend("Custom");
		}
		names = source.ToArray();
		this.allowCustom = allowCustom;
	}

	public SubcomponentAttribute(bool allowCustom, Type[] types, string[] names)
	{
		this.types = types.ToArray();
		if (allowCustom)
		{
			names = names.Prepend("Custom").ToArray();
		}
		this.names = names;
		this.allowCustom = allowCustom;
	}
}
