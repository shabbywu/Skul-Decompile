namespace Characters.Gear.Weapons.Gauges;

public sealed class WizardValueGauge : ValueGauge
{
	protected override string maxValueText => _currentValue.ToString();
}
