using System;
using EventData;
using UnityEngine;

public class GameManager : SingletonMono<GameManager>
{
    public GameEvent onSongStart;
    public GameEvent onEndGame;
    public GameEvent updateLeaderBoard;
    public GameEvent updateReward;

    private LevelData _levelData;
    private GameState _gameState;

    private int _coin;
    private int _gem;

    public void Awake()
    {
        _gameState = GameState.UI;
        _levelData = SceneManager.Instance.LevelData;
    }

    private void Start()
    {
        Invoke(nameof(StartGame), 3.0f);
    }

    public void StartGame()
    {
        int outValue;
        if (int.TryParse(_levelData.SongName, out outValue) == false)
        {
            Debug.LogError("Song name is not a number");
            return;
        }
        else
        {
            SongManager.Instance.ReadFromFile(outValue);
            _gameState = GameState.Play;
            _coin = 0;
            _gem = 0;
            onSongStart.Invoke(this, null);
        }
    }

    private void EndGame()
    {
        onEndGame.Invoke(this, new EndLevelData
        {
            Coin = _coin,
            Gem = _gem,
            Score = 0
        });
    }

    public void ProcessEndSong(Component sender, object data)
    {
        updateLeaderBoard.Invoke(this, new UpdateLeaderBoardReqInfo
        {
            Name = _levelData.SongName,
            Score = 0,
            SuccessCallback = EndGame
        });

        updateReward.Invoke(this, new RewardData
        {
            CoinKey = "CN",
            CoinAmount = 150,
            GemKey = "GM",
            GemAmount = 20
        });
    }


    public void PauseGame(Component sender, object data)
    {
        Time.timeScale = 0;
    }

    public void ResumeGame(Component sender, object data)
    {
        Time.timeScale = 1;
    }

    private void ProcessGameplay()
    {
    }

    private void Update()
    {
        switch (_gameState)
        {
            case GameState.UI:
                break;
            case GameState.Play:
                ProcessGameplay();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void AddReward(Component sender, object data)
    {
        var temp = (NoteReward)data;
        _coin += temp.Coin;
        _gem += temp.Gem;
    }
}

public enum GameState
{
    UI,
    Play
}