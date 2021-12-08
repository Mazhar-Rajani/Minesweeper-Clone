using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] private MinesweeperController minesweeperController = default;
    [SerializeField] private TMP_Text flagText = default;
    [SerializeField] private TMP_Text winText = default;
    [SerializeField] private Image generateBtn = default;
    [SerializeField] private Sprite smileSprite = default;
    [SerializeField] private Sprite sadSprite = default;

    private void Awake()
    {
        generateBtn.sprite = smileSprite;
        MapGenerator.OnGenerateMap += OnGenerateMap;
        MinesweeperController.OnGameStart += OnGameStart;
        MinesweeperController.OnGameWon += OnGameWon;
        MinesweeperController.OnGameLost += OnGameLost;
        MinesweeperController.OnFlagUpdate += OnFlagUpdate;
    }

    private void OnDestroy()
    {
        MapGenerator.OnGenerateMap -= OnGenerateMap;
        MinesweeperController.OnGameStart -= OnGameStart;
        MinesweeperController.OnGameWon -= OnGameWon;
        MinesweeperController.OnGameLost -= OnGameLost;
        MinesweeperController.OnFlagUpdate -= OnFlagUpdate;
    }

    private void OnGenerateMap()
    {
        generateBtn.sprite = smileSprite;
    }

    private void OnGameStart()
    {
        winText.text = string.Empty;
        OnFlagUpdate();
    }

    private void OnGameWon()
    {
        generateBtn.sprite = smileSprite;
        winText.text = "WON!";
        winText.color = Color.green;
    }

    private void OnGameLost()
    {
        generateBtn.sprite = sadSprite;
        winText.text = "LOST!";
        winText.color = Color.red;
    }

    private void OnFlagUpdate()
    {
        flagText.text = minesweeperController.RemainingFlagCount.ToString();
    }
}