using UnityEngine;
using System.Collections;

public enum TileType
{
	Empty = 0,
	Fire = 1,
	Water = 2,
	Grass = 3
}

public enum CombatResult
{
	None = 0,
	Damage = 1,
	Defense = 2,
	Health = 4,

}

public class Tile : MonoBehaviour 
{
	public int _x, _y;
	public int _stage;
	public TileType _type = TileType.Empty;
		
	public Material[] _tileMats;
	public Color[] _tints;

	public Tile[] _siblings;

#region Override and Operator
	public static bool operator ==(Tile a,Tile b)
	{
		// If both are null, or both are same instance, return true.
		if (System.Object.ReferenceEquals(a, b))
			return true;

		
		// If one is null, but not both, return false.
		if (((object)a == null) || ((object)b == null))
			return false;
				
		// Return true if the fields match:
		return a._x == b._x && a._y == b._y ;
	}

	public static bool operator !=(Tile a,Tile b)
	{
		return !(a == b);
	}

	public override bool Equals(object o)
	{
		Tile t = o as Tile;
		return this == t;
	}

	public override int GetHashCode()
	{ 
		return _x ^ _y;
	} 
#endregion

	void Awake()
	{
		_tints = new Color[4]{new Color(1,1,1), new Color(1,0.45f,0.45f), new Color(0.5f,0.7f,1), new Color(0.41f,1,0.41f)};
	}

	public void Setup(TileType type)
	{
		string[] n = this.name.Split('_');
		
		_x = int.Parse(n[1]);
		_y = int.Parse(n[2]);

		StartCoroutine( SetTypeWithTrasition(type));
	}

	public void SetType(TileType type)
	{
		_type = type;
		_stage = 1;
		gameObject.renderer.material = _tileMats[(int)type];
		gameObject.renderer.material.SetTexture("_DestinyTex", _tileMats[(int)type].mainTexture);
		gameObject.GetComponentInChildren<SpriteRenderer>().color = _tints[(int)_type];

		if(TileManager.Instance._tickCount > 1)
		{
			if(type != TileType.Empty)
			{
				gameObject.GetComponentInChildren<ChargeResource>().Born();
			}
			else
			{
				gameObject.GetComponentInChildren<ChargeResource>().Die();
			}
		}
	}

	public IEnumerator SetTypeWithTrasition(TileType type)
	{
		gameObject.renderer.material.SetTexture("_DestinyTex", _tileMats[(int)type].mainTexture);
		float pasttime = 0.0f;
		while(pasttime <= 1.0f)
		{
			pasttime += Time.deltaTime;
			gameObject.renderer.material.SetFloat("_PastTime", pasttime);
			yield return null;
		}
		SetType(type);
	}

	public IEnumerator Stomped(TileType type, bool spread)
	{
		StartCoroutine(SetTypeWithTrasition(type));

		float pasttime = 0.0f;
		while(pasttime <= 1)
		{
			pasttime += Time.deltaTime;
			transform.Translate(0, Mathf.Sin(pasttime * 360) * Time.deltaTime * 2.5f , 0);
			yield return null;

			if(pasttime >= 0.5 && spread)
			{
				spread = !spread;
				for(int i = 0; i < _siblings.Length; i++)
				{
					if(_siblings[i] != null)
					{
						if(_siblings[i]._type == TileType.Empty)
						{
							StartCoroutine(_siblings[i].Stomped(type, false));
						}
					}
				}
			}
		}

		Vector3 p = transform.position;
		p.y = 0;
		transform.position = p;
	}

	public void LoadSibling()
	{
		//Carregando irmaos
		_siblings = new Tile[8] { null, null, null, null, null, null, null, null };
		
		Tile temp = TileManager.Instance.getTileAt(_x-1,_y-1);
		int idx = 0; 
		if(temp != null) _siblings[idx++] = temp;
		
		temp = TileManager.Instance.getTileAt(_x,_y-1);
		if(temp != null) _siblings[idx++] = temp;
		
		temp = TileManager.Instance.getTileAt(_x+1,_y-1);
		if(temp != null) _siblings[idx++] = temp;
		
		temp = TileManager.Instance.getTileAt(_x-1,_y);
		if(temp != null) _siblings[idx++] = temp;
		
		temp = TileManager.Instance.getTileAt(_x+1,_y);
		if(temp != null) _siblings[idx++] = temp;
		
		temp = TileManager.Instance.getTileAt(_x-1,_y+1);
		if(temp != null) _siblings[idx++] = temp;
		
		temp = TileManager.Instance.getTileAt(_x,_y+1);
		if(temp != null) _siblings[idx++] = temp;
		
		temp = TileManager.Instance.getTileAt(_x+1,_y+1);
		if(temp != null) _siblings[idx++] = temp;
	}

	public void Battle()
	{
		int dmg = 0;
		int def = 0;
		int hp = 0;

		for(int i = 0; i < _siblings.Length; i++)
		{
			if(_siblings[i] != null)
			{
				switch(Combat(_type, _siblings[i]._type))
				{
					case CombatResult.Damage:
						dmg += _siblings[i]._stage; 
					break;
					case CombatResult.Defense:
						def += _siblings[i]._stage;
						break;
					case CombatResult.Health:
						hp++; 
						break;
				}
			}
		}
		int res = (def - dmg) + hp;

		//Se resultado positivo e estagio menor que 3, cresce!
		if(res > 2 && _stage < 3)
		{
			_stage++;
		}

		if(res >= 0)
		{
			//faz resnascer recurso
			gameObject.GetComponentInChildren<ChargeResource>().Born();
		}
		else
		{
			//Subtrai o dano.
			_stage += res;
			//Se chegou a estagio 0, morre e assume a outra forma
			if(_stage <= 0)
			{
				StartCoroutine( SetTypeWithTrasition(TileType.Empty));

				//SetType(TileType.Empty);
				if(TokenLogic.Instance._died)
				{
					if(Random.Range(0, 250) < _x * _y ||Random.Range(0, 250) > _x * _y)
					{
						TokenLogic.Instance._nextPosition = transform.position;
					}
				}
			}
		}
		
	}
	
	public CombatResult Combat(TileType own, TileType foe)
	{
		CombatResult res = CombatResult.None;
		switch(own)
		{
			case TileType.Fire:
				switch(foe)
				{
				case TileType.Fire:
					res = CombatResult.Defense;
					break;
				case TileType.Grass:
					res = CombatResult.Health;
					break;
				case TileType.Water:
					res = CombatResult.Damage;
					break;
				}
			break;
			case TileType.Water:
				switch(foe)
				{
				case TileType.Fire:
					res = CombatResult.Health;
					break;
				case TileType.Grass:
					res = CombatResult.Damage;
					break;
				case TileType.Water:
					res = CombatResult.Defense;
					break;
				}
			break;
			case TileType.Grass:
				switch(foe)
				{
				case TileType.Fire:
					res = CombatResult.Damage;
					break;
				case TileType.Grass:
					res = CombatResult.Defense;
					break;
				case TileType.Water:
					res = CombatResult.Health;
					break;
				}
			break;				
		}
		return res;
	}
}
