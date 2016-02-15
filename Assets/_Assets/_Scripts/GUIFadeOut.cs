using UnityEngine;
using System.Collections;

public class GUIFadeOut : MonoBehaviour {

	public float waitToStart;

	UILabel lbl;
	UISprite sprite;

	// Use this for initialization
	void Start () {
		lbl = GetComponentInChildren<UILabel>();
		sprite = GetComponentInChildren<UISprite>();

		StartCoroutine(Fade());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator Fade()
	{
		yield return new WaitForSeconds(waitToStart);

		float fadeLabel = 1.0f;

		while(true)
		{
			fadeLabel -= Time.deltaTime * 2;
			if(sprite != null) sprite.color = new Color(1, 1, 1, fadeLabel);
			if(lbl != null) lbl.color = new Color(1, 1, 1, fadeLabel);

			yield return null;
		}
	}
}
