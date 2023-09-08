using System;
using UnityEngine;

namespace Hardmode.Darktech;

[CreateAssetMenu]
public sealed class DarktechResource : ScriptableObject
{
	[Serializable]
	private class Info
	{
		[SerializeField]
		internal DarktechData.Type _type;

		[SerializeField]
		internal int _level;

		[SerializeField]
		internal Sprite _icon;

		[SerializeField]
		internal string _displayName;
	}

	[Header("품목 순환 장치")]
	[SerializeField]
	private Sprite _smallCloverIcon;

	[SerializeField]
	private Sprite _bigCloverIcon;

	[SerializeField]
	private Sprite _brutalityBuffIcon;

	[SerializeField]
	private Sprite _rageBuffIcon;

	[SerializeField]
	private Sprite _fortitudeBuffIcon;

	[SerializeField]
	private RuntimeAnimatorController _omenChestSpawnEffect;

	[SerializeField]
	private Info[] _infos;

	public Sprite smallCloverIcon => _smallCloverIcon;

	public Sprite bigCloverIcon => _bigCloverIcon;

	public Sprite brutalityBuffIcon => _brutalityBuffIcon;

	public Sprite rageBuffIcon => _rageBuffIcon;

	public Sprite fortitudeBuffIcon => _fortitudeBuffIcon;

	public RuntimeAnimatorController omenChestSpawnEffect => _omenChestSpawnEffect;

	public (Sprite, string) Find(DarktechData.Type type)
	{
		Info[] infos = _infos;
		foreach (Info info in infos)
		{
			if (info._type == type)
			{
				return (info._icon, info._displayName);
			}
		}
		return (null, string.Empty);
	}
}
