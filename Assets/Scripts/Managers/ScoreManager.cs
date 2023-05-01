using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
	public TextMeshProUGUI scoreText;

	private int currentScore = 0;
	private static ScoreManager instance;

	private void Start() {
		if(instance == null) {
			instance = this;
		}
		UpdateText();
	}

	private void OnEnable() {
		Events.AddScore += IncrementScore;
	}

	private void OnDisable() {
		Events.AddScore -= IncrementScore;
	}

	private void IncrementScore(int score) {
		currentScore += score;
		UpdateText();
	}

	private void UpdateText() {
		scoreText.text = $"{(currentScore < 0 ? "-" : "")}${Mathf.Abs(currentScore)}";
		if(currentScore < 0) {
			scoreText.color = Color.red;
		} else {
			scoreText.color = Color.white;
		}
	}

	public static int Score() {
		return instance.currentScore;
	}
}
