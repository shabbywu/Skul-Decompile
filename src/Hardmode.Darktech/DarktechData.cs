using System;

namespace Hardmode.Darktech;

[Serializable]
public struct DarktechData
{
	public enum Type
	{
		SkullManufacturingMachine,
		SuppliesManufacturingMachine,
		OmenAmplifier,
		ItemRotationEquipment,
		HopeExtractor,
		HealthAuxiliaryEquipment,
		LuckyMeasuringInstrument,
		InscriptionSynthesisEquipment,
		NextGenerationHopeExtractor,
		BoneParticleMagnetoscope,
		GoldenCalculator,
		AnxietyAccelerator,
		ObservationInstrument
	}

	public Type type;

	public DarktechData(Type type)
	{
		this.type = type;
	}
}
