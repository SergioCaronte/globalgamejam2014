using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour {

	public UILabel _timerLabel;
	public UILabel[] _players;
	public UILabel[] _floatPlayers;

	#region Initializers & Destructors
	void OnEnable () 
	{
		GameManager.Instance.onTimeChange += TimeChange;
		GameObject[] go = GameObject.FindGameObjectsWithTag("Player");
		for(int i = 0; i < go.Length; i++)
		{
				go[i].GetComponent<PlayerLogic>().onScoreChange += PlayerScore;
		}
	}
	
	void OnDisable()
	{
		if(GameManager.Instance != null)
		{
			GameManager.Instance.onTimeChange -= TimeChange;
			GameObject[] go = GameObject.FindGameObjectsWithTag("Player");
			for(int i = 0; i < go.Length; i++)
			{
				go[i].GetComponent<PlayerLogic>().onScoreChange -= PlayerScore;
			}
		}
	}
	
	void Start()
	{
		for(int i = 0; i < _floatPlayers.Length; i++)
			_floatPlayers[i].enabled = false;
	
	}
	#endregion

	void TimeChange(int min, int seg)
	{
		string s = seg.ToString();
		if(s.Length == 1) s = "0" + s;
		_timerLabel.text = "[ffbb00]0" + min.ToString() + ":" + s;
	}

	void PlayerScore(int player, int score, int mult, int charge)
	{
		_players[player-1].text = "[ffff00]P" + player.ToString() + "[-]: " + score.ToString();
		if(score > 0)
			StartCoroutine(FloatPoints(player, score, mult, charge));
	}

	IEnumerator FloatPoints(int player,int score, int mult, int charge)
	{
		_floatPlayers[player-1].enabled = true;
		_floatPlayers[player-1].text = "[ffcccc]+" + charge.ToString(); 
		_floatPlayers[player-1].transform.Translate(0, -2, 0);

		float floatTime = 0.0f;
		while(floatTime < 2.0f)
		{
			_floatPlayers[player-1].transform.Translate(0, 1 * Time.deltaTime, 0);
			floatTime += Time.deltaTime;
			yield return null;
		}
		_floatPlayers[player-1].enabled = false;
	}
	
}
