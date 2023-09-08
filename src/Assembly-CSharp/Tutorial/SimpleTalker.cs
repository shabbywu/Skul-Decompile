using UnityEngine;

namespace Tutorial;

public sealed class SimpleTalker : NPC
{
	[SerializeField]
	private SpriteRenderer _spriteRenderer;

	protected override void Activate()
	{
		((Renderer)_spriteRenderer).enabled = true;
	}

	protected override void Deactivate()
	{
	}
}
