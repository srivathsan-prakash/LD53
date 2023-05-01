using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
	public TextMeshProUGUI timerText;
	public GameObject endGamePopup;
	public TextMeshProUGUI endScoreText;
	public int maxSeconds = 300;

	private float currentSeconds = 0;

	private void Start() {
		currentSeconds = maxSeconds;
		timerText.text = TimeText(currentSeconds);
	}

	private void Update() {
		if(currentSeconds > 0) {
			currentSeconds -= Time.deltaTime;
			currentSeconds = Mathf.Max(currentSeconds, 0);
			timerText.text = TimeText(currentSeconds);

			if(currentSeconds <= 0) {
				EndGame();
			}
		}
	}

	private void EndGame() {
		Events.EndGame?.Invoke();
		endGamePopup.SetActive(true);
		endScoreText.text = $"${ScoreManager.Score()}";
	}

	private string TimeText(float timer) {
		int seconds = (int)timer;
		int minutes = 0;
		if (seconds >= 60) {
			minutes = seconds / 60;
			seconds = seconds % 60;
		}

		string secondsText = seconds < 10 ? "0" + seconds : seconds + "";
		string minutesText = minutes < 10 ? "0" + minutes : minutes + "";
		return minutesText + ":" + secondsText;
	}
}
