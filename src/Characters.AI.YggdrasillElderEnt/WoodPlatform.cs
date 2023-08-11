using UnityEngine;

namespace Characters.AI.YggdrasillElderEnt;

public class WoodPlatform : MonoBehaviour
{
	[GetComponent]
	[SerializeField]
	private Animator _animator;

	[SerializeField]
	[GetComponent]
	private Collider2D _collider;

	[GetComponent]
	[SerializeField]
	private SpriteRenderer _spriteRenderer;

	private bool _spawned;

	private void Awake()
	{
		_spriteRenderer.sprite = null;
		((Behaviour)_collider).enabled = false;
		_animator.Play("Empty");
	}

	public void Spawn()
	{
		_animator.Play("Appearance");
		((Behaviour)_collider).enabled = true;
	}

	public void Despawn()
	{
		_animator.Play("Disappearance");
		((Behaviour)_collider).enabled = false;
	}
}
