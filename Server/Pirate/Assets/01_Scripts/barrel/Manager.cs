using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;
using UnityEngine.UI;
using DummyClient;
using Unity.VisualScripting;

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
		Player2 = 1,
		None
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
	public bool[] isHole = new bool[16];

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
				UpdateReady();
				break;

			case GameProgress.Turn:
				UpdateTurn();
				break;

			case  GameProgress.Result:
				{
					//StartCoroutine(Delay());
				}
				break;

			case GameProgress.GameOver:
				//UpdateGameOver();
				break;
		}
	}

	IEnumerator Delay()
	{
		yield return new WaitForSeconds(1f);
		progress = GameProgress.Turn;
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

	// 시합 시작 전의 신호표시 시간.
	private const float waitTime = 1.0f;
	private float currentTime;

	public void UpdateReady()
	{
		currentTime += Time.deltaTime;
		//Debug.Log("UpdateReady");

		if (currentTime > waitTime)
		{
			// 게임 시작입니다.
			progress = GameProgress.Turn;
		}
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

		/*		if (winner == Winner.Player1)
				{
					progress = GameProgress.Result;
				}*/

		print(winner);
		if (winner != Winner.None)
		{
			//승리한 경우는 사운드효과를 냅니다.
			if ((winner == Winner.Player1 && localPlayer == Player.Player1)
				|| (winner == Winner.Player2 && localPlayer == Player.Player2))
			{

			}
			//BGM재생종료.

			// 게임 종료입니다.
			progress = GameProgress.Result;
		}

		playerTurn = playerTurn == Player.Player1 ? Player.Player2 : Player.Player1;
		print(progress);
	}

	private float timer;
	int index = 0;
	bool isClicked = false;

	public void HoleSelect(int i)
	{
		isHole[i] = true;
	}

	public void SetIndex(int i)
	{
		index = i;
		isClicked = true;
	}

	private bool DoOwnTurn()
	{
		if (isClicked)
		{
			C_SelectHole selctHole = new C_SelectHole();
			selctHole.holeNumber = index;

			if (playerTurn == Player.Player1)
			{
				selctHole.destinationId = (int)Player.Player2 + 1;
			}
			else
			{
				selctHole.destinationId = (int)Player.Player1 + 1;
			}

			networkManager.Send(selctHole.Write());
			isClicked = false;
			return true;
		}
		return false;
	}

	private bool DoOppnentTurn()
	{
		int index1 = PlayerManager.Instance.ReturnHole();
		print(index1);
		if (index1 <= 0)
		{
			// 아직 수신되지 않았습니다.
			Debug.Log($"수신된 값 : {index}");
			return false;
		}

		Player mark = (networkManager.IsServer() == true) ? Player.Player2 : Player.Player1;
		holes[index1 - 2].SetSelected();
		print(mark);

		print(playerTurn);

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
			holes[i].ResetGame(i);
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
		progress = GameProgress.GameOver;
		print(progress);
	}
}
