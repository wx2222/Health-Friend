using UnityEngine;

public class SegmentedSlimeController : MonoBehaviour
{
    public GameObject[] slimePrefabs;

    private GameObject currentSlime;
    private int currentSegment = 0;
    public Growth growthScript;

    private void Start()
    {
        growthScript = GetComponentInParent<Growth>();
        if (growthScript == null)
        {
            Debug.LogError("SegmentedSlimeController: Growth script not found!");
            return;
        }

        
        GameObject defaultSlime = Instantiate(slimePrefabs[0], transform.position, transform.rotation, transform);
        currentSlime = defaultSlime;
    }

    private void Update()
    {
        int currentSegment = growthScript.MarkGrowthSegment(growthScript.currentGrowth);
        if (currentSegment != this.currentSegment)
        {
            ChangeSlimeModel(currentSegment);
        }
    }

    public void ChangeSlimeModel(int segment)
    {
        if (segment > currentSegment && segment <= slimePrefabs.Length)
        {
            // Current slime pet appearance is based on segment
            int slimeIndex = segment - 1;

            // Instantiate the new slime
            GameObject newSlime = Instantiate(slimePrefabs[slimeIndex], transform.position, transform.rotation, transform);

            // Destroy the previous slime prefab if it exists
            if (currentSlime != null)
            {
                Destroy(currentSlime);
            }

            // Assign the new slime as the current slime
            currentSlime = newSlime;

            // Update the current segment
            currentSegment = segment;
        }
    }
}
