using Characters.Actions;
using CutScenes;
using Data;
using Level;
using Services;
using Singletons;
using SkulStories;
using UnityEngine;

namespace Characters.Gear.Weapons;

public class Skul : MonoBehaviour
{
	[SerializeField]
	private Action _spawn;

	[SerializeField]
	private Action _downButNotOut;

	[SerializeField]
	private Action _getSkul;

	[SerializeField]
	private Action _getScroll;

	[SerializeField]
	private Action _endPose;

	[SerializeField]
	private Action _introIdle;

	[SerializeField]
	private Action _introWalk;

	public Action spawn => _spawn;

	public Action downButNotOut => _downButNotOut;

	public Action getSkul => _getSkul;

	public Action getScroll => _getScroll;

	public Action endPose => _endPose;

	public Action introIdle => _introIdle;

	public Action introWalk => _introWalk;

	private void Start()
	{
		StartSkulSpawnAction();
		RemoveUsedSkulActions();
	}

	private void StartSkulSpawnAction()
	{
		LevelManager levelManager = Singleton<Service>.Instance.levelManager;
		if (levelManager.currentChapter.type != Chapter.Type.Castle && levelManager.currentChapter.type != Chapter.Type.HardmodeCastle)
		{
			levelManager.skulSpawnAnimaionEnable = true;
			return;
		}
		if (levelManager.skulSpawnAnimaionEnable)
		{
			_spawn.TryStart();
		}
		levelManager.skulSpawnAnimaionEnable = true;
	}

	private void RemoveUsedSkulActions()
	{
		LevelManager levelManager = Singleton<Service>.Instance.levelManager;
		if ((Object)(object)_getSkul != (Object)null && GameData.Progress.skulstory.GetData(SkulStories.Key.prologue))
		{
			levelManager.player.actions.Remove(_getSkul);
			Object.Destroy((Object)(object)((Component)_getSkul).gameObject);
			_getSkul = null;
		}
		if ((Object)(object)_getScroll != (Object)null && GameData.Generic.tutorial.isPlayed())
		{
			levelManager.player.actions.Remove(_getScroll);
			Object.Destroy((Object)(object)((Component)_getScroll).gameObject);
			_getScroll = null;
		}
		if (GameData.Progress.cutscene.GetData(CutScenes.Key.anotherWitch_Save))
		{
			levelManager.player.actions.Remove(_endPose);
			levelManager.player.actions.Remove(_introIdle);
			levelManager.player.actions.Remove(_introWalk);
			if ((Object)(object)_endPose != (Object)null)
			{
				Object.Destroy((Object)(object)((Component)_endPose).gameObject);
				_endPose = null;
			}
			if ((Object)(object)_introIdle != (Object)null)
			{
				Object.Destroy((Object)(object)((Component)_introIdle).gameObject);
				_introIdle = null;
			}
			if ((Object)(object)_introWalk != (Object)null)
			{
				Object.Destroy((Object)(object)((Component)_introWalk).gameObject);
				_introWalk = null;
			}
		}
	}
}
