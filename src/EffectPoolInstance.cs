using System;
using FX;
using UnityEngine;

public sealed class EffectPoolInstance : MonoBehaviour
{
	public enum State
	{
		Stopped,
		Delaying,
		Playing,
		Fading,
		Destroyed
	}

	[GetComponent]
	[SerializeField]
	private SpriteRenderer _renderer;

	[GetComponent]
	[SerializeField]
	private Animator _animator;

	private MaterialPropertyBlock _propertyBlock;

	private float _delay;

	private float _duration;

	private float _fadingTime;

	private float _fadingDuration;

	private AnimationCurve _fadingCurve;

	private float _initialFadeAlpha;

	public State state { get; private set; }

	public SpriteRenderer renderer => _renderer;

	public Animator animator => _animator;

	public ChronometerBase chronometer { get; set; }

	public float delay { get; set; }

	public int hue
	{
		set
		{
			((Renderer)renderer).GetPropertyBlock(_propertyBlock);
			_propertyBlock.SetInt(EffectInfo.huePropertyID, value);
			((Renderer)renderer).SetPropertyBlock(_propertyBlock);
		}
	}

	public event Action OnStop;

	private void Awake()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		_propertyBlock = new MaterialPropertyBlock();
	}

	private void OnDestroy()
	{
		_renderer.sprite = null;
		_renderer = null;
		_animator.runtimeAnimatorController = null;
		_animator = null;
		this.OnStop = null;
		state = State.Destroyed;
	}

	public void Copy(ReusableChronoSpriteEffect to)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		to.animator.runtimeAnimatorController = animator.runtimeAnimatorController;
		to.animator.speed = animator.speed;
		to.animator.updateMode = animator.updateMode;
		to.animator.cullingMode = animator.cullingMode;
		to.renderer.sprite = renderer.sprite;
		to.renderer.color = renderer.color;
		to.renderer.flipX = renderer.flipX;
		to.renderer.flipY = renderer.flipY;
		to.renderer.drawMode = renderer.drawMode;
		((Renderer)to.renderer).sortingLayerID = ((Renderer)renderer).sortingLayerID;
		((Renderer)to.renderer).sortingOrder = ((Renderer)renderer).sortingOrder;
		to.renderer.maskInteraction = renderer.maskInteraction;
		to.renderer.spriteSortPoint = renderer.spriteSortPoint;
		((Renderer)to.renderer).renderingLayerMask = ((Renderer)renderer).renderingLayerMask;
	}

	public void Play(RuntimeAnimatorController animation, float delay, float duration, bool loop, AnimationCurve fadeOutCurve, float fadeOutDuration)
	{
		if ((Object)(object)animation == (Object)null)
		{
			Debug.LogError((object)"Animation of effect is null!");
			Stop();
			return;
		}
		if (fadeOutCurve == null)
		{
			fadeOutCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
		}
		_animator.runtimeAnimatorController = animation;
		_delay = delay;
		if (loop)
		{
			_duration = float.PositiveInfinity;
		}
		else
		{
			_duration = duration;
		}
		_fadingTime = 0f;
		_fadingCurve = fadeOutCurve;
		_fadingDuration = fadeOutDuration;
		if (_delay > 0f)
		{
			state = State.Delaying;
		}
		else
		{
			StartPlay();
		}
	}

	public void UpdateEffect()
	{
		float deltaTime = ChronometerExtension.DeltaTime(chronometer);
		switch (state)
		{
		case State.Delaying:
			ProcessDelay(deltaTime);
			break;
		case State.Playing:
			ProcessPlay(deltaTime);
			break;
		case State.Fading:
			ProcessFade(deltaTime);
			break;
		}
	}

	private void ProcessDelay(float deltaTime)
	{
		((Renderer)_renderer).enabled = false;
		_delay -= deltaTime;
		if (!(_delay > 0f))
		{
			StartPlay();
		}
	}

	private void StartPlay()
	{
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)_renderer == (Object)null)
		{
			Debug.LogError((object)"Renderer is null!");
			Stop();
			return;
		}
		if ((Object)(object)_animator == (Object)null)
		{
			Debug.LogError((object)"Animator is null!");
			Stop();
			return;
		}
		state = State.Playing;
		if ((Object)(object)_animator.runtimeAnimatorController != (Object)null)
		{
			if (!((Behaviour)_animator).enabled)
			{
				((Behaviour)_animator).enabled = true;
			}
			_animator.Play(0, 0, 0f);
			_animator.Update(0f);
			if (_duration == 0f)
			{
				AnimatorStateInfo currentAnimatorStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
				_duration = ((AnimatorStateInfo)(ref currentAnimatorStateInfo)).length;
			}
		}
		_duration -= _fadingDuration;
		((Behaviour)_animator).enabled = false;
	}

	private void ProcessPlay(float deltaTime)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		_duration -= deltaTime;
		_animator.Update(deltaTime);
		if (!(_duration > 0f))
		{
			if (_fadingDuration > 0f)
			{
				_initialFadeAlpha = _renderer.color.a;
				state = State.Fading;
			}
			else
			{
				Stop();
			}
		}
	}

	private void ProcessFade(float deltaTime)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		if (_fadingTime > _fadingDuration)
		{
			Stop();
			return;
		}
		_fadingTime += deltaTime;
		_animator.Update(deltaTime);
		Color color = _renderer.color;
		color.a = _initialFadeAlpha * (1f - _fadingCurve.Evaluate(_fadingTime / _fadingDuration));
		_renderer.color = color;
	}

	public void Stop()
	{
		if ((Object)(object)((Component)this).transform.parent != (Object)null)
		{
			((Component)this).transform.SetParent((Transform)null, false);
		}
		((Renderer)_renderer).enabled = false;
		((Behaviour)_animator).enabled = false;
		_renderer.sprite = null;
		_animator.runtimeAnimatorController = null;
		state = State.Stopped;
		this.OnStop?.Invoke();
	}
}
