using UnityEngine;

public class SnakeSegmentVisual : MonoBehaviour
{
    [SerializeField] private Material[] snakeSegmentMaterials;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private SnakeSegment snakeSegmentScript;

    private void OnEnable()
    {
        SnakeSegment.OnSnakeSegmentSetup += SnakeSegmentSetup;
    }

    private void OnDisable()
    {
        SnakeSegment.OnSnakeSegmentSetup -= SnakeSegmentSetup;
    }

    private void SnakeSegmentSetup()
    {
        ChangeSnakeSegmentMaterial();
    }

    private void ChangeSnakeSegmentMaterial()
    {
        if (snakeSegmentScript.snakeSegmentListIndex == 0)
        {
            meshRenderer.material = snakeSegmentMaterials[0];
        }
        else
        {
            if (snakeSegmentScript.snakeSegmentListIndex % 2 == 0)
            {
                meshRenderer.material = snakeSegmentMaterials[1];
            }
            else
            {
                meshRenderer.material = snakeSegmentMaterials[2];
            }
        }       
    }
}
