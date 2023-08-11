using System;
using UnityEngine;

namespace GameResources;

public sealed class HUDResource : ScriptableObject
{
	[Serializable]
	public sealed class ResourceByMode
	{
		[SerializeField]
		private Sprite _normal;

		[SerializeField]
		private Sprite _hard;

		public Sprite normal => _normal;

		public Sprite hard => _hard;
	}

	[SerializeField]
	private ResourceByMode _playerFrame;

	[SerializeField]
	private ResourceByMode _playerDavyJonesFrame;

	[SerializeField]
	private ResourceByMode _playerQuintessenceFrame;

	[SerializeField]
	private ResourceByMode _playerSkill2Frame;

	[SerializeField]
	private ResourceByMode _playerSubBarFrame;

	[SerializeField]
	private ResourceByMode _playerSubSkill1Frame;

	[SerializeField]
	private ResourceByMode _playerSubSkill2Frame;

	[SerializeField]
	private ResourceByMode _playerSubSkullFrame;

	[SerializeField]
	private ResourceByMode _timerFrame;

	[SerializeField]
	private ResourceByMode _timerFrameWitchHardmodeLevel;

	[SerializeField]
	private ResourceByMode _unlock;

	[SerializeField]
	private ResourceByMode _minimap;

	public static ResourceByMode playerFrame { get; private set; }

	public static ResourceByMode playerDavyJonesFrame { get; private set; }

	public static ResourceByMode playerQuintessenceFrame { get; private set; }

	public static ResourceByMode playerSkill2Frame { get; private set; }

	public static ResourceByMode playerSubBarFrame { get; private set; }

	public static ResourceByMode playerSubSkill1Frame { get; private set; }

	public static ResourceByMode playerSubSkill2Frame { get; private set; }

	public static ResourceByMode playerSubSkullFrame { get; private set; }

	public static ResourceByMode timerFrame { get; private set; }

	public static ResourceByMode timerFrameWitchHardmodeLevel { get; private set; }

	public static ResourceByMode unlock { get; private set; }

	public static ResourceByMode minimap { get; private set; }

	public void Initialize()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		((Object)this).hideFlags = (HideFlags)(((Object)this).hideFlags | 0x20);
		playerFrame = _playerFrame;
		playerDavyJonesFrame = _playerDavyJonesFrame;
		playerQuintessenceFrame = _playerQuintessenceFrame;
		playerSkill2Frame = _playerSkill2Frame;
		playerSubBarFrame = _playerSubBarFrame;
		playerSubSkill1Frame = _playerSubSkill1Frame;
		playerSubSkill2Frame = _playerSubSkill2Frame;
		playerSubSkullFrame = _playerSubSkullFrame;
		timerFrame = _timerFrame;
		timerFrameWitchHardmodeLevel = _timerFrameWitchHardmodeLevel;
		unlock = _unlock;
		minimap = _minimap;
	}
}
