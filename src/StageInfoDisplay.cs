using GameResources;
using Scenes;
using UnityEngine;

public class StageInfoDisplay : MonoBehaviour
{
	[SerializeField]
	private string nameKey;

	[SerializeField]
	private string stageNumber;

	[SerializeField]
	private string subNameKey;

	private void Start()
	{
		Scene<GameBase>.instance.uiManager.stageName.Show(Localization.GetLocalizedString(nameKey), Localization.GetLocalizedString(stageNumber), Localization.GetLocalizedString(subNameKey));
	}
}
