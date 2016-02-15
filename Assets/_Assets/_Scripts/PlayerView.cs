using UnityEngine;
using System.Collections;

public class PlayerView : MonoBehaviour {

	public Light _light;
	public Animator _animator;

	#region Initializers & Destructors
	void OnEnable () 
	{
		_light = GetComponentInChildren<Light>();
		gameObject.GetComponent<PlayerLogic>().onColorChange += ChangeOutfit;
		gameObject.GetComponent<PlayerLogic>().onScoreChange += Stomp;
	}
	
	void OnDisable()
	{
		if(gameObject.GetComponent<PlayerLogic>() != null)
		{
			gameObject.GetComponent<PlayerLogic>().onColorChange -= ChangeOutfit;
			gameObject.GetComponent<PlayerLogic>().onScoreChange -= Stomp;
		}
	}

	void Start()
	{
		//ChangeOutfit(gameObject.GetComponent<PlayerLogic>()._color);

		_animator = GetComponent<Animator>();
		if(_animator == null)
			print ("Animator NULL");
		_animator.SetInteger("color", (int)GetComponent<PlayerLogic>()._color);
	}
	#endregion

	void ChangeOutfit(PlayerColor color)
	{
		transform.FindChild("Change_Fx_Front").GetComponent<Animator>().SetTrigger("Change");
		transform.FindChild("Change_Fx_Back").GetComponent<Animator>().SetTrigger("Change");

		StartCoroutine(DelayToChange(color));
	}

	void Stomp(int i, int j, int k, int l)
	{
		_animator.SetTrigger("stomp");
	}

	IEnumerator DelayToChange(PlayerColor color)
	{
		yield return new WaitForSeconds(0.5f);

		_animator.SetInteger("color", (int)color);
		switch(color)
		{
		case PlayerColor.Fire:
			_light.color = new Color(1, 0, 0);
			break;
		case PlayerColor.Water:
			_light.color = new Color(0, 0.3f, 1);
			break;
		case PlayerColor.Grass:
			_light.color = new Color(0, 1, 0);
			break;
		case PlayerColor.None:
			_light.color = new Color(0.5f, 0.5f, 0.5f);
			break;
		}
	}

}
