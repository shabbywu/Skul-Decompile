using System.Collections;
using Characters;
using Characters.AI;
using Characters.AI.Behaviours;
using Characters.Abilities;
using Characters.Actions;
using PhysicsUtils;
using UnityEditor;
using UnityEngine;

namespace Level.Traps;

public class TentacleAI : AIController
{
	[SerializeField]
	private SpriteRenderer _corpseRenderer;

	[SerializeField]
	private CharacterAnimation _animation;

	[Subcomponent(typeof(CheckWithinSight))]
	[SerializeField]
	private CheckWithinSight _checkWithinSight;

	[Space]
	[Header("Appearance")]
	[SerializeField]
	private Action _appearance;

	[SerializeField]
	[Header("Attack")]
	[Space]
	private Action _attackAction;

	[SerializeField]
	private Collider2D _attackTrigger;

	[Subcomponent(typeof(Idle))]
	[SerializeField]
	private Idle _idle;

	[Space]
	[SerializeField]
	[AbilityAttacher.Subcomponent]
	private AbilityAttacher _abilityAttacher;

	private float _elapsedTime;

	private readonly NonAllocOverlapper _overlapper = new NonAllocOverlapper(1);

	private void Awake()
	{
		_attackAction.Initialize(character);
		_appearance.Initialize(character);
		_abilityAttacher.Initialize(character);
		_abilityAttacher.StartAttach();
	}

	private new void OnEnable()
	{
		base.OnEnable();
		_elapsedTime = 0f;
		((Component)this).transform.parent = ((Component)Map.Instance).transform;
		((MonoBehaviour)this).StartCoroutine(_checkWithinSight.CRun(this));
		((MonoBehaviour)this).StartCoroutine(CProcess());
	}

	public void Appear(Transform point, Sprite corpse, bool flip)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		_corpseRenderer.sprite = corpse;
		if (flip)
		{
			_corpseRenderer.flipX = true;
		}
		character.health.onDied += delegate
		{
			((Component)_corpseRenderer).transform.SetParent(((Component)Map.Instance).transform);
			((Renderer)_corpseRenderer).sortingOrder = character.sortingGroup.sortingOrder;
		};
		((Component)this).transform.position = point.position;
		((Component)this).gameObject.SetActive(true);
	}

	public void Hide()
	{
		((Component)this).gameObject.SetActive(false);
	}

	private void FindPlayer()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		((Behaviour)_attackTrigger).enabled = true;
		((ContactFilter2D)(ref _overlapper.contactFilter)).SetLayerMask(LayerMask.op_Implicit(512));
		_overlapper.OverlapCollider(_attackTrigger);
		((Behaviour)_attackTrigger).enabled = false;
	}

	private void OnDestroy()
	{
		_abilityAttacher.StopAttach();
	}

	protected override IEnumerator CProcess()
	{
		yield return null;
		_appearance.TryStart();
		while (_appearance.running)
		{
			yield return null;
		}
		while (!base.dead)
		{
			if ((Object)(object)base.target == (Object)null)
			{
				yield return null;
				continue;
			}
			do
			{
				yield return null;
				FindPlayer();
			}
			while (_overlapper.results.Count == 0);
			_attackAction.TryStart();
			while (_attackAction.running)
			{
				yield return null;
			}
			yield return _idle.CRun(this);
		}
	}
}
