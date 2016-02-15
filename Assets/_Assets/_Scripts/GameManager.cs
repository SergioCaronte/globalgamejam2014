using UnityEngine;
using System.Collections;

public class GameManager : Singleton<GameManager> {

	public delegate void OnTimeChange(int min, int seg);
	public event OnTimeChange onTimeChange;

	public delegate void OnEndGame();
	public event OnEndGame onEndGame;

	public int _levelTime;
	float _timeOut;
	bool _warnedAboutEnd = false;

	// Use this for initialization
	void Start () 
	{
		_timeOut = PlayerPrefs.GetInt("PlayTime", _levelTime);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(_timeOut > 0)
		{
			_timeOut -= Time.deltaTime;

			int min = (int)_timeOut/60;
			int seg = (int)_timeOut%60;

			if(onTimeChange != null)
			{
				onTimeChange(min, seg);
			}
		}
		else if(!_warnedAboutEnd)
		{
			_warnedAboutEnd = true;
			if(onEndGame != null)
				onEndGame();
		}
	}
}
