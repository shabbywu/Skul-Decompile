using System.Collections;
using Characters.Gear.Weapons;
using UnityEngine;

namespace Level;

public sealed class GraveDiggerGrave : MonoBehaviour
{
	private static short _sortingOrder;

	private static readonly int _activatedParameterHash = Animator.StringToHash("Activated");

	[GetComponent]
	[SerializeField]
	private PoolObject _poolObject;

	[SerializeField]
	private Animator _animator;

	[SerializeField]
	private SpriteRenderer _spriteRenderer;

	[SerializeField]
	[Space]
	private float _lifeTime;

	private float _elapsed;

	private GraveDiggerGraveContainer _container;

	public bool activated { get; private set; }

	public Vector3 position => ((Component)this).transform.position;

	public void Spawn(Vector3 position, GraveDiggerGraveContainer container)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		RaycastHit2D val = Physics2D.Raycast(Vector2.op_Implicit(position), Vector2.down, 10f, LayerMask.op_Implicit(Layers.groundMask));
		if (RaycastHit2D.op_Implicit(val) && !((Object)(object)Physics2D.OverlapPoint(Vector2.op_Implicit(position), LayerMask.op_Implicit(Layers.groundMask)) != (Object)null))
		{
			GraveDiggerGrave component = ((Component)_poolObject.Spawn(Vector2.op_Implicit(((RaycastHit2D)(ref val)).point), true)).GetComponent<GraveDiggerGrave>();
			component.Initialize(container);
			((MonoBehaviour)component).StartCoroutine(component.CLifeSpan());
		}
	}

	public void Despawn()
	{
		_poolObject.Despawn();
	}

	public void Activate()
	{
		if (!activated)
		{
			activated = true;
			_animator.SetBool(_activatedParameterHash, true);
		}
	}

	public void Deactivate()
	{
		if (activated)
		{
			activated = false;
			_animator.SetBool(_activatedParameterHash, false);
		}
	}

	private void Initialize(GraveDiggerGraveContainer container)
	{
		_elapsed = 0f;
		_container = container;
		Deactivate();
		_container.Add(this);
		((Renderer)_spriteRenderer).sortingOrder = _sortingOrder++;
	}

	private IEnumerator CLifeSpan()
	{
		while (_elapsed < _lifeTime)
		{
			_elapsed += ((ChronometerBase)Chronometer.global).deltaTime;
			yield return null;
		}
		_container.Remove(this);
		Despawn();
	}
}
