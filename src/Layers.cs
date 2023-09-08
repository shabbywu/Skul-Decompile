using UnityEngine;

public static class Layers
{
	public const int @default = 0;

	public const int terrain = 8;

	public const int playerBody = 9;

	public const int monsterBody = 10;

	public const int prop = 11;

	public const int interaction = 12;

	public const int playerProjectile = 15;

	public const int monsterProjectile = 16;

	public const int platform = 17;

	public const int terrainFoothold = 18;

	public const int projectileBlock = 19;

	public const int playerBlock = 23;

	public const int minimap = 25;

	public const int collideWithTerrain = 27;

	public static readonly LayerMask footholdMask = LayerMask.op_Implicit(393216);

	public static readonly LayerMask groundMask = LayerMask.op_Implicit(8782080);

	public static readonly LayerMask terrainMask = LayerMask.op_Implicit(8651008);

	public static readonly LayerMask terrainMaskForProjectile = LayerMask.op_Implicit(786688);
}
