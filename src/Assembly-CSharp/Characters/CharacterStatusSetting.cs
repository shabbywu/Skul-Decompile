using System;
using Characters.Operations;
using FX;
using FX.SpriteEffects;
using UnityEngine;

namespace Characters;

[CreateAssetMenu]
public class CharacterStatusSetting : ScriptableObject
{
	[Serializable]
	public class Poison
	{
		[SerializeField]
		internal HitInfo _hitInfo;

		[SerializeField]
		internal float _baseTickDamage;

		[SerializeField]
		private float _duration;

		[SerializeField]
		private float _tickFrequency;

		[SerializeField]
		private RuntimeAnimatorController _loopEffectA;

		[SerializeField]
		private RuntimeAnimatorController _loopEffectB;

		[SerializeField]
		private RuntimeAnimatorController _loopEffectC;

		[SerializeField]
		private SoundInfo _attachSound;

		[SerializeField]
		[Space(10f)]
		private GenericSpriteEffect.ColorOverlay _colorOverlay;

		[SerializeField]
		private GenericSpriteEffect.ColorBlend _colorBlend;

		[SerializeField]
		private GenericSpriteEffect.Outline _outline;

		[SerializeField]
		private GenericSpriteEffect.GrayScale _grayScale;

		public HitInfo hitInfo => _hitInfo;

		public float baseTickDamage => _baseTickDamage;

		public float duration => _duration;

		public float tickFrequency => _tickFrequency;

		public RuntimeAnimatorController loopEffectA => _loopEffectA;

		public RuntimeAnimatorController loopEffectB => _loopEffectB;

		public RuntimeAnimatorController loopEffectC => _loopEffectC;

		public SoundInfo attachSound => _attachSound;

		public GenericSpriteEffect.ColorOverlay colorOverlay => _colorOverlay;

		public GenericSpriteEffect.ColorBlend colorBlend => _colorBlend;

		public GenericSpriteEffect.Outline outline => _outline;

		public GenericSpriteEffect.GrayScale grayScale => _grayScale;
	}

	[Serializable]
	public class Bleed
	{
		[SerializeField]
		internal HitInfo _hitInfo;

		[SerializeField]
		private float _baseDamage;

		[SerializeField]
		private RuntimeAnimatorController _loopEffectA;

		[SerializeField]
		private RuntimeAnimatorController _loopEffectB;

		[SerializeField]
		private RuntimeAnimatorController _dripEffect;

		[SerializeField]
		private RuntimeAnimatorController _impactEffect;

		[SerializeField]
		private SoundInfo _attachSound;

		[SerializeField]
		private SoundInfo _loopSound;

		[SerializeField]
		private SoundInfo _impactSound;

		[SerializeField]
		[Space(10f)]
		private GenericSpriteEffect.ColorOverlay _colorOverlay;

		[SerializeField]
		private GenericSpriteEffect.ColorBlend _colorBlend;

		[SerializeField]
		private GenericSpriteEffect.Outline _outline;

		[SerializeField]
		private GenericSpriteEffect.GrayScale _grayScale;

		[Space(10f)]
		[SerializeField]
		private GenericSpriteEffect.ColorOverlay _colorOverlayBleed;

		[SerializeField]
		private GenericSpriteEffect.ColorBlend _colorBlendBleed;

		[SerializeField]
		private GenericSpriteEffect.Outline _outlineBleed;

		[SerializeField]
		private GenericSpriteEffect.GrayScale _grayScaleBleed;

		public HitInfo hitInfo => _hitInfo;

		public float baseDamage => _baseDamage;

		public RuntimeAnimatorController loopEffectA => _loopEffectA;

		public RuntimeAnimatorController loopEffectB => _loopEffectB;

		public RuntimeAnimatorController dripEffect => _dripEffect;

		public RuntimeAnimatorController impactEffect => _impactEffect;

		public SoundInfo attachSound => _attachSound;

		public SoundInfo loopImpactSound => _loopSound;

		public SoundInfo impactSound => _impactSound;

		public GenericSpriteEffect.ColorOverlay colorOverlay => _colorOverlay;

		public GenericSpriteEffect.ColorBlend colorBlend => _colorBlend;

		public GenericSpriteEffect.Outline outline => _outline;

		public GenericSpriteEffect.GrayScale grayScale => _grayScale;

		public GenericSpriteEffect.ColorOverlay colorOverlayBleed => _colorOverlayBleed;

		public GenericSpriteEffect.ColorBlend colorBlendBleed => _colorBlendBleed;

		public GenericSpriteEffect.Outline outlineBleed => _outlineBleed;

		public GenericSpriteEffect.GrayScale grayScaleBleed => _grayScaleBleed;
	}

	[Serializable]
	public class Burn
	{
		[SerializeField]
		internal HitInfo _hitInfo;

		[SerializeField]
		internal HitInfo _rangeHitInfo;

		[SerializeField]
		private float _baseTargetTickDamage;

		[SerializeField]
		private float _baseRangeTickDamage;

		[SerializeField]
		private float _duration;

		[SerializeField]
		private float _tickInterval;

		[SerializeField]
		private float _rangeRadius;

		[SerializeField]
		private RuntimeAnimatorController _loopEffect;

		[SerializeField]
		private SoundInfo _attachSound;

		[SerializeField]
		private SoundInfo _attackSound;

		[SerializeField]
		[Space(10f)]
		private GenericSpriteEffect.ColorOverlay _colorOverlay;

		[SerializeField]
		private GenericSpriteEffect.ColorBlend _colorBlend;

		[SerializeField]
		private GenericSpriteEffect.Outline _outline;

		[SerializeField]
		private GenericSpriteEffect.GrayScale _grayScale;

		public HitInfo hitInfo => _hitInfo;

		public HitInfo rangeHitInfo => _rangeHitInfo;

		public float baseTargetTickDamage => _baseTargetTickDamage;

		public float baseRangeTickDamage => _baseRangeTickDamage;

		public float duration => _duration;

		public float tickInterval => _tickInterval;

		public float rangeRadius => _rangeRadius;

		public RuntimeAnimatorController loopEffect => _loopEffect;

		public SoundInfo attachSound => _attachSound;

		public SoundInfo attackSound => _attackSound;

		public GenericSpriteEffect.ColorOverlay colorOverlay => _colorOverlay;

		public GenericSpriteEffect.ColorBlend colorBlend => _colorBlend;

		public GenericSpriteEffect.Outline outline => _outline;

		public GenericSpriteEffect.GrayScale grayScale => _grayScale;
	}

	[Serializable]
	public class Freeze
	{
		[SerializeField]
		private float _duration;

		[SerializeField]
		private float _minimumTime;

		[SerializeField]
		private float _breakDuration;

		[SerializeField]
		private SoundInfo _attachSound;

		[SerializeField]
		private SoundInfo _hitSound;

		[SerializeField]
		private SoundInfo _detachSound;

		[SerializeField]
		private string[] _nonHitCountAttackKeys;

		public float duration => _duration;

		public float minimumTime => _minimumTime;

		public float breakDuration => _breakDuration;

		public SoundInfo attachSound => _attachSound;

		public SoundInfo hitSound => _hitSound;

		public SoundInfo detachSound => _detachSound;

		public string[] nonHitCountAttackKeys => _nonHitCountAttackKeys;
	}

	[Serializable]
	public class Stun
	{
		[SerializeField]
		private float _duration;

		[SerializeField]
		private SoundInfo _attachSound;

		public float duration => _duration;

		public SoundInfo attachSound => _attachSound;
	}

	private static CharacterStatusSetting _instance;

	[SerializeField]
	private Poison _poison;

	[SerializeField]
	private Bleed _bleed;

	[SerializeField]
	private Burn _burn;

	[SerializeField]
	private Freeze _freeze;

	[SerializeField]
	private Stun _stun;

	public Poison poison => _poison;

	public Bleed bleed => _bleed;

	public Burn burn => _burn;

	public Freeze freeze => _freeze;

	public Stun stun => _stun;

	public static CharacterStatusSetting instance
	{
		get
		{
			if ((Object)(object)_instance == (Object)null)
			{
				_instance = Resources.Load<CharacterStatusSetting>("CharacterStatusSetting");
			}
			return _instance;
		}
	}
}
