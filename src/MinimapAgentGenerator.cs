using FX;
using GameResources;
using UnityEngine;

public class MinimapAgentGenerator : MonoBehaviour
{
	[SerializeField]
	private Collider2D _collider;

	[SerializeField]
	private Color _color;

	[SerializeField]
	[HideInInspector]
	private bool _generated;

	private float _pixelPerUnit;

	private float _width;

	private float _height;

	private void Awake()
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		if (_generated)
		{
			Object.Destroy((Object)(object)this);
			return;
		}
		_generated = true;
		SpriteRenderer mainRenderer = ((Component)this).gameObject.GetComponentInParent<SpriteEffectStack>().mainRenderer;
		Generate(((Component)this).gameObject, _collider.bounds, _color, mainRenderer);
		Object.Destroy((Object)(object)this);
	}

	public static SpriteRenderer Generate(GameObject gameObject, Bounds bounds, Color color, SpriteRenderer spriteRenderer)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		return Generate(gameObject, bounds, color, ((Renderer)spriteRenderer).sortingLayerID, ((Renderer)spriteRenderer).sortingOrder);
	}

	public static SpriteRenderer Generate(GameObject gameObject, Bounds bounds, Color color, int sortingLayerID, int sortingOrder)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		Sprite pixelSprite = CommonResource.instance.pixelSprite;
		gameObject.transform.position = ((Bounds)(ref bounds)).center;
		float num = ((Bounds)(ref bounds)).size.x * pixelSprite.pixelsPerUnit;
		Rect rect = pixelSprite.rect;
		float num2 = num / ((Rect)(ref rect)).width;
		float num3 = ((Bounds)(ref bounds)).size.y * pixelSprite.pixelsPerUnit;
		rect = pixelSprite.rect;
		Vector2 val = default(Vector2);
		((Vector2)(ref val))._002Ector(num2, num3 / ((Rect)(ref rect)).height);
		gameObject.transform.localScale = Vector2.op_Implicit(val);
		gameObject.layer = 25;
		SpriteRenderer obj = gameObject.AddComponent<SpriteRenderer>();
		((Renderer)obj).sharedMaterial = MaterialResource.minimap;
		obj.sprite = pixelSprite;
		((Renderer)obj).sortingLayerID = sortingLayerID;
		((Renderer)obj).sortingOrder = sortingOrder;
		obj.color = color;
		return obj;
	}
}
