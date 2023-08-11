using FX;
using UnityEngine;

public class ApplyEffectLayerOrder : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer _spriteRenderer;

	private void Start()
	{
		((Renderer)_spriteRenderer).sortingOrder = Effects.GetSortingOrderAndIncrease();
	}
}
