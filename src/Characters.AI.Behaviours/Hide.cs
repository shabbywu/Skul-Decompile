using System.Collections;
using UnityEngine;

namespace Characters.AI.Behaviours;

public class Hide : Behaviour
{
	[SerializeField]
	private SpriteRenderer _spriteRenderer;

	[SerializeField]
	private Collider2D _collider2D;

	[SerializeField]
	[MinMaxSlider(0f, 10f)]
	private Vector2 _duration;

	public override IEnumerator CRun(AIController controller)
	{
		float num = Random.Range(_duration.x, _duration.y);
		((Renderer)_spriteRenderer).enabled = false;
		if ((Object)(object)_collider2D != (Object)null)
		{
			((Behaviour)_collider2D).enabled = false;
		}
		controller.character.attach.SetActive(false);
		yield return ChronometerExtension.WaitForSeconds((ChronometerBase)(object)controller.character.chronometer.master, num);
		((Renderer)_spriteRenderer).enabled = true;
		if ((Object)(object)_collider2D != (Object)null)
		{
			((Behaviour)_collider2D).enabled = true;
		}
		controller.character.attach.SetActive(true);
	}
}
