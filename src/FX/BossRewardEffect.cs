using Characters;
using GameResources;
using Level;
using Singletons;
using UnityEngine;
using UnityEngine.Rendering;

namespace FX;

public sealed class BossRewardEffect : MonoBehaviour
{
	private DroppedGear _droppedGear;

	private static readonly int grayScaleID = Shader.PropertyToID("_GrayScaleLerp");

	private bool _cachePopupVisible;

	private Renderer _renderer;

	private Animator _animator;

	private SortingGroup _group;

	private GameObject _child;

	private void Awake()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		_droppedGear = ((Component)this).GetComponent<DroppedGear>();
		_child = new GameObject();
		_child.transform.parent = ((Component)this).transform;
		_child.transform.localPosition = Vector2.op_Implicit(Vector2.zero);
		_renderer = (Renderer)(object)_child.AddComponent<SpriteRenderer>();
		_group = ((Component)_droppedGear).gameObject.AddComponent<SortingGroup>();
		_group.sortingLayerID = ((Renderer)_droppedGear.spriteRenderer).sortingLayerID;
		_renderer.sortingOrder = ((Renderer)_droppedGear.spriteRenderer).sortingOrder;
		_renderer.sortingLayerID = ((Renderer)_droppedGear.spriteRenderer).sortingLayerID;
		_renderer.sortingOrder = ((Renderer)_droppedGear.spriteRenderer).sortingOrder - 1;
		_animator = _child.AddComponent<Animator>();
		_animator.runtimeAnimatorController = CommonResource.instance.bossRewardDeactive;
		((Component)_renderer).transform.localScale = new Vector3(0.8f, 0.8f, 1f);
		UpdateGearMaterial(1f);
		_droppedGear.onLoot += HandleOnLoot;
	}

	private void HandleOnLoot(Character character)
	{
		UpdateGearMaterial(0f);
		_droppedGear.onLoot -= HandleOnLoot;
		_droppedGear.additionalPopupUIOffsetY = 0f;
		_droppedGear.dropMovement.ResetValue();
		Object.Destroy((Object)(object)_child);
		Object.Destroy((Object)(object)_group);
		Object.Destroy((Object)(object)this);
	}

	private void Update()
	{
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)_droppedGear == (Object)null)
		{
			return;
		}
		if (_droppedGear.popupVisible)
		{
			if (!_cachePopupVisible)
			{
				_cachePopupVisible = true;
				_droppedGear.dropMovement.floating = true;
				_animator.runtimeAnimatorController = CommonResource.instance.bossRewardActive;
				((Component)_renderer).transform.localScale = new Vector3(0.72f, 0.72f, 1f);
				PersistentSingleton<SoundManager>.Instance.PlaySound(CommonResource.instance.bossRewardActiveSound, ((Component)_droppedGear).transform.position, 0.1f);
				UpdateGearMaterial(0f);
			}
		}
		else if (_cachePopupVisible)
		{
			_cachePopupVisible = false;
			_droppedGear.dropMovement.floating = false;
			((Component)_renderer).transform.localScale = new Vector3(0.8f, 0.8f, 1f);
			_animator.runtimeAnimatorController = CommonResource.instance.bossRewardDeactive;
			UpdateGearMaterial(1f);
		}
	}

	private void UpdateGearMaterial(float grayScale)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		SpriteRenderer spriteRenderer = _droppedGear.spriteRenderer;
		MaterialPropertyBlock val = new MaterialPropertyBlock();
		((Renderer)spriteRenderer).sharedMaterial = MaterialResource.character;
		((Renderer)spriteRenderer).GetPropertyBlock(val);
		val.SetFloat(grayScaleID, grayScale);
		((Renderer)spriteRenderer).SetPropertyBlock(val);
	}
}
