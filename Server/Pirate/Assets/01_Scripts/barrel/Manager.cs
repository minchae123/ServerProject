using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;

public class Manager : MonoBehaviour
{
	private enum GameProgress
	{
		None = 0,       // ���� ���� ��.
		Ready,          // ���� ���� ��ȣ ǥ��.
		Turn,           // ���� ��.
		Result,         // ��� ǥ��.
		GameOver,       // ���� ����.
		Disconnect,     // ���� ����.
	};

	// �� ����.
	private enum Turn
	{
		Own = 0,        // �ڻ��� ��.
		Opponent,       // ����� ��.
	};

	private enum Player
	{
		Player1 = 0,
		Player2,
	};
    [SerializeField] private GameObject pirate;

	private enum Winner
	{
		Player1 = 0,         // �۽¸�.
		Player2 = 1          // ���¸�.
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
        foreach(Hole h in holes)
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
