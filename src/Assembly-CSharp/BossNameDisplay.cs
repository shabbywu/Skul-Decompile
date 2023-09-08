using GameResources;
using Scenes;
using UI.Boss;
using UnityEngine;

public class BossNameDisplay : MonoBehaviour
{
	[SerializeField]
	private string _nameKey;

	[SerializeField]
	private string _subNameKey;

	[SerializeField]
	private string _chapterNameKey;

	public void ShowAppearanceText()
	{
		Scene<GameBase>.instance.uiManager.bossUI.appearnaceText.Appear(Localization.GetLocalizedString(_nameKey), Localization.GetLocalizedString(_subNameKey));
	}

	public void HideAppearanceText()
	{
		Scene<GameBase>.instance.uiManager.bossUI.appearnaceText.Disappear();
	}

	public void ShowAndHideAppearanceText()
	{
		BossUIContainer bossUI = Scene<GameBase>.instance.uiManager.bossUI;
		((MonoBehaviour)bossUI).StartCoroutine(bossUI.appearnaceText.ShowAndHideText(Localization.GetLocalizedString(_nameKey), Localization.GetLocalizedString(_subNameKey)));
	}

	private void OnDestroy()
	{
		HideAppearanceText();
	}
}
