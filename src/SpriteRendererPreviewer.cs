using System.Collections.Generic;
using UnityEngine;

public class SpriteRendererPreviewer : MonoBehaviour
{
	[SerializeField]
	private Transform _container;

	[SerializeField]
	private List<SpriteRenderer> _sprites = new List<SpriteRenderer>();

	private const string _containerName = "Preview";

	private void Awake()
	{
		if ((Object)(object)_container != (Object)null)
		{
			Object.Destroy((Object)(object)((Component)_container).gameObject);
		}
	}

	public void Clear()
	{
		if (!((Object)(object)_container == (Object)null))
		{
			Object.DestroyImmediate((Object)(object)((Component)_container).gameObject);
		}
	}

	public void Show(SpriteRenderer[] spriteRenderers)
	{
		InitializeContainer();
		RemoveDuplicatedContainer();
		ClampSpriteRenderers(spriteRenderers.Length);
		((Component)_container).gameObject.SetActive(true);
		for (int i = 0; i < spriteRenderers.Length; i++)
		{
			CopySpriteRenderer(spriteRenderers[i], _sprites[i]);
		}
	}

	public void Hide()
	{
		if (!((Object)(object)_container == (Object)null))
		{
			((Component)_container).gameObject.SetActive(false);
		}
	}

	public void FlipX(bool flip)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		if (!flip)
		{
			_container.localScale = Vector3.one;
		}
		else
		{
			_container.localScale = new Vector3(-1f, 1f, 1f);
		}
	}

	private void InitializeContainer()
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)_container != (Object)null))
		{
			_container = new GameObject("Preview").transform;
			_container.parent = ((Component)this).transform;
			_container.localPosition = Vector3.zero;
		}
	}

	private void RemoveDuplicatedContainer()
	{
		if (((Component)this).transform.childCount <= 0)
		{
			return;
		}
		for (int num = ((Component)this).transform.childCount - 1; num >= 0; num--)
		{
			Transform child = ((Component)this).transform.GetChild(num);
			if ((Object)(object)child != (Object)(object)_container && ((Object)child).name.CompareTo("Preview") == 0)
			{
				Object.DestroyImmediate((Object)(object)((Component)child).gameObject);
			}
		}
	}

	private void ClampSpriteRenderers(int count)
	{
		for (int num = _sprites.Count - 1; num >= 0; num--)
		{
			if ((Object)(object)_sprites[num] == (Object)null)
			{
				_sprites.Remove(_sprites[num]);
			}
		}
		for (int i = _sprites.Count; i < count; i++)
		{
			CreateNewSpriteRenderer($"[{i}]");
		}
		for (int j = count; j < _sprites.Count; j++)
		{
			Object.DestroyImmediate((Object)(object)((Component)_sprites[j]).gameObject);
		}
		for (int num2 = _container.childCount - 1; num2 >= 0; num2--)
		{
			Transform child = _container.GetChild(num2);
			SpriteRenderer component = ((Component)child).GetComponent<SpriteRenderer>();
			if ((Object)(object)component == (Object)null || !_sprites.Contains(component))
			{
				Object.DestroyImmediate((Object)(object)((Component)child).gameObject);
			}
		}
	}

	private void CreateNewSpriteRenderer(string name)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		SpriteRenderer val = new GameObject(name).AddComponent<SpriteRenderer>();
		((Component)val).transform.parent = _container;
		((Component)val).transform.localScale = Vector3.one;
		_sprites.Add(val);
	}

	private void CopySpriteRenderer(SpriteRenderer targetInfo, SpriteRenderer sprite)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		sprite.sprite = targetInfo.sprite;
		sprite.color = targetInfo.color;
		sprite.flipX = targetInfo.flipX;
		((Renderer)sprite).sortingLayerID = ((Renderer)targetInfo).sortingLayerID;
		((Renderer)sprite).sortingOrder = ((Renderer)targetInfo).sortingOrder;
		((Component)sprite).transform.localPosition = ((Component)targetInfo).transform.position;
	}
}
