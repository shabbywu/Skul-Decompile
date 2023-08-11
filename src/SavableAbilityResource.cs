using UnityEngine;

[CreateAssetMenu]
public class SavableAbilityResource : ScriptableObject
{
	private static SavableAbilityResource _instance;

	[SerializeField]
	[Header("Curse")]
	private AudioClip _curseAttachSound;

	[SerializeField]
	private RuntimeAnimatorController _curseAttachEffect;

	[SerializeField]
	private RuntimeAnimatorController _curseAttackEffect;

	[SerializeField]
	private Sprite _curseIcon;

	[SerializeField]
	[Header("FogWolf")]
	private Sprite _fogWolfPhysicalAttackDamage;

	[SerializeField]
	private Sprite _fogWolfMagicalAttackDamage;

	[SerializeField]
	private Sprite _fogWolfAttackSpeed;

	[SerializeField]
	private Sprite _fogWolfCriticalChance;

	[SerializeField]
	private Sprite _fogWolfHealth;

	[SerializeField]
	[Header("품목순환장치")]
	private RuntimeAnimatorController _cloverShieldEffect;

	private Sprite[] _fogWolfBuffIcons;

	public AudioClip curseAttachSound => _curseAttachSound;

	public RuntimeAnimatorController curseAttachEffect => _curseAttachEffect;

	public RuntimeAnimatorController curseAttackEffect => _curseAttackEffect;

	public Sprite curseIcon => _curseIcon;

	public Sprite[] fogWolfBuffIcons => _fogWolfBuffIcons;

	public RuntimeAnimatorController cloverShieldEffect => _cloverShieldEffect;

	public static SavableAbilityResource instance
	{
		get
		{
			if ((Object)(object)_instance == (Object)null)
			{
				_instance = Resources.Load<SavableAbilityResource>("SavableAbilityResource");
				_instance.Initialize();
			}
			return _instance;
		}
	}

	private void Initialize()
	{
		_fogWolfBuffIcons = (Sprite[])(object)new Sprite[5] { _fogWolfPhysicalAttackDamage, _fogWolfMagicalAttackDamage, _fogWolfAttackSpeed, _fogWolfCriticalChance, _fogWolfHealth };
	}
}
