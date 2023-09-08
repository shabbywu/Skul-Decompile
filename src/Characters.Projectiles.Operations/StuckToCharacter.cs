using System.Collections;
using FX;
using UnityEngine;

namespace Characters.Projectiles.Operations;

public class StuckToCharacter : CharacterHitOperation
{
	[Information("0일 경우 삭제 되지 않음", InformationAttribute.InformationType.Info, false)]
	[SerializeField]
	private float _lifeTime;

	[SerializeField]
	private SpriteRenderer _spriteRenderer;

	[SerializeField]
	private Sprite _spriteToReplace;

	[SerializeField]
	private EffectInfo _despawnEffect;

	[SerializeField]
	private Vector2 _despawnEffectSpawnOffset;

	private IEnumerator CDespawn(Effects.SpritePoolObject effect, Character target)
	{
		for (float elapsed = 0f; elapsed < _lifeTime; elapsed += Chronometer.global.deltaTime)
		{
			if (target.health.dead)
			{
				break;
			}
			yield return null;
		}
		if (_despawnEffect != null)
		{
			Vector2 val = default(Vector2);
			((Vector2)(ref val))._002Ector(((Component)effect.poolObject).transform.position.x + _despawnEffectSpawnOffset.x, ((Component)effect.poolObject).transform.position.y + _despawnEffectSpawnOffset.y);
			_despawnEffect.Spawn(Vector2.op_Implicit(val));
		}
		effect.poolObject.Despawn();
	}

	private IEnumerator CSetPosiiton(Transform effect, Collider2D hitCollider, Character target)
	{
		Bounds before = hitCollider.bounds;
		Transform transform = ((Component)effect).transform;
		Bounds bounds = hitCollider.bounds;
		transform.position = ((Bounds)(ref bounds)).center;
		while (((Component)effect).gameObject.activeInHierarchy && !target.health.dead)
		{
			yield return null;
			if (before != hitCollider.bounds)
			{
				Transform transform2 = ((Component)effect).transform;
				bounds = hitCollider.bounds;
				transform2.position = ((Bounds)(ref bounds)).center;
				before = hitCollider.bounds;
			}
		}
	}

	public override void Run(IProjectile projectile, RaycastHit2D raycastHit, Character character)
	{
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)_spriteRenderer == (Object)null)
		{
			_spriteRenderer = projectile.GetComponentInParent<SpriteRenderer>();
		}
		if (!((Object)(object)_spriteRenderer == (Object)null))
		{
			Effects.SpritePoolObject effect = Effects.sprite.Spawn();
			effect.spriteRenderer.CopyFrom(_spriteRenderer);
			if ((Object)(object)_spriteToReplace != (Object)null)
			{
				effect.spriteRenderer.sprite = _spriteToReplace;
			}
			SpriteRenderer spriteRenderer = effect.spriteRenderer;
			int sortingOrder = ((Renderer)spriteRenderer).sortingOrder;
			((Renderer)spriteRenderer).sortingOrder = sortingOrder - 1;
			effect.spriteRenderer.color = _spriteRenderer.color;
			Transform transform = ((Component)effect.poolObject).transform;
			transform.position = Vector2.op_Implicit(((RaycastHit2D)(ref raycastHit)).point);
			transform.localScale = ((Component)_spriteRenderer).transform.lossyScale;
			transform.rotation = ((Component)_spriteRenderer).transform.rotation;
			((Component)effect.poolObject).transform.SetParent(character.attachWithFlip.transform);
			((MonoBehaviour)effect.poolObject).StartCoroutine(CDespawn(effect, character));
			((MonoBehaviour)effect.poolObject).StartCoroutine(CSetPosiiton(((Component)effect.poolObject).transform, ((RaycastHit2D)(ref raycastHit)).collider, character));
		}
	}
}
