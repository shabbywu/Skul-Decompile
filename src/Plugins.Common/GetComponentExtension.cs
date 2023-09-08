using System;
using System.Collections.Generic;
using UnityEngine;

public static class GetComponentExtension
{
	public static class SharedResult<T>
	{
		public static readonly List<T> list = new List<T>();
	}

	public static T GetComponent<TComponent, T>(this IEnumerable<TComponent> enumerable) where TComponent : Component where T : class
	{
		foreach (TComponent item in enumerable)
		{
			T component = ((Component)item).GetComponent<T>();
			if (component != null)
			{
				return component;
			}
		}
		return null;
	}

	public static T GetComponent<T>(this IEnumerable<RaycastHit2D> enumerable) where T : class
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		foreach (RaycastHit2D item in enumerable)
		{
			RaycastHit2D current = item;
			T component = ((Component)((RaycastHit2D)(ref current)).collider).GetComponent<T>();
			if (component != null)
			{
				return component;
			}
		}
		return null;
	}

	public static T GetComponent<TComponent, T>(this IEnumerable<TComponent> enumerable, Predicate<TComponent> predicate) where TComponent : Component where T : class
	{
		foreach (TComponent item in enumerable)
		{
			if (predicate(item))
			{
				T component = ((Component)item).GetComponent<T>();
				if (component != null)
				{
					return component;
				}
			}
		}
		return null;
	}

	public static T GetComponent<T>(this IEnumerable<RaycastHit2D> enumerable, Predicate<RaycastHit2D> predicate) where T : class
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		foreach (RaycastHit2D item in enumerable)
		{
			RaycastHit2D current = item;
			if (predicate(current))
			{
				T component = ((Component)((RaycastHit2D)(ref current)).collider).GetComponent<T>();
				if (component != null)
				{
					return component;
				}
			}
		}
		return null;
	}

	public static T GetComponent<TComponent, T>(this TComponent[] array, int count) where TComponent : Component where T : Component
	{
		for (int i = 0; i < array.Length; i++)
		{
			T component = ((Component)array[i]).GetComponent<T>();
			if ((Object)(object)component != (Object)null)
			{
				return component;
			}
		}
		return default(T);
	}

	public static T GetComponent<T>(this RaycastHit2D[] array, int count) where T : Component
	{
		for (int i = 0; i < array.Length; i++)
		{
			T component = ((Component)((RaycastHit2D)(ref array[i])).collider).GetComponent<T>();
			if ((Object)(object)component != (Object)null)
			{
				return component;
			}
		}
		return default(T);
	}

	public static T GetComponent<TComponent, T>(this TComponent[] array, Predicate<TComponent> predicate, int count) where TComponent : Component where T : Component
	{
		for (int i = 0; i < array.Length; i++)
		{
			if (predicate(array[i]))
			{
				T component = ((Component)array[i]).GetComponent<T>();
				if ((Object)(object)component != (Object)null)
				{
					return component;
				}
			}
		}
		return default(T);
	}

	public static T GetComponent<T>(this RaycastHit2D[] array, Predicate<RaycastHit2D> predicate, int count) where T : Component
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < array.Length; i++)
		{
			if (predicate(array[i]))
			{
				T component = ((Component)((RaycastHit2D)(ref array[i])).collider).GetComponent<T>();
				if ((Object)(object)component != (Object)null)
				{
					return component;
				}
			}
		}
		return default(T);
	}

	public static void GetComponents<TComponent, T>(this IEnumerable<TComponent> enumerable, List<T> list, bool clearList = true) where TComponent : Component where T : class
	{
		if (clearList)
		{
			list.Clear();
		}
		foreach (TComponent item in enumerable)
		{
			T component = ((Component)item).GetComponent<T>();
			if (component != null)
			{
				list.Add(component);
			}
		}
	}

	public static void GetComponents<T>(this IEnumerable<RaycastHit2D> enumerable, List<T> list, bool clearList = true) where T : class
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		if (clearList)
		{
			list.Clear();
		}
		foreach (RaycastHit2D item in enumerable)
		{
			RaycastHit2D current = item;
			T component = ((Component)((RaycastHit2D)(ref current)).collider).GetComponent<T>();
			if (component != null)
			{
				list.Add(component);
			}
		}
	}

	public static void GetComponents<TComponent, T>(this IEnumerable<TComponent> enumerable, List<T> list, Predicate<TComponent> predicate, bool clearList = true) where TComponent : Component where T : class
	{
		if (clearList)
		{
			list.Clear();
		}
		foreach (TComponent item in enumerable)
		{
			if (predicate(item))
			{
				T component = ((Component)item).GetComponent<T>();
				if (component != null)
				{
					list.Add(component);
				}
			}
		}
	}

	public static void GetComponents<T>(this IEnumerable<RaycastHit2D> enumerable, List<T> list, Predicate<RaycastHit2D> predicate, bool clearList = true) where T : class
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		if (clearList)
		{
			list.Clear();
		}
		foreach (RaycastHit2D item in enumerable)
		{
			RaycastHit2D current = item;
			if (predicate(current))
			{
				T component = ((Component)((RaycastHit2D)(ref current)).collider).GetComponent<T>();
				if (component != null)
				{
					list.Add(component);
				}
			}
		}
	}

	public static List<T> GetComponents<TComponent, T>(this IEnumerable<TComponent> enumerable, bool clearList = true) where TComponent : Component where T : class
	{
		enumerable.GetComponents(SharedResult<T>.list, clearList);
		return SharedResult<T>.list;
	}

	public static List<T> GetComponents<T>(this IEnumerable<RaycastHit2D> enumerable, bool clearList = true) where T : class
	{
		enumerable.GetComponents(SharedResult<T>.list, clearList);
		return SharedResult<T>.list;
	}

	public static List<T> GetComponents<TComponent, T>(this IEnumerable<TComponent> enumerable, Predicate<TComponent> predicate, bool clearList = true) where TComponent : Component where T : class
	{
		enumerable.GetComponents(SharedResult<T>.list, predicate, clearList);
		return SharedResult<T>.list;
	}

	public static List<T> GetComponents<T>(this IEnumerable<RaycastHit2D> enumerable, Predicate<RaycastHit2D> predicate, bool clearList = true) where T : class
	{
		enumerable.GetComponents(SharedResult<T>.list, predicate, clearList);
		return SharedResult<T>.list;
	}
}
