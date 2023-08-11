using UnityEngine;

namespace Tutorial;

public class Message : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer _spriteRenderer;

	public void Activate()
	{
		((Renderer)_spriteRenderer).enabled = true;
	}

	public void Deactivate()
	{
		((Renderer)_spriteRenderer).enabled = false;
	}
}
