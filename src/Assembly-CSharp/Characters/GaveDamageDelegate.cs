namespace Characters;

public delegate void GaveDamageDelegate(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt);
