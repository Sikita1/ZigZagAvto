using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using YG;

public class SelectCar : MonoBehaviour
{
    public static SelectCar Instance;

    private const string CarNo = "CarNo";
    private const string Select = "SELECT";
    private const string Buy = "BUY";
    private const string SelectAvto = "SelectAvto";

    private const string TotalDiamond = "totalDiamond";
    private const string TotalStar = "totalStar";

    [SerializeField] private Button _prevBtn;
    [SerializeField] private Button _nextBtn;
    [SerializeField] private Button _useBtn;
    [SerializeField] private GameObject _buyPanel;

    [SerializeField] private Button _buyCarBtn;
    [SerializeField] private TMP_Text _needMoreText;
    [SerializeField] private Button _closeBtnPanel;

    [Header("Buy Panel")]
    [SerializeField] private TMP_Text _haveStarText;
    [SerializeField] private TMP_Text _haveDiamondText;
    [SerializeField] private Button _buyStarDimondBtn;

    private int _currentCar;
    private string _ownCarIndex;
    private Color _redColor = Color.red;
    private Color _greenColor = Color.green;

    private int _haveStars;
    private int _haveDiamonds;
    private int _carValue = 700;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        ChangeCar(0);
    }

    private void Start()
    {
        _haveStars = PlayerPrefs.GetInt(TotalStar);
        _haveDiamonds = PlayerPrefs.GetInt(TotalDiamond);
    }

    public void ChangeCar(int change)
    {
        _currentCar += change;

        if (_currentCar > transform.childCount - 1)
            _currentCar = 0;
        else if (_currentCar < 0)
            _currentCar = transform.childCount - 1;

        ChooseCar(_currentCar);

        _ownCarIndex = CarNo + _currentCar;

        if (PlayerPrefs.GetInt(_ownCarIndex) == 1)
            ChangeBuyButton(_greenColor, Select);
        else
            ChangeBuyButton(_redColor, Buy);
    }

    public void UseBtnClick()
    {
        _haveStars = PlayerPrefs.GetInt(TotalStar);
        _haveDiamonds = PlayerPrefs.GetInt(TotalDiamond);

        if (PlayerPrefs.GetInt(_ownCarIndex) == 1)
        {
            PlayerPrefs.SetInt(SelectAvto, _currentCar);
            SceneManager.LoadScene(0);
        }
        else
        {
            _buyPanel.SetActive(true);

            _haveStarText.text = _haveStars.ToString();
            _haveDiamondText.text = _haveDiamonds.ToString();

            if (_haveStars < _carValue)
            {
                int needStar = _carValue - _haveStars;
                _buyCarBtn.interactable = false;
            }
            else
            {
                _buyCarBtn.interactable= true;
                _needMoreText.text = ($"Value: {_carValue} stars");
            }

            if(_haveDiamonds < 1)
                _buyStarDimondBtn.interactable = false;

                UnlockButtonsOnPanel(false);
        }
    }

    public void ClosePanel()
    {
        _buyPanel?.SetActive(false);
        UnlockButtonsOnPanel(true);
    }

    public void BuyThisCar()
    {
        PlayerPrefs.SetInt(_ownCarIndex, 1);
        _haveStars -= _carValue;
        PlayerPrefs.SetInt(TotalStar, _haveStars);
        int currentMinOne = _currentCar - 1;
        ChangeCar(currentMinOne);
        ClosePanel();
    }

    public void BuyStars()
    {
        YG2.InterstitialAdvShow();
        _haveDiamonds--;
        _haveStars += 10;
        PlayerPrefs.SetInt(TotalStar, _haveStars);
        PlayerPrefs.SetInt(TotalDiamond, _haveDiamonds);
        SetText();
    }

    public void EarnStar()
    {
        YG2.RewardedAdvShow("0", () =>
            {
                _haveStars = PlayerPrefs.GetInt(TotalStar);
                _haveStars += 100;
                PlayerPrefs.SetInt(TotalStar, _haveStars);
                SetText();
            });
    }

    private void SetText()
    {
        _buyPanel.SetActive(true);

        _haveStarText.text = _haveStars.ToString();
        _haveDiamondText.text = _haveDiamonds.ToString();

        if (_haveStars < _carValue)
        {
            int needStar = _carValue - _haveStars;
            _buyCarBtn.interactable = false;
            _needMoreText.text = ($"{needStar} more star needed");
        }

        if (_haveDiamonds < 1)
            _buyStarDimondBtn.interactable = false;

        UnlockButtonsOnPanel(false);
    }

    private void UnlockButtonsOnPanel(bool value)
    {
        _prevBtn.interactable = value;
        _nextBtn.interactable = value;
        _useBtn.interactable = value;
    }

    private void ChangeBuyButton(Color color, string text)
    {
        _useBtn.GetComponent<Image>().color = color;
        _useBtn.GetComponentInChildren<TMP_Text>().text = text;
    }

    private void ChooseCar(int index)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            string carNo = CarNo + i;

            if (i == 0)
                PlayerPrefs.SetInt(carNo, 1);

            transform.GetChild(i).gameObject.SetActive(i == index);
        }
    }
}
