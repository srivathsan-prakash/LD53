using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Spill : MonoBehaviour
{
	private List<PlayerMovement> touchingPlayers = new List<PlayerMovement>();

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
