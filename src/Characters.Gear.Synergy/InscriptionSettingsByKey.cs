using System;
using Characters.Gear.Synergy.Inscriptions;

namespace Characters.Gear.Synergy;

[Serializable]
public class InscriptionSettingsByKey : EnumArray<Inscription.Key, InscriptionSettings>
{
}
