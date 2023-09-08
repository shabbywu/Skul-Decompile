using System.Collections;
using Characters.Actions;
using UnityEditor;
using UnityEngine;

namespace Characters.AI;

public sealed class SpiritBottle : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer _spriteRenderer;

	[SerializeField]
	private Character _character;

	[Subcomponent(true, typeof(SimpleAction))]
	[SerializeField]
	private SimpleAction _onDied;

	private void Start()
	{
		_character.health.onDied += OnDied;
	}

	private void OnDied()
	{
		((Behaviour)_character.collider).enabled = false;
		((Component)_character).gameObject.SetActive(true);
		((Renderer)_spriteRenderer).enabled = true;
		_onDied?.TryStart();
		_character.health.onDied -= OnDied;
		((MonoBehaviour)this).StartCoroutine(CDestroy());
		IEnumerator CDestroy()
		{
			while (_onDied.running)
			{
				yield return null;
			}
			Object.Destroy((Object)(object)((Component)_character).gameObject);
		}
	}
}
