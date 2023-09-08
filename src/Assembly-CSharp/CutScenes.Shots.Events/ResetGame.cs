using Level;
using Services;
using Singletons;
using UnityEngine;

namespace CutScenes.Shots.Events;

public class ResetGame : Event
{
	[SerializeField]
	private bool _skulSpawnAnimation = true;

	[SerializeField]
	private Chapter.Type _type;

	public override void Run()
	{
		LevelManager levelManager = Singleton<Service>.Instance.levelManager;
		levelManager.skulSpawnAnimaionEnable = _skulSpawnAnimation;
		levelManager.ResetGame(_type);
	}
}
