using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;
using UnityEngine.UI;
using DummyClient;

public class Manager : MonoBehaviour
{
	private enum GameProgress
	{
		None = 0,
		Ready,
		Turn,
		Result,
		GameOver,
		Disconnect,
	};

	private enum Turn
	{
		Own = 0,
		Opponent,
	};

	private enum Player
	{
		Player1 = 0,
		Player2,
	};
	[SerializeField] private GameObject pirate;

	private enum Winner
	{
		Player1 = 0,
		Player2 = 1
	};

	private const float turnTime = 100;
	private GameProgress progress;
	private Player playerTurn;
	private Player localPlayer;
	private Player romotePlayer;
	private Winner winner;

	public bool isGameOver;

	private NetworkManager networkManager;

	public static Manager Instance;

	[SerializeField] private Transform holeParent;
	private Transform upParent;
	private Transform downParent;

	[SerializeField] private GameObject holePref;
	[SerializeField] private Hole[] holes;

	[SerializeField] private int upHoleCnt = 8;
	[SerializeField] private int downHoleCnt = 8;

	private void Awake()
	{
		Instance = this;
		networkManager = FindObjectOfType<NetworkManager>();
		if (networkManager != null)
		{
			networkManager.RegisterEventHandler(EventCallback);
		}

		isGameOver = false;

		upParent = holeParent.Find("Up");
		downParent = holeParent.Find("Down");

		for (int i = 0; i < upHoleCnt; i++)
		{
			float angle = i * (Mathf.PI * 2.0f) / upHoleCnt;
			GameObject pref = Instantiate(holePref, upParent);
			pref.transform.position = transform.position + new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * 1.5f;
			pref.transform.localPosition = new Vector3(pref.transform.localPosition.x, 0.45f, pref.transform.localPosition.z);
			pref.transform.rotation = Quaternion.Euler(90, -(360f / upHoleCnt) * i - 180, 0);
		}

		for (int i = 0; i < downHoleCnt; i++)
		{
			float angle = i * (Mathf.PI * 2.0f) / downHoleCnt;
			GameObject pref = Instantiate(holePref, downParent);
			pref.transform.position = transform.position + new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * 1.5f;
			pref.transform.localPosition = new Vector3(pref.transform.localPosition.x, 0.25f, pref.transform.localPosition.z);
			pref.transform.rotation = Quaternion.Euler(90, -(360f / downHoleCnt) * i - 180, 0);
		}

		downParent.transform.rotation = Quaternion.Euler(0, -20f, 0);

		ResetGame();
	}

	private void Start()
	{
		ResetGame();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			ResetGame();
		}

		switch (progress)
		{
			case GameProgress.Ready:
				break;
			case GameProgress.Turn:
				break;
			case GameProgress.GameOver:
				break;
		}
	}

	public void GameStart()
	{
		progress = GameProgress.Ready;

		playerTurn = 0;
		if (networkManager.IsServer())
		{
			localPlayer = Player.Player1;
			romotePlayer = Player.Player2;
		}
		else
		{
			localPlayer = Player.Player2;
			romotePlayer = Player.Player1;
		}

		isGameOver = false;
	}

	public void UpdateReady()
	{

	}

	public void UpdateTurn()
	{
		print(playerTurn.ToString());
		bool setMark = false;
		if (playerTurn == localPlayer)
		{
			setMark = DoOwnTurn();

		}
		else
		{
			setMark = DoOppnentTurn();

		}

		if (setMark == false)
		{
			return;
		}
		else
		{
		}

		if (winner == Winner.Player1)
		{

			progress = GameProgress.Result;
		}
	}

	private bool DoOwnTurn()
	{
		int index = 0;

		byte[] buffer = new byte[1];
		buffer[0] = (byte)index;
		C_SelectHole selctHole = new C_SelectHole();
		selctHole.holeNumber = index;
		networkManager.Send(selctHole.Write());
		return true;
	}

	private bool DoOppnentTurn()
	{

		return true;
	}

	public bool IsGameOver()
	{
		return isGameOver;
	}

	public void EventCallback(NetEventState state)
	{
		switch (state.type)
		{
			case NetEventType.Disconnect:
				if (progress < GameProgress.Result && isGameOver == false)
				{
					progress = GameProgress.Disconnect;
				}
				break;
		}
	}

	void NotifyDisconnection()
	{
		string message = "서버 연결이 종료되었습니다";
	}

	public void ResetGame()
	{
		progress = GameProgress.None;

		holes = holeParent.GetComponentsInChildren<Hole>();
		for (int i = 0; i < holes.Length; i++)
		{
			holes[i].ResetGame();
		}

		int random = Random.Range(0, holes.Length);
		holes[random].SetBoom();
	}

	public void SetOtherScreen()
	{
		foreach (Hole h in holes)
		{
			if (h.IsSelected)
			{
				h.CheckSelected();
			}
		}
	}

	public void SetPirateBoom()
	{
		pirate.GetComponent<Rigidbody>().AddForce(new Vector3(0f, 10f, 0), ForceMode.VelocityChange);
	}
}
