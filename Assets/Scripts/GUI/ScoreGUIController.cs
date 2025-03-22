using UnityEngine;
using TMPro;

public class ScoreGUIController : MonoBehaviour
{
    /// <summary>
    /// This class serves to update score GUI for current play session
    /// It takes data from ScoreController class
    /// </summary>


    [SerializeField] private TMP_Text scoreValueText;
    [SerializeField] private TMP_Text scoreHighestValueText;
    [SerializeField] private TMP_Text applesValueText;
    [SerializeField] private TMP_Text pumpkinsValueText;
    [SerializeField] private TMP_Text mushroomValueText;
    [SerializeField] private TMP_Text acornValueText;
    [SerializeField] private TMP_Text grapeValueText;
    [SerializeField] private GameObject scoreSection;

    private void Awake()
    {
        RefreshScore();
    }

    private void OnEnable()
    {
        ScoreController.OnScoreUpdated += RefreshScore;
        GameStateController.OnGameOver += HideScoreSection;
    }

    private void OnDisable()
    {
        ScoreController.OnScoreUpdated -= RefreshScore;
        GameStateController.OnGameOver -= HideScoreSection;
    }

    private void ShowScoreSection()
    {
        scoreSection.SetActive(true);
    }

    private void HideScoreSection()
    {
        scoreSection.SetActive(false);
    }

    private void RefreshScore()
    {
        scoreHighestValueText.text = ScoreController.scoreHighest.ToString();
        scoreValueText.text = ScoreController.scoreCurrent.ToString();
        applesValueText.text = ScoreController.applesConsumed.ToString();
        pumpkinsValueText.text = ScoreController.pumpkinsConsumed.ToString();
        mushroomValueText.text = ScoreController.mushroomsConsumed.ToString();
        acornValueText.text = ScoreController.acornsConsumed.ToString();
        grapeValueText.text = ScoreController.grapesConsumed.ToString();    
    }
}
