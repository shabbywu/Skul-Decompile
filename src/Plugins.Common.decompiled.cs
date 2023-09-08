using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: CompilationRelaxations(8)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: AssemblyVersion("0.0.0.0")]
[AttributeUsage(AttributeTargets.Field)]
public class ColliderSubcomponentAttribute : SubcomponentAttribute
{
	public new static readonly Type[] types = new Type[6]
	{
		typeof(BoxCollider2D),
		typeof(CircleCollider2D),
		typeof(CapsuleCollider2D),
		typeof(PolygonCollider2D),
		typeof(EdgeCollider2D),
		typeof(CompositeCollider2D)
	};

	public readonly int layer;

	public readonly bool isTrigger;

	private static int ignoreRaycast = LayerMask.NameToLayer("Ignore Raycast");

	public ColliderSubcomponentAttribute(bool isTrigger = false)
		: base(allowCustom: true, types)
	{
		layer = ignoreRaycast;
		this.isTrigger = isTrigger;
	}

	public ColliderSubcomponentAttribute(int layer, bool isTrigger = false)
		: base(allowCustom: true, types)
	{
		this.layer = layer;
		this.isTrigger = isTrigger;
	}

	public ColliderSubcomponentAttribute(string layer, bool isTrigger = false)
		: base(allowCustom: true, types)
	{
		this.layer = LayerMask.NameToLayer(layer);
		this.isTrigger = isTrigger;
	}
}
public class EnumFlagAttribute : PropertyAttribute
{
	public string name;

	public EnumFlagAttribute()
	{
	}

	public EnumFlagAttribute(string name)
	{
		this.name = name;
	}
}
public class FilePathAttribute : PropertyAttribute
{
	public readonly string title;

	public readonly string defaultName;

	public FilePathAttribute(string title)
	{
		this.title = title;
		defaultName = string.Empty;
	}

	public FilePathAttribute(string title, string defaultName)
	{
		this.title = title;
		this.defaultName = defaultName;
	}
}
public class FrameTimeAttribute : PropertyAttribute
{
}
[AttributeUsage(AttributeTargets.Field)]
public class GetComponentAttribute : ReadOnlyAttribute
{
}
[AttributeUsage(AttributeTargets.Field)]
public class GetComponentInChildrenAttribute : ReadOnlyAttribute
{
	public readonly bool includeInactive;

	public GetComponentInChildrenAttribute(bool includeInactive = false)
	{
		this.includeInactive = includeInactive;
	}
}
[AttributeUsage(AttributeTargets.Field)]
public class GetComponentInParentAttribute : ReadOnlyAttribute
{
	public readonly bool includeInactive;

	public GetComponentInParentAttribute(bool includeInactive = false)
	{
		this.includeInactive = includeInactive;
	}
}
public class InformationAttribute : PropertyAttribute
{
	public enum InformationType
	{
		Error,
		Info,
		None,
		Warning
	}

	public InformationAttribute(string message, InformationType type, bool messageAfterProperty)
	{
	}
}
public class LayerAttribute : PropertyAttribute
{
}
public class MinMaxSliderAttribute : PropertyAttribute
{
	public readonly float max;

	public readonly float min;

	public MinMaxSliderAttribute(float min, float max)
	{
		this.min = min;
		this.max = max;
	}
}
public class PopupAttribute : PropertyAttribute
{
	public readonly bool allowCustom;

	public readonly object[] list;

	public PopupAttribute(params object[] list)
	{
		this.list = list;
	}

	public PopupAttribute(bool allowCustom, params object[] list)
	{
		this.allowCustom = allowCustom;
		if (this.allowCustom)
		{
			this.list = list.Append("Custom").ToArray();
		}
		else
		{
			this.list = list;
		}
	}
}
[AttributeUsage(AttributeTargets.Field)]
public class ReadOnlyAttribute : PropertyAttribute
{
	public readonly bool runtimeOnly;

	public ReadOnlyAttribute(bool runtimeOnly = false)
	{
		this.runtimeOnly = runtimeOnly;
	}
}
[AttributeUsage(AttributeTargets.Field)]
public class SortingLayerAttribute : PropertyAttribute
{
}
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
public class Blackboard : Dictionary<string, object>, IBlackboard, IDictionary<string, object>, ICollection<KeyValuePair<string, object>>, IEnumerable<KeyValuePair<string, object>>, IEnumerable
{
	public virtual T Get<T>(string name)
	{
		if (TryGetValue(name, out var value))
		{
			return (T)value;
		}
		return default(T);
	}

	public virtual void Set<T>(string name, T value)
	{
		if (!ContainsKey(name))
		{
			Add(name, value);
		}
		else
		{
			base[name] = value;
		}
	}
}
public class BoolOverrider
{
	public bool @override;

	public bool value;

	public void Override(ref bool value)
	{
		if (@override)
		{
			value = this.value;
		}
	}
}
public class BoundedList<T> : ReadonlyBoundedList<T>, IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
{
	public T[] array => _itmes;

	public new int Count
	{
		get
		{
			return base.Count;
		}
		set
		{
			if (value < 0 || value > _itmes.Length)
			{
				throw new IndexOutOfRangeException();
			}
			base.Count = value;
		}
	}

	public bool IsReadOnly => false;

	public new T this[int index]
	{
		get
		{
			return base[index];
		}
		set
		{
			if (index >= Count)
			{
				throw new IndexOutOfRangeException();
			}
			base[index] = value;
		}
	}

	public BoundedList(int capacity)
		: base(capacity)
	{
	}

	public BoundedList(T[] sourceArray)
		: base(sourceArray)
	{
	}

	public ReadonlyBoundedList<T> AsReadonly()
	{
		return this;
	}

	public void Insert(int index, T item)
	{
		_itmes[Count++] = item;
	}

	public void RemoveAt(int index)
	{
		if (index >= Count)
		{
			throw new IndexOutOfRangeException();
		}
		for (int i = index; i < Count - 1; i++)
		{
			_itmes[i] = _itmes[i + 1];
		}
		Count--;
	}

	public void Add(T item)
	{
		_itmes[Count++] = item;
	}

	public void Clear()
	{
		Count = 0;
	}

	public bool Remove(T item)
	{
		int num = IndexOf(item);
		if (num == -1)
		{
			return false;
		}
		RemoveAt(num);
		return true;
	}
}
public class Chronometer : ChronometerBase
{
	public class Global : ChronometerBase
	{
		private const float _defaultFixedDeltaTime = 1f / 60f;

		public override ChronometerBase parent
		{
			get
			{
				return null;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		public override float localTimeScale => _timeScales.total;

		public override float timeScale => _timeScales.total;

		public override float deltaTime => Time.unscaledDeltaTime * _timeScales.total;

		public override float smoothDeltaTime => Time.smoothDeltaTime;

		public override float fixedDeltaTime => Time.fixedDeltaTime;

		protected override void Update()
		{
			Time.timeScale = _timeScales.total;
			if (Time.timeScale > float.Epsilon)
			{
				Time.fixedDeltaTime = Time.timeScale * (1f / 60f);
			}
			else
			{
				Time.fixedDeltaTime = 10f;
			}
		}

		public IEnumerator WaitForSeconds(float seconds)
		{
			yield return (object)new WaitForSeconds(seconds);
		}
	}

	public static readonly WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

	public static readonly WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

	protected static ChronometerBase _global;

	public static readonly Global global = new Global();

	private ChronometerBase _parent;

	public override ChronometerBase parent
	{
		get
		{
			if (_parent == null)
			{
				return global;
			}
			return _parent;
		}
		set
		{
			_parent = value;
		}
	}

	public override float localTimeScale => _timeScales.total;

	public override float timeScale => parent.timeScale * _timeScales.total;

	public override float deltaTime => parent.deltaTime * _timeScales.total;

	public override float smoothDeltaTime => parent.smoothDeltaTime * _timeScales.total;

	public override float fixedDeltaTime => parent.fixedDeltaTime * _timeScales.total;

	public Chronometer()
	{
	}

	public Chronometer(ChronometerBase parent)
	{
		this.parent = parent;
	}

	protected override void Update()
	{
	}
}
public static class ChronometerExtension
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IEnumerator WaitForSeconds(this ChronometerBase chronometer, float seconds)
	{
		while (seconds >= 0f)
		{
			yield return null;
			seconds -= chronometer?.deltaTime ?? Chronometer.global.deltaTime;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float TimeScale(this ChronometerBase chronometer)
	{
		return chronometer?.timeScale ?? Chronometer.global.timeScale;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float DeltaTime(this ChronometerBase chronometer)
	{
		return chronometer?.deltaTime ?? Chronometer.global.deltaTime;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float SmoothDeltaTime(this ChronometerBase chronometer)
	{
		return chronometer?.deltaTime ?? Chronometer.global.smoothDeltaTime;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float FixedDeltaTime(this ChronometerBase chronometer)
	{
		return chronometer?.deltaTime ?? Chronometer.global.fixedDeltaTime;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void SetChronometer(this GameObject gameObject, ChronometerBase chronometer)
	{
		IUseChronometer component = gameObject.GetComponent<IUseChronometer>();
		if (component != null)
		{
			component.chronometer = chronometer;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void SetChronometer(this GameObject gameObject, GameObject owner)
	{
		IUseChronometer component = gameObject.GetComponent<IUseChronometer>();
		if (component != null)
		{
			component.chronometer = owner.GetComponent<ChronometerBase>();
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void SetChronometer(this GameObject gameObject, Component owner)
	{
		IUseChronometer component = gameObject.GetComponent<IUseChronometer>();
		if (component != null)
		{
			component.chronometer = owner.GetComponent<ChronometerBase>();
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void SetChronometer(this IUseChronometer chronometerUser, ChronometerBase chronometer)
	{
		chronometerUser.chronometer = chronometer;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void SetChronometer(this IUseChronometer chronometerUser, GameObject owner)
	{
		chronometerUser.chronometer = owner.GetComponent<ChronometerBase>();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void SetChronometer(this IUseChronometer chronometerUser, Component owner)
	{
		chronometerUser.chronometer = owner.GetComponent<ChronometerBase>();
	}
}
public abstract class ChronometerBase
{
	protected static readonly List<IUseChronometer> _useChronometersCache = new List<IUseChronometer>();

	protected readonly ProductFloat _timeScales = new ProductFloat(1f);

	protected readonly Dictionary<object, Coroutine> _attachTimeScaleCoroutines = new Dictionary<object, Coroutine>();

	public abstract ChronometerBase parent { get; set; }

	public abstract float localTimeScale { get; }

	public abstract float timeScale { get; }

	public abstract float deltaTime { get; }

	public abstract float smoothDeltaTime { get; }

	public abstract float fixedDeltaTime { get; }

	protected abstract void Update();

	public void AttachTimeScale(object key, float timeScale)
	{
		if (_attachTimeScaleCoroutines.TryGetValue(key, out var value))
		{
			((MonoBehaviour)CoroutineProxy.instance).StopCoroutine(value);
			_attachTimeScaleCoroutines.Remove(key);
		}
		_timeScales[key] = timeScale;
		Update();
	}

	public void AttachTimeScale(object key, float timeScale, float duration)
	{
		if (_attachTimeScaleCoroutines.TryGetValue(key, out var value))
		{
			((MonoBehaviour)CoroutineProxy.instance).StopCoroutine(value);
			_attachTimeScaleCoroutines[key] = ((MonoBehaviour)CoroutineProxy.instance).StartCoroutine(CAttachTimeScale());
		}
		else
		{
			_attachTimeScaleCoroutines.Add(key, ((MonoBehaviour)CoroutineProxy.instance).StartCoroutine(CAttachTimeScale()));
		}
		IEnumerator CAttachTimeScale()
		{
			float remainTime = duration;
			_timeScales[key] = timeScale;
			Update();
			while (remainTime > 0f)
			{
				if (Time.timeScale > 0f)
				{
					remainTime -= Time.unscaledDeltaTime;
				}
				yield return null;
			}
			_timeScales.Remove(key);
			Update();
		}
	}

	public bool DetachTimeScale(object key)
	{
		if (_timeScales.Remove(key))
		{
			Update();
			return true;
		}
		return false;
	}

	public void AttachTo(Component component, bool includeChildren)
	{
		if (includeChildren)
		{
			component.GetComponentsInChildren<IUseChronometer>(true, _useChronometersCache);
		}
		else
		{
			component.GetComponents<IUseChronometer>(_useChronometersCache);
		}
		for (int i = 0; i < _useChronometersCache.Count; i++)
		{
			_useChronometersCache[i].chronometer = this;
		}
	}

	public void AttachTo(GameObject gameObject, bool includeChildren)
	{
		if (includeChildren)
		{
			gameObject.GetComponentsInChildren<IUseChronometer>(true, _useChronometersCache);
		}
		else
		{
			gameObject.GetComponents<IUseChronometer>(_useChronometersCache);
		}
		for (int i = 0; i < _useChronometersCache.Count; i++)
		{
			_useChronometersCache[i].chronometer = this;
		}
	}
}
public class ChronometerTime
{
	public Chronometer chronometer { get; private set; }

	public float time { get; private set; }

	public ChronometerTime(Chronometer chronometer, MonoBehaviour coroutineOwner)
	{
		this.chronometer = chronometer;
		coroutineOwner.StartCoroutine(CTimer());
	}

	private IEnumerator CTimer()
	{
		while (true)
		{
			time += Time.deltaTime * chronometer.timeScale;
			yield return null;
		}
	}
}
public class CoroutineProxy : MonoBehaviour
{
	private static CoroutineProxy _instance;

	public static CoroutineProxy instance
	{
		get
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Expected O, but got Unknown
			if ((Object)(object)_instance == (Object)null)
			{
				GameObject val = new GameObject("CoroutineProxy");
				_instance = val.AddComponent<CoroutineProxy>();
				Object.DontDestroyOnLoad((Object)val);
				((Object)val).hideFlags = (HideFlags)61;
			}
			return _instance;
		}
	}
}
public static class CoroutineReferenceExtension
{
	public static CoroutineReference StartCoroutineWithReference(this MonoBehaviour monoBehaviour, IEnumerator routine)
	{
		return new CoroutineReference(monoBehaviour, monoBehaviour.StartCoroutine(routine));
	}

	public static CoroutineReference StartCoroutineWithReference(this MonoBehaviour monoBehaviour, string methodName)
	{
		return new CoroutineReference(monoBehaviour, monoBehaviour.StartCoroutine(methodName));
	}

	public static CoroutineReference StartCoroutineWithReference(this MonoBehaviour monoBehaviour, string methodName, object value)
	{
		return new CoroutineReference(monoBehaviour, monoBehaviour.StartCoroutine(methodName, value));
	}
}
public struct CoroutineReference
{
	public Coroutine coroutine { get; private set; }

	public MonoBehaviour monoBehaviour { get; private set; }

	public CoroutineReference(MonoBehaviour monoBehaviour, Coroutine coroutine)
	{
		this.monoBehaviour = monoBehaviour;
		this.coroutine = coroutine;
	}

	public void Stop()
	{
		if (coroutine != null && (Object)(object)monoBehaviour != (Object)null)
		{
			monoBehaviour.StopCoroutine(coroutine);
			coroutine = null;
		}
	}

	public void Clear()
	{
		coroutine = null;
	}
}
public class CsvReader
{
	internal sealed class Field
	{
		internal int Start;

		internal int End;

		internal bool Quoted;

		internal int EscapedQuotesCount;

		private string cachedValue;

		internal int Length => End - Start + 1;

		internal Field()
		{
		}

		internal Field Reset(int start)
		{
			Start = start;
			End = start - 1;
			Quoted = false;
			EscapedQuotesCount = 0;
			cachedValue = null;
			return this;
		}

		internal string GetValue(char[] buf)
		{
			if (cachedValue == null)
			{
				cachedValue = GetValueInternal(buf);
			}
			return cachedValue;
		}

		private string GetValueInternal(char[] buf)
		{
			if (Quoted)
			{
				int start = Start + 1;
				int num = Length - 2;
				string text = ((num > 0) ? GetString(buf, start, num) : string.Empty);
				if (EscapedQuotesCount > 0)
				{
					text = text.Replace("\"\"", "\"");
				}
				return text;
			}
			int length = Length;
			if (length <= 0)
			{
				return string.Empty;
			}
			return GetString(buf, Start, length);
		}

		private string GetString(char[] buf, int start, int len)
		{
			int num = buf.Length;
			start = ((start < num) ? start : (start % num));
			if (start + len - 1 >= num)
			{
				int num2 = buf.Length - start;
				string text = new string(buf, start, num2);
				string text2 = new string(buf, 0, len - num2);
				return text + text2;
			}
			return new string(buf, start, len);
		}
	}

	private int delimLength;

	private TextReader rdr;

	private char[] buffer;

	private int bufferLength;

	private int bufferLoadThreshold;

	private int lineStartPos;

	private int actualBufferLen;

	private List<Field> fields;

	private int fieldsCount;

	private int linesRead;

	public string Delimiter { get; private set; }

	public int BufferSize { get; set; } = 32768;


	public bool TrimFields { get; set; } = true;


	public int FieldsCount => fieldsCount;

	public string this[int idx]
	{
		get
		{
			if (idx < fieldsCount)
			{
				_ = fields[idx];
				return fields[idx].GetValue(buffer);
			}
			return null;
		}
	}

	public CsvReader(TextReader rdr)
		: this(rdr, ",")
	{
	}

	public CsvReader(TextReader rdr, string delimiter)
	{
		this.rdr = rdr;
		Delimiter = delimiter;
		delimLength = delimiter.Length;
		if (delimLength == 0)
		{
			throw new ArgumentException("Delimiter cannot be empty.");
		}
	}

	private int ReadBlockAndCheckEof(char[] buffer, int start, int len, ref bool eof)
	{
		if (len == 0)
		{
			return 0;
		}
		int num = rdr.ReadBlock(buffer, start, len);
		if (num < len)
		{
			eof = true;
		}
		return num;
	}

	private bool FillBuffer()
	{
		bool eof = false;
		int num = bufferLength - actualBufferLen;
		if (num >= bufferLoadThreshold)
		{
			int num2 = (lineStartPos + actualBufferLen) % buffer.Length;
			if (num2 >= lineStartPos)
			{
				actualBufferLen += ReadBlockAndCheckEof(buffer, num2, buffer.Length - num2, ref eof);
				if (lineStartPos > 0)
				{
					actualBufferLen += ReadBlockAndCheckEof(buffer, 0, lineStartPos, ref eof);
				}
			}
			else
			{
				actualBufferLen += ReadBlockAndCheckEof(buffer, num2, num, ref eof);
			}
		}
		return eof;
	}

	private string GetLineTooLongMsg()
	{
		return string.Format("CSV line #{1} length exceedes buffer size ({0})", BufferSize, linesRead);
	}

	private int ReadQuotedFieldToEnd(int start, int maxPos, bool eof, ref int escapedQuotesCount)
	{
		int i;
		for (i = start; i < maxPos; i++)
		{
			int num = ((i < bufferLength) ? i : (i % bufferLength));
			if (buffer[num] == '"')
			{
				if (i + 1 >= maxPos || buffer[(i + 1) % bufferLength] != '"')
				{
					return i;
				}
				i++;
				escapedQuotesCount++;
			}
		}
		if (eof)
		{
			return i - 1;
		}
		throw new InvalidDataException(GetLineTooLongMsg());
	}

	private bool ReadDelimTail(int start, int maxPos, ref int end)
	{
		int i;
		for (i = 1; i < delimLength; i++)
		{
			int num = start + i;
			int num2 = ((num < bufferLength) ? num : (num % bufferLength));
			if (num >= maxPos || buffer[num2] != Delimiter[i])
			{
				return false;
			}
		}
		end = start + i - 1;
		return true;
	}

	private Field GetOrAddField(int startIdx)
	{
		fieldsCount++;
		while (fieldsCount > fields.Count)
		{
			fields.Add(new Field());
		}
		Field field = fields[fieldsCount - 1];
		field.Reset(startIdx);
		return field;
	}

	public int GetValueLength(int idx)
	{
		if (idx < fieldsCount)
		{
			Field field = fields[idx];
			if (!field.Quoted)
			{
				return field.Length;
			}
			return field.Length - field.EscapedQuotesCount;
		}
		return -1;
	}

	public void ProcessValueInBuffer(int idx, Action<char[], int, int> handler)
	{
		if (idx < fieldsCount)
		{
			Field field = fields[idx];
			if ((field.Quoted && field.EscapedQuotesCount > 0) || field.End >= bufferLength)
			{
				char[] array = field.GetValue(buffer).ToCharArray();
				handler(array, 0, array.Length);
			}
			else if (field.Quoted)
			{
				handler(buffer, field.Start + 1, field.Length - 2);
			}
			else
			{
				handler(buffer, field.Start, field.Length);
			}
		}
	}

	public bool Read()
	{
		if (fields == null)
		{
			fields = new List<Field>();
			fieldsCount = 0;
		}
		if (buffer == null)
		{
			bufferLoadThreshold = Math.Min(BufferSize, 8192);
			bufferLength = BufferSize + bufferLoadThreshold;
			buffer = new char[bufferLength];
			lineStartPos = 0;
			actualBufferLen = 0;
		}
		bool flag = FillBuffer();
		fieldsCount = 0;
		if (actualBufferLen <= 0)
		{
			return false;
		}
		linesRead++;
		int num = lineStartPos + actualBufferLen;
		int end = lineStartPos;
		Field orAddField = GetOrAddField(end);
		bool flag2 = false;
		char c = Delimiter[0];
		bool trimFields = TrimFields;
		while (true)
		{
			if (end < num)
			{
				int num2 = ((end < bufferLength) ? end : (end % bufferLength));
				char c2 = buffer[num2];
				switch (c2)
				{
				case '"':
					if (flag2)
					{
						orAddField.End = end;
					}
					else if (orAddField.Quoted || orAddField.Length > 0)
					{
						orAddField.End = end;
						orAddField.Quoted = false;
						flag2 = true;
					}
					else
					{
						int num3 = ReadQuotedFieldToEnd(end + 1, num, flag, ref orAddField.EscapedQuotesCount);
						orAddField.Start = end;
						orAddField.End = num3;
						orAddField.Quoted = true;
						end = num3;
					}
					goto IL_01f5;
				case '\r':
					if (end + 1 < num && buffer[(end + 1) % bufferLength] == '\n')
					{
						end++;
					}
					end++;
					break;
				case '\n':
					end++;
					break;
				default:
					if (c2 == c && (delimLength == 1 || ReadDelimTail(end, num, ref end)))
					{
						orAddField = GetOrAddField(end + 1);
						flag2 = false;
					}
					else if (!(c2 == ' ' && trimFields))
					{
						if (orAddField.Length == 0)
						{
							orAddField.Start = end;
						}
						if (orAddField.Quoted)
						{
							orAddField.Quoted = false;
							flag2 = true;
						}
						orAddField.End = end;
					}
					goto IL_01f5;
				}
				break;
			}
			if (flag)
			{
				break;
			}
			throw new InvalidDataException(GetLineTooLongMsg());
			IL_01f5:
			end++;
		}
		actualBufferLen -= end - lineStartPos;
		lineStartPos = end % bufferLength;
		if (fieldsCount == 1 && fields[0].Length == 0)
		{
			return Read();
		}
		return true;
	}
}
[Serializable]
public class Curve
{
	public static readonly Curve empty = new Curve(new AnimationCurve(), 0f, 0f);

	[SerializeField]
	private AnimationCurve _curve;

	[SerializeField]
	private float _valueMultiplier;

	[FrameTime]
	[SerializeField]
	private float _durationMultiplier;

	public float duration
	{
		get
		{
			return _durationMultiplier;
		}
		set
		{
			_durationMultiplier = value;
		}
	}

	public float valueMultiplier => _valueMultiplier;

	public Curve()
	{
		_curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
		_valueMultiplier = 1f;
		_durationMultiplier = 1f;
	}

	public Curve(AnimationCurve curve, float valueMultiplier = 1f, float durationMultiplier = 1f)
	{
		_curve = curve;
		_valueMultiplier = valueMultiplier;
		_durationMultiplier = durationMultiplier;
	}

	public float Evaluate(float time)
	{
		if (_durationMultiplier == 0f)
		{
			if (_curve.length == 0)
			{
				return 0f;
			}
			return ((Keyframe)(ref _curve.keys[_curve.length - 1])).value * _valueMultiplier;
		}
		return _curve.Evaluate(time / _durationMultiplier) * _valueMultiplier;
	}
}
[Serializable]
public class CustomAngle
{
	[Serializable]
	public class Reorderable : ReorderableArray<CustomAngle>
	{
		public Reorderable()
		{
			values = new CustomAngle[0];
		}

		public Reorderable(params CustomAngle[] values)
		{
			base.values = values;
		}
	}

	private enum Type
	{
		Constant,
		RandomBetweenTwoConstants,
		RandomWithinCurve
	}

	[SerializeField]
	private Type _type;

	[SerializeField]
	private float _value;

	[SerializeField]
	private float _maxValue;

	[SerializeField]
	private AnimationCurve _curve;

	public float value
	{
		get
		{
			switch (_type)
			{
			case Type.Constant:
				return _value;
			case Type.RandomBetweenTwoConstants:
				if (_value < _maxValue)
				{
					return Random.Range(_value, _maxValue);
				}
				return Random.Range(_maxValue, _value);
			case Type.RandomWithinCurve:
				return _value * _curve.Evaluate((float)Random.Range(0, _curve.length));
			default:
				return 0f;
			}
		}
	}

	public CustomAngle(float @default)
	{
		_type = Type.Constant;
		_value = @default;
	}

	public CustomAngle(float min, float max)
	{
		_type = Type.RandomBetweenTwoConstants;
		_value = min;
		_maxValue = max;
	}
}
[Serializable]
public class CustomFloat
{
	[Serializable]
	public class Reorderable : ReorderableArray<CustomFloat>
	{
	}

	private enum Type
	{
		Constant,
		RandomBetweenTwoConstants,
		RandomWithinCurve
	}

	[SerializeField]
	private Type _type;

	[SerializeField]
	private float _value;

	[SerializeField]
	private float _maxValue;

	[SerializeField]
	private AnimationCurve _curve;

	public float value => _type switch
	{
		Type.Constant => _value, 
		Type.RandomBetweenTwoConstants => Random.Range(_value, _maxValue), 
		Type.RandomWithinCurve => _value * _curve.Evaluate((float)Random.Range(0, _curve.length)), 
		_ => 0f, 
	};

	public CustomFloat(float @default)
	{
		Set(@default);
	}

	public CustomFloat(float min, float max)
	{
		Set(min, max);
	}

	public void Set(float value)
	{
		_value = value;
	}

	public void Set(float min, float max)
	{
		_type = Type.RandomBetweenTwoConstants;
		_value = min;
		_maxValue = max;
	}

	public float GetAverage()
	{
		switch (_type)
		{
		case Type.Constant:
			return _value;
		case Type.RandomBetweenTwoConstants:
			return (_value + _maxValue) / 2f;
		case Type.RandomWithinCurve:
		{
			float num = 10f;
			float num2 = 0f;
			for (int i = 0; (float)i <= num; i++)
			{
				num2 += _curve.Evaluate((float)(_curve.length * i) / num);
			}
			return _value * num2 / num;
		}
		default:
			return 0f;
		}
	}
}
public sealed class DontDestroyOnLoad : MonoBehaviour
{
	private void Awake()
	{
		Object.DontDestroyOnLoad((Object)(object)((Component)this).gameObject);
	}
}
public class DoubleBuffered<T>
{
	public T Current { get; protected set; }

	public T Next { get; protected set; }

	public DoubleBuffered(T current, T next)
	{
		Current = current;
		Next = next;
	}

	public T Swap()
	{
		T current = Current;
		Current = Next;
		Next = current;
		return Current;
	}
}
public struct EasingFunction
{
	public enum Method
	{
		EaseInQuad,
		EaseOutQuad,
		EaseInOutQuad,
		EaseInCubic,
		EaseOutCubic,
		EaseInOutCubic,
		EaseInQuart,
		EaseOutQuart,
		EaseInOutQuart,
		EaseInQuint,
		EaseOutQuint,
		EaseInOutQuint,
		EaseInSine,
		EaseOutSine,
		EaseInOutSine,
		EaseInExpo,
		EaseOutExpo,
		EaseInOutExpo,
		EaseInCirc,
		EaseOutCirc,
		EaseInOutCirc,
		Linear,
		Spring,
		EaseInBounce,
		EaseOutBounce,
		EaseInOutBounce,
		EaseInBack,
		EaseOutBack,
		EaseInOutBack,
		EaseInElastic,
		EaseOutElastic,
		EaseInOutElastic
	}

	public delegate float Function(float s, float e, float v);

	private Method _method;

	private const float NATURAL_LOG_OF_2 = 0.6931472f;

	public Method method
	{
		get
		{
			return _method;
		}
		set
		{
			_method = value;
			function = GetEasingFunction(_method);
		}
	}

	public Function function { get; private set; }

	public EasingFunction(Method method)
	{
		_method = method;
		function = GetEasingFunction(_method);
	}

	public static float Linear(float start, float end, float value)
	{
		return Mathf.Lerp(start, end, value);
	}

	public static float Spring(float start, float end, float value)
	{
		value = Mathf.Clamp01(value);
		value = (Mathf.Sin(value * (float)Math.PI * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + 1.2f * (1f - value));
		return start + (end - start) * value;
	}

	public static float EaseInQuad(float start, float end, float value)
	{
		end -= start;
		return end * value * value + start;
	}

	public static float EaseOutQuad(float start, float end, float value)
	{
		end -= start;
		return (0f - end) * value * (value - 2f) + start;
	}

	public static float EaseInOutQuad(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end * 0.5f * value * value + start;
		}
		value -= 1f;
		return (0f - end) * 0.5f * (value * (value - 2f) - 1f) + start;
	}

	public static float EaseInCubic(float start, float end, float value)
	{
		end -= start;
		return end * value * value * value + start;
	}

	public static float EaseOutCubic(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return end * (value * value * value + 1f) + start;
	}

	public static float EaseInOutCubic(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end * 0.5f * value * value * value + start;
		}
		value -= 2f;
		return end * 0.5f * (value * value * value + 2f) + start;
	}

	public static float EaseInQuart(float start, float end, float value)
	{
		end -= start;
		return end * value * value * value * value + start;
	}

	public static float EaseOutQuart(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return (0f - end) * (value * value * value * value - 1f) + start;
	}

	public static float EaseInOutQuart(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end * 0.5f * value * value * value * value + start;
		}
		value -= 2f;
		return (0f - end) * 0.5f * (value * value * value * value - 2f) + start;
	}

	public static float EaseInQuint(float start, float end, float value)
	{
		end -= start;
		return end * value * value * value * value * value + start;
	}

	public static float EaseOutQuint(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return end * (value * value * value * value * value + 1f) + start;
	}

	public static float EaseInOutQuint(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end * 0.5f * value * value * value * value * value + start;
		}
		value -= 2f;
		return end * 0.5f * (value * value * value * value * value + 2f) + start;
	}

	public static float EaseInSine(float start, float end, float value)
	{
		end -= start;
		return (0f - end) * Mathf.Cos(value * ((float)Math.PI / 2f)) + end + start;
	}

	public static float EaseOutSine(float start, float end, float value)
	{
		end -= start;
		return end * Mathf.Sin(value * ((float)Math.PI / 2f)) + start;
	}

	public static float EaseInOutSine(float start, float end, float value)
	{
		end -= start;
		return (0f - end) * 0.5f * (Mathf.Cos((float)Math.PI * value) - 1f) + start;
	}

	public static float EaseInExpo(float start, float end, float value)
	{
		end -= start;
		return end * Mathf.Pow(2f, 10f * (value - 1f)) + start;
	}

	public static float EaseOutExpo(float start, float end, float value)
	{
		end -= start;
		return end * (0f - Mathf.Pow(2f, -10f * value) + 1f) + start;
	}

	public static float EaseInOutExpo(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end * 0.5f * Mathf.Pow(2f, 10f * (value - 1f)) + start;
		}
		value -= 1f;
		return end * 0.5f * (0f - Mathf.Pow(2f, -10f * value) + 2f) + start;
	}

	public static float EaseInCirc(float start, float end, float value)
	{
		end -= start;
		return (0f - end) * (Mathf.Sqrt(1f - value * value) - 1f) + start;
	}

	public static float EaseOutCirc(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return end * Mathf.Sqrt(1f - value * value) + start;
	}

	public static float EaseInOutCirc(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return (0f - end) * 0.5f * (Mathf.Sqrt(1f - value * value) - 1f) + start;
		}
		value -= 2f;
		return end * 0.5f * (Mathf.Sqrt(1f - value * value) + 1f) + start;
	}

	public static float EaseInBounce(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		return end - EaseOutBounce(0f, end, num - value) + start;
	}

	public static float EaseOutBounce(float start, float end, float value)
	{
		value /= 1f;
		end -= start;
		if (value < 0.36363637f)
		{
			return end * (7.5625f * value * value) + start;
		}
		if (value < 0.72727275f)
		{
			value -= 0.54545456f;
			return end * (7.5625f * value * value + 0.75f) + start;
		}
		if ((double)value < 0.9090909090909091)
		{
			value -= 0.8181818f;
			return end * (7.5625f * value * value + 0.9375f) + start;
		}
		value -= 21f / 22f;
		return end * (7.5625f * value * value + 63f / 64f) + start;
	}

	public static float EaseInOutBounce(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		if (value < num * 0.5f)
		{
			return EaseInBounce(0f, end, value * 2f) * 0.5f + start;
		}
		return EaseOutBounce(0f, end, value * 2f - num) * 0.5f + end * 0.5f + start;
	}

	public static float EaseInBack(float start, float end, float value)
	{
		end -= start;
		value /= 1f;
		float num = 1.70158f;
		return end * value * value * ((num + 1f) * value - num) + start;
	}

	public static float EaseOutBack(float start, float end, float value)
	{
		float num = 1.70158f;
		end -= start;
		value -= 1f;
		return end * (value * value * ((num + 1f) * value + num) + 1f) + start;
	}

	public static float EaseInOutBack(float start, float end, float value)
	{
		float num = 1.70158f;
		end -= start;
		value /= 0.5f;
		if (value < 1f)
		{
			num *= 1.525f;
			return end * 0.5f * (value * value * ((num + 1f) * value - num)) + start;
		}
		value -= 2f;
		num *= 1.525f;
		return end * 0.5f * (value * value * ((num + 1f) * value + num) + 2f) + start;
	}

	public static float EaseInElastic(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		float num2 = num * 0.3f;
		float num3 = 0f;
		if (value == 0f)
		{
			return start;
		}
		if ((value /= num) == 1f)
		{
			return start + end;
		}
		float num4;
		if (num3 == 0f || num3 < Mathf.Abs(end))
		{
			num3 = end;
			num4 = num2 / 4f;
		}
		else
		{
			num4 = num2 / ((float)Math.PI * 2f) * Mathf.Asin(end / num3);
		}
		return 0f - num3 * Mathf.Pow(2f, 10f * (value -= 1f)) * Mathf.Sin((value * num - num4) * ((float)Math.PI * 2f) / num2) + start;
	}

	public static float EaseOutElastic(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		float num2 = num * 0.3f;
		float num3 = 0f;
		if (value == 0f)
		{
			return start;
		}
		if ((value /= num) == 1f)
		{
			return start + end;
		}
		float num4;
		if (num3 == 0f || num3 < Mathf.Abs(end))
		{
			num3 = end;
			num4 = num2 * 0.25f;
		}
		else
		{
			num4 = num2 / ((float)Math.PI * 2f) * Mathf.Asin(end / num3);
		}
		return num3 * Mathf.Pow(2f, -10f * value) * Mathf.Sin((value * num - num4) * ((float)Math.PI * 2f) / num2) + end + start;
	}

	public static float EaseInOutElastic(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		float num2 = num * 0.3f;
		float num3 = 0f;
		if (value == 0f)
		{
			return start;
		}
		if ((value /= num * 0.5f) == 2f)
		{
			return start + end;
		}
		float num4;
		if (num3 == 0f || num3 < Mathf.Abs(end))
		{
			num3 = end;
			num4 = num2 / 4f;
		}
		else
		{
			num4 = num2 / ((float)Math.PI * 2f) * Mathf.Asin(end / num3);
		}
		if (value < 1f)
		{
			return -0.5f * (num3 * Mathf.Pow(2f, 10f * (value -= 1f)) * Mathf.Sin((value * num - num4) * ((float)Math.PI * 2f) / num2)) + start;
		}
		return num3 * Mathf.Pow(2f, -10f * (value -= 1f)) * Mathf.Sin((value * num - num4) * ((float)Math.PI * 2f) / num2) * 0.5f + end + start;
	}

	public static float LinearD(float start, float end, float value)
	{
		return end - start;
	}

	public static float EaseInQuadD(float start, float end, float value)
	{
		return 2f * (end - start) * value;
	}

	public static float EaseOutQuadD(float start, float end, float value)
	{
		end -= start;
		return (0f - end) * value - end * (value - 2f);
	}

	public static float EaseInOutQuadD(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end * value;
		}
		value -= 1f;
		return end * (1f - value);
	}

	public static float EaseInCubicD(float start, float end, float value)
	{
		return 3f * (end - start) * value * value;
	}

	public static float EaseOutCubicD(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return 3f * end * value * value;
	}

	public static float EaseInOutCubicD(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return 1.5f * end * value * value;
		}
		value -= 2f;
		return 1.5f * end * value * value;
	}

	public static float EaseInQuartD(float start, float end, float value)
	{
		return 4f * (end - start) * value * value * value;
	}

	public static float EaseOutQuartD(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return -4f * end * value * value * value;
	}

	public static float EaseInOutQuartD(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return 2f * end * value * value * value;
		}
		value -= 2f;
		return -2f * end * value * value * value;
	}

	public static float EaseInQuintD(float start, float end, float value)
	{
		return 5f * (end - start) * value * value * value * value;
	}

	public static float EaseOutQuintD(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return 5f * end * value * value * value * value;
	}

	public static float EaseInOutQuintD(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return 2.5f * end * value * value * value * value;
		}
		value -= 2f;
		return 2.5f * end * value * value * value * value;
	}

	public static float EaseInSineD(float start, float end, float value)
	{
		return (end - start) * 0.5f * (float)Math.PI * Mathf.Sin((float)Math.PI / 2f * value);
	}

	public static float EaseOutSineD(float start, float end, float value)
	{
		end -= start;
		return (float)Math.PI / 2f * end * Mathf.Cos(value * ((float)Math.PI / 2f));
	}

	public static float EaseInOutSineD(float start, float end, float value)
	{
		end -= start;
		return end * 0.5f * (float)Math.PI * Mathf.Cos((float)Math.PI * value);
	}

	public static float EaseInExpoD(float start, float end, float value)
	{
		return 6.931472f * (end - start) * Mathf.Pow(2f, 10f * (value - 1f));
	}

	public static float EaseOutExpoD(float start, float end, float value)
	{
		end -= start;
		return 3.465736f * end * Mathf.Pow(2f, 1f - 10f * value);
	}

	public static float EaseInOutExpoD(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return 3.465736f * end * Mathf.Pow(2f, 10f * (value - 1f));
		}
		value -= 1f;
		return 3.465736f * end / Mathf.Pow(2f, 10f * value);
	}

	public static float EaseInCircD(float start, float end, float value)
	{
		return (end - start) * value / Mathf.Sqrt(1f - value * value);
	}

	public static float EaseOutCircD(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return (0f - end) * value / Mathf.Sqrt(1f - value * value);
	}

	public static float EaseInOutCircD(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end * value / (2f * Mathf.Sqrt(1f - value * value));
		}
		value -= 2f;
		return (0f - end) * value / (2f * Mathf.Sqrt(1f - value * value));
	}

	public static float EaseInBounceD(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		return EaseOutBounceD(0f, end, num - value);
	}

	public static float EaseOutBounceD(float start, float end, float value)
	{
		value /= 1f;
		end -= start;
		if (value < 0.36363637f)
		{
			return 2f * end * 7.5625f * value;
		}
		if (value < 0.72727275f)
		{
			value -= 0.54545456f;
			return 2f * end * 7.5625f * value;
		}
		if ((double)value < 0.9090909090909091)
		{
			value -= 0.8181818f;
			return 2f * end * 7.5625f * value;
		}
		value -= 21f / 22f;
		return 2f * end * 7.5625f * value;
	}

	public static float EaseInOutBounceD(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		if (value < num * 0.5f)
		{
			return EaseInBounceD(0f, end, value * 2f) * 0.5f;
		}
		return EaseOutBounceD(0f, end, value * 2f - num) * 0.5f;
	}

	public static float EaseInBackD(float start, float end, float value)
	{
		float num = 1.70158f;
		return 3f * (num + 1f) * (end - start) * value * value - 2f * num * (end - start) * value;
	}

	public static float EaseOutBackD(float start, float end, float value)
	{
		float num = 1.70158f;
		end -= start;
		value -= 1f;
		return end * ((num + 1f) * value * value + 2f * value * ((num + 1f) * value + num));
	}

	public static float EaseInOutBackD(float start, float end, float value)
	{
		float num = 1.70158f;
		end -= start;
		value /= 0.5f;
		if (value < 1f)
		{
			num *= 1.525f;
			return 0.5f * end * (num + 1f) * value * value + end * value * ((num + 1f) * value - num);
		}
		value -= 2f;
		num *= 1.525f;
		return 0.5f * end * ((num + 1f) * value * value + 2f * value * ((num + 1f) * value + num));
	}

	public static float EaseInElasticD(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		float num2 = num * 0.3f;
		float num3 = 0f;
		float num4;
		if (num3 == 0f || num3 < Mathf.Abs(end))
		{
			num3 = end;
			num4 = num2 / 4f;
		}
		else
		{
			num4 = num2 / ((float)Math.PI * 2f) * Mathf.Asin(end / num3);
		}
		float num5 = (float)Math.PI * 2f;
		return (0f - num3) * num * num5 * Mathf.Cos(num5 * (num * (value - 1f) - num4) / num2) / num2 - 3.465736f * num3 * Mathf.Sin(num5 * (num * (value - 1f) - num4) / num2) * Mathf.Pow(2f, 10f * (value - 1f) + 1f);
	}

	public static float EaseOutElasticD(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		float num2 = num * 0.3f;
		float num3 = 0f;
		float num4;
		if (num3 == 0f || num3 < Mathf.Abs(end))
		{
			num3 = end;
			num4 = num2 * 0.25f;
		}
		else
		{
			num4 = num2 / ((float)Math.PI * 2f) * Mathf.Asin(end / num3);
		}
		return num3 * (float)Math.PI * num * Mathf.Pow(2f, 1f - 10f * value) * Mathf.Cos((float)Math.PI * 2f * (num * value - num4) / num2) / num2 - 3.465736f * num3 * Mathf.Pow(2f, 1f - 10f * value) * Mathf.Sin((float)Math.PI * 2f * (num * value - num4) / num2);
	}

	public static float EaseInOutElasticD(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		float num2 = num * 0.3f;
		float num3 = 0f;
		float num4;
		if (num3 == 0f || num3 < Mathf.Abs(end))
		{
			num3 = end;
			num4 = num2 / 4f;
		}
		else
		{
			num4 = num2 / ((float)Math.PI * 2f) * Mathf.Asin(end / num3);
		}
		if (value < 1f)
		{
			value -= 1f;
			return -3.465736f * num3 * Mathf.Pow(2f, 10f * value) * Mathf.Sin((float)Math.PI * 2f * (num * value - 2f) / num2) - num3 * (float)Math.PI * num * Mathf.Pow(2f, 10f * value) * Mathf.Cos((float)Math.PI * 2f * (num * value - num4) / num2) / num2;
		}
		value -= 1f;
		return num3 * (float)Math.PI * num * Mathf.Cos((float)Math.PI * 2f * (num * value - num4) / num2) / (num2 * Mathf.Pow(2f, 10f * value)) - 3.465736f * num3 * Mathf.Sin((float)Math.PI * 2f * (num * value - num4) / num2) / Mathf.Pow(2f, 10f * value);
	}

	public static float SpringD(float start, float end, float value)
	{
		value = Mathf.Clamp01(value);
		end -= start;
		return end * (6f * (1f - value) / 5f + 1f) * (-2.2f * Mathf.Pow(1f - value, 1.2f) * Mathf.Sin((float)Math.PI * value * (2.5f * value * value * value + 0.2f)) + Mathf.Pow(1f - value, 2.2f) * ((float)Math.PI * (2.5f * value * value * value + 0.2f) + 23.561945f * value * value * value) * Mathf.Cos((float)Math.PI * value * (2.5f * value * value * value + 0.2f)) + 1f) - 6f * end * (Mathf.Pow(1f - value, 2.2f) * Mathf.Sin((float)Math.PI * value * (2.5f * value * value * value + 0.2f)) + value / 5f);
	}

	public static Function GetEasingFunction(Method easingFunction)
	{
		return easingFunction switch
		{
			Method.EaseInQuad => EaseInQuad, 
			Method.EaseOutQuad => EaseOutQuad, 
			Method.EaseInOutQuad => EaseInOutQuad, 
			Method.EaseInCubic => EaseInCubic, 
			Method.EaseOutCubic => EaseOutCubic, 
			Method.EaseInOutCubic => EaseInOutCubic, 
			Method.EaseInQuart => EaseInQuart, 
			Method.EaseOutQuart => EaseOutQuart, 
			Method.EaseInOutQuart => EaseInOutQuart, 
			Method.EaseInQuint => EaseInQuint, 
			Method.EaseOutQuint => EaseOutQuint, 
			Method.EaseInOutQuint => EaseInOutQuint, 
			Method.EaseInSine => EaseInSine, 
			Method.EaseOutSine => EaseOutSine, 
			Method.EaseInOutSine => EaseInOutSine, 
			Method.EaseInExpo => EaseInExpo, 
			Method.EaseOutExpo => EaseOutExpo, 
			Method.EaseInOutExpo => EaseInOutExpo, 
			Method.EaseInCirc => EaseInCirc, 
			Method.EaseOutCirc => EaseOutCirc, 
			Method.EaseInOutCirc => EaseInOutCirc, 
			Method.Linear => Linear, 
			Method.Spring => Spring, 
			Method.EaseInBounce => EaseInBounce, 
			Method.EaseOutBounce => EaseOutBounce, 
			Method.EaseInOutBounce => EaseInOutBounce, 
			Method.EaseInBack => EaseInBack, 
			Method.EaseOutBack => EaseOutBack, 
			Method.EaseInOutBack => EaseInOutBack, 
			Method.EaseInElastic => EaseInElastic, 
			Method.EaseOutElastic => EaseOutElastic, 
			Method.EaseInOutElastic => EaseInOutElastic, 
			_ => null, 
		};
	}

	public static Function GetEasingFunctionDerivative(Method easingFunction)
	{
		return easingFunction switch
		{
			Method.EaseInQuad => EaseInQuadD, 
			Method.EaseOutQuad => EaseOutQuadD, 
			Method.EaseInOutQuad => EaseInOutQuadD, 
			Method.EaseInCubic => EaseInCubicD, 
			Method.EaseOutCubic => EaseOutCubicD, 
			Method.EaseInOutCubic => EaseInOutCubicD, 
			Method.EaseInQuart => EaseInQuartD, 
			Method.EaseOutQuart => EaseOutQuartD, 
			Method.EaseInOutQuart => EaseInOutQuartD, 
			Method.EaseInQuint => EaseInQuintD, 
			Method.EaseOutQuint => EaseOutQuintD, 
			Method.EaseInOutQuint => EaseInOutQuintD, 
			Method.EaseInSine => EaseInSineD, 
			Method.EaseOutSine => EaseOutSineD, 
			Method.EaseInOutSine => EaseInOutSineD, 
			Method.EaseInExpo => EaseInExpoD, 
			Method.EaseOutExpo => EaseOutExpoD, 
			Method.EaseInOutExpo => EaseInOutExpoD, 
			Method.EaseInCirc => EaseInCircD, 
			Method.EaseOutCirc => EaseOutCircD, 
			Method.EaseInOutCirc => EaseInOutCircD, 
			Method.Linear => LinearD, 
			Method.Spring => SpringD, 
			Method.EaseInBounce => EaseInBounceD, 
			Method.EaseOutBounce => EaseOutBounceD, 
			Method.EaseInOutBounce => EaseInOutBounceD, 
			Method.EaseInBack => EaseInBackD, 
			Method.EaseOutBack => EaseOutBackD, 
			Method.EaseInOutBack => EaseInOutBackD, 
			Method.EaseInElastic => EaseInElasticD, 
			Method.EaseOutElastic => EaseOutElasticD, 
			Method.EaseInOutElastic => EaseInOutElasticD, 
			_ => null, 
		};
	}
}
public static class EnumValues<T> where T : Enum
{
	public static readonly ReadOnlyCollection<T> Values = Array.AsReadOnly(Enum.GetValues(typeof(T)) as T[]);

	public static readonly ReadOnlyCollection<string> Names = Array.AsReadOnly(Enum.GetNames(typeof(T)));

	public static readonly int Count = Names.Count;
}
public abstract class EnumArray
{
	public abstract int Length { get; }
}
[Serializable]
public class EnumArray<TEnum, T> : EnumArray, IEnumerable<T>, IEnumerable where TEnum : Enum
{
	[SerializeField]
	private T[] _array;

	public T[] Array
	{
		get
		{
			return _array;
		}
		private set
		{
			_array = value;
		}
	}

	public override int Length => Array.Length;

	public ReadOnlyCollection<TEnum> Keys => EnumValues<TEnum>.Values;

	public int Count => Keys.Count;

	public bool IsReadOnly => false;

	public T this[TEnum key]
	{
		get
		{
			return Array[Convert.ToInt32(key)];
		}
		set
		{
			Array[Convert.ToInt32(key)] = value;
		}
	}

	public EnumArray()
	{
		Array = new T[Keys.Count];
	}

	public EnumArray(params T[] array)
	{
		Array = new T[Keys.Count];
		for (int i = 0; i < Math.Min(array.Length, Array.Length); i++)
		{
			Array[i] = array[i];
		}
	}

	public EnumArray(EnumArray<TEnum, T> source)
	{
		Array = (T[])source.Array.Clone();
	}

	public bool IsDefined(string type)
	{
		return Enum.IsDefined(typeof(TEnum), type);
	}

	public T Get(string type)
	{
		if (!IsDefined(type))
		{
			throw new ArgumentException("Try to get invalid data type", "type");
		}
		return Array[(int)Enum.Parse(typeof(TEnum), type, ignoreCase: true)];
	}

	public T Get(TEnum type)
	{
		return Array[Convert.ToInt32(type)];
	}

	public T GetOrDefault(TEnum type)
	{
		int num = Convert.ToInt32(type);
		if (num < 0 || num >= Array.Length)
		{
			return default(T);
		}
		return Array[num];
	}

	public bool TryGet(TEnum type, out T value)
	{
		int num = Convert.ToInt32(type);
		if (num < 0 || num >= Array.Length)
		{
			value = default(T);
			return false;
		}
		value = Array[num];
		return true;
	}

	public void Set(string type, T value)
	{
		if (!IsDefined(type))
		{
			throw new ArgumentException("Try to set invalid data type", "type");
		}
		Array[(int)Enum.Parse(typeof(TEnum), type, ignoreCase: true)] = value;
	}

	public void Set(TEnum type, T value)
	{
		Array[Convert.ToInt32(type)] = value;
	}

	public void Set(EnumArray<TEnum, T> other)
	{
		System.Array.Copy(other.Array, Array, Array.Length);
	}

	public void SetAll(T value)
	{
		for (int i = 0; i < Array.Length; i++)
		{
			Array[i] = value;
		}
	}

	public KeyValuePair<TEnum, T>[] ToKeyValuePairs()
	{
		return Keys.Zip(Array, (TEnum key, T value) => new KeyValuePair<TEnum, T>(key, value)).ToArray();
	}

	public EnumArray<TEnum, T> Clone()
	{
		return new EnumArray<TEnum, T>(this);
	}

	public IEnumerator<T> GetEnumerator()
	{
		return ((IEnumerable<T>)Array).GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return Array.GetEnumerator();
	}
}
public class EnumDeck<TEnum> where TEnum : Enum
{
	private readonly List<TEnum> _list;

	private int _current;

	public int Count => _list.Count;

	public int Remains => _list.Count - _current;

	public EnumDeck(IEnumerable<TEnum> items)
	{
		_list = new List<TEnum>(items);
	}

	public EnumDeck(EnumArray<TEnum, int> items)
	{
		_list = new List<TEnum>();
		foreach (TEnum key in items.Keys)
		{
			for (int i = 0; i < items[key]; i++)
			{
				_list.Add(key);
			}
		}
		_list.Shuffle();
	}

	public void Reset()
	{
		_current = 0;
	}

	public void Shuffle()
	{
		_current = 0;
		_list.Shuffle();
	}

	public TEnum Draw()
	{
		return _list[_current++];
	}

	public TEnum Peek()
	{
		return _list[_current + 1];
	}
}
public class EnumOverrider<T> where T : Enum
{
	public bool @override;

	public T value;

	public void Override(ref T value)
	{
		if (@override)
		{
			value = this.value;
		}
	}
}
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
public class FloatOverrider
{
	public bool @override;

	public float value;

	public void Override(ref float value)
	{
		if (@override)
		{
			value = this.value;
		}
	}
}
public static class GameObjectModifier
{
	public delegate void Delegate(MonoBehaviour component);

	public static readonly Delegate None = delegate
	{
	};

	public static readonly Delegate OneScale = delegate(MonoBehaviour component)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		((Component)component).transform.localScale = Vector3.one;
	};

	public static readonly Delegate IdentityRotation = delegate(MonoBehaviour component)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		((Component)component).transform.rotation = Quaternion.identity;
	};

	public static readonly Delegate RandomRotation = delegate(MonoBehaviour component)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		((Component)component).transform.rotation = MMMaths.RandomRotation2D();
	};

	public static void Modify(this MonoBehaviour @this, Delegate @delegate)
	{
		@delegate(@this);
	}

	public static Delegate Scale(float scale)
	{
		return delegate(MonoBehaviour component)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			((Component)component).transform.localScale = Vector3.one * scale;
		};
	}

	public static Delegate Scale(float x, float y)
	{
		return delegate(MonoBehaviour component)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			((Component)component).transform.localScale = new Vector3(x, y, 1f);
		};
	}

	public static Delegate RandomScale(float min, float max)
	{
		return delegate(MonoBehaviour component)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			((Component)component).transform.localScale = Vector3.one * Random.Range(min, max);
		};
	}

	public static Delegate LerpScale(float from, float to, float time)
	{
		return delegate(MonoBehaviour component)
		{
			component.StartCoroutine(CLerpScale(component, from, to, time));
		};
	}

	public static IEnumerator CLerpScale(MonoBehaviour component, float from, float to, float time)
	{
		float elpasedTime = 0f;
		Vector3 fromScale = Vector3.one * from;
		Vector3 toScale = Vector3.one * to;
		for (; elpasedTime < time; elpasedTime += Chronometer.global.deltaTime)
		{
			((Component)component).transform.localScale = Vector3.Lerp(fromScale, toScale, elpasedTime / time);
			yield return null;
		}
	}

	public static Delegate TranslateBySpeedAndAcc(float speed, float acc, float accMultiplier)
	{
		return delegate(MonoBehaviour component)
		{
			component.StartCoroutine(CTranslateBySpeedAndAcc(component, speed, acc, accMultiplier));
		};
	}

	public static IEnumerator CTranslateBySpeedAndAcc(MonoBehaviour component, float speed, float acc, float accMultiflier)
	{
		while (true)
		{
			speed += acc * Time.deltaTime;
			acc += acc * accMultiflier * Time.deltaTime;
			((Component)component).transform.Translate(0f, speed * Time.deltaTime, 0f);
			yield return null;
		}
	}

	public static Delegate ScaleByAcc(float scale, float acc, float accMultiplier)
	{
		return delegate(MonoBehaviour component)
		{
			component.StartCoroutine(CScaleByAcc(component, scale, acc, accMultiplier));
		};
	}

	public static IEnumerator CScaleByAcc(MonoBehaviour component, float scale, float acc, float accMultiflier)
	{
		while (true)
		{
			scale += acc * Time.deltaTime;
			acc += acc * accMultiflier * Time.deltaTime;
			((Component)component).transform.localScale = new Vector3(scale, scale, 0f);
			yield return null;
		}
	}

	public static Delegate TranslateUniformMotion(float x, float y, float z)
	{
		return delegate(MonoBehaviour component)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			component.StartCoroutine(CTranslateUniformMotion(component, new Vector3(x, y, z)));
		};
	}

	public static Delegate TranslateUniformMotion(Vector3 speed)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return delegate(MonoBehaviour component)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			component.StartCoroutine(CTranslateUniformMotion(component, speed));
		};
	}

	public static IEnumerator CTranslateUniformMotion(MonoBehaviour component, Vector3 speed)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		while (true)
		{
			((Component)component).transform.Translate(speed * Time.deltaTime);
			yield return null;
		}
	}
}
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
public interface IBlackboard : IDictionary<string, object>, ICollection<KeyValuePair<string, object>>, IEnumerable<KeyValuePair<string, object>>, IEnumerable
{
	T Get<T>(string name);

	void Set<T>(string name, T value);
}
public interface IUseChronometer
{
	ChronometerBase chronometer { get; set; }
}
public class IntOverrider
{
	public bool @override;

	public int value;

	public void Override(ref int value)
	{
		if (@override)
		{
			value = this.value;
		}
	}
}
public static class MMMaths
{
	public static readonly Random random = new Random();

	public static Vector2 Vector3ToVector2(Vector3 target)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2(target.x, target.y);
	}

	public static Vector3 Vector2ToVector3(Vector2 target)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		return new Vector3(target.x, target.y, 0f);
	}

	public static Vector3 Vector2ToVector3(Vector2 target, float newZValue)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		return new Vector3(target.x, target.y, newZValue);
	}

	public static Vector2 RandomPointWithinBounds(Bounds bounds)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		return RandomVector2(Vector2.op_Implicit(((Bounds)(ref bounds)).min), Vector2.op_Implicit(((Bounds)(ref bounds)).max));
	}

	public static Vector2 RandomVector2(Vector2 minimum, Vector2 maximum)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2(Random.Range(minimum.x, maximum.x), Random.Range(minimum.y, maximum.y));
	}

	public static Vector3 RandomVector3(Vector3 minimum, Vector3 maximum)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		return new Vector3(Random.Range(minimum.x, maximum.x), Random.Range(minimum.y, maximum.y), Random.Range(minimum.z, maximum.z));
	}

	public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, float angle)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		angle *= (float)Math.PI / 180f;
		float num = Mathf.Cos(angle) * (point.x - pivot.x) - Mathf.Sin(angle) * (point.y - pivot.y) + pivot.x;
		float num2 = Mathf.Sin(angle) * (point.x - pivot.x) + Mathf.Cos(angle) * (point.y - pivot.y) + pivot.y;
		return new Vector3(num, num2, 0f);
	}

	public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angle)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = point - pivot;
		val = Quaternion.Euler(angle) * val;
		point = val + pivot;
		return point;
	}

	public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion quaternion)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = point - pivot;
		val = quaternion * val;
		point = val + pivot;
		return point;
	}

	public static float AngleBetween(Vector2 vectorA, Vector2 vectorB)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		float num = Vector2.Angle(vectorA, vectorB);
		if (Vector3.Cross(Vector2.op_Implicit(vectorA), Vector2.op_Implicit(vectorB)).z > 0f)
		{
			num = 360f - num;
		}
		return num;
	}

	public static int Sum(params int[] thingsToAdd)
	{
		int num = 0;
		for (int i = 0; i < thingsToAdd.Length; i++)
		{
			num += thingsToAdd[i];
		}
		return num;
	}

	public static int RollADice(int numberOfSides)
	{
		return Random.Range(1, numberOfSides);
	}

	public static bool PercentChance(int percent)
	{
		return Random.Range(0, 100) + 1 <= percent;
	}

	public static bool PercentChance(Random random, int percent)
	{
		return random.Next(0, 100) + 1 <= percent;
	}

	public static bool Chance(float normalizedChance)
	{
		return Random.value <= normalizedChance;
	}

	public static bool Chance(Random random, float normalizedChance)
	{
		return random.NextDouble() <= (double)normalizedChance;
	}

	public static bool Chance(double normalizedChance)
	{
		return (double)Random.value <= normalizedChance;
	}

	public static bool Chance(Random random, double normalizedChance)
	{
		return random.NextDouble() <= normalizedChance;
	}

	public static float Approach(float from, float to, float amount)
	{
		if (from < to)
		{
			from += amount;
			if (from > to)
			{
				return to;
			}
		}
		else
		{
			from -= amount;
			if (from < to)
			{
				return to;
			}
		}
		return from;
	}

	public static float Remap(float x, float A, float B, float C, float D)
	{
		return C + (x - A) / (B - A) * (D - C);
	}

	public static double Remap(double x, double A, double B, double C, double D)
	{
		return C + (x - A) / (B - A) * (D - C);
	}

	public static float RoundToClosest(float value, float[] possibleValues)
	{
		if (possibleValues.Length == 0)
		{
			return 0f;
		}
		float num = possibleValues[0];
		foreach (float num2 in possibleValues)
		{
			if (Mathf.Abs(num - value) > Mathf.Abs(num2 - value))
			{
				num = num2;
			}
		}
		return num;
	}

	public static bool RandomBool()
	{
		return Random.Range(0, 2) == 1;
	}

	public static Quaternion RandomRotation2D()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		return Quaternion.Euler(0f, 0f, (float)Random.Range(0, 360));
	}

	public static Vector2 Min(Vector2 vectorA, Vector2 vectorB)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2(Mathf.Min(vectorA.x, vectorB.x), Mathf.Min(vectorA.y, vectorB.y));
	}

	public static Vector2 Max(Vector2 vectorA, Vector2 vectorB)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2(Mathf.Max(vectorA.x, vectorB.x), Mathf.Max(vectorA.y, vectorB.y));
	}

	public static int CompareDistanceFrom(float a, float b, float origin)
	{
		float num = Mathf.Abs(origin - a);
		float value = Mathf.Abs(origin - b);
		return num.CompareTo(value);
	}

	public static bool Range(byte value, byte min, byte max)
	{
		if (value >= min)
		{
			return value <= max;
		}
		return false;
	}

	public static bool Range(short value, short min, short max)
	{
		if (value >= min)
		{
			return value <= max;
		}
		return false;
	}

	public static bool Range(int value, int min, int max)
	{
		if (value >= min)
		{
			return value <= max;
		}
		return false;
	}

	public static bool Range(long value, long min, long max)
	{
		if (value >= min)
		{
			return value <= max;
		}
		return false;
	}

	public static bool Range(float value, float min, float max)
	{
		if (value >= min)
		{
			return value <= max;
		}
		return false;
	}

	public static bool Range(double value, double min, double max)
	{
		if (value >= min)
		{
			return value <= max;
		}
		return false;
	}

	public static bool Range(float value, Vector2 range)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		if (value >= range.x)
		{
			return value <= range.y;
		}
		return false;
	}

	public static bool Range(float value, Vector2Int range)
	{
		if (value >= (float)((Vector2Int)(ref range)).x)
		{
			return value <= (float)((Vector2Int)(ref range)).y;
		}
		return false;
	}

	public static bool Range(int value, Vector2Int range)
	{
		if (value >= ((Vector2Int)(ref range)).x)
		{
			return value <= ((Vector2Int)(ref range)).y;
		}
		return false;
	}

	public static int[] MultipleRandomWithoutDuplactes(Random random, int count, int min, int max)
	{
		if (max <= min || count < 0 || (count > max - min && max - min > 0))
		{
			throw new ArgumentOutOfRangeException($"Range {min} to {max} ({max - min} values), or count {count} is illegal");
		}
		HashSet<int> hashSet = new HashSet<int>();
		for (int i = max - count; i < max; i++)
		{
			if (!hashSet.Add(random.Next(min, i + 1)))
			{
				hashSet.Add(i);
			}
		}
		int[] array = hashSet.ToArray();
		array.PseudoShuffle(random);
		return array;
	}

	public static int[] MultipleRandomWithoutDuplactes(int count, int min, int max)
	{
		return MultipleRandomWithoutDuplactes(random, count, min, max);
	}

	public static Vector2 BezierCurve(Vector2[] points, float time)
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		int num = points.Length - 1;
		float[] array = new float[points.Length];
		array[0] = 1f;
		for (int i = 1; i < array.Length; i++)
		{
			array[i] = array[i - 1] * ((float)num - (float)(i - 1)) / (float)i;
		}
		Vector2[] array2 = (Vector2[])(object)new Vector2[points.Length];
		for (int j = 0; j < array2.Length; j++)
		{
			array2[j] = points[j] * array[j] * Mathf.Pow(time, (float)j) * Mathf.Pow(1f - time, (float)(num - j));
		}
		Vector2 val = Vector2.zero;
		Vector2[] array3 = array2;
		foreach (Vector2 val2 in array3)
		{
			val += val2;
		}
		return val;
	}

	public static Vector3 GetBezierCurve(Vector3[] points, float time)
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		int num = points.Length - 1;
		float[] array = new float[points.Length];
		array[0] = 1f;
		for (int i = 1; i < array.Length; i++)
		{
			array[i] = array[i - 1] * ((float)num - (float)(i - 1)) / (float)i;
		}
		Vector3[] array2 = (Vector3[])(object)new Vector3[points.Length];
		for (int j = 0; j < array2.Length; j++)
		{
			array2[j] = points[j] * array[j] * Mathf.Pow(time, (float)j) * Mathf.Pow(1f - time, (float)(num - j));
		}
		Vector3 val = Vector3.zero;
		Vector3[] array3 = array2;
		foreach (Vector3 val2 in array3)
		{
			val += val2;
		}
		return val;
	}
}
public static class Mathfx
{
	public enum EaseMethod
	{
		Lerp = 0,
		SmoothStep = 1,
		Hermite = 2,
		Sinerp = 3,
		Coserp = 4,
		Berp = 5,
		EaseInQuad = 0,
		EaseOutQuad = 1,
		EaseInOutQuad = 2,
		EaseInCubic = 3,
		EaseOutCubic = 4,
		EaseInOutCubic = 5,
		EaseInQuart = 6,
		EaseOutQuart = 7,
		EaseInOutQuart = 8,
		EaseInQuint = 9,
		EaseOutQuint = 10,
		EaseInOutQuint = 11,
		EaseInSine = 12,
		EaseOutSine = 13,
		EaseInOutSine = 14,
		EaseInExpo = 15,
		EaseOutExpo = 16,
		EaseInOutExpo = 17,
		EaseInCirc = 18,
		EaseOutCirc = 19,
		EaseInOutCirc = 20,
		Linear = 21,
		Spring = 22,
		EaseInBounce = 23,
		EaseOutBounce = 24,
		EaseInOutBounce = 25,
		EaseInBack = 26,
		EaseOutBack = 27,
		EaseInOutBack = 28,
		EaseInElastic = 29,
		EaseOutElastic = 30,
		EaseInOutElastic = 31
	}

	public delegate float EaseFunction(float start, float end, float value);

	public static EaseFunction GetEaseFunction(EaseMethod method)
	{
		return method switch
		{
			EaseMethod.Hermite => Hermite, 
			EaseMethod.Sinerp => Sinerp, 
			EaseMethod.Coserp => Coserp, 
			EaseMethod.Berp => Berp, 
			EaseMethod.SmoothStep => SmoothStep, 
			_ => Lerp, 
		};
	}

	public static float Ease(EaseMethod method, float start, float end, float value)
	{
		return GetEaseFunction(method)(start, end, value);
	}

	public static float Hermite(float start, float end, float value)
	{
		return Mathf.Lerp(start, end, value * value * (3f - 2f * value));
	}

	public static Vector2 Hermite(Vector2 start, Vector2 end, float value)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2(Hermite(start.x, end.x, value), Hermite(start.y, end.y, value));
	}

	public static Vector3 Hermite(Vector3 start, Vector3 end, float value)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		return new Vector3(Hermite(start.x, end.x, value), Hermite(start.y, end.y, value), Hermite(start.z, end.z, value));
	}

	public static float Sinerp(float start, float end, float value)
	{
		return Mathf.Lerp(start, end, Mathf.Sin(value * (float)Math.PI * 0.5f));
	}

	public static Vector2 Sinerp(Vector2 start, Vector2 end, float value)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2(Mathf.Lerp(start.x, end.x, Mathf.Sin(value * (float)Math.PI * 0.5f)), Mathf.Lerp(start.y, end.y, Mathf.Sin(value * (float)Math.PI * 0.5f)));
	}

	public static Vector3 Sinerp(Vector3 start, Vector3 end, float value)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		return new Vector3(Mathf.Lerp(start.x, end.x, Mathf.Sin(value * (float)Math.PI * 0.5f)), Mathf.Lerp(start.y, end.y, Mathf.Sin(value * (float)Math.PI * 0.5f)), Mathf.Lerp(start.z, end.z, Mathf.Sin(value * (float)Math.PI * 0.5f)));
	}

	public static float Coserp(float start, float end, float value)
	{
		return Mathf.Lerp(start, end, 1f - Mathf.Cos(value * (float)Math.PI * 0.5f));
	}

	public static Vector2 Coserp(Vector2 start, Vector2 end, float value)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2(Coserp(start.x, end.x, value), Coserp(start.y, end.y, value));
	}

	public static Vector3 Coserp(Vector3 start, Vector3 end, float value)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		return new Vector3(Coserp(start.x, end.x, value), Coserp(start.y, end.y, value), Coserp(start.z, end.z, value));
	}

	public static float Berp(float start, float end, float value)
	{
		value = Mathf.Clamp01(value);
		value = (Mathf.Sin(value * (float)Math.PI * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + 1.2f * (1f - value));
		return start + (end - start) * value;
	}

	public static Vector2 Berp(Vector2 start, Vector2 end, float value)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2(Berp(start.x, end.x, value), Berp(start.y, end.y, value));
	}

	public static Vector3 Berp(Vector3 start, Vector3 end, float value)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		return new Vector3(Berp(start.x, end.x, value), Berp(start.y, end.y, value), Berp(start.z, end.z, value));
	}

	public static float SmoothStep(float x, float min, float max)
	{
		x = Mathf.Clamp(x, min, max);
		float num = (x - min) / (max - min);
		float num2 = (x - min) / (max - min);
		return -2f * num * num * num + 3f * num2 * num2;
	}

	public static Vector2 SmoothStep(Vector2 vec, float min, float max)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2(SmoothStep(vec.x, min, max), SmoothStep(vec.y, min, max));
	}

	public static Vector3 SmoothStep(Vector3 vec, float min, float max)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		return new Vector3(SmoothStep(vec.x, min, max), SmoothStep(vec.y, min, max), SmoothStep(vec.z, min, max));
	}

	public static float Lerp(float start, float end, float value)
	{
		return (1f - value) * start + value * end;
	}

	public static Vector3 NearestPoint(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = Vector3.Normalize(lineEnd - lineStart);
		float num = Vector3.Dot(point - lineStart, val);
		return lineStart + num * val;
	}

	public static Vector3 NearestPointStrict(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = lineEnd - lineStart;
		Vector3 val2 = Vector3.Normalize(val);
		float num = Vector3.Dot(point - lineStart, val2);
		return lineStart + Mathf.Clamp(num, 0f, Vector3.Magnitude(val)) * val2;
	}

	public static float Bounce(float x)
	{
		return Mathf.Abs(Mathf.Sin(6.28f * (x + 1f) * (x + 1f)) * (1f - x));
	}

	public static Vector2 Bounce(Vector2 vec)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2(Bounce(vec.x), Bounce(vec.y));
	}

	public static Vector3 Bounce(Vector3 vec)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		return new Vector3(Bounce(vec.x), Bounce(vec.y), Bounce(vec.z));
	}

	public static bool Approx(float val, float about, float range)
	{
		return Mathf.Abs(val - about) < range;
	}

	public static bool Approx(Vector3 val, Vector3 about, float range)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val2 = val - about;
		return ((Vector3)(ref val2)).sqrMagnitude < range * range;
	}

	public static float Clerp(float start, float end, float value)
	{
		float num = 0f;
		float num2 = 360f;
		float num3 = Mathf.Abs((num2 - num) / 2f);
		float num4 = 0f;
		float num5 = 0f;
		if (end - start < 0f - num3)
		{
			num5 = (num2 - start + end) * value;
			return start + num5;
		}
		if (end - start > num3)
		{
			num5 = (0f - (num2 - end + start)) * value;
			return start + num5;
		}
		return start + (end - start) * value;
	}
}
public class MaxOnlyTimedFloats
{
	private abstract class Runner
	{
		public readonly object key;

		protected float _timeToExpire;

		public abstract float value { get; }

		public bool TakeTime(float time)
		{
			return (_timeToExpire -= time) <= 0f;
		}

		public Runner(object key, float duration)
		{
			this.key = key;
			_timeToExpire = duration;
		}
	}

	private class CurveRunner : Runner
	{
		private readonly Curve _curve;

		public override float value => _curve.Evaluate(_timeToExpire);

		public CurveRunner(object key, Curve curve)
			: base(key, curve.duration)
		{
			_curve = curve;
		}
	}

	private class ConstantRunner : Runner
	{
		private readonly float _value;

		public override float value => _value;

		public ConstantRunner(object key, float value, float duration)
			: base(key, duration)
		{
			_value = value;
		}
	}

	private readonly List<Runner> _runners = new List<Runner>();

	public float value { get; protected set; }

	public void Attach(object key, float value, float duration)
	{
		_runners.Add(new ConstantRunner(key, value, duration));
	}

	public void Attach(object key, Curve curve)
	{
		_runners.Add(new CurveRunner(key, curve));
	}

	public void Detach(object key)
	{
		for (int i = 0; i < _runners.Count; i++)
		{
			if (_runners[i].key == key)
			{
				_runners.RemoveAt(i);
				break;
			}
		}
	}

	public void Update()
	{
		Update(Time.deltaTime);
	}

	public void Update(float deltaTime)
	{
		value = 0f;
		for (int num = _runners.Count - 1; num >= 0; num--)
		{
			Runner runner = _runners[num];
			if (runner.value > value)
			{
				value = runner.value;
			}
			if (runner.TakeTime(deltaTime))
			{
				_runners.RemoveAt(num);
			}
		}
	}

	public void Clear()
	{
		_runners.Clear();
		value = 0f;
	}
}
public class MultiMap<TKey, TValue> : Dictionary<TKey, List<TValue>>
{
	public int CountAll
	{
		get
		{
			int num = 0;
			foreach (List<TValue> value in base.Values)
			{
				num += value.Count;
			}
			return num;
		}
	}

	public MultiMap()
	{
	}

	public MultiMap(int capacity)
		: base(capacity)
	{
	}

	public void Add(TKey key, TValue value)
	{
		if (TryGetValue(key, out var value2))
		{
			value2.Add(value);
			return;
		}
		value2 = new List<TValue>();
		value2.Add(value);
		Add(key, value2);
	}

	public bool Remove(TKey key, TValue value)
	{
		if (TryGetValue(key, out var value2) && value2.Remove(value))
		{
			if (value2.Count == 0)
			{
				Remove(key);
			}
			return true;
		}
		return false;
	}

	public int RemoveAll(TKey key, TValue value)
	{
		int num = 0;
		if (TryGetValue(key, out var value2))
		{
			while (value2.Remove(value))
			{
				num++;
			}
			if (value2.Count == 0)
			{
				Remove(key);
			}
		}
		return num;
	}

	public bool Contains(TKey key, TValue value)
	{
		if (TryGetValue(key, out var value2))
		{
			return value2.Contains(value);
		}
		return false;
	}

	public bool Contains(TValue value)
	{
		foreach (List<TValue> value2 in base.Values)
		{
			if (value2.Contains(value))
			{
				return true;
			}
		}
		return false;
	}
}
public class OverlappedFloat
{
	private readonly Dictionary<object, float> _values = new Dictionary<object, float>();

	public float value { get; private set; }

	public OverlappedFloat(float defaultValue = 0f)
	{
		Attach(this, defaultValue);
	}

	public void SetDefault(float defaultValue)
	{
		_values[this] = defaultValue;
	}

	public void Attach(object owner, float bonus)
	{
		_values.Add(owner, bonus);
		value += bonus;
	}

	public void Change(object owner, float bonus)
	{
		value -= _values[owner];
		_values[owner] = bonus;
	}

	public void ChangeOrAttach(object owner, float bonus)
	{
		if (_values.ContainsKey(owner))
		{
			Change(owner, bonus);
			return;
		}
		_values.Add(owner, bonus);
		value += bonus;
	}

	public void Remove(object owner)
	{
		if (_values.TryGetValue(owner, out var num))
		{
			value -= num;
			_values.Remove(owner);
		}
	}
}
public class OverlappedInt
{
	private readonly Dictionary<object, int> _values = new Dictionary<object, int>();

	public int value { get; private set; }

	public OverlappedInt(int defaultValue = 0)
	{
		Attach(this, defaultValue);
	}

	public void SetDefault(int defaultValue)
	{
		_values[this] = defaultValue;
	}

	public void Attach(object owner, int bonus)
	{
		_values.Add(owner, bonus);
		value += bonus;
	}

	public void Change(object owner, int bonus)
	{
		value -= _values[owner];
		_values[owner] = bonus;
	}

	public void ChangeOrAttach(object owner, int bonus)
	{
		if (_values.ContainsKey(owner))
		{
			Change(owner, bonus);
			return;
		}
		_values.Add(owner, bonus);
		value += bonus;
	}

	public void Remove(object owner)
	{
		if (_values.TryGetValue(owner, out var num))
		{
			value -= num;
			_values.Remove(owner);
		}
	}
}
public class PriorityList<T> : IEnumerable<T>, IEnumerable
{
	public struct PriorityWrapper
	{
		public int priority;

		public T value;

		internal PriorityWrapper(int priority, T value)
		{
			this.priority = priority;
			this.value = value;
		}
	}

	public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
	{
		private readonly List<PriorityWrapper> _list;

		private int _index;

		public T Current { get; private set; }

		object IEnumerator.Current => Current;

		internal Enumerator(List<PriorityWrapper> list)
		{
			_list = list;
			_index = 0;
			Current = default(T);
		}

		public void Dispose()
		{
		}

		public bool MoveNext()
		{
			if (_index == _list.Count)
			{
				return false;
			}
			Current = _list[_index++].value;
			return true;
		}

		public void Reset()
		{
			_index = 0;
			Current = default(T);
		}
	}

	protected readonly List<PriorityWrapper> _items = new List<PriorityWrapper>();

	public int Count => _items.Count;

	public T this[int index] => _items[index].value;

	public void Add(int priority, T item)
	{
		PriorityWrapper item2 = new PriorityWrapper(priority, item);
		for (int i = 0; i < _items.Count; i++)
		{
			if (priority > _items[i].priority)
			{
				_items.Insert(i, item2);
				return;
			}
		}
		_items.Add(item2);
	}

	public void Clear()
	{
		_items.Clear();
	}

	public bool Contains(T item)
	{
		for (int i = 0; i < _items.Count; i++)
		{
			if (_items[i].value.Equals(item))
			{
				return true;
			}
		}
		return false;
	}

	public int IndexOf(T item)
	{
		for (int i = 0; i < _items.Count; i++)
		{
			if (_items[i].value.Equals(item))
			{
				return i;
			}
		}
		return -1;
	}

	public bool Remove(T item)
	{
		for (int i = 0; i < _items.Count; i++)
		{
			if (_items[i].value.Equals(item))
			{
				_items.RemoveAt(i);
				return true;
			}
		}
		return false;
	}

	public void RemoveAt(int index)
	{
		_items.RemoveAt(index);
	}

	public IEnumerator<T> GetEnumerator()
	{
		return new Enumerator(_items);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return new Enumerator(_items);
	}
}
public class Product<T>
{
	protected readonly Dictionary<object, T> _multiplication = new Dictionary<object, T>();

	protected T _base;

	public T total { get; protected set; }

	public T @base
	{
		get
		{
			return _base;
		}
		set
		{
			_base = value;
			ComputeValue();
		}
	}

	public T this[object key]
	{
		get
		{
			return _multiplication[key];
		}
		set
		{
			AddOrUpdate(key, value);
		}
	}

	public Product(T @base)
	{
		_base = @base;
		total = _base;
		Add(this, @base);
	}

	public void Add(object key, T value)
	{
		_multiplication.Add(key, value);
		total *= (dynamic)value;
	}

	public void AddOrUpdate(object key, T value)
	{
		if (_multiplication.ContainsKey(key))
		{
			_multiplication[key] = value;
			ComputeValue();
		}
		else
		{
			_multiplication.Add(key, value);
			total *= (dynamic)value;
		}
	}

	public bool Contains(object key)
	{
		return _multiplication.ContainsKey(key);
	}

	public bool Remove(object owner)
	{
		if (_multiplication.Remove(owner))
		{
			ComputeValue();
			return true;
		}
		return false;
	}

	public void Clear()
	{
		_multiplication.Clear();
		total = _base;
	}

	private void ComputeValue()
	{
		total = @base;
		foreach (T value in _multiplication.Values)
		{
			total *= (dynamic)value;
		}
	}
}
public class ProductFloat
{
	protected readonly Dictionary<object, float> _multiplication = new Dictionary<object, float>();

	protected float _base;

	public float total { get; protected set; }

	public float @base
	{
		get
		{
			return _base;
		}
		set
		{
			_base = value;
			ComputeValue();
		}
	}

	public float this[object key]
	{
		get
		{
			return _multiplication[key];
		}
		set
		{
			AddOrUpdate(key, value);
		}
	}

	public ProductFloat(float @base)
	{
		_base = @base;
		total = _base;
		Add(this, @base);
	}

	public void Add(object key, float value)
	{
		_multiplication.Add(key, value);
		total *= value;
	}

	public void AddOrUpdate(object key, float value)
	{
		if (_multiplication.ContainsKey(key))
		{
			_multiplication[key] = value;
			ComputeValue();
		}
		else
		{
			_multiplication.Add(key, value);
			total *= value;
		}
	}

	public bool Contains(object key)
	{
		return _multiplication.ContainsKey(key);
	}

	public bool Remove(object owner)
	{
		if (_multiplication.Remove(owner))
		{
			ComputeValue();
			return true;
		}
		return false;
	}

	public void Clear()
	{
		_multiplication.Clear();
		total = _base;
	}

	private void ComputeValue()
	{
		total = @base;
		foreach (float value in _multiplication.Values)
		{
			total *= value;
		}
	}
}
public class ReadonlyBoundedList<T> : IEnumerable, IEnumerable<T>, IReadOnlyList<T>, IReadOnlyCollection<T>
{
	public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
	{
		private readonly ReadonlyBoundedList<T> _list;

		private int _index;

		public T Current { get; private set; }

		object IEnumerator.Current => Current;

		internal Enumerator(ReadonlyBoundedList<T> list)
		{
			_list = list;
			_index = 0;
			Current = default(T);
		}

		public void Dispose()
		{
		}

		public bool MoveNext()
		{
			if (_index == _list.Count)
			{
				return false;
			}
			Current = _list._itmes[_index++];
			return true;
		}

		public void Reset()
		{
			_index = 0;
			Current = default(T);
		}
	}

	protected readonly T[] _itmes;

	public int capacity => _itmes.Length;

	public int Count { get; protected set; }

	public T this[int index]
	{
		get
		{
			return _itmes[index];
		}
		protected set
		{
			if (index >= Count)
			{
				throw new IndexOutOfRangeException();
			}
			_itmes[index] = value;
		}
	}

	public ReadonlyBoundedList(int capacity)
	{
		_itmes = new T[capacity];
		Count = capacity;
	}

	public ReadonlyBoundedList(T[] sourceArray)
	{
		_itmes = (T[])sourceArray.Clone();
		Count = sourceArray.Length;
	}

	public IEnumerator<T> GetEnumerator()
	{
		return new Enumerator(this);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return new Enumerator(this);
	}

	public int IndexOf(T item)
	{
		return Array.IndexOf(_itmes, item, 0, Count);
	}

	public bool Contains(T item)
	{
		return IndexOf(item) != -1;
	}

	public void CopyTo(T[] array, int arrayIndex)
	{
		Array.Copy(_itmes, 0, array, arrayIndex, Count);
	}
}
public static class RectUtility
{
	public static void UseWidth(this Rect rect, float width)
	{
		((Rect)(ref rect)).x = ((Rect)(ref rect)).x + width;
		((Rect)(ref rect)).width = ((Rect)(ref rect)).width - width;
	}

	public static void UseHeight(this Rect rect, float height)
	{
		((Rect)(ref rect)).y = ((Rect)(ref rect)).y + height;
		((Rect)(ref rect)).height = ((Rect)(ref rect)).height - height;
	}

	public static Rect SubWidth(this Rect rect, float startPercent, float endPercent)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		Rect result = rect;
		float num = endPercent - startPercent;
		((Rect)(ref result)).width = ((Rect)(ref rect)).width * num;
		((Rect)(ref result)).x = ((Rect)(ref result)).x + ((Rect)(ref rect)).width * startPercent;
		return result;
	}

	public static Rect SubHeight(this Rect rect, float startPercent, float endPercent)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		Rect result = rect;
		float num = endPercent - startPercent;
		((Rect)(ref result)).height = ((Rect)(ref rect)).height * num;
		((Rect)(ref result)).y = ((Rect)(ref result)).y + ((Rect)(ref rect)).height * startPercent;
		return result;
	}

	public static Rect[] GetHorizontal(this Rect rect, float height, float margin, params float[] weights)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		Rect rect2 = rect;
		((Rect)(ref rect2)).height = height;
		Rect[] array = (Rect[])(object)new Rect[weights.Length];
		float num = 0f;
		foreach (float num2 in weights)
		{
			num += num2;
		}
		float num3 = 0f;
		for (int j = 0; j < weights.Length; j++)
		{
			if (j == weights.Length - 1)
			{
				array[j] = rect2.SubWidth(num3, num3 += weights[j] / num);
			}
			else
			{
				array[j] = rect2.SubWidth(num3, (num3 += weights[j] / num) - margin);
			}
		}
		return array;
	}

	public static Rect[] GetVertical(this Rect rect, float width, float margin, params float[] weights)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		Rect rect2 = rect;
		((Rect)(ref rect2)).width = width;
		Rect[] array = (Rect[])(object)new Rect[weights.Length];
		float num = 0f;
		foreach (float num2 in weights)
		{
			num += num2;
		}
		float num3 = 0f;
		for (int j = 0; j < weights.Length; j++)
		{
			if (j == weights.Length - 1)
			{
				array[j] = rect2.SubHeight(num3, num3 += weights[j] / num);
			}
			else
			{
				array[j] = rect2.SubHeight(num3, (num3 += weights[j] / num) - margin);
			}
		}
		return array;
	}
}
public abstract class ReorderableListParent
{
}
public abstract class ReorderableArray<T> : ReorderableListParent
{
	[SerializeField]
	public T[] values;
}
public abstract class ReorderableList<T> : ReorderableListParent
{
	[SerializeField]
	public List<T> values;
}
[Serializable]
public class ReorderableFloatArray : ReorderableArray<float>
{
	public ReorderableFloatArray()
	{
	}

	public ReorderableFloatArray(params float[] @default)
	{
		values = @default;
	}
}
[Serializable]
public class ReorderableFloatList : ReorderableList<float>
{
	public ReorderableFloatList()
	{
	}

	public ReorderableFloatList(params float[] @default)
	{
		values = new List<float>(@default);
	}
}
public class SceneLoader : MonoBehaviour
{
	[HideInInspector]
	[SerializeField]
	private string _scenePath;

	[SerializeField]
	private LoadSceneMode _mode;

	private void Awake()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		SceneManager.LoadScene(_scenePath, _mode);
		Object.Destroy((Object)(object)this);
	}
}
[Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
	[SerializeField]
	private List<TKey> keys = new List<TKey>();

	[SerializeField]
	private List<TValue> values = new List<TValue>();

	public void OnBeforeSerialize()
	{
		keys.Clear();
		values.Clear();
		using Enumerator enumerator = GetEnumerator();
		while (enumerator.MoveNext())
		{
			KeyValuePair<TKey, TValue> current = enumerator.Current;
			keys.Add(current.Key);
			values.Add(current.Value);
		}
	}

	public void OnAfterDeserialize()
	{
		Clear();
		if (keys.Count != values.Count)
		{
			throw new Exception(string.Format("there are {0} keys and {1} values after deserialization. Make sure that both key and value types are serializable."));
		}
		for (int i = 0; i < keys.Count; i++)
		{
			Add(keys[i], values[i]);
		}
	}
}
public class SumFloat : Sum<float>
{
	public SumFloat(float @base)
		: base(@base)
	{
	}

	public void Add(object key, float value)
	{
		_sum.Add(key, value);
		base.total += value;
	}

	public override void AddOrUpdate(object key, float value)
	{
		if (_sum.ContainsKey(key))
		{
			_sum[key] = value;
			ComputeValue();
		}
		else
		{
			_sum.Add(key, value);
			base.total += value;
		}
	}

	public override void ComputeValue()
	{
		base.total = base.@base;
		foreach (float value in _sum.Values)
		{
			base.total += value;
		}
	}
}
public class SumDouble : Sum<double>
{
	public SumDouble(double @base)
		: base(@base)
	{
	}

	public void Add(object key, double value)
	{
		_sum.Add(key, value);
		base.total += value;
	}

	public override void AddOrUpdate(object key, double value)
	{
		if (_sum.ContainsKey(key))
		{
			_sum[key] = value;
			ComputeValue();
		}
		else
		{
			_sum.Add(key, value);
			base.total += value;
		}
	}

	public override void ComputeValue()
	{
		base.total = base.@base;
		foreach (double value in _sum.Values)
		{
			base.total += value;
		}
	}
}
public class SumInt : Sum<int>
{
	public SumInt(int @base)
		: base(@base)
	{
	}

	public void Add(object key, int value)
	{
		_sum.Add(key, value);
		base.total += value;
	}

	public override void AddOrUpdate(object key, int value)
	{
		if (_sum.ContainsKey(key))
		{
			_sum[key] = value;
			ComputeValue();
		}
		else
		{
			_sum.Add(key, value);
			base.total += value;
		}
	}

	public override void ComputeValue()
	{
		base.total = base.@base;
		foreach (int value in _sum.Values)
		{
			base.total += value;
		}
	}
}
public abstract class Sum<T>
{
	protected readonly Dictionary<object, T> _sum = new Dictionary<object, T>();

	protected T _base;

	public T total { get; protected set; }

	public T @base
	{
		get
		{
			return _base;
		}
		set
		{
			_base = value;
			ComputeValue();
		}
	}

	public T this[object key]
	{
		get
		{
			return _sum[key];
		}
		set
		{
			AddOrUpdate(key, value);
		}
	}

	public abstract void ComputeValue();

	public abstract void AddOrUpdate(object key, T value);

	public Sum()
	{
		_base = default(T);
		total = default(T);
	}

	public Sum(T @base)
	{
		_base = @base;
		total = @base;
	}

	public bool Contains(object key)
	{
		return _sum.ContainsKey(key);
	}

	public bool Remove(object owner)
	{
		if (_sum.Remove(owner))
		{
			ComputeValue();
			return true;
		}
		return false;
	}

	public void Clear()
	{
		_sum.Clear();
		total = _base;
	}
}
public abstract class TimedFloats
{
	protected readonly List<float> _values = new List<float>();

	protected readonly List<float> _times = new List<float>();

	protected float _staticValue;

	public abstract float value { get; }

	public TimedFloats()
	{
	}

	public TimedFloats(float defaultValue)
	{
		_staticValue = defaultValue;
	}

	public void SetDefault(float defaultValue)
	{
		_staticValue = defaultValue;
	}

	public virtual void Add(float value, float time)
	{
		_values.Add(value);
		_times.Add(time);
	}

	public virtual void Clear()
	{
		_values.Clear();
		_times.Clear();
	}

	public void TakeTime(float time)
	{
		for (int num = _times.Count - 1; num >= 0; num--)
		{
			if ((_times[num] -= time) <= 0f)
			{
				RemoveAt(num);
			}
		}
	}

	protected virtual void RemoveAt(int index)
	{
		_values.RemoveAt(index);
		_times.RemoveAt(index);
	}
}
public class TimedFloatsMax : TimedFloats
{
	private float _max;

	public override float value => Mathf.Max(_staticValue, _max);

	public override void Add(float value, float time)
	{
		base.Add(value, time);
		if (_max < value)
		{
			_max = value;
		}
	}

	protected override void RemoveAt(int index)
	{
		base.RemoveAt(index);
		_max = float.MinValue;
		for (int i = 0; i < _values.Count; i++)
		{
			float num = _values[i];
			if (num > _max)
			{
				_max = num;
			}
		}
	}

	public override void Clear()
	{
		base.Clear();
		_max = float.MinValue;
	}
}
public class TimedFloatsSum : TimedFloats
{
	private float _sum;

	public override float value => _staticValue + _sum;

	public override void Add(float value, float time)
	{
		base.Add(value, time);
		_sum += value;
	}

	protected override void RemoveAt(int index)
	{
		_sum -= _values[index];
		base.RemoveAt(index);
	}

	public override void Clear()
	{
		_sum = 0f;
		base.Clear();
	}
}
public sealed class TrueOnlyLogicalSumList
{
	private readonly List<object> _list = new List<object>();

	public bool value => _list.Count > 0;

	public TrueOnlyLogicalSumList(bool defaultValue = false)
	{
		if (defaultValue)
		{
			Attach(this);
		}
	}

	public void Attach(object key)
	{
		if (!_list.Contains(key))
		{
			_list.Add(key);
		}
	}

	public bool Detach(object key)
	{
		return _list.Remove(key);
	}
}
public struct UsingCollider : IDisposable
{
	public const string optimizeTooltip = "콜라이더 최적화 여부, Composite Collider등 특별한 경우가 아니면 true로 유지";

	private readonly Collider2D _collider;

	private readonly bool _optimize;

	public UsingCollider(Collider2D collider)
	{
		_optimize = true;
		_collider = collider;
		((Behaviour)_collider).enabled = true;
	}

	public UsingCollider(Collider2D collider, bool optimize)
	{
		_optimize = optimize;
		if (!_optimize)
		{
			_collider = null;
			return;
		}
		_collider = collider;
		((Behaviour)_collider).enabled = true;
	}

	public void Dispose()
	{
		if (_optimize)
		{
			((Behaviour)_collider).enabled = false;
		}
	}
}
public static class WeightedRandomizer
{
	public static WeightedRandomizer<R> From<R>(ICollection<(R key, float value)> spawnRate)
	{
		return new WeightedRandomizer<R>(spawnRate);
	}

	public static WeightedRandomizer<R> From<R>(params (R key, float value)[] spawnRate)
	{
		return new WeightedRandomizer<R>(spawnRate);
	}
}
public class WeightedRandomizer<T>
{
	private ICollection<(T key, float value)> _weights;

	public WeightedRandomizer(ICollection<(T key, float value)> weights)
	{
		_weights = weights;
	}

	public WeightedRandomizer(params (T key, float value)[] weights)
	{
		_weights = weights;
	}

	public T TakeOne()
	{
		List<(T, float)> list = Sort(_weights);
		float num = 0f;
		foreach (var weight in _weights)
		{
			num += weight.value;
		}
		float num2 = Random.Range(0f, num) + 1f;
		T result = list[list.Count - 1].Item1;
		foreach (var item in list)
		{
			num2 -= item.Item2;
			if (num2 < item.Item2)
			{
				(result, _) = item;
				break;
			}
		}
		return result;
	}

	public T TakeOne(Random seed)
	{
		List<(T, float)> list = Sort(_weights);
		float num = 0f;
		foreach (var weight in _weights)
		{
			num += weight.value;
		}
		float num2 = seed.Next((int)num) + 1;
		T result = list[list.Count - 1].Item1;
		foreach (var item in list)
		{
			num2 -= item.Item2;
			if (num2 < item.Item2)
			{
				(result, _) = item;
				break;
			}
		}
		return result;
	}

	private List<(T key, float value)> Sort(ICollection<(T key, float value)> weights)
	{
		List<(T, float)> list = new List<(T, float)>(weights);
		list.Sort(((T key, float value) firstPair, (T key, float value) nextPair) => firstPair.value.CompareTo(nextPair.value));
		return list;
	}
}
namespace Southpaw
{
	public class Random
	{
		public static System.Random random = new System.Random();

		public static double NextDouble()
		{
			return random.NextDouble();
		}

		public static double NextDouble(double minValue, double maxValue)
		{
			return minValue + random.NextDouble() * (maxValue - minValue);
		}

		public static int NextInt(double minValue, double maxValue)
		{
			return (int)(minValue + random.NextDouble() * (maxValue + 1.0 - minValue));
		}
	}
}
namespace ExpressionParser
{
	public interface IValue
	{
		double Value { get; }
	}
	public class Number : IValue
	{
		private double m_Value;

		public double Value
		{
			get
			{
				return m_Value;
			}
			set
			{
				m_Value = value;
			}
		}

		public Number(double aValue)
		{
			m_Value = aValue;
		}

		public override string ToString()
		{
			return m_Value.ToString() ?? "";
		}
	}
	public class OperationSum : IValue
	{
		private IValue[] m_Values;

		public double Value => m_Values.Select((IValue v) => v.Value).Sum();

		public OperationSum(params IValue[] aValues)
		{
			List<IValue> list = new List<IValue>(aValues.Length);
			foreach (IValue value in aValues)
			{
				if (!(value is OperationSum operationSum))
				{
					list.Add(value);
				}
				else
				{
					list.AddRange(operationSum.m_Values);
				}
			}
			m_Values = list.ToArray();
		}

		public override string ToString()
		{
			return "( " + string.Join(" + ", m_Values.Select((IValue v) => v.ToString()).ToArray()) + " )";
		}
	}
	public class OperationProduct : IValue
	{
		private IValue[] m_Values;

		public double Value => m_Values.Select((IValue v) => v.Value).Aggregate((double v1, double v2) => v1 * v2);

		public OperationProduct(params IValue[] aValues)
		{
			m_Values = aValues;
		}

		public override string ToString()
		{
			return "( " + string.Join(" * ", m_Values.Select((IValue v) => v.ToString()).ToArray()) + " )";
		}
	}
	public class OperationPower : IValue
	{
		private IValue m_Value;

		private IValue m_Power;

		public double Value => Math.Pow(m_Value.Value, m_Power.Value);

		public OperationPower(IValue aValue, IValue aPower)
		{
			m_Value = aValue;
			m_Power = aPower;
		}

		public override string ToString()
		{
			return "( " + m_Value?.ToString() + "^" + m_Power?.ToString() + " )";
		}
	}
	public class OperationNegate : IValue
	{
		private IValue m_Value;

		public double Value => 0.0 - m_Value.Value;

		public OperationNegate(IValue aValue)
		{
			m_Value = aValue;
		}

		public override string ToString()
		{
			return "( -" + m_Value?.ToString() + " )";
		}
	}
	public class OperationReciprocal : IValue
	{
		private IValue m_Value;

		public double Value => 1.0 / m_Value.Value;

		public OperationReciprocal(IValue aValue)
		{
			m_Value = aValue;
		}

		public override string ToString()
		{
			return "( 1/" + m_Value?.ToString() + " )";
		}
	}
	public class MultiParameterList : IValue
	{
		private IValue[] m_Values;

		public IValue[] Parameters => m_Values;

		public double Value => m_Values.Select((IValue v) => v.Value).FirstOrDefault();

		public MultiParameterList(params IValue[] aValues)
		{
			m_Values = aValues;
		}

		public override string ToString()
		{
			return string.Join(", ", m_Values.Select((IValue v) => v.ToString()).ToArray());
		}
	}
	public class CustomFunction : IValue
	{
		private IValue[] m_Params;

		private Func<double[], double> m_Delegate;

		private string m_Name;

		public double Value
		{
			get
			{
				if (m_Params == null)
				{
					return m_Delegate(null);
				}
				return m_Delegate(m_Params.Select((IValue p) => p.Value).ToArray());
			}
		}

		public CustomFunction(string aName, Func<double[], double> aDelegate, params IValue[] aValues)
		{
			m_Delegate = aDelegate;
			m_Params = aValues;
			m_Name = aName;
		}

		public override string ToString()
		{
			if (m_Params == null)
			{
				return m_Name;
			}
			return m_Name + "( " + string.Join(", ", m_Params.Select((IValue v) => v.ToString()).ToArray()) + " )";
		}
	}
	public class Parameter : Number
	{
		public string Name { get; private set; }

		public override string ToString()
		{
			return Name + "[" + base.ToString() + "]";
		}

		public Parameter(string aName)
			: base(0.0)
		{
			Name = aName;
		}
	}
	public class Expression : IValue
	{
		public class ParameterException : Exception
		{
			public ParameterException(string aMessage)
				: base(aMessage)
			{
			}
		}

		public IDictionary<string, Parameter> Parameters = new Dictionary<string, Parameter>();

		public IValue ExpressionTree { get; set; }

		public double Value => ExpressionTree.Value;

		public double[] MultiValue
		{
			get
			{
				if (ExpressionTree is MultiParameterList multiParameterList)
				{
					double[] array = new double[multiParameterList.Parameters.Length];
					for (int i = 0; i < array.Length; i++)
					{
						array[i] = multiParameterList.Parameters[i].Value;
					}
					return array;
				}
				return null;
			}
		}

		public override string ToString()
		{
			return ExpressionTree.ToString();
		}

		public ExpressionDelegate ToDelegate(params string[] aParamOrder)
		{
			List<Parameter> list = new List<Parameter>(aParamOrder.Length);
			for (int i = 0; i < aParamOrder.Length; i++)
			{
				if (Parameters.ContainsKey(aParamOrder[i]))
				{
					list.Add(Parameters[aParamOrder[i]]);
				}
				else
				{
					list.Add(null);
				}
			}
			Parameter[] parameters2 = list.ToArray();
			return (double[] p) => Invoke(p, parameters2);
		}

		public MultiResultDelegate ToMultiResultDelegate(params string[] aParamOrder)
		{
			List<Parameter> list = new List<Parameter>(aParamOrder.Length);
			for (int i = 0; i < aParamOrder.Length; i++)
			{
				if (Parameters.ContainsKey(aParamOrder[i]))
				{
					list.Add(Parameters[aParamOrder[i]]);
				}
				else
				{
					list.Add(null);
				}
			}
			Parameter[] parameters2 = list.ToArray();
			return (double[] p) => InvokeMultiResult(p, parameters2);
		}

		private double Invoke(double[] aParams, Parameter[] aParamList)
		{
			int num = Math.Min(aParamList.Length, aParams.Length);
			for (int i = 0; i < num; i++)
			{
				if (aParamList[i] != null)
				{
					aParamList[i].Value = aParams[i];
				}
			}
			return Value;
		}

		private double[] InvokeMultiResult(double[] aParams, Parameter[] aParamList)
		{
			int num = Math.Min(aParamList.Length, aParams.Length);
			for (int i = 0; i < num; i++)
			{
				if (aParamList[i] != null)
				{
					aParamList[i].Value = aParams[i];
				}
			}
			return MultiValue;
		}

		public static Expression Parse(string aExpression)
		{
			return new ExpressionParser().EvaluateExpression(aExpression);
		}
	}
	public delegate double ExpressionDelegate(params double[] aParams);
	public delegate double[] MultiResultDelegate(params double[] aParams);
	public class ExpressionParser
	{
		public class ParseException : Exception
		{
			public ParseException(string aMessage)
				: base(aMessage)
			{
			}
		}

		private List<string> m_BracketHeap = new List<string>();

		private Dictionary<string, Func<double>> m_Consts = new Dictionary<string, Func<double>>();

		private Dictionary<string, Func<double[], double>> m_Funcs = new Dictionary<string, Func<double[], double>>();

		private Expression m_Context;

		public ExpressionParser()
		{
			Random rnd = new Random();
			m_Consts.Add("PI", () => Math.PI);
			m_Consts.Add("e", () => Math.E);
			m_Funcs.Add("sqrt", (double[] p) => Math.Sqrt(p.FirstOrDefault()));
			m_Funcs.Add("abs", (double[] p) => Math.Abs(p.FirstOrDefault()));
			m_Funcs.Add("ln", (double[] p) => Math.Log(p.FirstOrDefault()));
			m_Funcs.Add("floor", (double[] p) => Math.Floor(p.FirstOrDefault()));
			m_Funcs.Add("ceiling", (double[] p) => Math.Ceiling(p.FirstOrDefault()));
			m_Funcs.Add("round", (double[] p) => Math.Round(p.FirstOrDefault()));
			m_Funcs.Add("sin", (double[] p) => Math.Sin(p.FirstOrDefault()));
			m_Funcs.Add("cos", (double[] p) => Math.Cos(p.FirstOrDefault()));
			m_Funcs.Add("tan", (double[] p) => Math.Tan(p.FirstOrDefault()));
			m_Funcs.Add("asin", (double[] p) => Math.Asin(p.FirstOrDefault()));
			m_Funcs.Add("acos", (double[] p) => Math.Acos(p.FirstOrDefault()));
			m_Funcs.Add("atan", (double[] p) => Math.Atan(p.FirstOrDefault()));
			m_Funcs.Add("atan2", (double[] p) => Math.Atan2(p.FirstOrDefault(), p.ElementAtOrDefault(1)));
			m_Funcs.Add("min", (double[] p) => Math.Min(p.FirstOrDefault(), p.ElementAtOrDefault(1)));
			m_Funcs.Add("max", (double[] p) => Math.Max(p.FirstOrDefault(), p.ElementAtOrDefault(1)));
			m_Funcs.Add("rnd", delegate(double[] p)
			{
				if (p.Length == 2)
				{
					return p[0] + rnd.NextDouble() * (p[1] - p[0]);
				}
				return (p.Length == 1) ? (rnd.NextDouble() * p[0]) : rnd.NextDouble();
			});
		}

		public void AddFunc(string aName, Func<double[], double> aMethod)
		{
			if (m_Funcs.ContainsKey(aName))
			{
				m_Funcs[aName] = aMethod;
			}
			else
			{
				m_Funcs.Add(aName, aMethod);
			}
		}

		public void AddConst(string aName, Func<double> aMethod)
		{
			if (m_Consts.ContainsKey(aName))
			{
				m_Consts[aName] = aMethod;
			}
			else
			{
				m_Consts.Add(aName, aMethod);
			}
		}

		public void RemoveFunc(string aName)
		{
			if (m_Funcs.ContainsKey(aName))
			{
				m_Funcs.Remove(aName);
			}
		}

		public void RemoveConst(string aName)
		{
			if (m_Consts.ContainsKey(aName))
			{
				m_Consts.Remove(aName);
			}
		}

		private int FindClosingBracket(ref string aText, int aStart, char aOpen, char aClose)
		{
			int num = 0;
			for (int i = aStart; i < aText.Length; i++)
			{
				if (aText[i] == aOpen)
				{
					num++;
				}
				if (aText[i] == aClose)
				{
					num--;
				}
				if (num == 0)
				{
					return i;
				}
			}
			return -1;
		}

		private void SubstitudeBracket(ref string aExpression, int aIndex)
		{
			int num = FindClosingBracket(ref aExpression, aIndex, '(', ')');
			if (num > aIndex + 1)
			{
				string item = aExpression.Substring(aIndex + 1, num - aIndex - 1);
				m_BracketHeap.Add(item);
				string text = "&" + (m_BracketHeap.Count - 1) + ";";
				aExpression = aExpression.Substring(0, aIndex) + text + aExpression.Substring(num + 1);
				return;
			}
			throw new ParseException("Bracket not closed!");
		}

		private IValue Parse(string aExpression)
		{
			aExpression = aExpression.Trim();
			for (int num = aExpression.IndexOf('('); num >= 0; num = aExpression.IndexOf('('))
			{
				SubstitudeBracket(ref aExpression, num);
			}
			if (Enumerable.Contains(aExpression, ','))
			{
				string[] array = aExpression.Split(new char[1] { ',' });
				List<IValue> list = new List<IValue>(array.Length);
				for (int i = 0; i < array.Length; i++)
				{
					string text = array[i].Trim();
					if (!string.IsNullOrEmpty(text))
					{
						list.Add(Parse(text));
					}
				}
				return new MultiParameterList(list.ToArray());
			}
			if (Enumerable.Contains(aExpression, '+'))
			{
				string[] array2 = aExpression.Split(new char[1] { '+' });
				List<IValue> list2 = new List<IValue>(array2.Length);
				for (int j = 0; j < array2.Length; j++)
				{
					string text2 = array2[j].Trim();
					if (!string.IsNullOrEmpty(text2))
					{
						list2.Add(Parse(text2));
					}
				}
				if (list2.Count == 1)
				{
					return list2[0];
				}
				return new OperationSum(list2.ToArray());
			}
			if (Enumerable.Contains(aExpression, '-'))
			{
				string[] array3 = aExpression.Split(new char[1] { '-' });
				List<IValue> list3 = new List<IValue>(array3.Length);
				if (!string.IsNullOrEmpty(array3[0].Trim()))
				{
					list3.Add(Parse(array3[0]));
				}
				for (int k = 1; k < array3.Length; k++)
				{
					string text3 = array3[k].Trim();
					if (!string.IsNullOrEmpty(text3))
					{
						list3.Add(new OperationNegate(Parse(text3)));
					}
				}
				if (list3.Count == 1)
				{
					return list3[0];
				}
				return new OperationSum(list3.ToArray());
			}
			if (Enumerable.Contains(aExpression, '*'))
			{
				string[] array4 = aExpression.Split(new char[1] { '*' });
				List<IValue> list4 = new List<IValue>(array4.Length);
				for (int l = 0; l < array4.Length; l++)
				{
					list4.Add(Parse(array4[l]));
				}
				if (list4.Count == 1)
				{
					return list4[0];
				}
				return new OperationProduct(list4.ToArray());
			}
			if (Enumerable.Contains(aExpression, '/'))
			{
				string[] array5 = aExpression.Split(new char[1] { '/' });
				List<IValue> list5 = new List<IValue>(array5.Length);
				if (!string.IsNullOrEmpty(array5[0].Trim()))
				{
					list5.Add(Parse(array5[0]));
				}
				for (int m = 1; m < array5.Length; m++)
				{
					string text4 = array5[m].Trim();
					if (!string.IsNullOrEmpty(text4))
					{
						list5.Add(new OperationReciprocal(Parse(text4)));
					}
				}
				return new OperationProduct(list5.ToArray());
			}
			if (Enumerable.Contains(aExpression, '^'))
			{
				int num2 = aExpression.IndexOf('^');
				IValue aValue = Parse(aExpression.Substring(0, num2));
				IValue aPower = Parse(aExpression.Substring(num2 + 1));
				return new OperationPower(aValue, aPower);
			}
			int num3 = aExpression.IndexOf("&");
			if (num3 > 0)
			{
				string text5 = aExpression.Substring(0, num3);
				foreach (KeyValuePair<string, Func<double[], double>> func in m_Funcs)
				{
					if (text5 == func.Key)
					{
						string aExpression2 = aExpression.Substring(func.Key.Length);
						IValue value = Parse(aExpression2);
						return new CustomFunction(aValues: (!(value is MultiParameterList multiParameterList)) ? new IValue[1] { value } : multiParameterList.Parameters, aName: func.Key, aDelegate: func.Value);
					}
				}
			}
			foreach (KeyValuePair<string, Func<double>> C in m_Consts)
			{
				if (aExpression == C.Key)
				{
					return new CustomFunction(C.Key, (Func<double[], double>)((double[] p) => C.Value()), (IValue[])null);
				}
			}
			int num4 = aExpression.IndexOf('&');
			int num5 = aExpression.IndexOf(';');
			if (num4 >= 0 && num5 >= 2)
			{
				if (int.TryParse(aExpression.Substring(num4 + 1, num5 - num4 - 1), out var result) && result >= 0 && result < m_BracketHeap.Count)
				{
					return Parse(m_BracketHeap[result]);
				}
				throw new ParseException("Can't parse substitude token");
			}
			if (double.TryParse(aExpression, out var result2))
			{
				return new Number(result2);
			}
			if (ValidIdentifier(aExpression))
			{
				if (m_Context.Parameters.ContainsKey(aExpression))
				{
					return m_Context.Parameters[aExpression];
				}
				Parameter parameter = new Parameter(aExpression);
				m_Context.Parameters.Add(aExpression, parameter);
				return parameter;
			}
