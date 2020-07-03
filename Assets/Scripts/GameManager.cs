using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityUtils;
using TMPro;

public enum GameState {
    MAIN_MENU,
    GAME,
    END_SCREEN,
    LEADERBOARD,
}

public class GameManager : Singleton<GameManager>
{
    public const string FULL_TRAJECTORY_KEY = "fullTrajectoryPurchased";
    public int TotalTime;
    public int TotalAttempts;
    public int scoreFromRing1;
    public int scoreFromRing2;
    public int scoreFromRing3;
    public int scoreFromTime;
    public int scoreFromLives;
    public GameObject _gamePrefab;
    public int MaxDistance;
    public int MaxForce;

    [SerializeField]
    private GameObject _trailPrefab;

    [SerializeField]
    private int[] _appcoinRewards;

    [SerializeField]
    private UIManager _uiManager;
    [SerializeField]
    private ScoreManager _scoreManager;
    [SerializeField]
    private RewardsController _rewardsController;

    [SerializeField]
    private GameObject _barrierMsgPrefab;

    [SerializeField]
    private GameObject _ringMsgPrefab;

    [SerializeField]
    private Canvas _canvas;
    private RectTransform _canvasRect;

    private GameState _state;
    private int _gameStartTime;
    private int _latestScore;
    private int _currentScore;
    private bool _touchedBarrier;
    private Spawner _spawner;

    private GameObject _gameInstance;
    private int _amountAttempts;
    private Attempt _currentAttempt;

    private bool _wentThroughRing1 = false;
    private bool _wentThroughRing2 = false;
    private bool _wentThroughRing3 = false;

    private bool _hasFullTrajectory = false;

    // Use this for initialization
    protected override void Awake()
    {
        base.Awake();

        Application.targetFrameRate = 60;

        GoToMainMenu();
        _canvasRect = _canvas.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_uiManager.TimeHasEnded() && _state != GameState.END_SCREEN)
        {
            EndGame();
        }
    }

    void ChangeState(GameState state)
    {
        _state = state;
    }

    public void StartCountdown()
    {
        _uiManager.GoToPreGame();
    }

    public void StartGame()
    {
        ResetScore();
        _rewardsController.Reset();

        _amountAttempts = TotalAttempts;
        _uiManager.GoToGame(TotalAttempts);
        _uiManager.StartTimer(TotalTime);

        _gameInstance = (GameObject)Instantiate(_gamePrefab, _gamePrefab.transform.position, _gamePrefab.transform.rotation);
        _spawner = _gameInstance.GetComponentInChildren<Spawner>();

        _wentThroughRing1 = false;
        _wentThroughRing2 = false;
        _wentThroughRing3 = false;

        StartTurn();

        ChangeState(GameState.GAME);
    }

    void StartTurn()
    {
        _spawner.SpawnProjectile();
        _currentAttempt = new Attempt();

        _touchedBarrier = false;
    }

    public void LostAttempt()
    {
        _amountAttempts--;
    }

    public void EndTurn()
    {
        _rewardsController.AddNewAttempt(_currentAttempt);

        bool allRingsHit = (_rewardsController.TimesCaughtRing1() > 0) && (_rewardsController.TimesCaughtRing2() > 0) && (_rewardsController.TimesCaughtRing3() > 0);
        if (_amountAttempts <= 0 || allRingsHit)
            EndGame();
        else
        {
            _uiManager.LostAttempt();
            StartTurn();
        }
    }

    void EndGame()
    {
        Destroy(_gameInstance);

        _rewardsController.timeLeft = (int)_uiManager.GetTimeLeft();

        int timeScore = ((int)_rewardsController.timeLeft) * scoreFromTime;
        int advantage1Score = _rewardsController.ScoreForRing(1);
        int advantage2Score = _rewardsController.ScoreForRing(2);
        int advantage3Score = _rewardsController.ScoreForRing(3);
        int livesScore = _amountAttempts * scoreFromLives;

        int totalScore = timeScore + advantage1Score + advantage2Score + advantage3Score;
        _latestScore = totalScore;

        ChangeState(GameState.END_SCREEN);
        _uiManager.GoToEndScreen();
    }

    public int GetLatestScore()
    {
        return _latestScore;
    }

    public void GoToScoreSubmission()
    {
        ChangeState(GameState.LEADERBOARD);
        _uiManager.GoToLeaderboard(_latestScore);
    }

    public void GoBack()
    {
        //TODO don't setup menu again
        _uiManager.GoBackToEndScreen();
    }

    public ScoreManager GetScoreManager()
    {
        return _scoreManager;
    }

    public UIManager GetUIManager()
    {
        return _uiManager;
    }

    public void OnTouchedBarrier(GameObject colliderObj)
    {
        _touchedBarrier = true;
        Barrier barrier = colliderObj.GetComponentInParent<Barrier>();
        if (barrier != null)
        {
            barrier.OnProjectileCollision();

            Vector3 posWithOffset = colliderObj.transform.position + new Vector3(-1.5f, -2f, 0);
            Vector2 pos = WorldToCanvasPosition(_canvas, _canvasRect, Camera.main, posWithOffset);
            GameObject msg = UIMessageSpawner.SpawnMessageOnPositionUsingPrefab(pos, _barrierMsgPrefab, _canvasRect);
            UIMessage uiMsg = msg.GetComponent<UIMessage>();
            uiMsg.SetupWithMessage(barrier.barrierMsg);
        }
    }

    public void WentThroughRing(GameObject ring)
    {
        //Don't take into account further touches after having touched a barrier
        if (_touchedBarrier)
            return;

        int pointsAwarded = 0;
        string powerUpMessage = "";

        if (ring.name.Contains("ring1"))
        {
            //Avoid double triggers
            if (_wentThroughRing1)
                return;
            _currentAttempt.caughtRing1 = true;
            pointsAwarded = scoreFromRing1;
            powerUpMessage = "81% Power-Up";
            _wentThroughRing1 = true;
            _currentScore += pointsAwarded;
            GetUIManager().UpdateScore(_currentScore);
        }
        else if (ring.name.Contains("ring2"))
        {
            //Avoid double triggers
            if (_wentThroughRing2)
                return;
            _currentAttempt.caughtRing2 = true;
            pointsAwarded = scoreFromRing2;
            powerUpMessage = "POA Power-Up";
            _wentThroughRing2 = true;
            _currentScore += pointsAwarded;
            GetUIManager().UpdateScore(_currentScore);
        }
        else if (ring.name.Contains("ring3"))
        {
            //Avoid double triggers
            if (_wentThroughRing3)
                return;
            _currentAttempt.caughtRing3 = true;
            pointsAwarded = scoreFromRing3;
            powerUpMessage = "Unity Power-Up";
            _wentThroughRing3 = true;
            _currentScore += pointsAwarded;
            GetUIManager().UpdateScore(_currentScore);
        }

        //FIX ME The offset is not working!
        Vector3 posWithOffset = ring.transform.position + new Vector3(0, 0f, 0);
        Vector2 pos = WorldToCanvasPosition(_canvas, _canvasRect, Camera.main, posWithOffset);
        GameObject msg = UIMessageSpawner.SpawnMessageOnPositionUsingPrefab(pos, _ringMsgPrefab, _canvasRect);

        RingUIMessage ringMsg = msg.GetComponent<RingUIMessage>();
        ringMsg.SetupWithMessageAndAdvantage("x " + pointsAwarded.ToString(), powerUpMessage);

        //TODO update score label
    }

    public RewardsController GetRewardsController()
    {
        return _rewardsController;
    }

    public Transform GetGameInstanceRoot()
    {
        if (_gameInstance != null)
            return _gameInstance.transform;
        return null;
    }

    public void ClearLeaderboardEntries()
    {
        if (_scoreManager != null)
        {
            _scoreManager.DeleteScoresFile();
        }

        //Check if we're in the leaderboard screen and immediately update it
        if (_state == GameState.LEADERBOARD)
        {
            GetUIManager().RefreshLeaderboard();
        }
    }

    public int GetAttemptsLeft()
    {
        return _amountAttempts;
    }

    Vector2 WorldToCanvasPosition(Canvas canvas, RectTransform canvasRect, Camera camera, Vector3 position)
    {
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(camera, position);
        Vector2 result;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : camera, out result);

        return canvas.transform.TransformPoint(result);
    }

    public int GetAppcoinsRewardForPlace(int place)
    {
        if (place > 5)
            return 0;
        return _appcoinRewards[place - 1];
    }

    public void ResetScore()
    {
        _currentScore = 0;
        GetUIManager().UpdateScore(_currentScore);
    }

    public void UpdateLatestScore(int score)
    {
        _latestScore = score;
    }

    public void GoToMainMenu()
    {
        _uiManager.GoToMainMenu();
    }

    public void DebugNoBarriers()
    {

        foreach (GameObject fooObj in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            fooObj.SetActive(false);
        }
    }

    #region Trajectory Plot
    //public static Vector2[] Plot(Rigidbody2D rigidbody, Vector2 pos, Vector2 velocity, int steps)
    //{
    //    Vector2[] results = new Vector2[steps];

    //    float timestep = Time.fixedDeltaTime / Physics2D.velocityIterations;
    //    Vector2 gravityAccel = Physics2D.gravity * rigidbody.gravityScale * timestep * timestep;
    //    float drag = 1f - timestep * rigidbody.drag;
    //    Vector2 moveStep = velocity * timestep;

    //    for (int i = 0; i < steps; ++i)
    //    {
    //        moveStep += gravityAccel;
    //        moveStep *= drag;
    //        pos += moveStep;
    //        results[i] = pos;
    //    }

    //    return results;
    //}

    //public void PlotProjectileTrajectory(Rigidbody2D rigidbody, Vector2 initialPos, Vector2 force)
    //{
    //    Vector2[] results = Plot(rigidbody, initialPos, force, 10);

    //    for (int i = 0; i < results.Length; i++)
    //    {
    //        Vector3 pos = new Vector3(results[i].x, results[i].y, 0);
    //        GameObject trailObject = _trailObjects[i];
    //        trailObject.SetActive(true);
    //        trailObject.transform.position = pos;
    //    }
    //}

    //public void HideTrajectoryPlot() {
    //    for (int i = 0; i < _trailLength; i++) {
    //        _trailObjects[i].SetActive(false);
    //    }
    //}
#endregion

    public void OnFullTrajectoryPurchased() {
        _hasFullTrajectory = true;
        EncryptedPlayerPrefs.SetInt(FULL_TRAJECTORY_KEY, 1);
        PlayerPrefs.Save();
    }

    public bool HasPurchasedFullTrajectory() {
        return EncryptedPlayerPrefs.GetInt(FULL_TRAJECTORY_KEY) == 1;
    }
}
