using UnityEngine;
using TMPro;

public class ScoreGUIController : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreValueText;
    [SerializeField] private TMP_Text applesValueText;
    [SerializeField] private TMP_Text pumpkinsValueText;

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
        scoreValueText.text = ScoreController.scoreCurrent.ToString();
        applesValueText.text = ScoreController.applesConsumed.ToString();
        pumpkinsValueText.text = ScoreController.pumpkinsConsumed.ToString();
    }
}
