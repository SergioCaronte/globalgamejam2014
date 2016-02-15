using UnityEngine;
using System.Collections;

public class TokenLogic : Singleton<TokenLogic> {

	public Vector3 _nextPosition = new Vector3(-1, -1, -1);

	public bool _died = true;

	public Animator _anim;

	void Start()
	{
		_anim = GetComponent<Animator>();
	}

	public void Die()
	{
		_died = true;
		collider.enabled = false;
		GetComponent<SpriteRenderer>().enabled = false;
		GetComponent<Collider>().enabled = false;
	}
	
	public void Born()
	{
		if(_nextPosition.x != -1 && _nextPosition.z != -1 && _died)
		{
			_anim.SetTrigger("Born");

			_died = false;
			_nextPosition.y = 1;
			transform.position = _nextPosition;
			collider.enabled = true;
			GetComponent<SpriteRenderer>().enabled = true;
			GetComponent<Collider>().enabled = true;
		}
	}
}
