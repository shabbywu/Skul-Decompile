using UnityEngine;

public class SpriteRandomizer : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer _spriteRenderer;

	[SerializeField]
	private Sprite[] _sprites;

	private void Awake()
	{
		_spriteRenderer.sprite = _sprites.Random();
	}
}
