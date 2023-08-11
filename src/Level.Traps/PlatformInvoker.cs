using Characters;
using PhysicsUtils;
using Runnables;
using UnityEngine;

namespace Level.Traps;

public class PlatformInvoker : MonoBehaviour
{
	private static readonly NonAllocOverlapper _lapper;

	[SerializeField]
	private LayerMask _targetLayer;

	[SerializeField]
	private Collider2D _platformCollider;

	private Bounds _platformUpperBounds;

	[SerializeField]
	private Runnable _takeOnPlatform;

	[SerializeField]
	private Runnable _takeOffPlatform;

	private bool _isExecuted;

	static PlatformInvoker()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		_lapper = new NonAllocOverlapper(15);
	}

	private void Start()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		_platformUpperBounds = _platformCollider.bounds;
		((Bounds)(ref _platformUpperBounds)).center = Vector2.op_Implicit(new Vector2(((Bounds)(ref _platformUpperBounds)).center.x, ((Bounds)(ref _platformUpperBounds)).center.y + 0.1f));
		((ContactFilter2D)(ref _lapper.contactFilter)).SetLayerMask(_targetLayer);
	}

	private void Update()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		ReadonlyBoundedList<Collider2D> results = _lapper.OverlapArea(_platformUpperBounds).results;
		if (results.Count <= 0)
		{
			if (_isExecuted)
			{
				_takeOffPlatform.Run();
				_isExecuted = false;
			}
			return;
		}
		foreach (Collider2D item in results)
		{
			if (((Component)(object)item).TryFindCharacterComponent(out var character) && SteppedOn(character) && !_isExecuted)
			{
				_takeOnPlatform.Run();
				_isExecuted = true;
			}
		}
	}

	private bool SteppedOn(Character character)
	{
		if ((Object)(object)character.movement.controller.collisionState.lastStandingCollider != (Object)(object)_platformCollider)
		{
			return false;
		}
		if (!character.movement.isGrounded)
		{
			return false;
		}
		return true;
	}
}
