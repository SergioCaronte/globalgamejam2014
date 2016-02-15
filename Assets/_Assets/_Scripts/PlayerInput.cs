using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {

	public int _joyNumber;

#region Delegates & Events

	public delegate void OnAxisChange(float xAxis, float yAxis);
	public event OnAxisChange onAxisChange;
	
	public delegate void OnButtonDown(XboxButton btn);
	public event OnButtonDown onButtonDown;

#endregion

#region Initializers & Destructors

	void OnEnable () 
	{
		InputManager.Instance.onAxisChange += OnMove;
		InputManager.Instance.onButtonDown += OnButton;
	}
	
	void OnDisable()
	{
		if(InputManager.Instance != null)
		{
			InputManager.Instance.onAxisChange -= OnMove;
			InputManager.Instance.onButtonDown -= OnButton;
		}
	}



#endregion

#region Joystick Events
	
	void OnMove(int joyNumber, float axisX, float axisY)
	{
		if(_joyNumber != joyNumber)
			return;

		if(onAxisChange != null)
		{
			onAxisChange(axisX, axisY);
		}
	}
	
	void OnButton(int joyNumber, XboxButton btn)
	{
		print ("Joy " + joyNumber + " Pressed " + btn.ToString());

		if(_joyNumber != joyNumber)
			return;

		if(onButtonDown != null)
		{
			onButtonDown(btn);
		}

		if(!GetComponent<SpriteRenderer>().enabled && btn == XboxButton.buttonStart)
		{
			GetComponent<PlayerLogic>().enabled = true;
			GetComponent<PlayerView>().enabled = true;
			GetComponent<SpriteRenderer>().enabled = true;
			GetComponent<Collider>().enabled = true;
			gameObject.SetActive(true);
			for(int i = 0; i < transform.childCount; i++)
				transform.GetChild(i).gameObject.SetActive(true);
		}
	}

#endregion





}
