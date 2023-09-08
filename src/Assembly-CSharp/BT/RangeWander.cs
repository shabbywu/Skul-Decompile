using UnityEngine;

namespace BT;

public class RangeWander : Node
{
	private static readonly int _idleHash = Animator.StringToHash("Idle");

	private static readonly int _walkHash = Animator.StringToHash("Walk");

	[SerializeField]
	private SpriteRenderer _spriteRenderer;

	[SerializeField]
	private Animator _animator;

	[SerializeField]
	private CustomFloat _speed;

	[MinMaxSlider(0f, 20f)]
	[SerializeField]
	private Vector2 _idleTime;

	[SerializeField]
	[MinMaxSlider(0f, 20f)]
	private Vector2 _wanderTime;

	[SerializeField]
	private Collider2D _customRange;

	private const float _groundFindingRayDistance = 9f;

	private const float _minDistanceFromSide = 2f;

	private bool _isWandering;

	private float _remainTime;

	private Vector2 _direction;

	private float _speedValue;

	private Collider2D _range;

	protected override void OnInitialize()
	{
		Initialize();
		base.OnInitialize();
	}

	protected override NodeState UpdateDeltatime(Context context)
	{
		Transform val = context.Get<Transform>(Key.OwnerTransform);
		if ((Object)(object)val == (Object)null)
		{
			Debug.LogError((object)"OwnerTransform is null");
			return NodeState.Fail;
		}
		float deltaTime = context.deltaTime;
		if (_isWandering)
		{
			Wander(val, deltaTime);
		}
		_remainTime -= deltaTime;
		if (_remainTime <= 0f)
		{
			Initialize();
		}
		return NodeState.Success;
	}

	private void Initialize()
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		_speedValue = _speed.value;
		_isWandering = MMMaths.RandomBool();
		if ((Object)(object)_customRange == (Object)null)
		{
			RaycastHit2D val = Physics2D.Raycast(Vector2.op_Implicit(((Component)this).transform.position), Vector2.down, 9f, LayerMask.op_Implicit(Layers.groundMask));
			if (RaycastHit2D.op_Implicit(val))
			{
				_range = ((RaycastHit2D)(ref val)).collider;
			}
		}
		else
		{
			_range = _customRange;
		}
		if (_isWandering)
		{
			OnStartWander();
		}
		else
		{
			OnStartIdle();
		}
	}

	private void Wander(Transform owner, float deltaTime)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		Flip(owner);
		if (_direction.x > 0f)
		{
			_spriteRenderer.flipX = false;
		}
		else
		{
			_spriteRenderer.flipX = true;
		}
		owner.Translate(Vector2.op_Implicit(_direction * deltaTime * _speedValue));
	}

	private void OnStartWander()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		_direction = (MMMaths.RandomBool() ? Vector2.right : Vector2.left);
		_remainTime = Random.Range(_wanderTime.x, _wanderTime.y);
		_animator.Play(_walkHash);
	}

	private void OnStartIdle()
	{
		_remainTime = Random.Range(_idleTime.x, _idleTime.y);
		_animator.Play(_idleHash);
	}

	private void Flip(Transform owner)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		Bounds bounds = _range.bounds;
		if (_direction == Vector2.right && ((Bounds)(ref bounds)).max.x - 2f < ((Component)owner).transform.position.x)
		{
			_direction = Vector2.left;
		}
		if (_direction == Vector2.left && ((Bounds)(ref bounds)).min.x + 2f > ((Component)owner).transform.position.x)
		{
			_direction = Vector2.right;
		}
	}
}
