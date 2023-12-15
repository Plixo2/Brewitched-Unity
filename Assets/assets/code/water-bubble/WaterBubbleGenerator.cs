using System.Collections;
using UnityEngine;


/// <summary>
/// Handles the bubble and danger icon system, when and where to spawn these etc.
/// </summary>
public class WaterBubbleGenerator : MonoBehaviour
{
    
    [SerializeField] public bool bubbleGenerationActive = false;
    [SerializeField] float bubbleRadiusMin = 0.3f;
    [SerializeField] float bubbleRadiusMax = 1.8f;
    [SerializeField] float bubbleTimeIntervalMin = 2.5f;
    [SerializeField] float bubbleTimeIntervalMax = 6f;
    
    private float timeAfterLastGeneration = 0; // Elapsed time after the last spawned bubble
    public Transform waterTransform;
    
    private float bubbleY; // y coordinate the next bubble will spawn at
    [SerializeField] float bubbleGenerationXMin = -10.6f; // left border of visible game area
    [SerializeField] float bubbleGenerationXMax = 10.6f; // right border of visible game area
    
    public GameObject bubbleToSpawn;
    private int totalBubbleGenerated = 0; // total amount of bubbles spawned so far in the scene
    private float bubbleTimeInterval = 0; // time until the next bubble spawn, will be decided randomly between it's min and max variables
    public GameObject dangerIconToSpawn;
    
    [SerializeField] float timeBetweenDangerAndBubble = 1.5f; // Elapsed time between danger icon appearance and it's corresponding bubble spawn.
    [SerializeField] float timeBeforeFirstBubble = 5f; // Elapsed time between scene beginning and first bubble spawn

    
    void Start()
    {
        StartCoroutine(waitFirstBubble());
    }

    void Update()
    {
        
        if(bubbleGenerationActive)
        {
            timeAfterLastGeneration += Time.deltaTime;
            if(totalBubbleGenerated == 0) // if this will be the first bubble ever
            {
                StartCoroutine(createBubble());
                bubbleTimeInterval = Random.Range(bubbleTimeIntervalMin, bubbleTimeIntervalMax);
            }
            else
            {
                if(timeAfterLastGeneration > bubbleTimeInterval)
                {
                    StartCoroutine(createBubble());
                    bubbleTimeInterval = Random.Range(bubbleTimeIntervalMin, bubbleTimeIntervalMax);
                }
            }
        }
        else
        {
            timeAfterLastGeneration = 0;
        }
    }

    
    /// <summary>
    /// Handles danger icon and bubble spawn
    /// a Danger Icon will appear at the place of the bubble before it spawns, it has proportional size to the bubble
    /// a Dangerous Water Bubble spawns at a random x coordinate on the screen, it goes up and can kill the player on contact
    /// </summary>
    private IEnumerator createBubble()
    {
        totalBubbleGenerated++;
        timeAfterLastGeneration = 0;
        
        float bubbleRadius = Random.Range(bubbleRadiusMin, bubbleRadiusMax);
        float bubbleX = Random.Range(bubbleGenerationXMin + bubbleRadius * 2, bubbleGenerationXMax - bubbleRadius * 2);
        bubbleY = waterTransform.position.y;
        Vector3 bubblePosition = new Vector3(bubbleX, bubbleY, 0);
        Vector3 bubbleScale = new Vector3(bubbleRadius, bubbleRadius, 1);

        GameObject spawnedIcon = Instantiate(dangerIconToSpawn, bubblePosition, Quaternion.identity);
        spawnedIcon.transform.localScale = bubbleScale;

        yield return new WaitForSeconds(timeBetweenDangerAndBubble);
        Destroy(spawnedIcon);

        GameObject spawnedBubble = Instantiate(bubbleToSpawn, bubblePosition, Quaternion.identity);
        spawnedBubble.transform.localScale = bubbleScale;
    }

    private IEnumerator waitFirstBubble()
    {
        yield return new WaitForSeconds(timeBeforeFirstBubble);
        bubbleGenerationActive = true;
    }
}
