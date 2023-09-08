using System.Collections;
using FX;
using UnityEngine;

namespace Characters.Projectiles.Operations;

public class Stuck : HitOperation
{
	[SerializeField]
	[Information("0일 경우 삭제 되지 않음", InformationAttribute.InformationType.Info, false)]
	private float _lifeTime;

	[SerializeField]
	private SpriteRenderer _spriteRenderer;

	[SerializeField]
	private Sprite _spriteToReplace;

	[SerializeField]
	private EffectInfo _despawnEffect;

	[SerializeField]
	private Vector2 _despawnEffectSpawnOffset;

	public override void Run(IProjectile projectile, RaycastHit2D raycastHit)
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
			((MonoBehaviour)effect.poolObject).StartCoroutine(Despawn(effect));
		}
	}

	private IEnumerator Despawn(Effects.SpritePoolObject effect)
	{
		yield return Chronometer.global.WaitForSeconds(_lifeTime);
		if (_despawnEffect != null)
		{
			Vector2 val = default(Vector2);
			((Vector2)(ref val))._002Ector(((Component)effect.poolObject).transform.position.x + _despawnEffectSpawnOffset.x, ((Component)effect.poolObject).transform.position.y + _despawnEffectSpawnOffset.y);
			_despawnEffect.Spawn(Vector2.op_Implicit(val));
		}
		effect.poolObject.Despawn();
	}
}
