using System;
using System.Collections.Generic;
using System.Linq;
using FX;
using FX.SpriteEffects;
using GameResources;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Statuses;

public sealed class StatusEffect
{
	public abstract class EffectHandler
	{
		public abstract void Initialize(Character character);

		public virtual void UpdateTime(float detaTime)
		{
		}

		public abstract void Dispose();

		public abstract void HandleOnAttach(Character attacker, Character target);

		public abstract void HandleOnDetach(Character attacker, Character target);

		public abstract void HandleOnRefresh(Character attacker, Character target);
	}

	[Serializable]
	public class Stun : EffectHandler
	{
		private static readonly string _floatingTextKey = "floating/status/stun";

		private static readonly string _floatingTextColor = "#ffffff";

		[SerializeField]
		private Collider2D _range;

		private Character _character;

		private EffectInfo _effect;

		public Stun(Character character)
		{
			Initialize(character);
		}

		public Stun(Stun stun, Character character)
		{
			_range = stun._range;
			Initialize(character);
		}

		public override void Initialize(Character character)
		{
			_character = character;
			if ((Object)(object)_range == (Object)null)
			{
				_range = (Collider2D)(object)character.collider;
			}
			_effect = new EffectInfo(CommonResource.instance.stunEffect)
			{
				attachInfo = new EffectInfo.AttachInfo(attach: true, layerOnly: false, 1, EffectInfo.AttachInfo.Pivot.Top),
				loop = true,
				trackChildren = true
			};
		}

		public override void HandleOnAttach(Character attacker, Character target)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			_effect.DespawnChildren();
			_effect.Spawn(((Component)target).transform.position, target);
			SpawnFloatingText();
			PersistentSingleton<SoundManager>.Instance.PlaySound(CharacterStatusSetting.instance.stun.attachSound, ((Component)target).transform.position);
		}

		public override void HandleOnRefresh(Character attacker, Character target)
		{
			SpawnFloatingText();
		}

		public override void HandleOnDetach(Character attacker, Character target)
		{
			_effect.DespawnChildren();
		}

		public override void Dispose()
		{
		}

		private void SpawnFloatingText()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			Vector2 val = MMMaths.RandomPointWithinBounds(_range.bounds);
			Singleton<Service>.Instance.floatingTextSpawner.SpawnStatus(Localization.GetLocalizedString(_floatingTextKey), Vector2.op_Implicit(val), _floatingTextColor);
		}
	}

	public abstract class FreezeHandler : EffectHandler
	{
		public abstract void HandleOnTakeDamage(Character attacker, Character target);
	}

	[Serializable]
	public class Freeze : FreezeHandler
	{
		private static readonly ColorBlend _colorOverlay;

		private static readonly EnumArray<Character.SizeForEffect, ParticleEffectInfo> _particles;

		private static readonly string _floatingTextKey;

		private static readonly string _floatingTextColor;

		private EffectInfo _effect;

		private EffectPoolInstance _frontEffect;

		private EffectPoolInstance _backEffect;

		private Vector3 _basePosition;

		private Character _character;

		static Freeze()
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			_colorOverlay = new ColorBlend(100, new Color(0f, 0.14509805f, 11f / 15f, 1f), 0f);
			_particles = new EnumArray<Character.SizeForEffect, ParticleEffectInfo>();
			_floatingTextKey = "floating/status/freeze";
			_floatingTextColor = "#04FFE6";
			_particles[Character.SizeForEffect.Small] = CommonResource.instance.freezeSmallParticle;
			_particles[Character.SizeForEffect.Medium] = CommonResource.instance.freezeMediumParticle;
			_particles[Character.SizeForEffect.Large] = CommonResource.instance.freezeLargeParticle;
			_particles[Character.SizeForEffect.ExtraLarge] = CommonResource.instance.freezeLargeParticle;
		}

		public Freeze(Character character)
		{
			Initialize(character);
		}

		public override void Initialize(Character character)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			_character = character;
			if ((Object)(object)character.collider != (Object)null)
			{
				RuntimeAnimatorController freezeAnimator = CommonResource.instance.GetFreezeAnimator(character.collider.size * 32f);
				EffectInfo obj = new EffectInfo(freezeAnimator)
				{
					attachInfo = new EffectInfo.AttachInfo(attach: true, layerOnly: false, 1, EffectInfo.AttachInfo.Pivot.Center),
					loop = true,
					trackChildren = true
				};
				SortingLayer val = SortingLayer.layers.Last();
				obj.sortingLayerId = ((SortingLayer)(ref val)).id;
				_effect = obj;
			}
		}

		public override void UpdateTime(float deltaTime)
		{
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			if (!(deltaTime <= 0f) && _character.status.freeze.remainTime < CharacterStatusSetting.instance.freeze.breakDuration && _character.status.freeze.remainTime > 0f && (Object)(object)_frontEffect != (Object)null)
			{
				Vector3 localPosition = _basePosition + Vector2.op_Implicit(Random.insideUnitCircle) * 0.1f;
				((Component)_frontEffect).transform.localPosition = localPosition;
				((Component)_backEffect).transform.localPosition = localPosition;
			}
		}

		public override void Dispose()
		{
		}

		public override void HandleOnAttach(Character attacker, Character target)
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			target.spriteEffectStack.Add(_colorOverlay);
			_effect.DespawnChildren();
			_frontEffect = _effect.Spawn(((Component)target).transform.position, target);
			((Renderer)_frontEffect.renderer).sharedMaterial = MaterialResource.effect_linearDodge;
			SpriteRenderer renderer = _frontEffect.renderer;
			SortingLayer val = SortingLayer.layers.Last();
			((Renderer)renderer).sortingLayerID = ((SortingLayer)(ref val)).id;
			_backEffect = _effect.Spawn(((Component)target).transform.position, target);
			((Renderer)_backEffect.renderer).sortingLayerID = ((SortingLayer)(ref SortingLayer.layers[0])).id;
			_basePosition = ((Component)_frontEffect).transform.localPosition;
			SpawnFloatingText();
			PersistentSingleton<SoundManager>.Instance.PlaySound(CharacterStatusSetting.instance.freeze.attachSound, ((Component)_character).transform.position);
		}

		public override void HandleOnRefresh(Character attacker, Character target)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			SpawnFloatingText();
			PersistentSingleton<SoundManager>.Instance.PlaySound(CharacterStatusSetting.instance.freeze.attachSound, ((Component)_character).transform.position);
		}

		public override void HandleOnDetach(Character attacker, Character target)
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			target.spriteEffectStack.Remove(_colorOverlay);
			_effect.DespawnChildren();
			_particles[target.sizeForEffect].Emit(Vector2.op_Implicit(((Component)target).transform.position), ((Collider2D)target.collider).bounds, Vector2.zero);
			PersistentSingleton<SoundManager>.Instance.PlaySound(CharacterStatusSetting.instance.freeze.detachSound, ((Component)_character).transform.position);
		}

		private void SpawnFloatingText()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			Vector2 val = MMMaths.RandomPointWithinBounds(((Collider2D)_character.collider).bounds);
			Singleton<Service>.Instance.floatingTextSpawner.SpawnStatus(Localization.GetLocalizedString(_floatingTextKey), Vector2.op_Implicit(val), _floatingTextColor);
		}

		public override void HandleOnTakeDamage(Character attacker, Character target)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			PersistentSingleton<SoundManager>.Instance.PlaySound(CharacterStatusSetting.instance.freeze.hitSound, ((Component)_character).transform.position);
		}
	}

	[Serializable]
	public class GigantEnemyFreeze : FreezeHandler
	{
		private static readonly ParticleEffectInfo _particle;

		private static readonly string _floatingTextKey;

		private static readonly string _floatingTextColor;

		[Range(0f, 1f)]
		[SerializeField]
		private float _shakeIntensity = 0.05f;

		[SerializeField]
		private float _shakeDuration = 0.2f;

		[SerializeField]
		[Tooltip("빙결 끝나지 x초 전")]
		private float _shakeStartTime = 1.5f;

		[SerializeField]
		private EffectInfo _spriteEffect;

		[SerializeField]
		private Collider2D[] _toggleEffects;

		private Vector2[] _originPositions;

		private Character _character;

		static GigantEnemyFreeze()
		{
			_floatingTextKey = "floating/status/freeze";
			_floatingTextColor = "#04FFE6";
			_particle = CommonResource.instance.freezeLargeParticle;
		}

		public GigantEnemyFreeze(Character character)
		{
			Initialize(character);
		}

		public GigantEnemyFreeze(GigantEnemyFreeze copyFrom, Character character)
		{
			_shakeIntensity = copyFrom._shakeIntensity;
			_shakeDuration = copyFrom._shakeDuration;
			_shakeStartTime = copyFrom._shakeStartTime;
			_spriteEffect = copyFrom._spriteEffect;
			_toggleEffects = copyFrom._toggleEffects;
			Initialize(character);
		}

		public override void Initialize(Character character)
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			_character = character;
			_originPositions = (Vector2[])(object)new Vector2[_toggleEffects.Length];
			for (int i = 0; i < _toggleEffects.Length; i++)
			{
				_originPositions[i] = Vector2.op_Implicit(((Component)_toggleEffects[i]).transform.localPosition);
			}
		}

		public override void UpdateTime(float detaTime)
		{
			if (!_character.health.dead && _character.status.freeze.remainTime < _shakeStartTime && _character.status.freeze.remainTime > _shakeStartTime - _shakeDuration)
			{
				Shake();
			}
		}

		public override void Dispose()
		{
			Collider2D[] toggleEffects = _toggleEffects;
			for (int i = 0; i < toggleEffects.Length; i++)
			{
				((Component)toggleEffects[i]).gameObject.SetActive(false);
			}
		}

		public override void HandleOnAttach(Character attacker, Character target)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			SpawnFloatingText();
			for (int i = 0; i < _toggleEffects.Length; i++)
			{
				((Component)_toggleEffects[i]).transform.localPosition = Vector2.op_Implicit(_originPositions[i]);
				((Component)_toggleEffects[i]).gameObject.SetActive(true);
				_spriteEffect.Spawn(((Component)_toggleEffects[i]).transform.position);
			}
			PersistentSingleton<SoundManager>.Instance.PlaySound(CharacterStatusSetting.instance.freeze.attachSound, ((Component)_character).transform.position);
		}

		public override void HandleOnRefresh(Character attacker, Character target)
		{
			SpawnFloatingText();
		}

		public override void HandleOnDetach(Character attacker, Character target)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			Collider2D[] toggleEffects = _toggleEffects;
			foreach (Collider2D val in toggleEffects)
			{
				((Component)val).gameObject.SetActive(false);
				_particle.Emit(Vector2.op_Implicit(((Component)val).transform.position), val.bounds, Vector2.zero);
				_spriteEffect.Spawn(((Component)val).transform.position);
			}
			PersistentSingleton<SoundManager>.Instance.PlaySound(CharacterStatusSetting.instance.freeze.detachSound, ((Component)_character).transform.position);
		}

		public override void HandleOnTakeDamage(Character attacker, Character target)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			PersistentSingleton<SoundManager>.Instance.PlaySound(CharacterStatusSetting.instance.freeze.hitSound, ((Component)_character).transform.position);
		}

		private void Shake()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			for (int i = 0; i < _toggleEffects.Length; i++)
			{
				Vector2 val = _originPositions[i] + Random.insideUnitCircle * _shakeIntensity;
				((Component)_toggleEffects[i]).transform.localPosition = Vector2.op_Implicit(val);
			}
		}

		private void SpawnFloatingText()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			Vector2 val = MMMaths.RandomPointWithinBounds(((Collider2D)_character.collider).bounds);
			Singleton<Service>.Instance.floatingTextSpawner.SpawnStatus(Localization.GetLocalizedString(_floatingTextKey), Vector2.op_Implicit(val), _floatingTextColor);
		}
	}

	public abstract class BurnHandler : EffectHandler
	{
		public abstract void HandleOnTookBurnDamage(Character attacker, Character target);

		public abstract void HandleOnTookEmberDamage(Character attacker, Character target);
	}

	[Serializable]
	public class Burn : BurnHandler
	{
		private static readonly ParticleEffectInfo _hitParticle;

		private static readonly string _floatingTextKey;

		private static readonly string _floatingTextColor;

		[SerializeField]
		private Collider2D _range;

		[SerializeField]
		private CustomFloat _scale = new CustomFloat(0.3f, 0.5f);

		private EffectInfo _burnEffect;

		private EffectInfo _emberEffect;

		private GenericSpriteEffect _spriteEffect;

		private Character _character;

		static Burn()
		{
			_floatingTextKey = "floating/status/burn";
			_floatingTextColor = "#DD4900";
			_hitParticle = CommonResource.instance.hitParticle;
		}

		public Burn(Character character)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Expected O, but got Unknown
			Initialize(character);
		}

		public Burn(Burn copyFrom, Character character)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Expected O, but got Unknown
			_range = copyFrom._range;
			_scale = copyFrom._scale;
			Initialize(character);
		}

		public override void Initialize(Character character)
		{
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Expected O, but got Unknown
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Expected O, but got Unknown
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Expected O, but got Unknown
			_character = character;
			if ((Object)(object)_range == (Object)null)
			{
				_range = (Collider2D)(object)character.collider;
			}
			_burnEffect = new EffectInfo(CharacterStatusSetting.instance.burn.loopEffect)
			{
				angle = new CustomAngle(0f, 360f),
				scale = _scale
			};
			_emberEffect = new EffectInfo(CharacterStatusSetting.instance.burn.loopEffect)
			{
				angle = new CustomAngle(0f, 360f),
				scale = new CustomFloat(0.4f, 0.6f)
			};
			_spriteEffect = new GenericSpriteEffect(-1, CharacterStatusSetting.instance.burn.duration, 1f, CharacterStatusSetting.instance.burn.colorOverlay, CharacterStatusSetting.instance.burn.colorBlend, CharacterStatusSetting.instance.burn.outline, CharacterStatusSetting.instance.burn.grayScale);
		}

		public override void Dispose()
		{
		}

		public override void HandleOnAttach(Character attacker, Character target)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			PersistentSingleton<SoundManager>.Instance.PlaySound(CharacterStatusSetting.instance.burn.attachSound, ((Component)target).transform.position);
			SpawnFloatingText();
		}

		public override void HandleOnRefresh(Character attacker, Character target)
		{
			SpawnFloatingText();
		}

		public override void HandleOnDetach(Character attacker, Character target)
		{
			_character.spriteEffectStack.Remove(_spriteEffect);
		}

		public override void HandleOnTookBurnDamage(Character attacker, Character target)
		{
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			_character.spriteEffectStack.Remove(_spriteEffect);
			_spriteEffect.Reset();
			_character.spriteEffectStack.Add(_spriteEffect);
			PersistentSingleton<SoundManager>.Instance.PlaySound(CharacterStatusSetting.instance.burn.attackSound, ((Component)target).transform.position);
			for (int i = 0; i < 3; i++)
			{
				_burnEffect.Spawn(Vector2.op_Implicit(MMMaths.RandomPointWithinBounds(_range.bounds)));
			}
			_hitParticle.Emit(Vector2.op_Implicit(((Component)_range).transform.position), _range.bounds, Vector2.zero);
		}

		public override void HandleOnTookEmberDamage(Character attacker, Character target)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			_emberEffect.Spawn(Vector2.op_Implicit(MMMaths.RandomPointWithinBounds(((Collider2D)target.collider).bounds)));
			_hitParticle.Emit(Vector2.op_Implicit(((Component)target).transform.position), ((Collider2D)target.collider).bounds, Vector2.zero);
		}

		public void SpawnFloatingText()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			Vector2 val = MMMaths.RandomPointWithinBounds(_range.bounds);
			Singleton<Service>.Instance.floatingTextSpawner.SpawnStatus(Localization.GetLocalizedString(_floatingTextKey), Vector2.op_Implicit(val), _floatingTextColor);
		}
	}

	public abstract class PoisonHandler : EffectHandler
	{
		public abstract void HandleOnTookPoisonTickDamage(Character attacker, Character target);
	}

	[Serializable]
	public class Poison : PoisonHandler
	{
		private static readonly string _floatingTextKey = "floating/status/poision";

		private static readonly string _floatingTextColor = "#A229FF";

		[SerializeField]
		private CustomFloat _scale = new CustomFloat(0.15f, 0.2f);

		[SerializeField]
		private Collider2D _range;

		private EffectInfo[] _effects;

		private GenericSpriteEffect _spriteEffect;

		private Character _character;

		public Poison(Character character)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Expected O, but got Unknown
			Initialize(character);
		}

		public Poison(Poison copyFrom, Character character)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Expected O, but got Unknown
			_scale = copyFrom._scale;
			_range = copyFrom._range;
			Initialize(character);
		}

		public override void Initialize(Character character)
		{
			_character = character;
			if ((Object)(object)_range == (Object)null)
			{
				_range = (Collider2D)(object)character.collider;
			}
			_effects = new EffectInfo[3]
			{
				new EffectInfo(CharacterStatusSetting.instance.poison.loopEffectA)
				{
					scale = _scale
				},
				new EffectInfo(CharacterStatusSetting.instance.poison.loopEffectB)
				{
					scale = _scale
				},
				new EffectInfo(CharacterStatusSetting.instance.poison.loopEffectC)
				{
					scale = _scale
				}
			};
			_spriteEffect = new GenericSpriteEffect(-1, CharacterStatusSetting.instance.poison.colorOverlay.duration, 1f, CharacterStatusSetting.instance.poison.colorOverlay, CharacterStatusSetting.instance.poison.colorBlend, CharacterStatusSetting.instance.poison.outline, CharacterStatusSetting.instance.poison.grayScale);
		}

		public override void Dispose()
		{
		}

		public override void HandleOnTookPoisonTickDamage(Character attacker, Character target)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			EffectPoolInstance effectPoolInstance = ExtensionMethods.Random<EffectInfo>((IEnumerable<EffectInfo>)_effects).Spawn(Vector2.op_Implicit(MMMaths.RandomPointWithinBounds(_range.bounds)));
			if (MMMaths.RandomBool())
			{
				((Component)effectPoolInstance).transform.localScale = new Vector3(-1f, 1f, 1f) * _scale.value;
			}
			_character.spriteEffectStack.Remove(_spriteEffect);
			_spriteEffect.Reset();
			_character.spriteEffectStack.Add(_spriteEffect);
		}

		public override void HandleOnAttach(Character attacker, Character target)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			SpawnFloatingText();
			PersistentSingleton<SoundManager>.Instance.PlaySound(CharacterStatusSetting.instance.poison.attachSound, ((Component)target).transform.position);
		}

		public override void HandleOnRefresh(Character attacker, Character target)
		{
			SpawnFloatingText();
		}

		public override void HandleOnDetach(Character attacker, Character target)
		{
		}

		public void SpawnFloatingText()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			Vector2 val = MMMaths.RandomPointWithinBounds(_range.bounds);
			Singleton<Service>.Instance.floatingTextSpawner.SpawnStatus(Localization.GetLocalizedString(_floatingTextKey), Vector2.op_Implicit(val), _floatingTextColor);
			_character.spriteEffectStack.Remove(_spriteEffect);
		}
	}

	[Serializable]
	public class Wound : EffectHandler
	{
		private static readonly ParticleEffectInfo _hitParticle;

		private static readonly string _floatingWoundTextKey;

		private static readonly string _floatingBleedTextKey;

		private static readonly string _floatingBleedTextColor;

		[SerializeField]
		private Collider2D _range;

		[SerializeField]
		private CustomFloat _loopEffectScale = new CustomFloat(0.2f, 0.4f);

		[SerializeField]
		private CustomFloat _loopEffectImpactScale = new CustomFloat(0.3f, 0.5f);

		[SerializeField]
		private CustomFloat _impactEffectScale = new CustomFloat(0.4f, 0.6f);

		private EffectInfo[] _loopEffects;

		private EffectInfo _loopEffectImpact;

		private EffectInfo _impactEffect;

		private CustomAngle _loopEffectAngle;

		private CustomFloat _loopEffectInterval;

		private CustomFloat _loopEffectImpactInterval;

		private float _loopEffectRemainTime;

		private float _loopEffectImpactRemainTime;

		private Character _character;

		private GenericSpriteEffect _woundSpriteEffect;

		private GenericSpriteEffect _bleedSpriteEffect;

		static Wound()
		{
			_floatingWoundTextKey = "floating/status/wound";
			_floatingBleedTextKey = "floating/status/bleed";
			_floatingBleedTextColor = "#d62d00";
			_hitParticle = CommonResource.instance.hitParticle;
		}

		public Wound(Character character)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Expected O, but got Unknown
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Expected O, but got Unknown
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Expected O, but got Unknown
			Initialize(character);
		}

		public Wound(Wound copyFrom, Character character)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Expected O, but got Unknown
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Expected O, but got Unknown
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Expected O, but got Unknown
			_range = copyFrom._range;
			_loopEffectScale = copyFrom._loopEffectScale;
			_loopEffectImpactScale = copyFrom._loopEffectImpactScale;
			_impactEffectScale = copyFrom._impactEffectScale;
			Initialize(character);
		}

		public override void Initialize(Character character)
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Expected O, but got Unknown
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Expected O, but got Unknown
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Expected O, but got Unknown
			_character = character;
			if ((Object)(object)_range == (Object)null)
			{
				_range = (Collider2D)(object)character.collider;
			}
			_loopEffectInterval = new CustomFloat(0.5f, 1.5f);
			_loopEffectImpactInterval = new CustomFloat(2f, 3f);
			_loopEffectAngle = new CustomAngle(-60f, 60f);
			_loopEffects = new EffectInfo[2]
			{
				new EffectInfo(CharacterStatusSetting.instance.bleed.loopEffectA)
				{
					scale = _loopEffectScale,
					angle = _loopEffectAngle
				},
				new EffectInfo(CharacterStatusSetting.instance.bleed.loopEffectB)
				{
					scale = _loopEffectScale,
					angle = _loopEffectAngle
				}
			};
			_loopEffectImpact = new EffectInfo(CharacterStatusSetting.instance.bleed.dripEffect)
			{
				scale = _loopEffectImpactScale
			};
			_impactEffect = new EffectInfo(CharacterStatusSetting.instance.bleed.impactEffect)
			{
				scale = _impactEffectScale
			};
			_woundSpriteEffect = new GenericSpriteEffect(-1, CharacterStatusSetting.instance.bleed.colorBlend.duration, 1f, CharacterStatusSetting.instance.bleed.colorOverlay, CharacterStatusSetting.instance.bleed.colorBlend, CharacterStatusSetting.instance.bleed.outline, CharacterStatusSetting.instance.bleed.grayScale);
			_bleedSpriteEffect = new GenericSpriteEffect(-1, CharacterStatusSetting.instance.bleed.colorBlendBleed.duration, 1f, CharacterStatusSetting.instance.bleed.colorOverlayBleed, CharacterStatusSetting.instance.bleed.colorBlendBleed, CharacterStatusSetting.instance.bleed.outlineBleed, CharacterStatusSetting.instance.bleed.grayScaleBleed);
		}

		public override void UpdateTime(float deltaTime)
		{
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			_loopEffectRemainTime -= deltaTime;
			_loopEffectImpactRemainTime -= deltaTime;
			if (_loopEffectRemainTime < 0f)
			{
				EffectInfo effectInfo = ExtensionMethods.Random<EffectInfo>((IEnumerable<EffectInfo>)_loopEffects);
				effectInfo.flipX = MMMaths.RandomBool();
				effectInfo.Spawn(Vector2.op_Implicit(MMMaths.RandomPointWithinBounds(_range.bounds)));
				_loopEffectRemainTime += _loopEffectInterval.value;
				PersistentSingleton<SoundManager>.Instance.PlaySound(CharacterStatusSetting.instance.bleed.loopImpactSound, ((Component)_character).transform.position);
			}
			if (_loopEffectImpactRemainTime < 0f)
			{
				_loopEffectImpact.flipX = MMMaths.RandomBool();
				_loopEffectImpact.Spawn(Vector2.op_Implicit(MMMaths.RandomPointWithinBounds(_range.bounds)));
				_loopEffectImpactRemainTime += _loopEffectImpactInterval.value;
			}
		}

		public override void HandleOnAttach(Character attacker, Character target)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			PersistentSingleton<SoundManager>.Instance.PlaySound(CharacterStatusSetting.instance.bleed.attachSound, ((Component)target).transform.position);
			SpawnFloatingText(Localization.GetLocalizedString(_floatingWoundTextKey));
			_character.spriteEffectStack.Remove(_woundSpriteEffect);
			_woundSpriteEffect.Reset();
			_character.spriteEffectStack.Add(_woundSpriteEffect);
		}

		public override void HandleOnDetach(Character attacker, Character target)
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			_character.spriteEffectStack.Remove(_woundSpriteEffect);
			PersistentSingleton<SoundManager>.Instance.PlaySound(CharacterStatusSetting.instance.bleed.impactSound, ((Component)target).transform.position);
			_hitParticle.Emit(Vector2.op_Implicit(((Component)_range).transform.position), _range.bounds, Vector2.zero);
			bool flipX = ((Component)attacker).transform.position.x > ((Component)target).transform.position.x;
			_impactEffect.flipX = flipX;
			_impactEffect.Spawn(Vector2.op_Implicit(MMMaths.RandomPointWithinBounds(_range.bounds)), (float)Random.Range(-15, -45), 1f);
			SpawnFloatingText(Localization.GetLocalizedString(_floatingBleedTextKey));
			_character.spriteEffectStack.Remove(_bleedSpriteEffect);
			_bleedSpriteEffect.Reset();
			_character.spriteEffectStack.Add(_bleedSpriteEffect);
		}

		public override void Dispose()
		{
		}

		private void SpawnFloatingText(string text)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			Singleton<Service>.Instance.floatingTextSpawner.SpawnStatus(text, Vector2.op_Implicit(MMMaths.RandomPointWithinBounds(_range.bounds)), _floatingBleedTextColor);
		}

		public override void HandleOnRefresh(Character attacker, Character target)
		{
		}
	}

	public static Stun GetDefaultStunEffectHandler(Character character)
	{
		return new Stun(character);
	}

	public static Freeze GetDefaultFreezeEffectHanlder(Character character)
	{
		return new Freeze(character);
	}

	public static Burn GetDefaultBurnEffectHanlder(Character character)
	{
		return new Burn(character);
	}

	public static Poison GetDefaultPoisonEffectHanlder(Character character)
	{
		return new Poison(character);
	}

	public static Wound GetDefaultWoundEffectHanlder(Character character)
	{
		return new Wound(character);
	}

	public static Stun CopyFrom(Stun stun, Character character)
	{
		return new Stun(stun, character);
	}

	public static GigantEnemyFreeze CopyFrom(GigantEnemyFreeze freeze, Character character)
	{
		return new GigantEnemyFreeze(freeze, character);
	}

	public static Burn CopyFrom(Burn freeze, Character character)
	{
		return new Burn(freeze, character);
	}

	public static Poison CopyFrom(Poison freeze, Character character)
	{
		return new Poison(freeze, character);
	}

	public static Wound CopyFrom(Wound freeze, Character character)
	{
		return new Wound(freeze, character);
	}
}
