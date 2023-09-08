using System;
using System.Collections.Generic;
using System.Reflection;

public static class FieldCopier
{
	public sealed class FieldCopyAttribute : Attribute
	{
		internal readonly string name;

		public FieldCopyAttribute()
		{
			name = null;
		}

		public FieldCopyAttribute(string customTargetName)
		{
			name = customTargetName;
		}
	}
}
public class FieldCopier<T> where T : class
{
	public static readonly List<FieldInfo> fields;

	private static readonly Dictionary<string, int> _nameIndexDictionary;

	public readonly T source;

	public readonly T destination;

	static FieldCopier()
	{
		fields = new List<FieldInfo>();
		_nameIndexDictionary = new Dictionary<string, int>();
		typeof(T).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
	}

	public static void Copy(T source, T destination)
	{
		for (int i = 0; i < fields.Count; i++)
		{
			fields[i].SetValue(destination, fields[i].GetValue(source));
		}
	}

	public static void Copy(int i, T source, T destination)
	{
		fields[i].SetValue(destination, fields[i].GetValue(source));
	}

	public static void Copy(string fieldName, T source, T destination)
	{
		int index = _nameIndexDictionary[fieldName];
		fields[index].SetValue(destination, fields[index].GetValue(source));
	}

	public FieldCopier(T source, T destination)
	{
		this.source = source;
		this.destination = destination;
	}

	public void Copy()
	{
		for (int i = 0; i < fields.Count; i++)
		{
			fields[i].SetValue(destination, fields[i].GetValue(source));
		}
	}

	public void CopyFromDestination()
	{
		for (int i = 0; i < fields.Count; i++)
		{
			fields[i].SetValue(source, fields[i].GetValue(destination));
		}
	}
}
public class FieldCopier<TSource, TTarget> where TTarget : class
{
	public static readonly List<FieldInfo> sourceFields;

	public static readonly List<FieldInfo> destFields;

	private static readonly Dictionary<string, int> _nameIndexDictionary;

	public readonly TSource source;

	public readonly TTarget destination;

	static FieldCopier()
	{
		sourceFields = new List<FieldInfo>();
		destFields = new List<FieldInfo>();
		_nameIndexDictionary = new Dictionary<string, int>();
		FieldInfo[] fields = typeof(TSource).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		int num = 0;
		FieldInfo[] array = fields;
		foreach (FieldInfo fieldInfo in array)
		{
			FieldCopier.FieldCopyAttribute customAttribute = fieldInfo.GetCustomAttribute<FieldCopier.FieldCopyAttribute>();
			if (customAttribute != null)
			{
				string text = (string.IsNullOrEmpty(customAttribute.name) ? fieldInfo.Name : customAttribute.name);
				FieldInfo field = typeof(TTarget).GetField(text, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (field != null)
				{
					sourceFields.Add(fieldInfo);
					destFields.Add(field);
					_nameIndexDictionary.Add(text, num);
					num++;
				}
			}
		}
	}

	public static void Copy(TSource source, TTarget destination)
	{
		for (int i = 0; i < destFields.Count; i++)
		{
			destFields[i].SetValue(destination, sourceFields[i].GetValue(source));
		}
	}

	public static void Copy(int i, TSource source, TTarget destination)
	{
		destFields[i].SetValue(destination, sourceFields[i].GetValue(source));
	}

	public static void Copy(string fieldName, TSource source, TTarget destination)
	{
		int index = _nameIndexDictionary[fieldName];
		destFields[index].SetValue(destination, sourceFields[index].GetValue(source));
	}

	public static void Copy(TTarget source, TSource destination)
	{
		for (int i = 0; i < sourceFields.Count; i++)
		{
			sourceFields[i].SetValue(destination, destFields[i].GetValue(source));
		}
	}

	public static void Copy(int i, TTarget source, TSource destination)
	{
		sourceFields[i].SetValue(destination, destFields[i].GetValue(source));
	}

	public static void Copy(string fieldName, TTarget source, TSource destination)
	{
		int index = _nameIndexDictionary[fieldName];
		sourceFields[index].SetValue(destination, destFields[index].GetValue(source));
	}

	public FieldCopier(TSource source, TTarget destination)
	{
		this.source = source;
		this.destination = destination;
	}

	public void Copy()
	{
		for (int i = 0; i < destFields.Count; i++)
		{
			destFields[i].SetValue(destination, sourceFields[i].GetValue(source));
		}
	}

	public void CopyFromDestination()
	{
		for (int i = 0; i < sourceFields.Count; i++)
		{
			sourceFields[i].SetValue(source, destFields[i].GetValue(destination));
		}
	}
}
