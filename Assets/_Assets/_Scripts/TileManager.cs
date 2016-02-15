using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TileManager :  Singleton<TileManager> 
{
	public GameObject tile;
	public Dictionary<string, Tile> _tiles;

	bool _hasEnded;
	public int _tickCount;

	#region Initializers & Destructors

	void OnEnable () 
	{
		GameManager.Instance.onEndGame += End;
	}
	
	void OnDisable()
	{
		if(gameObject.GetComponent<PlayerInput>() != null)
		{
			GameManager.Instance.onEndGame -= End;
		}
	}

	#endregion

	void Start()
	{
		_hasEnded = false;
		_tickCount = 0;

		_tiles = new Dictionary<string, Tile>();
		GameObject go;
		Tile t;
		for(int x = 0; x < 18; x++)
		{
			for(int y = 0; y < 9; y++)
			{
				go = GameObject.Instantiate(tile, new Vector3(x, 0, y), Quaternion.Euler(0, 180, 0)) as GameObject;
				go.name = "Tile_" + x + "_" + y;
				t = go.GetComponent<Tile>();

				_tiles.Add("tile_" + x.ToString() + "_" + y.ToString(), t);


				int sort = Random.Range(0, 3);
				if(sort == 0)
					t.Setup(TileType.Water);
				else if(sort == 1)
					t.Setup(TileType.Fire);
				else
					t.Setup(TileType.Grass);

				go.transform.parent = transform;
			}
		}

		foreach(KeyValuePair<string, Tile> entry in _tiles)
		{
			entry.Value.LoadSibling();
		}

		StartCoroutine(Tick ());
	}

	void End()
	{
		_hasEnded = true;
		
	}

	public Tile getTileAt(int x, int y)
	{
		if(_tiles.ContainsKey("tile_" + x.ToString() + "_" + y.ToString()))
		{
			return _tiles["tile_" + x.ToString() + "_" + y.ToString()];
		}
		return null;
	}

	IEnumerator Tick()
	{
		while(!_hasEnded)
		{
			var items = items = from pair in _tiles	orderby pair.Key ascending select pair;
			int rnd = Random.Range (0,3);
			if(rnd == 0)
				items = from pair in _tiles	orderby pair.Key descending select pair;

			foreach(KeyValuePair<string, Tile> entry in items)
			{
				if(entry.Value._type != TileType.Empty)
				{
					entry.Value.Battle();
				}
			}
			//Nasce o token desse turno
			TokenLogic.Instance.Born();
			_tickCount++;
			yield return new WaitForSeconds(10.0f);
		}
	}

}
