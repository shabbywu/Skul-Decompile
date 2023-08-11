using System;
using Characters.Abilities;
using UnityEngine;

namespace Characters.Player;

public class PlayerComponents : IDisposable
{
	public readonly Character character;

	public readonly Inventory inventory;

	public readonly CombatDetector combatDetector;

	public readonly MinionLeader minionLeader;

	public readonly Visibility visibility;

	public readonly SavableAbilityManager savableAbilityManager;

	public PlayerComponents(Character character)
	{
		this.character = character;
		inventory = new Inventory(character);
		combatDetector = new CombatDetector(character);
		minionLeader = new MinionLeader(character);
		visibility = ((Component)character).gameObject.AddComponent<Visibility>();
		savableAbilityManager = ((Component)character).GetComponent<SavableAbilityManager>();
	}

	public void Dispose()
	{
		minionLeader.Dispose();
	}

	public void Initialize()
	{
		inventory.Initialize();
	}

	public void Update(float deltaTime)
	{
		combatDetector.Update(deltaTime);
	}
}
