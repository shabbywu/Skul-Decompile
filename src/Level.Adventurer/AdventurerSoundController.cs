using Characters;
using Characters.Player;
using CutScenes;
using Data;
using FX;
using Services;
using Singletons;
using UnityEngine;

namespace Level.Adventurer;

public class AdventurerSoundController : MonoBehaviour
{
	[SerializeField]
	private MusicInfo _musicInfo;

	[SerializeField]
	private MusicInfo _rockstarMusicInfo;

	[GetComponent]
	[SerializeField]
	private Collider2D _trigger;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Character component = ((Component)collision).GetComponent<Character>();
		if (!((Object)(object)component == (Object)null) && component.type == Character.Type.Player)
		{
			PlaySound();
		}
	}

	private void PlaySound()
	{
		if (!GameData.Progress.cutscene.GetData(CutScenes.Key.rookieHero))
		{
			PersistentSingleton<SoundManager>.Instance.StopBackGroundMusic();
			return;
		}
		WeaponInventory weapon = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.weapon;
		if (weapon.Has("RockStar") || weapon.Has("RockStar_2"))
		{
			PersistentSingleton<SoundManager>.Instance.PlayBackgroundMusic(_rockstarMusicInfo);
		}
		else
		{
			PersistentSingleton<SoundManager>.Instance.PlayBackgroundMusic(_musicInfo);
		}
	}
}
