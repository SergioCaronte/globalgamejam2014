using UnityEngine;
using System.Collections;

public class MenuParticleDelay : MonoBehaviour {

	public float _waitToStart;

	private ParticleSystem psys;

	// Use this for initialization
	void Start () {
		psys = GetComponent<ParticleSystem>();
		psys.enableEmission = false;
	
		StartCoroutine(Delay());
	}
	
	IEnumerator Delay()
	{
		yield return new WaitForSeconds(_waitToStart);

		psys.enableEmission = true;
	
	}
}
