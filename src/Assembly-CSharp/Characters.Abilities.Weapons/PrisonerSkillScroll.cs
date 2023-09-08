using Characters.Gear.Weapons;
using FX;
using GameResources;
using Level;
using Runnables.Triggers;
using Scenes;
using Singletons;
using UI.GearPopup;
using UnityEngine;
using UnityEngine.Rendering;

namespace Characters.Abilities.Weapons;

public class PrisonerSkillScroll : InteractiveObject
{
	private static short _sortingOrder;

	[SerializeField]
	private SoundInfo _lootSound;

	[Space]
	[SerializeField]
	private SpriteRenderer _iconHolder;

	[SerializeField]
	private DropMovement _dropMovement;

	[SerializeField]
	private SortingGroup _sortingGroup;

	[Trigger.Subcomponent]
	[SerializeField]
	private Trigger _activationTrigger;

	private Weapon _weapon;

	private PrisonerSkillInfosByGrade _skills;

	private SkillInfo _skillInfo;

	protected override void Awake()
	{
		base.Awake();
		_sortingGroup.sortingOrder = _sortingOrder;
		_sortingOrder++;
		_dropMovement.onGround += OnGround;
	}

	private void OnGround()
	{
		((Component)this).gameObject.layer = 12;
		Activate();
	}

	private void UpdateGearPopup(GearPopup gearPopup)
	{
		gearPopup.Set(_skillInfo.displayName, _skillInfo.description);
		string localizedString = Localization.GetLocalizedString("label/interaction/replace");
		string displayName = _weapon.currentSkills[0].displayName;
		string displayName2 = _weapon.currentSkills[1].displayName;
		PrisonerSkill component = ((Component)_weapon.currentSkills[0]).GetComponent<PrisonerSkill>();
		PrisonerSkill component2 = ((Component)_weapon.currentSkills[1]).GetComponent<PrisonerSkill>();
		if ((Object)(object)component.parent == (Object)(object)_skills)
		{
			_interactionType = CharacterInteraction.InteractionType.Normal;
			gearPopup.SetInteractionLabel(localizedString + " (" + displayName + ")");
		}
		else if ((Object)(object)component2.parent == (Object)(object)_skills)
		{
			_interactionType = CharacterInteraction.InteractionType.Normal;
			gearPopup.SetInteractionLabel(localizedString + " (" + displayName2 + ")");
		}
		else
		{
			_interactionType = CharacterInteraction.InteractionType.Pressing;
			gearPopup.SetInteractionLabel(this, localizedString + " (" + displayName + ")", localizedString + " (" + displayName2 + ")");
		}
	}

	public void SetSkillInfo(Weapon weapon, PrisonerSkillInfosByGrade skills, SkillInfo skillInfo)
	{
		_weapon = weapon;
		_skillInfo = skillInfo;
		_skills = skills;
		_iconHolder.sprite = _skillInfo.cachedIcon;
	}

	public override void OpenPopupBy(Character character)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		if (_activationTrigger.IsSatisfied())
		{
			pressingPercent = 0f;
			Vector3 position = ((Component)this).transform.position;
			Vector3 position2 = ((Component)character).transform.position;
			position.x = position2.x + ((position.x > position2.x) ? InteractiveObject._popupUIOffset.x : (0f - InteractiveObject._popupUIOffset.x));
			position.y += InteractiveObject._popupUIOffset.y;
			GearPopupCanvas gearPopupCanvas = Scene<GameBase>.instance.uiManager.gearPopupCanvas;
			UpdateGearPopup(gearPopupCanvas.gearPopup);
			gearPopupCanvas.Open(position);
		}
	}

	public override void ClosePopup()
	{
		base.ClosePopup();
		Scene<GameBase>.instance.uiManager.gearPopupCanvas.Close();
	}

	public override void InteractWith(Character character)
	{
		if (_activationTrigger.IsSatisfied())
		{
			PrisonerSkill component = ((Component)_weapon.currentSkills[0]).GetComponent<PrisonerSkill>();
			PrisonerSkill component2 = ((Component)_weapon.currentSkills[1]).GetComponent<PrisonerSkill>();
			if ((Object)(object)component.parent == (Object)(object)_skills)
			{
				ApplyScroll(0);
			}
			else if ((Object)(object)component2.parent == (Object)(object)_skills)
			{
				ApplyScroll(1);
			}
			else
			{
				ApplyScroll(0);
			}
		}
	}

	public override void InteractWithByPressing(Character character)
	{
		if (_activationTrigger.IsSatisfied())
		{
			ApplyScroll(1);
		}
	}

	private void ApplyScroll(int targetSkillIndex)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		ReplaceSkill(targetSkillIndex);
		PersistentSingleton<SoundManager>.Instance.PlaySound(_lootSound, ((Component)this).transform.position);
		((Component)this).gameObject.layer = 0;
		Deactivate();
		_dropMovement.Move();
	}

	private void ReplaceSkill(int targetSkillIndex)
	{
		PrisonerSkill component = ((Component)_weapon.currentSkills[targetSkillIndex]).GetComponent<PrisonerSkill>();
		SkillInfo skillInfo = _weapon.currentSkills[targetSkillIndex];
		_weapon.currentSkills[targetSkillIndex] = _skillInfo;
		SetSkillInfo(_weapon, component.parent, skillInfo);
		_weapon.SetSkillButtons();
		GearPopupCanvas gearPopupCanvas = Scene<GameBase>.instance.uiManager.gearPopupCanvas;
		UpdateGearPopup(gearPopupCanvas.gearPopup);
	}
}
