using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Attack : MonoBehaviour
{
    // Makes possible to reference in Unity
    public GameObject myDice;
    Dice myDiceScript;
    // Sprite renderers
    private Sprite[] attackMoves;
    private SpriteRenderer rend;

    // Start is called before the first frame update
    void Start()
    {
        // Assign Dice components
        myDiceScript = myDice.GetComponent<Dice>();

        // Assign Renderer component
        rend = GetComponent<SpriteRenderer>();
        attackMoves = Resources.LoadAll<Sprite>("Player1/");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            myDiceScript.Roll();
            StartCoroutine("Attack");
        }
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(1.5f);
        for (int i = 0; i < 3; i++)
        {
            rend.sprite = attackMoves[i];

            // Pause before next itteration
            yield return new WaitForSeconds(0.10f);
        }
    }
}
