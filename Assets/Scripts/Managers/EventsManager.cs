using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class SpawnLocation
{
	public Transform location;
	[HideInInspector] public GameObject spawned;
}

[System.Serializable]
public class SpawnableObstacle
{
	public GameObject obstacle;
	public SpawnLocation[] spawnLocations;
	public Vector2 spawnInterval;
	public int maxSpawned;
	[HideInInspector] public int currentSpawned;

	public SpawnLocation GetRandomPosition() {
		IEnumerable<SpawnLocation> open = spawnLocations.Where(x => x.spawned == null);
		if(open.Count() > 0) {
			return open.ElementAt(Random.Range(0, open.Count()));
		} else {
			return null;
		}
	}
}

public class EventsManager : MonoBehaviour
{
	public Transform obstacleParent;
	public SpawnableObstacle fire;
	public SpawnableObstacle spill;

	private bool isPlaying;
	private Coroutine fireRoutine;
	private Coroutine spillRoutine;

	private void OnEnable() {
		isPlaying = true;
		Events.FireExtinguished += FireExtinguished;
		Events.SpillCleaned += SpillCleaned;
		Events.EndGame += EndGame;
		fireRoutine = StartCoroutine(DoStartSpawn(fire));
		spillRoutine = StartCoroutine(DoStartSpawn(spill));
	}

	private void OnDisable() {
		EndGame();
	}

	private void EndGame() {
		isPlaying = false;
		Events.EndGame -= EndGame;
		Events.FireExtinguished -= FireExtinguished;
		Events.SpillCleaned -= SpillCleaned;
		if(fireRoutine != null) {
			StopCoroutine(fireRoutine);
		}
		if(spillRoutine != null) {
			StopCoroutine(spillRoutine);
		}
	}

	private IEnumerator DoStartSpawn(SpawnableObstacle o) {
		yield return new WaitForSeconds(Random.Range(o.spawnInterval.x, o.spawnInterval.y));
		StartCoroutine(DoSpawnObstacle(o));
	}

	private IEnumerator DoSpawnObstacle(SpawnableObstacle o) {
		while(isPlaying && o.currentSpawned < o.maxSpawned) {
			if(o.currentSpawned < o.maxSpawned) {
				SpawnLocation pos = o.GetRandomPosition();
				if(pos != null) {
					pos.spawned = Instantiate(o.obstacle, pos.location.position, Quaternion.identity, obstacleParent);
					o.currentSpawned++;
					if(o.currentSpawned < o.maxSpawned) {
						yield return new WaitForSeconds(Random.Range(o.spawnInterval.x, o.spawnInterval.y));
					}
				}
			}
		}
	}

	private IEnumerator RestartSpawnObstacle(SpawnableObstacle o) {
		yield return new WaitForSeconds(Random.Range(o.spawnInterval.x, o.spawnInterval.y));
		StartCoroutine(DoSpawnObstacle(o));
	}

	private void FireExtinguished() {
		fire.currentSpawned--;
		if(fire.currentSpawned == fire.maxSpawned - 1) {
			fireRoutine = StartCoroutine(RestartSpawnObstacle(fire));
		}
	}
	private void SpillCleaned() {
		spill.currentSpawned--;
		if(spill.currentSpawned == spill.maxSpawned - 1) {
			spillRoutine = StartCoroutine(RestartSpawnObstacle(spill));
		}
	}
}
