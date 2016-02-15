using UnityEngine;
using System.Collections;

public class GUIFade : MonoBehaviour {

	public float waitToStart;

	UILabel lbl;
	UISprite sprite;

	// Use this for initialization
	void Start () {
		lbl = GetComponentInChildren<UILabel>();
		sprite = GetComponentInChildren<UISprite>();
		if(sprite != null)	sprite.color = new Color(1, 1, 1, 0);
		if(lbl != null)		lbl.color = new Color(1, 1, 1, 0);

		StartCoroutine(Fade());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator Fade()
	{
		yield return new WaitForSeconds(waitToStart);

		float fadeSprite = -0.1f;
		float fadeLabel = 0;

		while(true)
		{
			fadeSprite += Time.deltaTime * 2;
			fadeLabel += Time.deltaTime * 2;

			if(sprite != null)	sprite.color = new Color(1, 1, 1, fadeSprite);
			if(lbl != null)		lbl.color = new Color(1, 1, 1, fadeLabel);

			yield return null;
		}
	}
}
