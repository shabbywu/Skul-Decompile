using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Characters;
using Characters.Player;
using Data;
using InControl;
using Level;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameResources;

[PreferBinarySerialization]
public class CommonResource : ScriptableObject
{
	public const string assets = "Assets";

	public const string resources = "Resources";

	private const string audio = "Audio/";

	public const string sfx = "Audio/Sfx/";

	public const string music = "Audio/Music/";

	public const string level = "Assets/Level/";

	public const string levelCastle = "Assets/Level/Castle/";

	public const string levelTest = "Assets/Level/Test/";

	public const string levelChapter1 = "Assets/Level/Chapter1/";

	public const string levelChapter2 = "Assets/Level/Chapter2/";

	public const string levelChapter3 = "Assets/Level/Chapter3/";

	public const string levelChapter4 = "Assets/Level/Chapter4/";

	public const string levelChapter5 = "Assets/Level/Chapter5/";

	public const string strings = "Strings";

	public const string shaders = "Shaders/";

	public const string enemy = "Assets/Enemies/";

	public const string enemyElite = "Assets/Enemies/Elite/";

	public const string enemyBoss = "Assets/Enemies/Boss/";

	public const string enemyChapter1 = "Assets/Enemies/Chapter1/";

	public const string enemyChapter2 = "Assets/Enemies/Chapter2/";

	public const string enemyChapter3 = "Assets/Enemies/Chapter3/";

	public const string enemyChapter4 = "Assets/Enemies/Chapter4/";

	public const string parts = "Parts/";

	public const string followers = "Followers/";

	public const string monsters = "Monsters/";

	public const string weaponDirectory = "Gear/Weapons/";

	public const string itemDirectory = "Gear/Items/";

	public const string quintessenceDirectory = "Gear/Quintessences/";

	public const string upgradeDirectory = "Upgrades/Objects/";

	[SerializeField]
	private AudioClip _bossRewardActiveSound;

	[SerializeField]
	private RuntimeAnimatorController _bossRewardActive;

	[SerializeField]
	private RuntimeAnimatorController _bossRewardDeactive;

	[SerializeField]
	private RuntimeAnimatorController _destroyWeapon;

	[SerializeField]
	private RuntimeAnimatorController _destroyItem;

	[SerializeField]
	private RuntimeAnimatorController _destroyEssence;

	public ParticleEffectInfo hitParticle;

	public ParticleEffectInfo reassembleParticle;

	[SerializeField]
	private Character _player;

	[SerializeField]
	private PlayerDieHeadParts _playerDieHeadParts;

	[SerializeField]
	[Space]
	private ParticleEffectInfo _freezeLargeParticle;

	[SerializeField]
	private ParticleEffectInfo _freezeMediumParticle;

	[SerializeField]
	private ParticleEffectInfo _freezeMediumParticle2;

	[SerializeField]
	private ParticleEffectInfo _freezeSmallParticle;

	[SerializeField]
	[Space]
	private RuntimeAnimatorController _freezeLarge;

	[SerializeField]
	private RuntimeAnimatorController _freezeMedium1;

	[SerializeField]
	private RuntimeAnimatorController _freezeMedium2;

	[SerializeField]
	private RuntimeAnimatorController _freezeSmall;

	[SerializeField]
	[Space]
	private Potion _smallPotion;

	[SerializeField]
	private Potion _mediumPotion;

	[SerializeField]
	private Potion _largePotion;

	[SerializeField]
	[Space]
	private Sprite _flexibleSpineIcon;

	[SerializeField]
	private Sprite _soulAccelerationIcon;

	[SerializeField]
	private Sprite _reassembleIcon;

	[SerializeField]
	private PoolObject _emptyEffect;

	[SerializeField]
	private PoolObject _vignetteEffect;

	[SerializeField]
	private PoolObject _screenFlashEffect;

	[SerializeField]
	private Sprite _curseOfLightIcon;

	[SerializeField]
	[Space]
	private RuntimeAnimatorController _curseOfLightAttachEffect;

	[SerializeField]
	private RuntimeAnimatorController _enemyInSightEffect;

	[SerializeField]
	private RuntimeAnimatorController _enemyAppearanceEffect;

	[SerializeField]
	private RuntimeAnimatorController _poisonEffect;

	[SerializeField]
	private RuntimeAnimatorController _slowEffect;

	[SerializeField]
	private RuntimeAnimatorController _bindingEffect;

	[SerializeField]
	private RuntimeAnimatorController _bleedEffect;

	[SerializeField]
	private RuntimeAnimatorController _stunEffect;

	[SerializeField]
	private RuntimeAnimatorController _swapEffect;

	[Space]
	[SerializeField]
	private GameObject _hardmodeChest;

	[SerializeField]
	[Space]
	private PoolObject _goldParticle;

	[SerializeField]
	private PoolObject _darkQuartzParticle;

	[SerializeField]
	private PoolObject _boneParticle;

	[SerializeField]
	private PoolObject _heartQuartzParticle;

	[SerializeField]
	private PoolObject _droppedSkulHead;

	[SerializeField]
	private PoolObject _droppedHeroSkulHead;

	[SerializeField]
	private Sprite _pixelSprite;

	[SerializeField]
	private Sprite _emptySprite;

	[SerializeField]
	private SpriteRenderer _footShadow;

	[SerializeField]
	private AssetReference _deathCamRenderTexture;

	[SerializeField]
	[Space]
	private Sprite[] _keyboardButtons;

	private Dictionary<string, Sprite> _keyboardButtonDictionary;

	[SerializeField]
	private Sprite[] _keyboardButtonsOutline;

	private Dictionary<string, Sprite> _keyboardButtonOutlineDictionary;

	[SerializeField]
	private Sprite[] _mouseButtons;

	private Dictionary<string, Sprite> _mouseButtonDictionary;

	[SerializeField]
	private Sprite[] _mouseButtonsOutline;

	private Dictionary<string, Sprite> _mouseButtonOutlineDictionary;

	[SerializeField]
	private Sprite[] _controllerButtons;

	private Dictionary<string, Sprite> _controllerButtonDictionary;

	[SerializeField]
	private Sprite[] _controllerButtonsOutline;

	private Dictionary<string, Sprite> _controllerButtonOutlineDictionary;

	[SerializeField]
	[Space]
	private Sprite[] _keywordIcons;

	[SerializeField]
	private Sprite[] _keywordFullactiveIcons;

	[SerializeField]
	private Sprite[] _keywordDeactiveIcons;

	public static CommonResource instance { get; private set; }

	public AudioClip bossRewardActiveSound => _bossRewardActiveSound;

	public RuntimeAnimatorController bossRewardActive => _bossRewardActive;

	public RuntimeAnimatorController bossRewardDeactive => _bossRewardDeactive;

	public RuntimeAnimatorController destroyWeapon => _destroyWeapon;

	public RuntimeAnimatorController destroyItem => _destroyItem;

	public RuntimeAnimatorController destroyEssence => _destroyEssence;

	public Character player => _player;

	public PlayerDieHeadParts playerDieHeadParts => _playerDieHeadParts;

	public ParticleEffectInfo freezeLargeParticle => _freezeLargeParticle;

	public ParticleEffectInfo freezeMediumParticle => _freezeMediumParticle;

	public ParticleEffectInfo freezeMediumParticle2 => _freezeMediumParticle2;

	public ParticleEffectInfo freezeSmallParticle => _freezeSmallParticle;

	public Potion smallPotion => _smallPotion;

	public Potion mediumPotion => _mediumPotion;

	public Potion largePotion => _largePotion;

	public EnumArray<Potion.Size, Potion> potions { get; private set; }

	public Sprite flexibleSpineIcon => _flexibleSpineIcon;

	public Sprite soulAccelerationIcon => _soulAccelerationIcon;

	public Sprite reassembleIcon => _reassembleIcon;

	public PoolObject emptyEffect => _emptyEffect;

	public PoolObject vignetteEffect => _vignetteEffect;

	public PoolObject screenFlashEffect => _screenFlashEffect;

	public Sprite curseOfLightIcon => _curseOfLightIcon;

	public RuntimeAnimatorController curseOfLightAttachEffect => _curseOfLightAttachEffect;

	public RuntimeAnimatorController enemyInSightEffect => _enemyInSightEffect;

	public RuntimeAnimatorController enemyAppearanceEffect => _enemyAppearanceEffect;

	public RuntimeAnimatorController poisonEffect => _poisonEffect;

	public RuntimeAnimatorController slowEffect => _slowEffect;

	public RuntimeAnimatorController bindingEffect => _bindingEffect;

	public RuntimeAnimatorController bleedEffect => _bleedEffect;

	public RuntimeAnimatorController stunEffect => _stunEffect;

	public RuntimeAnimatorController swapEffect => _swapEffect;

	public GameObject hardmodeChest => _hardmodeChest;

	public PoolObject goldParticle => _goldParticle;

	public PoolObject darkQuartzParticle => _darkQuartzParticle;

	public PoolObject boneParticle => _boneParticle;

	public PoolObject heartQuartzParticle => _heartQuartzParticle;

	public PoolObject droppedSkulHead => _droppedSkulHead;

	public PoolObject droppedHeroSkulHead => _droppedHeroSkulHead;

	public Sprite pixelSprite => _pixelSprite;

	public Sprite emptySprite => _emptySprite;

	public SpriteRenderer footShadow => _footShadow;

	public RenderTexture deathCamRenderTexture { get; private set; }

	public Dictionary<string, Sprite> keywordIconDictionary { get; private set; }

	public Dictionary<string, Sprite> keywordFullactiveIconDictionary { get; private set; }

	public Dictionary<string, Sprite> keywordDeactiveIconDictionary { get; private set; }

	public static string AssetPathToResourcesPath(string path)
	{
		return path.Replace('\\', '/');
	}

	public RuntimeAnimatorController GetFreezeAnimator(Vector2 pixelSize)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		int num = 1;
		while (true)
		{
			if (Fit(40 * num, 50 * num))
			{
				return _freezeSmall;
			}
			if (Fit(64 * num, 66 * num))
			{
				return _freezeMedium1;
			}
			if (Fit(79 * num, 71 * num))
			{
				return _freezeMedium2;
			}
			if (Fit(125 * num, 120 * num))
			{
				break;
			}
			num++;
		}
		return _freezeLarge;
		bool Fit(float x, float y)
		{
			if (pixelSize.x < x && pixelSize.y < y)
			{
				return true;
			}
			return false;
		}
	}

	public PoolObject GetCurrencyParticle(GameData.Currency.Type type)
	{
		return (PoolObject)(type switch
		{
			GameData.Currency.Type.Gold => _goldParticle, 
			GameData.Currency.Type.DarkQuartz => _darkQuartzParticle, 
			GameData.Currency.Type.Bone => _boneParticle, 
			GameData.Currency.Type.HeartQuartz => _heartQuartzParticle, 
			_ => null, 
		});
	}

	public Sprite GetKeyIconOrDefault(BindingSource bindingSource, bool outline = false)
	{
		if (TryGetKeyIcon(bindingSource, out var sprite, outline))
		{
			return sprite;
		}
		return (outline ? _controllerButtonOutlineDictionary : _controllerButtonDictionary)["unknown"];
	}

	public bool TryGetKeyIcon(BindingSource bindingSource, out Sprite sprite, bool outline = false)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Invalid comparison between Unknown and I4
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Invalid comparison between Unknown and I4
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Invalid comparison between Unknown and I4
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Invalid comparison between Unknown and I4
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Invalid comparison between Unknown and I4
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Invalid comparison between Unknown and I4
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Invalid comparison between Unknown and I4
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Invalid comparison between Unknown and I4
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Invalid comparison between Unknown and I4
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Invalid comparison between Unknown and I4
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Invalid comparison between Unknown and I4
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Invalid comparison between Unknown and I4
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Invalid comparison between Unknown and I4
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Invalid comparison between Unknown and I4
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Invalid comparison between Unknown and I4
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Invalid comparison between Unknown and I4
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Invalid comparison between Unknown and I4
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Invalid comparison between Unknown and I4
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Invalid comparison between Unknown and I4
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Invalid comparison between Unknown and I4
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Invalid comparison between Unknown and I4
		KeyBindingSource val = (KeyBindingSource)(object)((bindingSource is KeyBindingSource) ? bindingSource : null);
		if (val == null)
		{
			MouseBindingSource val2 = (MouseBindingSource)(object)((bindingSource is MouseBindingSource) ? bindingSource : null);
			if (val2 == null)
			{
				DeviceBindingSource val3 = (DeviceBindingSource)(object)((bindingSource is DeviceBindingSource) ? bindingSource : null);
				if (val3 != null)
				{
					InputControlType val4 = val3.Control;
					string text = ((object)(InputControlType)(ref val4)).ToString();
					if ((int)val3.Control == 101 || (int)val3.Control == 104 || (int)val3.Control == 308)
					{
						val4 = (InputControlType)106;
						text = ((object)(InputControlType)(ref val4)).ToString();
					}
					if ((int)val3.Control == 100 || (int)val3.Control == 109 || (int)val3.Control == 107 || (int)val3.Control == 105 || (int)val3.Control == 307)
					{
						val4 = (InputControlType)102;
						text = ((object)(InputControlType)(ref val4)).ToString();
					}
					Dictionary<string, Sprite> dictionary = (outline ? _controllerButtonOutlineDictionary : _controllerButtonDictionary);
					if (((int)((BindingSource)val3).DeviceStyle == 4 || (int)((BindingSource)val3).DeviceStyle == 5 || (int)((BindingSource)val3).DeviceStyle == 6 || (int)((BindingSource)val3).DeviceStyle == 7 || (int)((BindingSource)val3).DeviceStyle == 9 || (int)((BindingSource)val3).DeviceStyle == 8) && dictionary.TryGetValue("PS_" + text, out sprite))
					{
						return Object.op_Implicit((Object)(object)sprite);
					}
					if ((int)((BindingSource)val3).DeviceStyle == 17 || (int)((BindingSource)val3).DeviceStyle == 18 || (int)((BindingSource)val3).DeviceStyle == 15 || (int)((BindingSource)val3).DeviceStyle == 16 || (int)((BindingSource)val3).DeviceStyle == 21 || (int)((BindingSource)val3).DeviceStyle == 19 || (int)((BindingSource)val3).DeviceStyle == 20)
					{
						string text2 = text;
						val4 = (InputControlType)106;
						if (text2.Equals(((object)(InputControlType)(ref val4)).ToString(), StringComparison.InvariantCultureIgnoreCase))
						{
							val4 = (InputControlType)113;
							text = ((object)(InputControlType)(ref val4)).ToString();
						}
						string text3 = text;
						val4 = (InputControlType)102;
						if (text3.Equals(((object)(InputControlType)(ref val4)).ToString(), StringComparison.InvariantCultureIgnoreCase))
						{
							val4 = (InputControlType)114;
							text = ((object)(InputControlType)(ref val4)).ToString();
						}
						if (dictionary.TryGetValue("NSW_" + text, out sprite))
						{
							return Object.op_Implicit((Object)(object)sprite);
						}
					}
					return dictionary.TryGetValue(text, out sprite);
				}
				sprite = null;
				return false;
			}
			Dictionary<string, Sprite> obj = (outline ? _mouseButtonOutlineDictionary : _mouseButtonDictionary);
			Mouse control = val2.Control;
			return obj.TryGetValue(((object)(Mouse)(ref control)).ToString(), out sprite);
		}
		Dictionary<string, Sprite> obj2 = (outline ? _keyboardButtonOutlineDictionary : _keyboardButtonDictionary);
		KeyCombo control2 = val.Control;
		Key include = ((KeyCombo)(ref control2)).GetInclude(0);
		return obj2.TryGetValue(((object)(Key)(ref include)).ToString().Trim(), out sprite);
	}

	public static T[] LoadAll<T>(string path, string searchPattern, SearchOption searchOption = SearchOption.TopDirectoryOnly) where T : Object
	{
		return new T[0];
	}

	public static KeyValuePair<string, T>[] LoadAllWithPath<T>(string path, string searchPattern, SearchOption searchOption = SearchOption.TopDirectoryOnly) where T : Object
	{
		return new KeyValuePair<string, T>[0];
	}

	public void UpdateResource(bool updateWeaponUpgrades)
	{
	}

	public void Initialize()
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		instance = this;
		((Object)this).hideFlags = (HideFlags)(((Object)this).hideFlags | 0x20);
		potions = new EnumArray<Potion.Size, Potion>(_smallPotion, _mediumPotion, _largePotion);
		_keyboardButtonDictionary = _keyboardButtons.ToDictionary<Sprite, string>((Sprite sprite) => ((Object)sprite).name, StringComparer.OrdinalIgnoreCase);
		_keyboardButtonOutlineDictionary = _keyboardButtonsOutline.ToDictionary<Sprite, string>((Sprite sprite) => ((Object)sprite).name, StringComparer.OrdinalIgnoreCase);
		_mouseButtonDictionary = _mouseButtons.ToDictionary<Sprite, string>((Sprite sprite) => ((Object)sprite).name, StringComparer.OrdinalIgnoreCase);
		_mouseButtonOutlineDictionary = _mouseButtonsOutline.ToDictionary<Sprite, string>((Sprite sprite) => ((Object)sprite).name, StringComparer.OrdinalIgnoreCase);
		_controllerButtonDictionary = _controllerButtons.ToDictionary<Sprite, string>((Sprite sprite) => ((Object)sprite).name, StringComparer.OrdinalIgnoreCase);
		_controllerButtonOutlineDictionary = _controllerButtonsOutline.ToDictionary<Sprite, string>((Sprite sprite) => ((Object)sprite).name, StringComparer.OrdinalIgnoreCase);
		keywordIconDictionary = _keywordIcons.ToDictionary((Sprite sprite) => ((Object)sprite).name);
		keywordFullactiveIconDictionary = _keywordFullactiveIcons.ToDictionary((Sprite sprite) => ((Object)sprite).name.Split(new char[1] { '_' })[0]);
		keywordDeactiveIconDictionary = _keywordDeactiveIcons.ToDictionary((Sprite sprite) => ((Object)sprite).name.Split(new char[1] { '_' })[0]);
		deathCamRenderTexture = _deathCamRenderTexture.LoadAssetAsync<RenderTexture>().WaitForCompletion();
	}
}
