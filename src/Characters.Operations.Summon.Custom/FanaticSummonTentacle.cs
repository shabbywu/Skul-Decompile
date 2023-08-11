using System;
using Level;
using UnityEngine;

namespace Characters.Operations.Summon.Custom;

[Serializable]
public class FanaticSummonTentacle : IBDCharacterSetting
{
	[SerializeField]
	private Character _summoner;

	[SerializeField]
	private Sprite _corpseSprite;

	public void ApplyTo(Character character)
	{
		SpriteRenderer corpseSpriteRenderer = character.attach.GetComponentInChildren<SpriteRenderer>();
		corpseSpriteRenderer.sprite = _corpseSprite;
		corpseSpriteRenderer.flipX = _summoner.lookingDirection == Character.LookingDirection.Left;
		character.health.onDied += delegate
		{
			((Component)corpseSpriteRenderer).transform.SetParent(((Component)Map.Instance).transform);
			((Renderer)corpseSpriteRenderer).sortingOrder = character.sortingGroup.sortingOrder;
		};
	}
}
