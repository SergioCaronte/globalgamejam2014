using UnityEngine;
using System.Collections;

public class EndGameGUIHide : MonoBehaviour {

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
		for(int i = 0; i < transform.childCount; i++)
			transform.GetChild(i).gameObject.SetActive(false);
	}
}
