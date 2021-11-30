using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BubblePopManager : MonoBehaviour
{
    public static BubblePopManager gameManager;
    public static int specialDamage;

    public GameObject circle;
    List<int> numbersPopped = new List<int>();
    List<int> availableNums = new List<int>();
    List<Vector3> bubblePositions = new List<Vector3>();
    private readonly int minLimit = -50;
    private readonly int maxLimit = 50;
    private readonly int bubbleCount = 5;


    [SerializeField]
    Canvas minigameCanvas;

    private void Awake()
    {
        gameManager = this;
    }

    void Start()
    {
        specialDamage = 0;
        bubblePositions.Add(new Vector3(Random.Range(-7f, 7f), Random.Range(-4f, 4f), 0f));

        for (int i = 0; i < bubbleCount - 1; i++)
        {
            while (true)
            {
                bool foundPos = true;

                Vector3 tempPos = new Vector3(Random.Range(-7f, 7f), Random.Range(-4f, 4f), 0f);
                foreach (var bubblePos in bubblePositions)
                {
                    if(Vector3.Distance(tempPos, bubblePos) < 2.5f)
                    {
                        foundPos = false;
                        break;
                    }
                }

                if (foundPos)
                {
                    bubblePositions.Add(tempPos);
                    break;
                }
            }
        }


        for (int i = 0; i < bubbleCount; i++)
        {
            int num = Random.Range(minLimit, maxLimit);
            availableNums.Add(num);

            GameObject tempBubble = Instantiate(circle, bubblePositions[i], Quaternion.identity, minigameCanvas.transform);
            tempBubble.GetComponent<Bubble>().SetNumber(num);
            tempBubble.GetComponent<SpriteRenderer>().color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        }

        availableNums.Sort();
    }
    public void BubblePressed(int value)
    {
        Debug.Log($"bubble popped {value}");

        if (value != availableNums[numbersPopped.Count])
        {
            Debug.Log("mistakes were made");
            StartCoroutine("DestroyBubbles");
        }

        numbersPopped.Add(value);
        if (numbersPopped.Count == bubbleCount)
        {
            Debug.Log("YOU WON");

            specialDamage = 15;

            MinigameInterface minigameInterface = MinigameInterface.minigameInterface;
            minigameInterface.BubblePopFinished();

            SceneManager.UnloadSceneAsync("BubblePop");
        }
    }

    IEnumerator DestroyBubbles()
    {
        int childCounted = minigameCanvas.transform.childCount - 1;

        foreach(Transform child in minigameCanvas.transform)
            child.gameObject.GetComponent<CircleCollider2D>().enabled = false;

        for (int i = 0; i < childCounted; i++)
        { 
            yield return new WaitForSeconds(0.5f);

            Destroy(minigameCanvas.transform.GetChild(0).gameObject);
        }

        Debug.Log("YOU LOST");

        specialDamage = -1;

        MinigameInterface minigameInterface = MinigameInterface.minigameInterface;
        minigameInterface.BubblePopFinished();

        SceneManager.UnloadSceneAsync("BubblePop");
    }
}
