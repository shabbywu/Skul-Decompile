using System.Collections;
using GameResources;
using UnityEngine;

namespace FX;

public static class Effects
{
	public struct SpritePoolObject
	{
		public PoolObject poolObject;

		public SpriteRenderer spriteRenderer;

		public SpritePoolObject(PoolObject poolObject)
		{
			this.poolObject = poolObject;
			spriteRenderer = ((Component)poolObject).GetComponent<SpriteRenderer>();
		}

		public SpritePoolObject(PoolObject poolObject, SpriteRenderer spriteRenderer)
		{
			this.poolObject = poolObject;
			this.spriteRenderer = spriteRenderer;
		}

		public SpritePoolObject Spawn()
		{
			return new SpritePoolObject(poolObject.Spawn(true));
		}

		public SpritePoolObject Spawn(SpriteRendererValues spriteRendererValues)
		{
			SpritePoolObject result = Spawn();
			result.spriteRenderer.CopyFrom(spriteRenderer);
			return result;
		}

		public SpritePoolObject Spawn(SpriteRenderer spriteRenderer)
		{
			SpritePoolObject result = Spawn();
			result.spriteRenderer.CopyFrom(spriteRenderer);
			return result;
		}

		public void FadeOut(Chronometer chronometer, AnimationCurve curve, float duration)
		{
			poolObject.FadeOut(spriteRenderer, (ChronometerBase)chronometer, curve, duration);
		}
	}

	private static short _sortingOrder = short.MinValue;

	public static readonly SpritePoolObject sprite = new SpritePoolObject(CommonResource.instance.emptyEffect);

	public static short GetSortingOrderAndIncrease()
	{
		return _sortingOrder++;
	}

	public static IEnumerator CFadeOut(this PoolObject poolObject, SpriteRenderer spriteRenderer, ChronometerBase chronometer, AnimationCurve curve, float duration)
	{
		float t = 0f;
		Color color = spriteRenderer.color;
		float alpha = color.a;
		float multiplier = 1f / duration;
		while (t < 1f)
		{
			yield return null;
			t += chronometer.DeltaTime() * multiplier;
			color.a = alpha * (1f - curve.Evaluate(t));
			spriteRenderer.color = color;
		}
		poolObject.Despawn();
	}

	public static IEnumerator CFadeOut(this PoolObject poolObject, SpriteRenderer[] spriteRenderers, ChronometerBase chronometer, AnimationCurve curve, float duration)
	{
		float t = 0f;
		Color[] colorArray = (Color[])(object)new Color[spriteRenderers.Length];
		float[] alphaArray = new float[spriteRenderers.Length];
		for (int i = 0; i < colorArray.Length; i++)
		{
			if (!((Object)(object)spriteRenderers[i] == (Object)null))
			{
				colorArray[i] = spriteRenderers[i].color;
				alphaArray[i] = colorArray[i].a;
			}
		}
		float multiplier = 1f / duration;
		while (t < 1f)
		{
			yield return null;
			t += chronometer.DeltaTime() * multiplier;
			for (int j = 0; j < colorArray.Length; j++)
			{
				colorArray[j].a = alphaArray[j] * (1f - curve.Evaluate(t));
				spriteRenderers[j].color = colorArray[j];
			}
		}
		poolObject.Despawn();
	}

	public static void FadeOut(this PoolObject poolObject, SpriteRenderer spriteRenderer, ChronometerBase chronometer, AnimationCurve curve, float duration)
	{
		((MonoBehaviour)poolObject).StartCoroutine(poolObject.CFadeOut(spriteRenderer, chronometer, curve, duration));
	}

	public static void CopyFrom(this SpriteRenderer spriteRenderer, SpriteRenderer from)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		spriteRenderer.sprite = from.sprite;
		spriteRenderer.color = from.color;
		spriteRenderer.flipX = from.flipX;
		spriteRenderer.flipY = from.flipY;
		((Renderer)spriteRenderer).sharedMaterial = ((Renderer)from).sharedMaterial;
		spriteRenderer.drawMode = from.drawMode;
		((Renderer)spriteRenderer).sortingLayerID = ((Renderer)from).sortingLayerID;
		((Renderer)spriteRenderer).sortingOrder = ((Renderer)from).sortingOrder;
		spriteRenderer.maskInteraction = from.maskInteraction;
		spriteRenderer.spriteSortPoint = from.spriteSortPoint;
		((Renderer)spriteRenderer).renderingLayerMask = ((Renderer)from).renderingLayerMask;
	}

	public static void CopyFrom(this SpriteRenderer spriteRenderer, SpriteRendererValues from)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		spriteRenderer.sprite = from.sprite;
		spriteRenderer.color = from.color;
		spriteRenderer.flipX = from.flipX;
		spriteRenderer.flipY = from.flipY;
		((Renderer)spriteRenderer).sharedMaterial = from.sharedMaterial;
		spriteRenderer.drawMode = from.drawMode;
		((Renderer)spriteRenderer).sortingLayerID = from.sortingLayerID;
		((Renderer)spriteRenderer).sortingOrder = from.sortingOrder;
		spriteRenderer.maskInteraction = from.maskInteraction;
		spriteRenderer.spriteSortPoint = from.spriteSortPoint;
		((Renderer)spriteRenderer).renderingLayerMask = from.renderingLayerMask;
	}
}
