using UnityEngine;
using TMPro;

public class ScoreGUIController : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreValueText;
    [SerializeField] private TMP_Text applesValueText;
    [SerializeField] private TMP_Text pumpkinsValueText;
    [SerializeField] private TMP_Text mushroomValueText;
    [SerializeField] private TMP_Text acornValueText;

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
        mushroomValueText.text = ScoreController.mushroomsConsumed.ToString();
        acornValueText.text = ScoreController.acornsConsumed.ToString();
    }
}
