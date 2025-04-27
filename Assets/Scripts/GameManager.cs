using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using YG;

public class GameManager : MonoBehaviour
{
    private const string BestScore = "bestScore";
    private const string TotalDiamond = "totalDiamond";
    private const string TotalStar = "totalStar";
    private const string SelectAvto = "SelectAvto";
    private const string OldScore = "OldScore";

    public static GameManager Instance;

    [SerializeField] private PlatformSpawner _spawner;

    [Header("GameOver")]
    [SerializeField] private GameOverPanel _gameOverPanel;
    [SerializeField] private Image _newHightScoreImage;
    [SerializeField] private TMP_Text _lastScoreText;

    [Header("Score")]
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _bestText;
    [SerializeField] private TMP_Text _diamondText;
    [SerializeField] private TMP_Text _starText;

    [Header("For Player")]
    [SerializeField] private Player[] _players;

    private Vector3 _playerStartPosition = new Vector3(0, 2f, 0);

    private int _score;
    private int _bestScore;
    private int _totalDiamond = 0;
    private int _totalStar = 0;

    private int _selectedCar = 0;

    private bool _inGame;
    private bool _isGameStarted;

    private Player _player;

    private WaitForSeconds _wait;
    private float _delay = 1f;

    private Coroutine _coroutine;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        _wait = new WaitForSeconds(_delay);

        _selectedCar = PlayerPrefs.GetInt(SelectAvto);
        _player = Instantiate(_players[_selectedCar], _playerStartPosition, Quaternion.identity);
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt(OldScore) != 0)
        {
            _score = PlayerPrefs.GetInt(OldScore);
            PlayerPrefs.SetInt(OldScore, 0);
        }

        _totalDiamond = PlayerPrefs.GetInt(TotalDiamond);
        _diamondText.text = _totalDiamond.ToString();

        _totalStar = PlayerPrefs.GetInt(TotalStar);
        _starText.text = _totalStar.ToString();

        _bestScore = PlayerPrefs.GetInt(BestScore);
        _bestText.text = _bestScore.ToString();

        //ViewText(_totalDiamond, TotalDiamond, _diamondText);
        //ViewText(_totalStar, TotalStar, _starText);
        //ViewText(_bestScore, BestScore, _bestText);

        _scoreText.text = _score.ToString();
    }

    private void Update()
    {
        if (_isGameStarted == false)
            if (Input.GetMouseButtonDown(0))
                GameStart();
    }

    public Player GetPlayer() =>
        _player;

    public bool IsGameStarted() =>
        _isGameStarted;

    public void StartWithScore()
    {
        YG2.RewardedAdvShow("1", () =>
        {
            PlayerPrefs.SetInt(OldScore, _score);
            SceneManager.LoadScene(0);
        });
    }

    public void GameOver()
    {
        _gameOverPanel.gameObject.SetActive(true);
        _lastScoreText.text = _score.ToString();
        _inGame = false;
        _spawner.gameObject.SetActive(false);

        if (_score > _bestScore)
        {
            PlayerPrefs.SetInt(BestScore, _score);
            _newHightScoreImage.gameObject.SetActive(true);
        }
    }

    public void ReplayGame()
    {
        SceneManager.LoadScene(0);
    }

    public void GetStar()
    {
        //UpdateItem(_totalStar, TotalStar, _starText);
        int newItem = _totalStar++;
        PlayerPrefs.SetInt(TotalStar, newItem);
        _starText.text = _totalStar.ToString();
    }

    public void GetDiamond()
    {
        //UpdateItem(_totalDiamond, TotalDiamond, _diamondText);
        int newItem = _totalDiamond++;
        PlayerPrefs.SetInt(TotalDiamond, newItem);
        _diamondText.text = _totalDiamond.ToString();
    }

    private void UpdateItem(int count, string item, TMP_Text textItem)
    {
        int newItem = count++;
        PlayerPrefs.SetInt(item, newItem);
        textItem.text = count.ToString();
    }

    private void GameStart()
    {
        _inGame = true;
        _spawner.gameObject.SetActive(true);
        _isGameStarted = true;

        if (_coroutine != null)
            StopCoroutine(UpdateScore());

        _coroutine = StartCoroutine(UpdateScore());
    }

    private IEnumerator UpdateScore()
    {
        while (_inGame)
        {
            yield return _wait;
            _score++;

            if (_score > _bestScore)
                _bestText.text = _score.ToString();

            _scoreText.text = _score.ToString();
        }
    }

    private void ViewText(int count, string item, TMP_Text textItem)
    {
        count = PlayerPrefs.GetInt(item);
        textItem.text = count.ToString();
    }
}
