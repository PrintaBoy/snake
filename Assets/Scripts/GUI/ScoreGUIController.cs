using UnityEngine;
using TMPro;

public class ScoreGUIController : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreValueText;
    [SerializeField] private TMP_Text scoreHighestValueText;
    [SerializeField] private TMP_Text applesValueText;
    [SerializeField] private TMP_Text pumpkinsValueText;
    [SerializeField] private TMP_Text mushroomValueText;
    [SerializeField] private TMP_Text acornValueText;
    [SerializeField] private TMP_Text grapeValueText;

    private void Awake()
    {
        RefreshScore();
    }

    private void OnEnable()
    {
        ScoreController.OnScoreUpdated += RefreshScore;
    }

    private void OnDisable()
    {
        ScoreController.OnScoreUpdated -= RefreshScore;
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
