using System;
using System.Collections.ObjectModel;

public static class EnumValues<T> where T : Enum
{
	public static readonly ReadOnlyCollection<T> Values = Array.AsReadOnly(Enum.GetValues(typeof(T)) as T[]);

	public static readonly ReadOnlyCollection<string> Names = Array.AsReadOnly(Enum.GetNames(typeof(T)));

	public static readonly int Count = Names.Count;
}
