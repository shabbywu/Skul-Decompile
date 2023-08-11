using Singletons;
using UnityEngine;

namespace FX;

public class FreezeSprites : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer _front;

	[SerializeField]
	private SpriteRenderer _back;

	[SerializeField]
	private Vector2Int _size;

	[SerializeField]
	protected SoundInfo _freezeSound;

	public Vector2Int size => _size;

	public void Initialize(SpriteRenderer spriteRenderer, int multiplier)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		SetLayer(((Renderer)spriteRenderer).sortingLayerID, ((Renderer)spriteRenderer).sortingOrder);
		((Component)this).transform.localScale = Vector3.one * (float)multiplier;
	}

	public void SetLayer(int sortingLayerID, int sortingOrder)
	{
		((Renderer)_front).sortingLayerID = sortingLayerID;
		((Renderer)_front).sortingOrder = sortingOrder + 1;
		((Renderer)_back).sortingLayerID = sortingLayerID;
		((Renderer)_back).sortingOrder = sortingOrder - 1;
	}

	public void Enable()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		((Component)this).gameObject.SetActive(true);
		PersistentSingleton<SoundManager>.Instance.PlaySound(_freezeSound, ((Component)this).transform.position);
	}

	public void Disable()
	{
		((Component)this).gameObject.SetActive(false);
	}
}
