using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MountRanking : MonoBehaviour {

	#region Initializers & Destructors
	
	void OnEnable () 
	{
		GameManager.Instance.onEndGame += End;
	}
	
	void OnDisable()
	{
		if(GameManager.Instance != null)
		{
			GameManager.Instance.onEndGame -= End;
		}
	}
	
	#endregion

	void End()
	{

		GameObject g1 = transform.FindChild("Ranking1").gameObject;
		g1.gameObject.SetActive(true);
		g1.transform.GetChild(0).gameObject.SetActive(true);
		g1.transform.GetChild(1).gameObject.SetActive(true);
		UILabel r1 = g1.GetComponentInChildren<UILabel>();

		GameObject g2 = transform.FindChild("Ranking2").gameObject;
		g2.gameObject.SetActive(true);
		g2.transform.GetChild(0).gameObject.SetActive(true);
		g2.transform.GetChild(1).gameObject.SetActive(true);
		UILabel r2 = g2.GetComponentInChildren<UILabel>();
		
		GameObject g3 = transform.FindChild("Ranking3").gameObject;
		g3.gameObject.SetActive(true);
		g3.transform.GetChild(0).gameObject.SetActive(true);
		g3.transform.GetChild(1).gameObject.SetActive(true);
		UILabel r3 = g3.GetComponentInChildren<UILabel>();
		
		GameObject g4 = transform.FindChild("Ranking4").gameObject;
		g4.gameObject.SetActive(true);
		g4.transform.GetChild(0).gameObject.SetActive(true);
		g4.transform.GetChild(1).gameObject.SetActive(true);
		UILabel r4 = g4.GetComponentInChildren<UILabel>();

		GameObject[] go = GameObject.FindGameObjectsWithTag("Player") as GameObject[];

		Dictionary<string, int> ranking = new Dictionary<string, int>();

		print (go.Length);
		for(int i = 0; i < go.Length; i++)
		{
			int score = go[i].GetComponent<PlayerLogic>()._score;
			print (go[i].name);
			ranking.Add (go[i].name, score);
		}
		// Acquire keys and sort them.
		var items = from pair in ranking orderby pair.Value ascending
				select pair;
		
		int l= 1;
		// Loop through keys.
		foreach (KeyValuePair<string, int> pair in items)
		{
			string t = pair.Key + " - " + pair.Value.ToString();

			if(l == 1)
				r4.text = t;
			else if(l == 2)
				r3.text = t;
			else if(l == 3)
				r2.text = t;
			else if(l == 4)
				r1.text = t;

			l++;
		}
	}
}
