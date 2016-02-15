using UnityEngine;
using System.Collections;

public class HowToNav : MonoBehaviour {

	public Texture[] pages;
	public int currPage;
	#region Initializers & Destructors
	
	void OnEnable () 
	{
		InputManager.Instance.onButtonHit += OnButton;
	}
	
	void OnDisable()
	{
		if(InputManager.Instance != null)
		{
			InputManager.Instance.onButtonHit -= OnButton;
		}
	}

	void Start()
	{
		currPage = 0;
	}

	#endregion

	void OnButton(int joyNumber, XboxButton btn)
	{
		if(btn == XboxButton.buttonA)
		{
			if(currPage < 2)
			{
				currPage++;
				GetComponent<GUITexture>().texture = pages[currPage];
			}
			else
			{
				Application.LoadLevel("mainmenu");
			}
		}
		else if(btn == XboxButton.buttonB)
		{
			if(currPage > 0)
			{
				currPage--;
				GetComponent<GUITexture>().texture = pages[currPage];
			}
		}
	}
}
