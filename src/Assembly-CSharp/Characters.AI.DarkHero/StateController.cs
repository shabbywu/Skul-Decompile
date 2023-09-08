using BehaviorDesigner.Runtime;
using Characters.Movements;
using Runnables;
using UnityEngine;

namespace Characters.AI.DarkHero;

public sealed class StateController : MonoBehaviour
{
	public enum State
	{
		None,
		Ground,
		Wall
	}

	[SerializeField]
	private Character _owner;

	[SerializeField]
	private BehaviorTree _tree;

	[SerializeField]
	private BehaviorDesignerCommunicator _behaviorTreeCommunicator;

	[SerializeField]
	private CharacterAnimation _characterAnimation;

	[SerializeField]
	private RuntimeAnimatorController _baseAnimator;

	[SerializeField]
	private AnimationClip _idleClip;

	[SerializeField]
	private AnimationClip _walkClip;

	[SerializeField]
	private AnimationClip _jumpClip;

	[SerializeField]
	private AnimationClip _fallClip;

	[SerializeField]
	private AnimationClip _fallRepeatClip;

	[SerializeField]
	private Movement.Config _inWallMovementConfig;

	[Runnable.Subcomponent]
	[SerializeField]
	private Runnable.Subcomponents _onEnterGroundState;

	[Runnable.Subcomponent]
	[SerializeField]
	private Runnable.Subcomponents _onExitGroundState;

	[Runnable.Subcomponent]
	[SerializeField]
	private Runnable.Subcomponents _onEnterWallState;

	[Runnable.Subcomponent]
	[SerializeField]
	private Runnable.Subcomponents _onExitWallState;

	private AnimationClipOverrider _overrider;

	private State _lastState;

	[SerializeField]
	private int[] _stepHealthConditions;

	private int _nextStep;

	private readonly string stepName = "Step";

	private void Awake()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		if (_overrider == null)
		{
			_overrider = new AnimationClipOverrider(_baseAnimator);
			_overrider.Override("EmptyIdle", _idleClip);
			_overrider.Override("EmptyWalk", _walkClip);
			_overrider.Override("EmptyJumpUp", _jumpClip);
			_overrider.Override("EmptyJumpDown", _fallClip);
			_overrider.Override("EmptyJumpDownLoop", _fallRepeatClip);
		}
	}

	private void Update()
	{
		UpdateStep();
	}

	private void UpdateStep()
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Invalid comparison between Unknown and I4
		if (!((Object)(object)_owner == (Object)null) && _nextStep < _stepHealthConditions.Length && (int)((Behavior)_tree).ExecutionStatus == 3 && _owner.health.percent * 100.0 <= (double)_stepHealthConditions[_nextStep])
		{
			_nextStep++;
			_behaviorTreeCommunicator.SetVariable<SharedInt>(stepName, (object)_nextStep);
		}
	}

	private void Start()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		_owner.movement.controller.lastStandingMask = Layers.footholdMask;
	}

	private void OnDestroy()
	{
		_overrider.Dispose();
		_baseAnimator = null;
		_idleClip = null;
		_walkClip = null;
		_jumpClip = null;
		_fallClip = null;
		_fallRepeatClip = null;
	}

	public void OnEnterGroundState()
	{
		if (_lastState != State.Ground)
		{
			_lastState = State.Ground;
			_characterAnimation.DetachOverrider(_overrider);
			_owner.movement.configs.Remove(_inWallMovementConfig);
			_onEnterGroundState.Run();
		}
	}

	public void OnExitGroundState()
	{
		_onExitGroundState.Run();
	}

	public void OnEnterWallState()
	{
		if (_lastState != State.Wall)
		{
			_lastState = State.Wall;
			_onEnterWallState.Run();
			_characterAnimation.AttachOverrider(_overrider);
			_owner.movement.configs.Add(int.MaxValue, _inWallMovementConfig);
		}
	}

	public void OnExitWallState()
	{
		_onExitWallState.Run();
	}
}
