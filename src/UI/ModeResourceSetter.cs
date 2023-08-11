using Data;
using GameResources;
using UnityEngine;
using UnityEngine.UI;

namespace UI;

public sealed class ModeResourceSetter : MonoBehaviour
{
	[SerializeField]
	private Image _playerFrame;

	[SerializeField]
	private Image _playerDavyJones;

	[SerializeField]
	private Image _playerQuintessenceFrame;

	[SerializeField]
	private Image _playerSkill2Frame;

	[SerializeField]
	private Image _playerSubBarFrame;

	[SerializeField]
	private Image _playerSubSkill1Frame;

	[SerializeField]
	private Image _playerSubSkill2Frame;

	[SerializeField]
	private Image _playerSubSkullFrame;

	[SerializeField]
	private Image _unlock;

	[SerializeField]
	private Image _minimap;

	private bool _isHardmode;

	private void Awake()
	{
		_isHardmode = GameData.HardmodeProgress.hardmode;
		if (_isHardmode)
		{
			SetHard();
		}
		else
		{
			SetNormal();
		}
	}

	private void Update()
	{
		if (!_isHardmode && GameData.HardmodeProgress.hardmode)
		{
			_isHardmode = true;
			SetHard();
		}
		else if (_isHardmode && !GameData.HardmodeProgress.hardmode)
		{
			_isHardmode = false;
			SetNormal();
		}
	}

	private void SetNormal()
	{
		_playerFrame.sprite = HUDResource.playerFrame.normal;
		_playerDavyJones.sprite = HUDResource.playerDavyJonesFrame.normal;
		_playerQuintessenceFrame.sprite = HUDResource.playerQuintessenceFrame.normal;
		_playerSkill2Frame.sprite = HUDResource.playerSkill2Frame.normal;
		_playerSubBarFrame.sprite = HUDResource.playerSubBarFrame.normal;
		_playerSubSkill1Frame.sprite = HUDResource.playerSubSkill1Frame.normal;
		_playerSubSkill2Frame.sprite = HUDResource.playerSubSkill2Frame.normal;
		_playerSubSkullFrame.sprite = HUDResource.playerSubSkullFrame.normal;
		_unlock.sprite = HUDResource.unlock.normal;
		_minimap.sprite = HUDResource.minimap.normal;
	}

	private void SetHard()
	{
		_playerFrame.sprite = HUDResource.playerFrame.hard;
		_playerDavyJones.sprite = HUDResource.playerDavyJonesFrame.hard;
		_playerQuintessenceFrame.sprite = HUDResource.playerQuintessenceFrame.hard;
		_playerSkill2Frame.sprite = HUDResource.playerSkill2Frame.hard;
		_playerSubBarFrame.sprite = HUDResource.playerSubBarFrame.hard;
		_playerSubSkill1Frame.sprite = HUDResource.playerSubSkill1Frame.hard;
		_playerSubSkill2Frame.sprite = HUDResource.playerSubSkill2Frame.hard;
		_playerSubSkullFrame.sprite = HUDResource.playerSubSkullFrame.hard;
		_unlock.sprite = HUDResource.unlock.hard;
		_minimap.sprite = HUDResource.minimap.hard;
	}
}
