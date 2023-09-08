using Data;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace Hardmode.Darktech;

public sealed class DarktechManager : Singleton<DarktechManager>
{
	public struct DarktechByLevel
	{
		public DarktechData.Type[] types;
	}

	private DarktechByLevel[] _darkTechByLevels = new DarktechByLevel[11]
	{
		new DarktechByLevel
		{
			types = new DarktechData.Type[2]
			{
				DarktechData.Type.SkullManufacturingMachine,
				DarktechData.Type.SuppliesManufacturingMachine
			}
		},
		new DarktechByLevel
		{
			types = new DarktechData.Type[1] { DarktechData.Type.OmenAmplifier }
		},
		new DarktechByLevel
		{
			types = new DarktechData.Type[1] { DarktechData.Type.ItemRotationEquipment }
		},
		new DarktechByLevel
		{
			types = new DarktechData.Type[1] { DarktechData.Type.HopeExtractor }
		},
		new DarktechByLevel
		{
			types = new DarktechData.Type[1] { DarktechData.Type.HealthAuxiliaryEquipment }
		},
		new DarktechByLevel
		{
			types = new DarktechData.Type[1] { DarktechData.Type.LuckyMeasuringInstrument }
		},
		new DarktechByLevel
		{
			types = new DarktechData.Type[1] { DarktechData.Type.InscriptionSynthesisEquipment }
		},
		new DarktechByLevel
		{
			types = new DarktechData.Type[1] { DarktechData.Type.NextGenerationHopeExtractor }
		},
		new DarktechByLevel
		{
			types = new DarktechData.Type[2]
			{
				DarktechData.Type.BoneParticleMagnetoscope,
				DarktechData.Type.GoldenCalculator
			}
		},
		new DarktechByLevel
		{
			types = new DarktechData.Type[1] { DarktechData.Type.AnxietyAccelerator }
		},
		new DarktechByLevel
		{
			types = new DarktechData.Type[1] { DarktechData.Type.ObservationInstrument }
		}
	};

	private DarktechSetting _setting;

	private DarktechDataStorage _storage;

	private DarktechResource _resource;

	public DarktechByLevel[] darkTechByLevels => _darkTechByLevels;

	public DarktechResource resource => _resource;

	public DarktechSetting setting => _setting;

	protected override void Awake()
	{
		base.Awake();
		_storage = new DarktechDataStorage();
		if ((Object)(object)_resource == (Object)null)
		{
			_resource = Resources.Load<DarktechResource>("HardmodeSetting/DarktechResource");
		}
		if ((Object)(object)_setting == (Object)null)
		{
			_setting = Resources.Load<DarktechSetting>("HardmodeSetting/DarktechSetting");
		}
		_setting.Initialize();
		Singleton<Service>.Instance.levelManager.onChapterLoaded += TryUnlock;
		TryUnlock();
	}

	private void TryUnlock()
	{
		if (Singleton<Service>.Instance.levelManager.currentChapter.type != Chapter.Type.HardmodeCastle)
		{
			return;
		}
		int clearedLevel = GameData.HardmodeProgress.clearedLevel;
		for (int i = 0; i < _darkTechByLevels.Length; i++)
		{
			if (i <= clearedLevel - 1)
			{
				DarktechData.Type[] types = _darkTechByLevels[i].types;
				foreach (DarktechData.Type type in types)
				{
					_storage.Activate(type);
				}
			}
			if (i > clearedLevel)
			{
				DarktechData.Type[] types = _darkTechByLevels[i].types;
				foreach (DarktechData.Type type2 in types)
				{
					_storage.Lock(type2);
				}
			}
			else
			{
				DarktechData.Type[] types = _darkTechByLevels[i].types;
				foreach (DarktechData.Type type3 in types)
				{
					UnlockDarktech(type3);
				}
			}
		}
	}

	public bool IsUnlocked(DarktechData data)
	{
		return _storage.IsUnlocked(data.type);
	}

	public bool IsUnlocked(DarktechData.Type type)
	{
		return _storage.IsUnlocked(type);
	}

	public bool IsActivated(DarktechData data)
	{
		return _storage.IsActivated(data.type);
	}

	public bool IsActivated(DarktechData.Type type)
	{
		return _storage.IsActivated(type);
	}

	public void ActivateDarktech(DarktechData.Type type)
	{
		_storage.Activate(type);
	}

	public void UnlockDarktech(DarktechData.Type type)
	{
		_resource.Find(type);
		_storage.Unlock(type);
	}
}
