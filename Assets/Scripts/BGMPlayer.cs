using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
	private void Start() {
		Events.PlayMusic("BGM");
	}
}
