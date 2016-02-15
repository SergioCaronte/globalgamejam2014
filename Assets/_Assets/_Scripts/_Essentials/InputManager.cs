using UnityEngine;
using System.Collections;

/*
Buttons (Key or Mouse Button)	 Axis (Joystick Axis)
joystick button 0 = A	 
joystick button 1 = B	 
joystick button 2 = X	 
joystick button 3 = Y	 
joystick button 4 = L	
joystick button 5 = R	
joystick button 6 = Back	
joystick button 7 = Home
X axis = Left analog X
Y axis = Left analog Y
3rd axis = LT/RT
4th axis = Right analog X
5th axis = Right analog Y
6th axis = Dpad X
7th axis = Dpad Y
*/

public enum XboxButton
{
	buttonA = 1,
	buttonB = 2,
	buttonX = 4,
	buttonY = 8,
	buttonRB = 16,
	buttonLB = 32,
	buttonStart = 64
}

public class InputManager : Singleton<InputManager> {

	public float joySlope = 0.5f;

	public delegate void OnAxisChange(int joyNumber, float xAxis, float yAxis);
	public event OnAxisChange onAxisChange;

	public delegate void OnButtonDown(int joyNumber, XboxButton btn);
	public event OnButtonDown onButtonDown;

	public delegate void OnButtonHit(int joyNumber, XboxButton btn);
	public event OnButtonHit onButtonHit;

	void Start ()
	{
		Screen.showCursor = false;
	}

	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}

		//Stick movement event
		if(onAxisChange != null)
		{
			int vertical = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0;
			int horizontal = Input.GetKey(KeyCode.D) ? 1 : Input.GetKey(KeyCode.A) ? -1 : 0;
			onAxisChange(1, horizontal, vertical);


			vertical = Input.GetKey(KeyCode.Keypad8) ? 1 : Input.GetKey(KeyCode.Keypad5) ? -1 : 0;
			horizontal = Input.GetKey(KeyCode.Keypad6) ? 1 : Input.GetKey(KeyCode.Keypad4) ? -1 : 0;
			onAxisChange(2, horizontal, vertical);


			for(int i = 1; i <= Input.GetJoystickNames().Length; i++)
			{
				if(Mathf.Abs(Input.GetAxis("Joy"+i+"X")) > joySlope || Mathf.Abs (Input.GetAxis("Joy"+i+"Y")) > joySlope)
				{
					onAxisChange(i, Input.GetAxis("Joy"+i+"X"), Input.GetAxis("Joy"+i+"Y"));
				}
			}
		}

		//Press button event
		if(onButtonDown != null)
		{
			if(Input.GetKey(KeyCode.Return))
			{
				onButtonDown(1, XboxButton.buttonA);
			}
			if(Input.GetKey (KeyCode.Backspace))
			{
				onButtonDown(1, XboxButton.buttonB);
			}
			if(Input.GetKey (KeyCode.RightShift))
			{
				onButtonDown(1, XboxButton.buttonX);
			}

			if(Input.GetKey(KeyCode.KeypadEnter))
			{
				onButtonDown(2, XboxButton.buttonA);
			}
			if(Input.GetKey (KeyCode.KeypadMinus))
			{
				onButtonDown(2, XboxButton.buttonB);
			}
			if(Input.GetKey (KeyCode.KeypadPlus))
			{
				onButtonDown(2, XboxButton.buttonX);
			}
			if(Input.GetKey (KeyCode.KeypadMultiply))
			{
				onButtonDown(2, XboxButton.buttonStart);
			}

			for(int i = 1; i <= Input.GetJoystickNames().Length; i++)
			{
				if(Input.GetKey ("joystick "+i+" button 0"))
			    {
					onButtonDown(i, XboxButton.buttonA);
				}
				if(Input.GetKey ("joystick "+i+" button 1"))
				{
					onButtonDown(i, XboxButton.buttonB);
				}
				if(Input.GetKey ("joystick "+i+" button 2"))
				{
					onButtonDown(i, XboxButton.buttonX);
				}
				if(Input.GetKey ("joystick "+i+" button 3"))
				{
					onButtonDown(i, XboxButton.buttonY);
				}
				if(Input.GetKey ("joystick "+i+" button 7"))
				{
					onButtonDown(i, XboxButton.buttonStart);
				}
				if(Input.GetKey ("joystick "+i+" button 5"))
				{
					onButtonDown(i, XboxButton.buttonRB);
				}
				if(Input.GetKey ("joystick "+i+" button 4"))
				{
					onButtonDown(i, XboxButton.buttonLB);
				}
				if(Input.GetKey ("joystick "+i+" button 6"))
				{
					Application.Quit();
				}
			}
		}

		if(onButtonHit != null)
		{
			if(Input.GetKeyDown(KeyCode.Return))
			{
				onButtonHit(1, XboxButton.buttonA);
			}
			if(Input.GetKeyDown(KeyCode.Backspace))
			{
				onButtonHit(1, XboxButton.buttonB);
			}

			for(int i = 1; i <= Input.GetJoystickNames().Length; i++)
			{
				if(Input.GetKeyDown ("joystick "+i+" button 0"))
				{
					onButtonHit(i, XboxButton.buttonA);
				}
				if(Input.GetKeyDown ("joystick "+i+" button 1"))
				{
					onButtonHit(i, XboxButton.buttonB);
				}
			}
		}
	}
}