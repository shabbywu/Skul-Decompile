using System;
using System.Collections.Generic;
using Characters;
using GameResources;
using Singletons;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace FX;

[Serializable]
public class EffectInfo : IDisposable
{
	[Serializable]
	public class AttachInfo
	{
		public enum Pivot
		{
			Center,
			TopLeft,
			Top,
			TopRight,
			Left,
			Right,
			BottomLeft,
			Bottom,
			BottomRight,
			Custom
		}

		private static readonly EnumArray<Pivot, Vector2> _pivotValues = new EnumArray<Pivot, Vector2>(new Vector2(0f, 0f), new Vector2(-0.5f, 0.5f), new Vector2(0f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(-0.5f, 0f), new Vector2(0f, 0.5f), new Vector2(-0.5f, -0.5f), new Vector2(0f, -0.5f), new Vector2(0.5f, -0.5f), new Vector2(0f, 0f));

		[SerializeField]
		internal bool _attach;

		[SerializeField]
		private Pivot _pivot;

		[HideInInspector]
		[SerializeField]
		private Vector2 _pivotValue;

		public bool attach => _attach;

		public Pivot pivot => _pivot;

		public Vector2 pivotValue => _pivotValue;

		public AttachInfo()
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			_attach = false;
			_pivot = Pivot.Center;
			_pivotValue = Vector2.zero;
		}

		public AttachInfo(bool attach, bool layerOnly, int layerOrderOffset, Pivot pivot)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			_attach = attach;
			_pivot = pivot;
			_pivotValue = _pivotValues[pivot];
		}
	}

	[Serializable]
	public class SizeForEffectAndAnimatorArray : EnumArray<Character.SizeForEffect, RuntimeAnimatorController>, IDisposable
	{
		public void Dispose()
		{
			for (int i = 0; i < base.Array.Length; i++)
			{
				base.Array[i] = null;
			}
		}
	}

	public enum Blend
	{
		Normal,
		Darken,
		Lighten,
		LinearBurn,
		LinearDodge
	}

	public static readonly int huePropertyID = Shader.PropertyToID("_Hue");

	[SerializeField]
	private bool _fold;

	public bool subordinated;

	public PoolObject effect;

	public RuntimeAnimatorController animation;

	public AssetReference animationReference;

	private AsyncOperationHandle<RuntimeAnimatorController> _animationAssetHandle;

	public SizeForEffectAndAnimatorArray animationBySize;

	public AttachInfo attachInfo;

	public CustomFloat scale;

	public CustomFloat scaleX;

	public CustomFloat scaleY;

	public CustomAngle angle;

	public PositionNoise noise;

	public Color color = Color.white;

	public Blend blend;

	[Range(-180f, 180f)]
	public int hue;

	public int sortingLayerId;

	public bool autoLayerOrder = true;

	public short sortingLayerOrder;

	public bool trackChildren;

	public bool loop;

	public float delay;

	public float duration;

	[Header("Flips")]
	[Tooltip("Owner의 방향에 따라서 각도를 뒤집음")]
	public bool flipDirectionByOwnerDirection;

	[Tooltip("Owner의 방향에 따라서 X 스케일을 뒤집음")]
	public bool flipXByOwnerDirection = true;

	[Tooltip("Owner의 방향에 따라서 Y 스케일을 뒤집음")]
	public bool flipYByOwnerDirection;

	[Tooltip("이미지를 좌우 반전시킴")]
	public bool flipX;

	[Tooltip("이미지를 상하 반전시킴")]
	public bool flipY;

	public AnimationCurve fadeOutCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	public float fadeOutDuration;

	public Chronometer chronometer;

	private readonly List<EffectPoolInstance> _children = new List<EffectPoolInstance>();

	public EffectInfo()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		attachInfo = new AttachInfo();
		scale = new CustomFloat(1f);
		scaleX = new CustomFloat(1f);
		scaleY = new CustomFloat(1f);
		angle = new CustomAngle(0f);
		noise = new PositionNoise();
		color = Color.white;
		sortingLayerId = int.MinValue;
		fadeOutCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
		flipDirectionByOwnerDirection = false;
		flipXByOwnerDirection = true;
		flipYByOwnerDirection = false;
	}

	public EffectInfo(PoolObject effect)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		this.effect = effect;
		attachInfo = new AttachInfo();
		scale = new CustomFloat(1f);
		scaleX = new CustomFloat(1f);
		scaleY = new CustomFloat(1f);
		angle = new CustomAngle(0f);
		noise = new PositionNoise();
		color = Color.white;
		sortingLayerId = SortingLayer.NameToID("Effect");
		fadeOutCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
		flipDirectionByOwnerDirection = false;
		flipXByOwnerDirection = true;
		flipYByOwnerDirection = false;
	}

	public EffectInfo(RuntimeAnimatorController animation)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		this.animation = animation;
		animationBySize = new SizeForEffectAndAnimatorArray();
		for (int i = 0; i < animationBySize.Array.Length; i++)
		{
			animationBySize.Array[i] = animation;
		}
		attachInfo = new AttachInfo();
		scale = new CustomFloat(1f);
		scaleX = new CustomFloat(1f);
		scaleY = new CustomFloat(1f);
		angle = new CustomAngle(0f);
		noise = new PositionNoise();
		color = Color.white;
		sortingLayerId = SortingLayer.NameToID("Effect");
		fadeOutCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
		flipDirectionByOwnerDirection = false;
		flipXByOwnerDirection = true;
		flipYByOwnerDirection = false;
	}

	public EffectInfo(RuntimeAnimatorController animation, SizeForEffectAndAnimatorArray animationBySize)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		this.animation = animation;
		this.animationBySize = animationBySize;
		attachInfo = new AttachInfo();
		scale = new CustomFloat(1f);
		scaleX = new CustomFloat(1f);
		scaleY = new CustomFloat(1f);
		angle = new CustomAngle(0f);
		noise = new PositionNoise();
		color = Color.white;
		sortingLayerId = SortingLayer.NameToID("Effect");
		fadeOutCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
		flipDirectionByOwnerDirection = false;
		flipXByOwnerDirection = true;
		flipYByOwnerDirection = false;
	}

	~EffectInfo()
	{
		Dispose();
	}

	public void Dispose()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		effect = null;
		animation = null;
		if (_animationAssetHandle.IsValid())
		{
			Addressables.Release<RuntimeAnimatorController>(_animationAssetHandle);
		}
	}

	private void SetTransform(EffectPoolInstance effect, Vector3 position, float extraAngle, float extraScale, bool flip = false)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		float num = angle.value + extraAngle;
		if (flip && flipDirectionByOwnerDirection)
		{
			num = (180f - num) % 360f;
		}
		((Component)effect).transform.SetPositionAndRotation(position + noise.Evaluate(), Quaternion.Euler(0f, 0f, num));
		Vector3 localScale = Vector3.one * extraScale * scale.value;
		float value = scaleX.value;
		if (value > 0f)
		{
			localScale.x *= value;
		}
		float value2 = scaleY.value;
		if (value2 > 0f)
		{
			localScale.y *= value2;
		}
		effect.renderer.flipX = flipX;
		effect.renderer.flipY = flipY;
		if (flip)
		{
			if (flipXByOwnerDirection)
			{
				localScale.x *= -1f;
			}
			if (flipYByOwnerDirection)
			{
				localScale.y *= -1f;
			}
		}
		((Component)effect).transform.localScale = localScale;
	}

	public void LoadReference()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		if (animationReference != null && animationReference.RuntimeKeyIsValid())
		{
			_animationAssetHandle = animationReference.LoadAssetAsync<RuntimeAnimatorController>();
		}
	}

	public EffectPoolInstance Spawn(Vector3 position, float extraAngle = 0f, float extraScale = 1f)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)effect != (Object)null)
		{
			return Spawn(position, ((Component)effect).GetComponent<Animator>().runtimeAnimatorController, extraAngle, extraScale);
		}
		return Spawn(position, animation, extraAngle, extraScale);
	}

	public EffectPoolInstance Spawn(Vector3 position, RuntimeAnimatorController animation, float extraAngle = 0f, float extraScale = 1f, bool flip = false)
	{
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)animation == (Object)null)
		{
			if (!_animationAssetHandle.IsValid())
			{
				return null;
			}
			animation = _animationAssetHandle.WaitForCompletion();
			if ((Object)(object)animation == (Object)null)
			{
				return null;
			}
		}
		EffectPoolInstance effect = Singleton<EffectPool>.Instance.Play(animation, delay, duration, loop, fadeOutCurve, fadeOutDuration);
		SetTransform(effect, position, extraAngle, extraScale, flip);
		effect.hue = hue;
		((Renderer)effect.renderer).sortingOrder = (autoLayerOrder ? Effects.GetSortingOrderAndIncrease() : sortingLayerOrder);
		if (SortingLayer.IsValid(sortingLayerId))
		{
			if (((Renderer)effect.renderer).sortingLayerID != sortingLayerId)
			{
				((Renderer)effect.renderer).sortingLayerID = sortingLayerId;
			}
		}
		else
		{
			int num = SortingLayer.NameToID("Effect");
			Debug.LogError((object)$"The sorting layer id of effect is invalid! id : {sortingLayerId}, effect id : {num}");
			((Renderer)effect.renderer).sortingLayerID = num;
		}
		Material sharedMaterial = (Material)(blend switch
		{
			Blend.Darken => MaterialResource.effect_darken, 
			Blend.Lighten => MaterialResource.effect_lighten, 
			Blend.LinearBurn => MaterialResource.effect_linearBurn, 
			Blend.LinearDodge => MaterialResource.effect_linearDodge, 
			_ => MaterialResource.effect, 
		});
		((Renderer)effect.renderer).sharedMaterial = sharedMaterial;
		effect.chronometer = chronometer;
		if (trackChildren)
		{
			_children.Add(effect);
			effect.OnStop += RemoveFromList;
		}
		((Renderer)effect.renderer).enabled = true;
		effect.renderer.color = color;
		return effect;
		void RemoveFromList()
		{
			effect.OnStop -= RemoveFromList;
			_children.Remove(effect);
		}
	}

	public EffectPoolInstance Spawn(Vector3 position, Character target, float extraAngle = 0f, float extraScale = 1f)
	{
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		if (attachInfo.attach && target.sizeForEffect == Character.SizeForEffect.None)
		{
			return null;
		}
		RuntimeAnimatorController val;
		if ((Object)(object)effect != (Object)null)
		{
			val = ((Component)effect).GetComponent<Animator>().runtimeAnimatorController;
		}
		else
		{
			val = animationBySize?[target.sizeForEffect];
			if ((Object)(object)val == (Object)null)
			{
				val = animation;
			}
		}
		EffectPoolInstance effectPoolInstance = Spawn(position, val, extraAngle, extraScale, target.lookingDirection == Character.LookingDirection.Left);
		if ((Object)(object)effectPoolInstance == (Object)null)
		{
			return null;
		}
		effectPoolInstance.chronometer = target.chronometer.effect;
		if (attachInfo.attach)
		{
			((Component)effectPoolInstance).transform.parent = (flipXByOwnerDirection ? target.attachWithFlip.transform : target.attach.transform);
			Vector3 position2 = ((Component)target).transform.position;
			position2.x += ((Collider2D)target.collider).offset.x;
			position2.y += ((Collider2D)target.collider).offset.y;
			Bounds bounds = ((Collider2D)target.collider).bounds;
			Vector3 size = ((Bounds)(ref bounds)).size;
			size.x *= attachInfo.pivotValue.x;
			size.y *= attachInfo.pivotValue.y;
			((Component)effectPoolInstance).transform.position = position2 + size;
		}
		return effectPoolInstance;
	}

	public void DespawnChildren()
	{
		for (int num = _children.Count - 1; num >= 0; num--)
		{
			_children[num].Stop();
		}
	}
}
