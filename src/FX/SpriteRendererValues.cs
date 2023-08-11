using UnityEngine;

namespace FX;

public struct SpriteRendererValues
{
	public static readonly SpriteRendererValues @default = new SpriteRendererValues(Effects.sprite.spriteRenderer);

	public Sprite sprite;

	public Color color;

	public bool flipX;

	public bool flipY;

	public Material sharedMaterial;

	public SpriteDrawMode drawMode;

	public int sortingLayerID;

	public int sortingOrder;

	public SpriteMaskInteraction maskInteraction;

	public SpriteSortPoint spriteSortPoint;

	public uint renderingLayerMask;

	public SpriteRendererValues(Sprite sprite, Color color, bool flipX, bool flipY, Material sharedMaterial, SpriteDrawMode drawMode, int sortingLayerID, int sortingOrder, SpriteMaskInteraction maskInteraction, SpriteSortPoint spriteSortPoint, uint renderingLayerMask)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		this.sprite = sprite;
		this.color = color;
		this.flipX = flipX;
		this.flipY = flipY;
		this.sharedMaterial = sharedMaterial;
		this.drawMode = drawMode;
		this.sortingLayerID = sortingLayerID;
		this.sortingOrder = sortingOrder;
		this.maskInteraction = maskInteraction;
		this.spriteSortPoint = spriteSortPoint;
		this.renderingLayerMask = renderingLayerMask;
	}

	public SpriteRendererValues(SpriteRenderer from)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		sprite = from.sprite;
		color = from.color;
		flipX = from.flipX;
		flipY = from.flipY;
		sharedMaterial = ((Renderer)from).sharedMaterial;
		drawMode = from.drawMode;
		sortingLayerID = ((Renderer)from).sortingLayerID;
		sortingOrder = ((Renderer)from).sortingOrder;
		maskInteraction = from.maskInteraction;
		spriteSortPoint = from.spriteSortPoint;
		renderingLayerMask = ((Renderer)from).renderingLayerMask;
	}
}
