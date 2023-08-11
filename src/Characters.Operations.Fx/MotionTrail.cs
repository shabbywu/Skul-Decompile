using System.Collections;
using FX;
using GameResources;
using UnityEngine;

namespace Characters.Operations.Fx;

public class MotionTrail : CharacterOperation
{
	public enum Layer
	{
		Behind,
		Front
	}

	protected static readonly int _overlayColor = Shader.PropertyToID("_OverlayColor");

	protected static readonly int _outlineEnabled = Shader.PropertyToID("_IsOutlineEnabled");

	protected static readonly int _outlineColor = Shader.PropertyToID("_OutlineColor");

	protected static readonly int _outlineSize = Shader.PropertyToID("_OutlineSize");

	protected static readonly int _alphaThreshold = Shader.PropertyToID("_AlphaThreshold");

	protected const string _outsideMaterialKeyword = "SPRITE_OUTLINE_OUTSIDE";

	[SerializeField]
	private Layer _layer;

	[FrameTime]
	[SerializeField]
	[Header("Time")]
	private float _duration;

	[FrameTime]
	[SerializeField]
	private float _interval;

	[SerializeField]
	[Header("Color")]
	private bool _changeColor = true;

	[SerializeField]
	private Color _color;

	[Header("Fadeout")]
	[SerializeField]
	private AnimationCurve _fadeOutCurve;

	[SerializeField]
	[FrameTime]
	private float _fadeOutDuration;

	private CoroutineReference _cTrail;

	private MaterialPropertyBlock _propertyBlock;

	private void Awake()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		_propertyBlock = new MaterialPropertyBlock();
	}

	public override void Run(Character owner)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		foreach (CharacterAnimation animation in owner.animationController.animations)
		{
			if (((Component)animation).gameObject.activeInHierarchy)
			{
				_cTrail = CoroutineReferenceExtension.StartCoroutineWithReference((MonoBehaviour)(object)this, CTrail(owner, animation.spriteRenderer, owner.chronometer.animation));
			}
		}
	}

	private IEnumerator CTrail(Character owner, SpriteRenderer spriteRenderer, Chronometer chronometer)
	{
		float remainTime = ((_duration == 0f) ? float.PositiveInfinity : _duration);
		float remainInterval = 0f;
		int sortingOrderCount = 0;
		while (remainTime > 0f)
		{
			if (remainInterval <= 0f)
			{
				Effects.SpritePoolObject spritePoolObject = Effects.sprite.Spawn();
				spritePoolObject.spriteRenderer.CopyFrom(spriteRenderer);
				((Renderer)spritePoolObject.spriteRenderer).sortingLayerID = owner.sortingGroup.sortingLayerID;
				sortingOrderCount++;
				int sortingOrder = owner.sortingGroup.sortingOrder;
				sortingOrder = ((_layer != Layer.Front) ? (sortingOrder - sortingOrderCount) : (sortingOrder + sortingOrderCount));
				((Renderer)spritePoolObject.spriteRenderer).sortingOrder = sortingOrder;
				spritePoolObject.spriteRenderer.color = Color.white;
				((Renderer)spritePoolObject.spriteRenderer).sharedMaterial = MaterialResource.character;
				((Renderer)spritePoolObject.spriteRenderer).GetPropertyBlock(_propertyBlock);
				if (_changeColor)
				{
					_propertyBlock.SetColor(_overlayColor, _color);
				}
				else
				{
					_propertyBlock.SetColor(_overlayColor, Color.clear);
				}
				((Renderer)spritePoolObject.spriteRenderer).SetPropertyBlock(_propertyBlock);
				Transform transform = ((Component)spritePoolObject.poolObject).transform;
				Transform transform2 = ((Component)spriteRenderer).transform;
				transform.SetPositionAndRotation(transform2.position, transform2.rotation);
				transform.localScale = transform2.lossyScale;
				transform.rotation = transform2.rotation;
				spritePoolObject.FadeOut(chronometer, _fadeOutCurve, _fadeOutDuration);
				remainInterval = _interval;
			}
			yield return null;
			float num = ChronometerExtension.DeltaTime((ChronometerBase)(object)chronometer);
			remainInterval -= num;
			remainTime -= num;
		}
	}

	public override void Stop()
	{
		((CoroutineReference)(ref _cTrail)).Stop();
	}
}
