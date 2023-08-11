using UnityEngine;

namespace Runnables.Triggers;

public class SpritePlayed : Trigger
{
	[SerializeField]
	private SpriteRenderer _spriteRenderer;

	[SerializeField]
	private Sprite[] _sprites;

	private bool _playedThisSprite;

	protected override bool Check()
	{
		Sprite[] sprites = _sprites;
		foreach (Sprite val in sprites)
		{
			if ((Object)(object)_spriteRenderer.sprite == (Object)(object)val)
			{
				if (_playedThisSprite)
				{
					return false;
				}
				_playedThisSprite = true;
				return true;
			}
		}
		_playedThisSprite = false;
		return false;
	}

	private void OnDestroy()
	{
		for (int i = 0; i < _sprites.Length; i++)
		{
			_sprites[i] = null;
		}
	}
}
