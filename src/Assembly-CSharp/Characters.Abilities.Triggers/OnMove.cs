using System;
using UnityEngine;

namespace Characters.Abilities.Triggers;

[Serializable]
public class OnMove : Trigger
{
	private Character _character;

	[SerializeField]
	private float _distanceToTrigger = 1f;

	[SerializeField]
	private float _horizontalMultiplier = 1f;

	[SerializeField]
	private float _verticalMultiplier;

	private float _distance;

	public override void Attach(Character character)
	{
		_character = character;
		_character.movement.onMoved += OnMoved;
	}

	public override void Detach()
	{
		_character.movement.onMoved -= OnMoved;
	}

	private void OnMoved(Vector2 distance)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		_distance += Mathf.Abs(distance.x) * _horizontalMultiplier + Mathf.Abs(distance.y) * _verticalMultiplier;
		if (_distance > _distanceToTrigger)
		{
			_distance -= _distanceToTrigger;
			Invoke();
		}
	}
}
