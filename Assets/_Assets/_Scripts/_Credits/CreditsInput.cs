using UnityEngine;
using System.Collections;

public class CreditsInput : MonoBehaviour {
	#region Initializers & Destructors
	
	void OnEnable () 
	{
		InputManager.Instance.onButtonDown += OnButton;
	}
	
	void OnDisable()
	{
		if(InputManager.Instance != null)
		{
			InputManager.Instance.onButtonDown -= OnButton;
		}
	}
	
	#endregion
	
	void OnButton(int joyNumber, XboxButton btn)
	{
		if(btn == XboxButton.buttonB)
		{
			Application.LoadLevel("mainmenu");

		}
	}
}
