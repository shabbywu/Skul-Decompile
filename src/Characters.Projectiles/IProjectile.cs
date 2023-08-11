using Characters.Utils;
using UnityEngine;

namespace Characters.Projectiles;

public interface IProjectile : IMonoBehaviour
{
	Character owner { get; }

	Collider2D collider { get; }

	float baseDamage { get; }

	float speedMultiplier { get; }

	Vector2 firedDirection { get; }

	Vector2 direction { get; set; }

	float speed { get; }

	void Despawn();

	void DetectCollision(Vector2 origin, Vector2 direction, float speed);

	void ClearHitHistroy();

	void Fire(Character owner, float attackDamage, float direction, bool flipX = false, bool flipY = false, float speedMultiplier = 1f, HitHistoryManager hitHistoryManager = null, float delay = 0f);
}
