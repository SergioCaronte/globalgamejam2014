using UnityEngine;
using System.Collections;

public class MainMenuInput : MonoBehaviour {
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

		if(btn == XboxButton.buttonA)
		{
			Application.LoadLevel("gameplay");
			
		}
		else if(btn == XboxButton.buttonB)
		{
			Application.LoadLevel("how to");
			
		}
		else if(btn == XboxButton.buttonX)
		{
			Application.LoadLevel("credits");
			
		}

	}
}
