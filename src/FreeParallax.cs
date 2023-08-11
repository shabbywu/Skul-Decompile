using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeParallax : MonoBehaviour
{
	[Tooltip("Camera to use for the parallax. Defaults to main camera.")]
	public Camera parallaxCamera;

	[Tooltip("The speed at which the parallax moves, which will likely be opposite from the speed at which your character moves. Elements can be set to move as a percentage of this value.")]
	public Vector2 Speed;

	[Tooltip("Randomize position on initialize")]
	public bool randomize = true;

	[Tooltip("The elements in the parallax.")]
	public List<FreeParallaxElement> Elements;

	[Tooltip("Whether the parallax moves horizontally or vertically. Horizontal moves left and right, vertical moves up and down.")]
	public bool IsHorizontal = true;

	[Tooltip("The overlap in world units for wrapping elements. This can help fix rare one pixel gaps.")]
	public float WrapOverlap;

	public CameraController cameraController;

	private float _originHeight;

	private SpriteRenderer[] _spriteRenderers;

	private void Awake()
	{
		_spriteRenderers = ((Component)this).GetComponentsInChildren<SpriteRenderer>();
	}

	private void SetFadeAlpha(float t)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		SpriteRenderer[] spriteRenderers = _spriteRenderers;
		Color color2 = default(Color);
		foreach (SpriteRenderer obj in spriteRenderers)
		{
			Color color = obj.color;
			((Color)(ref color2))._002Ector(color.r, color.g, color.b, t);
			obj.color = color2;
		}
	}

	public void FadeIn()
	{
		((MonoBehaviour)this).StartCoroutine(CFadeIn());
	}

	public IEnumerator CFadeIn()
	{
		float t = 0f;
		SetFadeAlpha(1f);
		yield return null;
		for (; t < 1f; t += Time.unscaledDeltaTime * 0.3f)
		{
			SetFadeAlpha(1f - t);
			yield return null;
		}
		SetFadeAlpha(0f);
	}

	public void FadeOut()
	{
		((MonoBehaviour)this).StartCoroutine(CFadeOut());
	}

	public IEnumerator CFadeOut()
	{
		float t = 0f;
		SetFadeAlpha(0f);
		yield return null;
		for (; t < 1f; t += Time.unscaledDeltaTime * 0.3f)
		{
			SetFadeAlpha(t);
			yield return null;
		}
		SetFadeAlpha(1f);
	}

	private void SetupElementAtIndex(int i)
	{
		FreeParallaxElement freeParallaxElement = Elements[i];
		if (freeParallaxElement.GameObjects == null || freeParallaxElement.GameObjects.Count == 0)
		{
			Debug.LogError((object)("No game objects found at element index " + i + ", be sure to set at least one game object for each element in the parallax"));
			return;
		}
		foreach (GameObject gameObject in freeParallaxElement.GameObjects)
		{
			if ((Object)(object)gameObject == (Object)null)
			{
				Debug.LogError((object)("Null game object found at element index " + i));
				return;
			}
		}
		freeParallaxElement.SetupState(this, parallaxCamera, i);
		freeParallaxElement.SetupScale(this, parallaxCamera, i);
		freeParallaxElement.SetupPosition(this, parallaxCamera, i);
	}

	public void Reset()
	{
		SetupElements(randomize: false);
	}

	public void SetupElements(bool randomize)
	{
		if ((Object)(object)parallaxCamera == (Object)null)
		{
			parallaxCamera = Camera.main;
			if ((Object)(object)parallaxCamera == (Object)null)
			{
				Debug.LogError((object)"Cannot run parallax without a camera");
				return;
			}
		}
		if (Elements == null || Elements.Count == 0)
		{
			return;
		}
		for (int i = 0; i < Elements.Count; i++)
		{
			SetupElementAtIndex(i);
			if (randomize)
			{
				Elements[i].Randomize(this, parallaxCamera);
			}
		}
	}

	public void AddElement(FreeParallaxElement e)
	{
		if (Elements == null)
		{
			Elements = new List<FreeParallaxElement>();
		}
		int count = Elements.Count;
		Elements.Add(e);
		SetupElementAtIndex(count);
	}

	public static void SetPosition(GameObject obj, Renderer r, float x, float y)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		Vector3 position = default(Vector3);
		((Vector3)(ref position))._002Ector(x, y, obj.transform.position.z);
		obj.transform.position = position;
		Bounds bounds = r.bounds;
		float num = ((Bounds)(ref bounds)).min.x - obj.transform.position.x;
		if (num != 0f)
		{
			position.x -= num;
			obj.transform.position = position;
		}
		bounds = r.bounds;
		float num2 = ((Bounds)(ref bounds)).min.y - obj.transform.position.y;
		if (num2 != 0f)
		{
			position.y -= num2;
			obj.transform.position = position;
		}
	}

	public void Initialize(float originHeight)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		_originHeight = originHeight;
		SetupElements(randomize);
		Translate(new Vector2(0f, 0f - originHeight));
	}

	public void Translate(Vector2 delta)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < Elements.Count; i++)
		{
			Elements[i].Update(this, delta, parallaxCamera);
		}
	}

	private void Randomize()
	{
		for (int i = 0; i < Elements.Count; i++)
		{
			Elements[i].Randomize(this, parallaxCamera);
		}
	}

	private void Update()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		Vector3 delta = cameraController.delta;
		delta.x *= Speed.x;
		delta.y *= Speed.y;
		foreach (FreeParallaxElement element in Elements)
		{
			element.Update(this, Vector2.op_Implicit(delta), parallaxCamera);
		}
	}
}
