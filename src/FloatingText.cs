using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour
{
	private const float duration = 0.5f;

	private static readonly WaitForSeconds waitForDuration = new WaitForSeconds(0.5f);

	[SerializeField]
	[GetComponent]
	private PoolObject _poolObject;

	[SerializeField]
	private TextMeshPro _text;

	public PoolObject poolObject => _poolObject;

	public Color color
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return ((Graphic)_text).color;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((Graphic)_text).color = value;
		}
	}

	public string text
	{
		get
		{
			return ((TMP_Text)_text).text;
		}
		set
		{
			((TMP_Text)_text).text = value;
		}
	}

	public int sortingOrder
	{
		get
		{
			return _text.sortingOrder;
		}
		set
		{
			_text.sortingOrder = value;
		}
	}

	private void Awake()
	{
		Object.DontDestroyOnLoad((Object)(object)((Component)this).gameObject);
	}

	public void SetGradientColor(Color a, Color b)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		((Graphic)_text).color = Color.white;
		((TMP_Text)_text).colorGradient = new VertexGradient(a, a, b, b);
	}

	public FloatingText Spawn()
	{
		return ((Component)_poolObject.Spawn(true)).GetComponent<FloatingText>();
	}

	public void Despawn()
	{
		_poolObject.Despawn();
	}

	public void Despawn(float seconds)
	{
		((MonoBehaviour)this).StartCoroutine(CDespawn(seconds));
	}

	private IEnumerator CDespawn(float seconds)
	{
		yield return Chronometer.global.WaitForSeconds(seconds);
		Despawn();
	}

	public void FadeOut(float duration)
	{
		((MonoBehaviour)this).StartCoroutine(CFadeOut(duration));
	}

	private IEnumerator CFadeOut(float duration)
	{
		float t = duration;
		SetFadeAlpha(1f);
		yield return null;
		while (t > 0f)
		{
			SetFadeAlpha(t / duration);
			yield return null;
			t -= ((ChronometerBase)Chronometer.global).deltaTime;
		}
		SetFadeAlpha(0f);
	}

	private void SetFadeAlpha(float alpha)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		Color val = color;
		val.a = alpha;
		color = val;
	}

	public void Initialize(string text, Vector3 position)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		((MonoBehaviour)this).StopAllCoroutines();
		((TMP_Text)_text).text = text;
		((Component)this).transform.position = position;
		((Component)this).transform.localScale = Vector3.one;
		((TMP_Text)_text).colorGradient = new VertexGradient(Color.white, Color.white, Color.white, Color.white);
		((Component)this).gameObject.SetActive(true);
	}

	public void Initialize(string text, Vector3 position, Color color)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		Initialize(text, position);
		((Graphic)_text).color = color;
	}
}
