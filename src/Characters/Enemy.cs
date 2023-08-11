using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Characters;

public sealed class Enemy : MonoBehaviour
{
	[SerializeField]
	private Character[] _characters;

	[SerializeField]
	private Behavior[] _behaviors;

	public ICollection<Character> characters => _characters;

	public ICollection<Behavior> behaviors => _behaviors;
}
