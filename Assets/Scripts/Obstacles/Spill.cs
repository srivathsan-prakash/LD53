using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Spill : MonoBehaviour
{
	public int requiredLicks = 3;

	private List<PlayerMovement> touchingPlayers = new List<PlayerMovement>();
	private int currentLicks = 0;

	public void IncrementLicks() {
		currentLicks++;
		if(currentLicks >= requiredLicks) {
			foreach(PlayerMovement p in touchingPlayers) {
				p.MakeSlippery(false);
			}
			Events.SpillCleaned?.Invoke();
			Destroy(gameObject);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		PlayerMovement player = collision.GetComponent<PlayerMovement>();
		if(player != null && !touchingPlayers.Contains(player)) {
			touchingPlayers.Add(player);
			player.MakeSlippery(true);
		}
	}

	private void OnTriggerExit2D(Collider2D collision) {
		PlayerMovement player = collision.GetComponent<PlayerMovement>();
		if(player != null) {
			touchingPlayers.Remove(player);
			player.MakeSlippery(false);
		}
	}
}
