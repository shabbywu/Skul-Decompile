using System.Collections;
using FX;
using UnityEngine;

public sealed class ReusableChronoSpriteEffect : MonoBehaviour, IUseChronometer, IDelayable, IPoolObjectCopiable<ReusableChronoSpriteEffect>
{
	[SerializeField]
	private PoolObject _reusable;

	[GetComponent]
	[SerializeField]
	private SpriteRenderer _renderer;

	[GetComponent]
	[SerializeField]
	private Animator _animator;

	private MaterialPropertyBlock _propertyBlock;

	private CoroutineReference _cPlayReference;

	public PoolObject reusable => _reusable;

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

	private void Awake()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		_propertyBlock = new MaterialPropertyBlock();
		_reusable.onDespawn += OnDespawn;
	}

	private void OnDespawn()
	{
		((CoroutineReference)(ref _cPlayReference)).Stop();
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

	public void Play(float delay, float duration, bool loop, AnimationCurve fadeOutCurve, float fadeOutDuration)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		_cPlayReference = CoroutineReferenceExtension.StartCoroutineWithReference((MonoBehaviour)(object)CoroutineProxy.instance, CPlay(delay, duration, loop, fadeOutCurve, fadeOutDuration));
	}

	private IEnumerator CPlay(float delay, float duration, bool loop, AnimationCurve fadeOutCurve, float fadeOutDuration)
	{
		float remain2 = delay;
		while (remain2 > float.Epsilon)
		{
			((Renderer)_renderer).enabled = false;
			yield return null;
			float num = ChronometerExtension.DeltaTime(chronometer);
			remain2 -= num;
		}
		((Renderer)_renderer).enabled = true;
		if ((Object)(object)_animator.runtimeAnimatorController != (Object)null)
		{
			if (!((Behaviour)_animator).enabled)
			{
				((Behaviour)_animator).enabled = true;
			}
			_animator.Play(0, 0, 0f);
		}
		if (loop)
		{
			yield break;
		}
		if (duration == 0f)
		{
			AnimatorStateInfo currentAnimatorStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
			duration = ((AnimatorStateInfo)(ref currentAnimatorStateInfo)).length;
		}
		duration -= fadeOutDuration;
		if (duration <= 0f)
		{
			Debug.LogError((object)"Duration - Fade out duration이 0이하입니다.");
			((CoroutineReference)(ref _cPlayReference)).Clear();
			_reusable.Despawn();
			yield break;
		}
		remain2 += duration;
		((Behaviour)_animator).enabled = false;
		while (remain2 > float.Epsilon)
		{
			yield return null;
			float num2 = ChronometerExtension.DeltaTime(chronometer);
			_animator.Update(num2);
			remain2 -= num2;
		}
		if (fadeOutDuration > 0f)
		{
			yield return CFadeOut(0f - remain2, fadeOutDuration, fadeOutCurve);
		}
		((Behaviour)_animator).enabled = true;
		((CoroutineReference)(ref _cPlayReference)).Clear();
		_reusable.Despawn();
	}

	private IEnumerator CFadeOut(float time, float duration, AnimationCurve fadeOutCurve)
	{
		Color color = _renderer.color;
		float alpha = color.a;
		while (time < duration)
		{
			yield return null;
			float num = ChronometerExtension.DeltaTime(chronometer);
			time += num;
			_animator.Update(num);
			color.a = alpha * (1f - fadeOutCurve.Evaluate(time / duration));
			_renderer.color = color;
		}
	}
}
