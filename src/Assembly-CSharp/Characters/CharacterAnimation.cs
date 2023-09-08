using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Characters;

[RequireComponent(typeof(Animator))]
public class CharacterAnimation : MonoBehaviour
{
	public class Parameter
	{
		private readonly Animator animator;

		public readonly AnimatorBool walk;

		public readonly AnimatorBool grounded;

		public readonly AnimatorFloat movementSpeed;

		public readonly AnimatorFloat ySpeed;

		public readonly AnimatorFloat actionSpeed;

		internal Parameter(Animator animator)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Expected O, but got Unknown
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Expected O, but got Unknown
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Expected O, but got Unknown
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Expected O, but got Unknown
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Expected O, but got Unknown
			this.animator = animator;
			walk = new AnimatorBool(animator, "Walk");
			movementSpeed = new AnimatorFloat(animator, "MovementSpeed");
			ySpeed = new AnimatorFloat(animator, "YSpeed");
			actionSpeed = new AnimatorFloat(animator, "ActionSpeed");
			grounded = new AnimatorBool(animator, "Grounded");
		}
	}

	public const string idleClipName = "EmptyIdle";

	public const string walkClipName = "EmptyWalk";

	public const string jumpUpClipName = "EmptyJumpUp";

	public const string fallClipName = "EmptyJumpDown";

	public const string fallRepeatClipName = "EmptyJumpDownLoop";

	public const string actionClipName = "EmptyAction";

	public static readonly AnimatorParameter action = new AnimatorParameter("Action");

	public static readonly AnimatorParameter idle = new AnimatorParameter("Idle");

	public static readonly AnimatorParameter ground = new AnimatorParameter("Ground");

	public static readonly AnimatorParameter air = new AnimatorParameter("Air");

	[GetComponent]
	[SerializeField]
	protected Animator _animator;

	protected AnimationClipOverrider _defaultOverrider;

	[GetComponent]
	[SerializeField]
	protected SpriteRenderer _spriteRenderer;

	[SerializeField]
	[CharacterAnimationController.Key]
	private string _key;

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

	private AnimationClip _actionClip;

	private float _cycleOffset;

	private readonly List<AnimationClipOverrider> _overriders = new List<AnimationClipOverrider>();

	public AnimationClip walkClip => _walkClip;

	public Parameter parameter { get; protected set; }

	public float speed
	{
		get
		{
			return _animator.speed;
		}
		set
		{
			_animator.speed = value;
		}
	}

	public string key => _key;

	public SpriteRenderer spriteRenderer => _spriteRenderer;

	public AnimatorParameter state
	{
		get
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			AnimatorStateInfo currentAnimatorStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
			int tagHash = ((AnimatorStateInfo)(ref currentAnimatorStateInfo)).tagHash;
			if (tagHash == action.hash)
			{
				return action;
			}
			if (tagHash == ground.hash)
			{
				return ground;
			}
			if (tagHash == air.hash)
			{
				return air;
			}
			return null;
		}
	}

	private void OnDestroy()
	{
		if (_defaultOverrider != null)
		{
			_defaultOverrider.Dispose();
			_defaultOverrider = null;
		}
		foreach (AnimationClipOverrider overrider in _overriders)
		{
			overrider.Dispose();
		}
		_overriders.Clear();
		_idleClip = null;
		_walkClip = null;
		_jumpClip = null;
		_fallClip = null;
		_fallRepeatClip = null;
		_actionClip = null;
		_animator = null;
		if ((Object)(object)_spriteRenderer != (Object)null)
		{
			_spriteRenderer.sprite = null;
		}
		_spriteRenderer = null;
	}

	public void Initialize()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		if (_defaultOverrider == null)
		{
			_defaultOverrider = new AnimationClipOverrider(_animator.runtimeAnimatorController);
			AttachOverrider(_defaultOverrider);
		}
		_defaultOverrider.Override("EmptyIdle", _idleClip);
		_defaultOverrider.Override("EmptyWalk", _walkClip);
		_defaultOverrider.Override("EmptyJumpUp", _jumpClip);
		_defaultOverrider.Override("EmptyJumpDown", _fallClip);
		_defaultOverrider.Override("EmptyJumpDownLoop", _fallRepeatClip);
		parameter = new Parameter(_animator);
	}

	public void Play(AnimationClip clip, float speed)
	{
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected O, but got Unknown
		_actionClip = clip;
		_overriders.Last().Override("EmptyAction", clip);
		AnimationEvent val = ((IEnumerable<AnimationEvent>)clip.events).SingleOrDefault((Func<AnimationEvent, bool>)((AnimationEvent @event) => @event.functionName.Equals("CycleOffset")));
		AnimationEvent val2 = ((IEnumerable<AnimationEvent>)clip.events).SingleOrDefault((Func<AnimationEvent, bool>)((AnimationEvent @event) => @event.functionName.Equals("Repeat")));
		if (val == null)
		{
			_cycleOffset = 0f;
		}
		else
		{
			if (val2 == null)
			{
				AnimationEvent val3 = new AnimationEvent();
				val3.functionName = "Repeat";
				val3.time = clip.length;
				clip.AddEvent(val3);
			}
			_cycleOffset = val.time / clip.length;
		}
		Play(speed);
	}

	public void Play(float speed)
	{
		((AnimatorVariable<float>)(object)parameter.actionSpeed).Value = speed;
		Play();
	}

	public void Play()
	{
		_animator.Play(action.hash, 0, 0f);
	}

	public void Stun()
	{
		Play(_idleClip, 1.5f);
	}

	public void Stop()
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		if (((Behaviour)_animator).isActiveAndEnabled)
		{
			AnimatorStateInfo currentAnimatorStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
			if (((AnimatorStateInfo)(ref currentAnimatorStateInfo)).shortNameHash == action.hash)
			{
				_animator.Play(idle.hash, 0, 0f);
			}
		}
	}

	public void Repeat()
	{
		_animator.Play(action.hash, 0, _cycleOffset);
	}

	public void CycleOffset()
	{
	}

	public void SetIdle(AnimationClip clip)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		if (_defaultOverrider == null)
		{
			_defaultOverrider = new AnimationClipOverrider(_animator.runtimeAnimatorController);
			AttachOverrider(_defaultOverrider);
		}
		_defaultOverrider.Override("EmptyIdle", clip);
	}

	public void SetWalk(AnimationClip clip)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		if (_defaultOverrider == null)
		{
			_defaultOverrider = new AnimationClipOverrider(_animator.runtimeAnimatorController);
			AttachOverrider(_defaultOverrider);
		}
		_defaultOverrider.Override("EmptyWalk", clip);
	}

	public void AttachOverrider(AnimationClipOverrider overrider)
	{
		if (!_overriders.Contains(overrider))
		{
			_overriders.Add(overrider);
			_animator.runtimeAnimatorController = (RuntimeAnimatorController)(object)_overriders.Last().animatorController;
		}
	}

	public void DetachOverrider(AnimationClipOverrider overrider)
	{
		_overriders.Remove(overrider);
		_animator.runtimeAnimatorController = (RuntimeAnimatorController)(object)_overriders.Last().animatorController;
	}
}
