using System.Collections;
using System.Collections.Generic;
using Characters.Projectiles;
using FX;
using UnityEngine;

public class RandomSpriteInBounds : MonoBehaviour
{
	[SerializeField]
	private Projectile _projectile;

	[SerializeField]
	private Transform _origin;

	[SerializeField]
	private EffectInfo _spawnEffect;

	[SerializeField]
	private Sprite[] _sprites;

	[SerializeField]
	private CustomFloat _countRange;

	[SerializeField]
	private BoxCollider2D _range;

	[SerializeField]
	private Transform[] _positions;

	private SpriteRenderer[] _spriteRenderers;

	private IProjectile _iProjectile;

	private void Awake()
	{
		if ((Object)(object)_projectile == (Object)null)
		{
			_iProjectile = ((Component)this).GetComponentInParent<IProjectile>();
		}
		else
		{
			_iProjectile = _projectile;
		}
		_spriteRenderers = (SpriteRenderer[])(object)new SpriteRenderer[_positions.Length];
		for (int i = 0; i < _positions.Length; i++)
		{
			_spriteRenderers[i] = ((Component)_positions[i]).GetComponent<SpriteRenderer>();
		}
	}

	private void OnEnable()
	{
		DeactiveAll();
		int count = (int)_countRange.value;
		Activate(count);
		((MonoBehaviour)this).StartCoroutine(CEffects(count));
	}

	private void Activate(int count)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < count; i++)
		{
			Vector2 val = Random.insideUnitCircle * 2f;
			_positions[i].position = Vector2.op_Implicit(Vector2.op_Implicit(_origin.position) + val);
			_spriteRenderers[i].sprite = ExtensionMethods.Random<Sprite>((IEnumerable<Sprite>)_sprites);
			((Renderer)_spriteRenderers[i]).enabled = true;
		}
	}

	private IEnumerator CEffects(int count)
	{
		yield return null;
		for (int i = 0; i < count; i++)
		{
			_spawnEffect.Spawn(_positions[i].position, _iProjectile.owner);
		}
	}

	private void DeactiveAll()
	{
		for (int i = 0; i < _positions.Length; i++)
		{
			((Renderer)_spriteRenderers[i]).enabled = false;
		}
	}
}
