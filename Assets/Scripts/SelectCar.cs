using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using YG;
using System.Collections.Generic;

public class SelectCar : MonoBehaviour
{
    private const string CarNo = "CarNo";
    private const string SelectAvto = "SelectAvto";
    private const string TotalDiamond = "totalDiamond";
    private const string TotalStar = "totalStar";

    [SerializeField] private Button _prevBtn;
    [SerializeField] private Button _nextBtn;
    [SerializeField] private Button _useBtn;
    [SerializeField] private BuyPanel _buyPanel;

    [SerializeField] private Button _buyCarBtn;
    [SerializeField] private TMP_Text _needMoreText;
    [SerializeField] private Button _closeBtnPanel;
    [SerializeField] private TMP_Text _buyCarBtnStars;

    [Header("Buy Panel")]
    [SerializeField] private TMP_Text _haveStarText;
    [SerializeField] private TMP_Text _haveDiamondText;
    [SerializeField] private Button _buyStarDimondBtn;

    [Header("Car Materials")]
    [SerializeField] private Material _lockedCarMaterial;
    private Dictionary<int, Material[]> _originalMaterials = new Dictionary<int, Material[]>();
    private Renderer[] _allCarRenderers;

    private int _currentCar;
    private string _ownCarIndex;

    private Color _redColor = Color.red;
    private Color _greenColor = Color.green;

    private int _haveStars;
    private int _haveDiamonds;

    private void Awake()
    {
        SaveOriginalMaterial();

        ChangeCar(0);
    }

    private void Start()
    {
        _haveStars = PlayerPrefs.GetInt(TotalStar);
        _haveDiamonds = PlayerPrefs.GetInt(TotalDiamond);
    }

    private int GetCurrentCarPrice()
    {
        Transform currentCar = transform.GetChild(_currentCar);
        Player playerComponent = currentCar.GetComponent<Player>();
        return playerComponent != null ? playerComponent.GetPrice() : 500;
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
            ChangeBuyButton(_greenColor);
        else
            ChangeBuyButton(_redColor);
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
            SetText();
        }
    }

    public void ClosePanel()
    {
        _buyPanel?.gameObject.SetActive(false);
        UnlockButtonsOnPanel(true);
    }

    public void BuyThisCar()
    {
        PlayerPrefs.SetInt(_ownCarIndex, 1);
        _haveStars -= GetCurrentCarPrice();
        PlayerPrefs.SetInt(TotalStar, _haveStars);
        int currentMinOne = _currentCar - 1;
        ChangeCar(currentMinOne);
        UpdateCarMaterials();
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

    private void SaveOriginalMaterial()
    {
        _allCarRenderers = new Renderer[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform carParent = transform.GetChild(i);
            _allCarRenderers[i] = carParent.GetComponentInChildren<Renderer>(true);

            if (_allCarRenderers[i] != null)
                _originalMaterials[i] = _allCarRenderers[i].materials;
        }
    }

    private void SetText()
    {
        int currentPrice = GetCurrentCarPrice();
        int needStar = currentPrice - _haveStars;

        _buyPanel.gameObject.SetActive(true);
        _haveStarText.text = _haveStars.ToString();
        _haveDiamondText.text = _haveDiamonds.ToString();

        if (_haveStars < currentPrice)
        {
            _buyCarBtn.interactable = false;
            _needMoreText.text = ($"{_haveStars}/{currentPrice}");
            _needMoreText.color = Color.red;
        }
        else
        {
            _buyCarBtn.interactable = true;
            _needMoreText.text = ($"{_haveStars}/{currentPrice}");
            _needMoreText.color = Color.green;
        }

        if (_haveDiamonds < 1)
            _buyStarDimondBtn.interactable = false;

        _buyCarBtnStars.text = currentPrice.ToString();

        UnlockButtonsOnPanel(false);
    }

    private void UnlockButtonsOnPanel(bool value)
    {
        _prevBtn.interactable = value;
        _nextBtn.interactable = value;
        _useBtn.interactable = value;
    }

    private void ChangeBuyButton(Color color)
    {
        _useBtn.GetComponent<Image>().color = color;
    }

    private void UpdateCarMaterials()
    {
        for (int i = 0; i < _allCarRenderers.Length; i++)
        {
            if (_allCarRenderers[i] == null)
                continue;

            string carNo = CarNo + i;
            bool isUnlocked = PlayerPrefs.GetInt(carNo, i == 0 ? 1 : 0) == 1;

            if (isUnlocked)
            {
                if (_originalMaterials.TryGetValue(i, out Material[] originalMats))
                    _allCarRenderers[i].materials = originalMats;
            }
            else
            {
                Material[] lockedMaterials = new Material[_allCarRenderers[i].materials.Length];

                for (int j = 0; j < lockedMaterials.Length; j++)
                    lockedMaterials[j] = _lockedCarMaterial;

                _allCarRenderers[i].materials = lockedMaterials;
            }
        }
    }

    private void ChooseCar(int index)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            string carNo = CarNo + i;

            if (i == 0)
                PlayerPrefs.SetInt(carNo, 1);

            Transform carParent = transform.GetChild(i);
            bool isActive = i == index;
            carParent.gameObject.SetActive(isActive);

            UpdateCarMaterials();

            _ownCarIndex = CarNo + index;
            ChangeBuyButton(PlayerPrefs.GetInt(_ownCarIndex, index == 0 ? 1 : 0) == 1 ? _greenColor : _redColor);
        }
    }
}