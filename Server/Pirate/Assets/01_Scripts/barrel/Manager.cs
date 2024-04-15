using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Manager : MonoBehaviour
{
	private enum GameProgress
	{
		None = 0,       // 시합 시작 전.
		Ready,          // 시합 시작 신호 표시.
		Turn,           // 시함 중.
		Result,         // 결과 표시.
		GameOver,       // 게임 종료.
		Disconnect,     // 연결 끊기.
	};

	// 턴 종류.
	private enum Turn
	{
		Own = 0,        // 자산의 턴.
		Opponent,       // 상대의 턴.
	};

	private enum Player
	{
		Player1 = 0,
		Player2,
	};

	private enum Winner
	{
		Player1 = 0,         // ○승리.
		Player2 = 1          // ×승리.
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
	[SerializeField] private Hole[] holes;

	private void Awake()
	{
		Instance = this;
		networkManager = FindObjectOfType<NetworkManager>();
		if (networkManager != null)
		{
			networkManager.RegisterEventHandler(EventCallback);
		}

		isGameOver = false;
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

	private void GameStart()
	{
		progress = GameProgress.Ready;

		playerTurn = 0;
		if(networkManager.IsServer())
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

	private void UpdateReady()
	{

	}

	private void UpdateTurn()
	{

	}

	private void DoOwnTurn()
	{

	}

	private void DoDppnentTurn()
	{

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
		string message = "회선이 끊겼습니다.\n\n버튼을 누르세요.";

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
}
