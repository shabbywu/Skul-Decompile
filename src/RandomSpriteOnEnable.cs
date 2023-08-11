using System.Collections.Generic;
using UnityEngine;

public class RandomSpriteOnEnable : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer _spriteRenderer;

	[SerializeField]
	private Sprite[] _sprites;

	private void OnEnable()
	{
		_spriteRenderer.sprite = ExtensionMethods.Random<Sprite>((IEnumerable<Sprite>)_sprites);
	}
}
