using UnityEngine;
using System.Collections;

public class ChargeResource : MonoBehaviour
{
	public Animator _anim;
	public bool _died;

	void Start()
	{
		_died = true;
		collider.enabled = false;
		GetComponent<SpriteRenderer>().enabled = false;
		_anim = GetComponent<Animator>();

	}

	public TileType GetResourceType()
	{
		return transform.parent.GetComponent<Tile>()._type;
	}

	public void Die()
	{
		_died = true;
		collider.enabled = false;
		GetComponent<SpriteRenderer>().enabled = false;
	}

	public void Born()
	{
		if(_died)
		{
			_died = false;
			collider.enabled = true;
			GetComponent<SpriteRenderer>().enabled = true;
			_anim.SetTrigger("Born");
		 }
	}
}
