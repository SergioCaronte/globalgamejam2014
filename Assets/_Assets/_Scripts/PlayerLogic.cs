using UnityEngine;
using System.Collections;

public enum PlayerColor
{
	None = 0,
	Fire = 1,
	Water = 2,
	Grass = 3,
}

public class PlayerLogic : MonoBehaviour {

	public ParticleSystem[] psys;

	public float _lazySpeed = 2;
	public float _normalSpeed = 5;
	public float _maxSpeed = 10;
	public int _charge = 0;
	public int _score = 0;
	public TileType _lastCharged = TileType.Empty;

	public Tile _currentTile;
	public PlayerColor _color = PlayerColor.None;
	public bool _warpAble = true;

	private float _speed;
	private PlayerInput _input;
	private bool _hasEnded;

	#region Delegates & Events
	
	public delegate void OnColorChange(PlayerColor color);
	public event OnColorChange onColorChange;
		
	public delegate void OnScoreChange(int player, int score, int mult, int charge);
	public event OnScoreChange onScoreChange;
	
	#endregion

	#region Initializers & Destructors
	
	void OnEnable () 
	{
		gameObject.GetComponent<PlayerInput>().onAxisChange += Move;
		gameObject.GetComponent<PlayerInput>().onButtonDown += Act;
		GameManager.Instance.onEndGame += End;
	}
	
	void OnDisable()
	{
		if(gameObject.GetComponent<PlayerInput>() != null)
		{
			gameObject.GetComponent<PlayerInput>().onAxisChange -= Move;
			gameObject.GetComponent<PlayerInput>().onButtonDown -= Act;
		}
		if(GameManager.Instance != null)
		{
			GameManager.Instance.onEndGame -= End;
		}
	}
	
	void Start()
	{
		int x = Random.Range(1,16);
		int y = Random.Range(1,7);
		transform.position = new Vector3(x, 0.5f, y);

		_hasEnded = false;

		_currentTile = TileManager.Instance.getTileAt(x, y);

		_speed = _normalSpeed;

		_input = gameObject.GetComponent<PlayerInput>();

		if(onScoreChange != null)
			onScoreChange(_input._joyNumber, _score, 0, _charge);

		for(int i = 0; i < psys.Length; i++)
		{
			psys[i].enableEmission = false;
		}
		psys[15].enableEmission = true;
	}

	void End()
	{
		_hasEnded = true;
	}
	
	#endregion

	#region Move Methods
	void Move(float axisX, float axisY)
	{
		if(_hasEnded)
			return;

		//Verificando direçao que desejar se mover
		if(Mathf.Abs(axisX) > Mathf.Abs(axisY))
			axisY = 0;
		else
			axisX = 0;

		transform.Translate(_speed * Time.deltaTime * axisX, _speed * Time.deltaTime * axisY, 0);
		VerifyCurrentTile(axisX, axisY);
	}

	//Verifica qual o tile atual
	void VerifyCurrentTile(float axisX, float axisY)
	{
		Tile _newTile;
		int x, y;

		//frac part
		float fracX = transform.localPosition.x - Mathf.Floor(transform.localPosition.x);
		float fracZ = transform.localPosition.z - Mathf.Floor(transform.localPosition.z);
		
		//Change tile test
		if( (fracX > 0.85f || fracX < 0.15f) || (fracZ > 0.85f || fracZ < 0.15f))
		{
			x = Mathf.RoundToInt(transform.position.x);
			y = Mathf.RoundToInt(transform.position.z);
			
			_newTile = TileManager.Instance.getTileAt(x, y);
			if(_newTile == null)
			{
				transform.Translate(-_speed * Time.deltaTime * axisX, -_speed * Time.deltaTime * axisY, 0);
			}
			else if(_newTile != _currentTile)
			{
				ProcessNewTile(_newTile);
			}

			//Prevendo travar antes de buraco
			x = Mathf.RoundToInt(transform.position.x + (axisX*0.5f));
			y = Mathf.RoundToInt(transform.position.z + (axisY*0.5f));
			_newTile = TileManager.Instance.getTileAt(x, y);
			if(_newTile == null)
			{
				transform.Translate(-_speed * Time.deltaTime * axisX, -_speed * Time.deltaTime * axisY, 0);
			}
		}
	}

	//
	void ProcessNewTile(Tile t)
	{
		//_warpAble = false;
		_speed = _normalSpeed;

		_currentTile = t;
		switch(_currentTile._type)
		{
		case TileType.Fire:
			if(_color == PlayerColor.Fire)
			{
				_speed = _maxSpeed; 
			}
			else if(_color == PlayerColor.Grass)
			{
				_charge = _charge-1 > 0 ? _charge-1 : 0;
				if(_charge == 9)
				{
					psys[1].enableEmission = false;
					psys[2].enableEmission = false;
				}
				_speed = _lazySpeed; 
			}
			break;
		
		case TileType.Water:
			if(_color == PlayerColor.Water)
			{
				_speed = _maxSpeed; 
			}
			else if(_color == PlayerColor.Fire)
			{
				_charge = _charge-1 > 0 ? _charge-1 : 0;
				if(_charge == 9)
				{
					psys[4].enableEmission = false;
					psys[5].enableEmission = false;
				}
				_speed = _lazySpeed; 
			}
			break;
		
		case TileType.Grass:
			if(_color == PlayerColor.Grass)
			{
				_speed = _maxSpeed; 
			}
			else if(_color == PlayerColor.Water)
			{
				_charge = _charge-1 > 0 ? _charge-1 : 0;
				if(_charge == 9)
				{
					psys[7].enableEmission = false;
					psys[8].enableEmission = false;
				}
				_speed = _lazySpeed; 
			}
			break;
		case TileType.Empty:
			break;
		}
	}
	#endregion

	#region Act Methods
	void Act(XboxButton btn)
	{
		if(!_hasEnded)
		{
			/*if(_charge >= 10 || _warpAble)
			{
				if(btn == XboxButton.buttonA)
				{
					//_warpAble = false;
					_color = PlayerColor.Grass;
					onColorChange(PlayerColor.Grass);
				}
				else if(btn == XboxButton.buttonB)
				{
					//_warpAble = false;
					_color = PlayerColor.Fire;
					onColorChange(PlayerColor.Fire);
				}
				else if(btn == XboxButton.buttonX)
				{
					//_warpAble = false;
					_color = PlayerColor.Water;
					onColorChange(PlayerColor.Water);
				}
			}*/

			if(btn == XboxButton.buttonA && _currentTile._type == TileType.Empty && _charge >= 10)
			{
				StartCoroutine(_currentTile.Stomped((TileType)_color, true));

				//Chain de elemento diferente, vai aumentando o multiplicador
				int mul = 1;
				for(int i = 0; i < _currentTile._siblings.Length; i++)
				{
					if(_currentTile._siblings[i] != null && _currentTile._siblings[i]._type == TileType.Empty)
						mul++;
				}

				_score += _charge * mul;
								
				if(onScoreChange != null)
					onScoreChange(_input._joyNumber, _score, mul, _charge);

				//Volta a ser neutro
				_charge = 0;
				psys[(int)(_color-1) * 5 ].enableEmission = false;
				psys[(int)(_color-1) * 5 + 1].enableEmission = false;
				psys[(int)(_color-1) * 5 + 2].enableEmission = false;
				psys[(int)(_color-1) * 5 + 3].playOnAwake = true;
				psys[(int)(_color-1) * 5 + 3].enableEmission = true;
				psys[(int)(_color-1) * 5 + 4].playOnAwake = true;
				psys[(int)(_color-1) * 5 + 4].enableEmission = true;

				_color = PlayerColor.None;
				onColorChange(_color);
				//Air Move
				psys[15].enableEmission = true;

			}
		}
		else
		{
			if(btn == XboxButton.buttonA)
			{
				Application.LoadLevel("gameplay");
			}
			else if(btn == XboxButton.buttonB)
			{
				Application.LoadLevel("mainmenu");
			}
		}
	}
	
	#endregion

	#region Physics Methods
	void OnTriggerEnter(Collider col)
	{
		if(_hasEnded)
			return;

		if(col.tag.Equals("Player"))
		{
			PlayerColor foe = col.GetComponent<PlayerLogic>()._color;

			switch(_color)
			{
				case PlayerColor.Fire:
					switch(foe)
					{
					case PlayerColor.Fire:
						break;
					case PlayerColor.Grass:
						break;
					case PlayerColor.Water:
						_charge = 0;
						psys[(int)(_color-1) * 5].enableEmission = false;
						psys[(int)(_color-1) * 5 + 1].enableEmission = false;
						psys[(int)(_color-1) * 5 + 2].enableEmission = false;

						_color = PlayerColor.None;
						onColorChange(PlayerColor.None);
						//Air Move
						psys[15].enableEmission = true;

						break;
					}
				break;
				case PlayerColor.Water:
					switch(foe)
					{
					case PlayerColor.Fire:
						break;
					case PlayerColor.Grass:
						_charge = 0;
						psys[(int)(_color-1) * 5].enableEmission = false;
						psys[(int)(_color-1) * 5 + 1].enableEmission = false;
						psys[(int)(_color-1) * 5 + 2].enableEmission = false;
					
						_color = PlayerColor.None;
						onColorChange(PlayerColor.None);
						//Air Move
						psys[15].enableEmission = true;
						break;
					case PlayerColor.Water:
						break;
					}
				break;
				case PlayerColor.Grass:
					switch(foe)
					{
					case PlayerColor.Fire:
						_charge = 0;
						psys[(int)(_color-1) * 5].enableEmission = false;
						psys[(int)(_color-1) * 5 + 1].enableEmission = false;
						psys[(int)(_color-1) * 5 + 2].enableEmission = false;
						
						_color = PlayerColor.None;
						onColorChange(PlayerColor.None);
						//Air Move
						psys[15].enableEmission = true;
						break;
					case PlayerColor.Grass:
						break;
					case PlayerColor.Water:
						break;
					}
				break;				
			}
		}
		else if(col.tag.Equals("Resource"))
		{
			ChargeResource cr = col.gameObject.GetComponent<ChargeResource>();
			if((int)cr.GetResourceType() == (int)_color)
			{
				_charge++;
				cr.Die();

				//ativando o psys porque chegou ao charge 10
				if(_charge == 10)
				{
					psys[(int)(_color-1) * 5 + 1].enableEmission = true;
					psys[(int)(_color-1) * 5 + 2].enableEmission = true;
				}
			}
		}
		else if(col.tag.Equals("Token") && _color == PlayerColor.None)
		{
			//desativando o psys do elemento antigo

			psys[15].enableEmission = false;

			_color = (PlayerColor)Random.Range (1,4);
			onColorChange(_color);

			psys[(int)(_color-1) * 5].enableEmission = true;

			TokenLogic tl = col.gameObject.GetComponent<TokenLogic>();
			tl.Die();
		}
	}
	#endregion

}
