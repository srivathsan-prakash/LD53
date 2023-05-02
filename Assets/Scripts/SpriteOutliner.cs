using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteOutliner : MonoBehaviour
{
	[SerializeField] private SpriteRenderer mainRend;
	[SerializeField] private SpriteRenderer outlineRend;

	public bool matchSprite = true;

	public void EnableOutline(bool enable) {
		if(matchSprite) {
			outlineRend.sprite = mainRend.sprite;
		}
		outlineRend.enabled = enable;
	}
}
