using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

public static class ExtensionMethods
{
	public enum CompareOperation
	{
		LessThan,
		LessThanOrEqualTo,
		EqualTo,
		NotEqualTo,
		GreaterThanOrEqualTo,
		GreaterThan
	}

	private static readonly Random random = new Random();

	public static bool HasParameterOfType(this Animator self, string name, AnimatorControllerParameterType type)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		if (name == null || name == "")
		{
			return false;
		}
		AnimatorControllerParameter[] parameters = self.parameters;
		foreach (AnimatorControllerParameter val in parameters)
		{
			if (val.type == type && val.name == name)
			{
				return true;
			}
		}
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool Contains(this LayerMask mask, int layer)
	{
		return (((LayerMask)(ref mask)).value & (1 << layer)) > 0;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool Contains(this LayerMask mask, GameObject gameobject)
	{
		return (((LayerMask)(ref mask)).value & (1 << gameobject.layer)) > 0;
	}

	public static void Empty(this Transform transform)
	{
		for (int num = transform.childCount - 1; num >= 0; num--)
		{
			Object.Destroy((Object)(object)((Component)transform.GetChild(num)).gameObject);
		}
	}

	public static void EmptyImmediate(this Transform transform)
	{
		for (int num = transform.childCount - 1; num >= 0; num--)
		{
			Object.DestroyImmediate((Object)(object)((Component)transform.GetChild(num)).gameObject);
		}
	}

	public static void ToAbcStringNoAlloc(this double @this, List<char> charList, int decimalMark, int precision)
	{
		charList.Clear();
		string text = @this.ToString("0");
		if (text.Length <= decimalMark)
		{
			charList.AddRange(text);
			return;
		}
		int num = (text.Length - 1) / decimalMark - 1;
		int num2 = text.Length % decimalMark;
		if (num2 == 0)
		{
			num2 = decimalMark;
		}
		int i;
		for (i = 0; i < num2; i++)
		{
			charList.Add(text[i]);
		}
		if (precision > 0)
		{
			charList.Add('.');
			for (int j = 0; j < precision; j++)
			{
				charList.Add(text[i + j]);
			}
		}
		int count = charList.Count;
		do
		{
			charList.Insert(count, "ABCDEFGHIJKLMNOPQRSTUVWXYZ"[num % "ABCDEFGHIJKLMNOPQRSTUVWXYZ".Length]);
			num /= "ABCDEFGHIJKLMNOPQRSTUVWXYZ".Length;
		}
		while (num-- != 0);
	}

	public static string ToAbcString(this double @this, int decimalMark, int precision, bool canStartWithZero = false)
	{
		string text = @this.ToString("0");
		if (text.Length < decimalMark || (!canStartWithZero && text.Length == decimalMark))
		{
			return text;
		}
		int num = (text.Length - 1) / decimalMark - 1;
		StringBuilder stringBuilder = new StringBuilder(decimalMark + precision + num / "ABCDEFGHIJKLMNOPQRSTUVWXYZ".Length + 1);
		int num2 = text.Length % decimalMark;
		if (num2 == 0)
		{
			if (canStartWithZero)
			{
				stringBuilder.Append(0);
				num++;
			}
			else
			{
				num2 = decimalMark;
			}
		}
		int i;
		for (i = 0; i < num2; i++)
		{
			stringBuilder.Append(text[i]);
		}
		if (num2 > 1)
		{
			precision--;
		}
		if (precision > 0)
		{
			stringBuilder.Append('.');
			for (int j = 0; j < precision; j++)
			{
				stringBuilder.Append(text[i + j]);
			}
		}
		int length = stringBuilder.Length;
		do
		{
			stringBuilder.Insert(length, "ABCDEFGHIJKLMNOPQRSTUVWXYZ"[num % "ABCDEFGHIJKLMNOPQRSTUVWXYZ".Length]);
			num /= "ABCDEFGHIJKLMNOPQRSTUVWXYZ".Length;
		}
		while (num-- != 0);
		return stringBuilder.ToString();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int RandomIndex<T>(this IEnumerable<T> enumerable)
	{
		return Random.Range(0, enumerable.Count());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int RandomIndex<T>(this IEnumerable<T> enumerable, Random random)
	{
		return random.Next(0, enumerable.Count());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T Random<T>(this IEnumerable<T> enumerable)
	{
		return enumerable.ElementAt(Random.Range(0, enumerable.Count()));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T Random<T>(this IEnumerable<T> enumerable, Random random)
	{
		return enumerable.ElementAt(random.Next(0, enumerable.Count()));
	}

	public static void Add<T1, T2>(this ICollection<KeyValuePair<T1, T2>> target, T1 item1, T2 item2)
	{
		target.Add(new KeyValuePair<T1, T2>(item1, item2));
	}

	public static Coroutine ExcuteInNextFrame(this MonoBehaviour monoBehaviour, Action action)
	{
		return monoBehaviour.StartCoroutine(CNextFrame(action));
	}

	public static Coroutine DelayedExcute(this MonoBehaviour monoBehaviour, Action action, object delay)
	{
		return monoBehaviour.StartCoroutine(CDelayedExcute(action, delay));
	}

	private static IEnumerator CNextFrame(Action action)
	{
		yield return null;
		action();
	}

	private static IEnumerator CDelayedExcute(Action action, object delay)
	{
		yield return delay;
		action();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static AnimationState AnimationState(this Animation animation)
	{
		return animation[((Object)animation.clip).name];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static AnimationState AnimationState(this Animation animation, AnimationClip clip)
	{
		return animation[((Object)clip).name];
	}

	public static void Shuffle<T>(this IList<T> list)
	{
		RNGCryptoServiceProvider rNGCryptoServiceProvider = new RNGCryptoServiceProvider();
		int num = list.Count;
		while (num > 1)
		{
			byte[] array = new byte[1];
			do
			{
				rNGCryptoServiceProvider.GetBytes(array);
			}
			while (array[0] >= num * (255 / num));
			int index = array[0] % num;
			num--;
			T value = list[index];
			list[index] = list[num];
			list[num] = value;
		}
	}

	public static void PseudoShuffle<T>(this IList<T> list)
	{
		list.PseudoShuffle(random);
	}

	public static void PseudoShuffle<T>(this IList<T> list, Random random)
	{
		int num = list.Count;
		while (num > 1)
		{
			num--;
			int index = random.Next(num + 1);
			T value = list[index];
			list[index] = list[num];
			list[num] = value;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void Swap<T>(this IList<T> list, int indexA, int indexB)
	{
		T value = list[indexA];
		list[indexA] = list[indexB];
		list[indexB] = value;
	}

	public static void Reverse<T>(this IList<T> list)
	{
		for (int i = 0; i < list.Count / 2; i++)
		{
			list.Swap(i, list.Count - i - 1);
		}
	}

	public static string GetAutoName(this object @object)
	{
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		if (@object == null)
		{
			return "null";
		}
		stringBuilder.Append(@object.GetType().Name);
		stringBuilder.Append(" (");
		stringBuilder2.Append(@object.GetType().Name);
		stringBuilder2.Append(" (");
		FieldInfo[] fields = @object.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		bool flag = true;
		FieldInfo[] array = fields;
		foreach (FieldInfo fieldInfo in array)
		{
			object[] customAttributes = fieldInfo.GetCustomAttributes(inherit: true);
			bool flag2 = false;
			if (fieldInfo.IsPublic)
			{
				flag2 = true;
				object[] array2 = customAttributes;
				for (int j = 0; j < array2.Length; j++)
				{
					if (array2[j] is HideInInspector)
					{
						flag2 = false;
					}
				}
			}
			else
			{
				object[] array2 = customAttributes;
				for (int j = 0; j < array2.Length; j++)
				{
					if (array2[j] is SerializeField)
					{
						flag2 = true;
					}
				}
			}
			if (!flag2)
			{
				continue;
			}
			object value = fieldInfo.GetValue(@object);
			if (!(value is UnityEvent) && !(value is IEnumerable))
			{
				if (flag)
				{
					flag = false;
				}
				else
				{
					stringBuilder.Append(", ");
					stringBuilder2.Append(", ");
				}
				stringBuilder2.Append(fieldInfo.Name.Replace("_", ""));
				stringBuilder2.Append(":");
				string value2;
				try
				{
					value2 = ((!(value is Component) || value is MonoBehaviour) ? value.ToString() : ((Object)(Component)value).name);
				}
				catch (UnassignedReferenceException)
				{
					value2 = "null";
				}
				catch (NullReferenceException)
				{
					value2 = "null";
				}
				catch (MissingReferenceException)
				{
					value2 = "null";
				}
				stringBuilder.Append(value2);
				stringBuilder2.Append(value2);
			}
		}
		stringBuilder.Append(")");
		stringBuilder2.Append(")");
		if (stringBuilder2.Length >= 40)
		{
			return stringBuilder.ToString();
		}
		return stringBuilder2.ToString();
	}

	public static void GetComponentsInChildren<T>(this GameObject gameObject, List<T> results, int maxDepth, bool includeInactive) where T : Component
	{
		results.Clear();
		if (includeInactive || gameObject.activeInHierarchy)
		{
			T component = gameObject.GetComponent<T>();
			if ((Object)(object)component != (Object)null)
			{
				results.Add(component);
			}
			AddComponentsInChildren(gameObject.transform, 0);
		}
		void AddComponentsInChildren(Transform transform, int depth)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Expected O, but got Unknown
			if (depth > maxDepth)
			{
				return;
			}
			foreach (Transform item in gameObject.transform)
			{
				Transform val = item;
				if (includeInactive || ((Component)val).gameObject.activeSelf)
				{
					T component2 = ((Component)val).GetComponent<T>();
					if ((Object)(object)component2 != (Object)null)
					{
						results.Add(component2);
					}
					AddComponentsInChildren(val, depth + 1);
				}
			}
		}
	}

	public static void GetComponentsInChildren<T>(this Component component, List<T> results, int maxDepth, bool includeInactive) where T : Component
	{
		component.gameObject.GetComponentsInChildren(results, maxDepth, includeInactive);
	}

	public static int LastIndexOf(this StringBuilder stringBuilder, char value)
	{
		return stringBuilder.LastIndexOf(value, stringBuilder.Length - 1);
	}

	public static int LastIndexOf(this StringBuilder stringBuilder, char value, int startIndex)
	{
		for (int num = startIndex; num >= 0; num--)
		{
			if (stringBuilder[num] == value)
			{
				return num;
			}
		}
		return -1;
	}

	public static Vector2 GetMostLeftTop(this Bounds bounds)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2(((Bounds)(ref bounds)).min.x, ((Bounds)(ref bounds)).max.y);
	}

	public static Vector2 GetMostRightTop(this Bounds bounds)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return Vector2.op_Implicit(((Bounds)(ref bounds)).max);
	}

	public static Vector2 GetMostLeftBottom(this Bounds bounds)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return Vector2.op_Implicit(((Bounds)(ref bounds)).min);
	}

	public static Vector2 GetMostRightBottom(this Bounds bounds)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2(((Bounds)(ref bounds)).max.x, ((Bounds)(ref bounds)).min.y);
	}

	public static bool Compare(this int a, int b, CompareOperation operation)
	{
		return operation switch
		{
			CompareOperation.LessThan => a < b, 
			CompareOperation.LessThanOrEqualTo => a <= b, 
			CompareOperation.EqualTo => a == b, 
			CompareOperation.NotEqualTo => a != b, 
			CompareOperation.GreaterThanOrEqualTo => a >= b, 
			CompareOperation.GreaterThan => a > b, 
			_ => false, 
		};
	}
}
