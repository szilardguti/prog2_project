using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }
public class BattleSystem : MonoBehaviour
{
    public Dice myDice;
    public Button attackButton;
    public Button defendButton;
    public Button specialButton;
    public SpecialButton spButton;
    public ManageBattleStations battleStationManager;
    public BattleOrderManager battleOrderManager;
    public EndOfGamePanel endOfGamePanel;
    public TextMeshProUGUI levelCounter;

    public GameObject warriorPrefab;
    public GameObject rougePrefab;
    public GameObject wizardPrefab;
    public GameObject rangerPrefab;
    public GameObject enemyPrefab;

    // Only need the location so Transform is enough
    public Transform playerBattleStation1;
    public Transform playerBattleStation2;
    public Transform playerBattleStation3;
    public Transform playerBattleStation4;

    public Transform enemyBattleStation1;
    public Transform enemyBattleStation2;
    public Transform enemyBattleStation3;
    public Transform enemyBattleStation4;

    //Which state the Battle is in
    public BattleState state;


    Unit playerUnit;
    Unit enemyUnit;
    Unit onTurnUnit;


    public BattleHUD playerHUD1;
    public BattleHUD playerHUD2;
    public BattleHUD playerHUD3;
    public BattleHUD playerHUD4;
    public BattleHUD enemyHUD1;
    public BattleHUD enemyHUD2;
    public BattleHUD enemyHUD3;
    public BattleHUD enemyHUD4;

    List<Unit> Units = new List<Unit>();
    List<BattleHUD> HUDs = new List<BattleHUD>();
    private List<GameObject> objectsInGame = new List<GameObject>();
    private int previousSelect = 4;
    private int enemyKilled = 0, playerKilled = 0;
    private int sceneLevel = 0;
    private int specialDamage = 0;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        SetupBattle();
    }

    public void SetupBattle()
    {

        if (Units.Count == 0)
        {
            GameObject playerGO = Instantiate(wizardPrefab, playerBattleStation1);
            Units.Add(playerGO.GetComponent<Unit>());

            playerGO = Instantiate(rangerPrefab, playerBattleStation2);
            Units.Add(playerGO.GetComponent<Unit>());

            playerGO = Instantiate(rougePrefab, playerBattleStation3);
            Units.Add(playerGO.GetComponent<Unit>());

            playerGO = Instantiate(warriorPrefab, playerBattleStation4);
            Units.Add(playerGO.GetComponent<Unit>());

            for (int i = 0; i < 4; i++)
                Units.Add(null);

            BattleHUD[] listOfBH = { playerHUD1, playerHUD2, playerHUD3, playerHUD4, enemyHUD1, enemyHUD2, enemyHUD3, enemyHUD4 };
            HUDs.AddRange(listOfBH);

            attackButton.GetComponent<Button>();
            defendButton.GetComponent<Button>();
            specialButton.GetComponent<Button>();
            spButton.GetComponent<SpecialButton>();
            specialButton.interactable = false;
        }

        enemyKilled = 0; playerKilled = 0;

        SpawnRandomEnemies();


        for (int i = 0; i < Units.Count; i++)
            HUDs[i].SetHUD(Units[i]);


        battleOrderManager.SetupBattleOrder(Units);

        for (int i = 0; i < Units.Count; i++)
        { 
            if (!Units[i].isDead)
                Units[i].gameObject.transform.parent.GetComponent<SelectBattleStation>().DoTrigger();
            else if (Units[i].isDead)
                battleOrderManager.UnitDied(Units[i]);
        }

        onTurnUnit = battleOrderManager.PeekOnTurnUnit();
        battleStationManager.TurnSelected(onTurnUnit);




        levelCounter.text = $"Level {++sceneLevel}";
        
        NextTurn();
    }

    void PlayerTurn()
    {
        Debug.Log(onTurnUnit.unitName + "'s turn!");
        attackButton.interactable = true;
        defendButton.interactable = true;

        spButton.SetSpecialSlider(onTurnUnit);
        if (onTurnUnit.specialLoad == 5)
            specialButton.interactable = true;

        battleStationManager.TurnSelected(onTurnUnit);
        battleStationManager.Selected(Units[previousSelect].transform.parent.gameObject.GetComponent<SelectBattleStation>());
    }

    void EnemyTurn()
    {
        Debug.Log(onTurnUnit.unitName + "'s turn!");

        spButton.SetSpecialSlider(onTurnUnit);
        StartCoroutine("EnemyAttack");
    }
    public void OnAnyButtonPress()
    {
        attackButton.interactable = false;
        defendButton.interactable = false;
        specialButton.interactable = false;

        spButton.IncrementSpecial(onTurnUnit);
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;
        StartCoroutine("PlayerAttack");
    }

    public void OnDefendButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;
        StartCoroutine("PlayerDefense");
    }

    public void OnSpecialButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        object[] obj = GameObject.FindObjectsOfType(typeof(GameObject));
        foreach (object o in obj)
        {
            GameObject g = (GameObject)o;
            if(g.scene.name == "Game" && g.name != "BattleSystem")
            {
                objectsInGame.Add(g);
                g.SetActive(false);
            }
        }

        SceneManager.LoadScene("BubblePop", LoadSceneMode.Additive);
    }

    IEnumerator PlayerAttack()
    {
        playerUnit = onTurnUnit;

        enemyUnit = battleStationManager.GetActiveUnit();

        myDice.Roll();

        int correctIndex = Units.IndexOf(enemyUnit);

        previousSelect = correctIndex;

        yield return new WaitForSeconds(1.5f);

        bool isDead = enemyUnit.TakeDamage(myDice.diceResult + playerUnit.damage - enemyUnit.protection + specialDamage);
        specialDamage = 0;
        battleStationManager.RemoveShieldFrom(enemyUnit);

        HUDs[correctIndex].SetHP(enemyUnit.currHP);

        if (isDead)
        {
            HUDs[correctIndex].NullHP();

            battleOrderManager.UnitDied(enemyUnit);

            enemyUnit.gameObject.transform.parent.GetComponent<SelectBattleStation>().DoNotTrigger();

            foreach (var unit in Units)
            {
                if(unit.isHero == false && unit.isDead == false)
                {
                    previousSelect = Units.IndexOf(unit);
                    break;
                }
            }

            Debug.Log("you killed a " + enemyUnit.unitName);

            KilledUnit(enemyUnit);
            if(enemyKilled == 4)
            {
                state = BattleState.WON;
                Debug.Log("YOU WON");
                endOfGamePanel.Activate("YOU WON!", true);
                yield break;
            }
        }

        NextTurn();
    }

    IEnumerator EnemyAttack()
    {
        enemyUnit = onTurnUnit;
        while(true)
        {
            playerUnit = Units[Random.Range(0, 4)];
            if (playerUnit.isDead == false)
                break;
        }


        battleStationManager.TurnSelected(onTurnUnit);
        battleStationManager.Selected(playerUnit.transform.parent.gameObject.GetComponent<SelectBattleStation>());

        yield return new WaitForSeconds(0.5f);

        myDice.Roll();

        yield return new WaitForSeconds(1.5f);

        int correctIndex = Units.IndexOf(playerUnit);

        bool isDead = playerUnit.TakeDamage(myDice.diceResult + enemyUnit.damage - playerUnit.protection);
        battleStationManager.RemoveShieldFrom(playerUnit);

        HUDs[correctIndex].SetHP(playerUnit.currHP);

        if (isDead)
        {
            HUDs[correctIndex].NullHP();

            battleOrderManager.UnitDied(playerUnit);

            playerUnit.gameObject.transform.parent.GetComponent<SelectBattleStation>().DoNotTrigger();

            Debug.Log(playerUnit.unitName + " has died!");

            KilledUnit(playerUnit);
            if (playerKilled == 4)
            {
                state = BattleState.LOST;
                Debug.Log("YOU LOST");
                endOfGamePanel.Activate("YOU LOST!");
                yield break;
            }
        }



        NextTurn();
    }

    IEnumerator PlayerDefense()
    {
        if (!onTurnUnit.isDefending)
        {
            onTurnUnit.SetDefense();
            Instantiate(Resources.Load<GameObject>("Shield"), onTurnUnit.gameObject.transform.parent);
        }

        yield return new WaitForSeconds(0.5f);


        NextTurn();
    }

    void NextTurn()
    {
        onTurnUnit = battleOrderManager.GetOnTurnUnit();
        if (Units.IndexOf(onTurnUnit) < 4)
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            EnemyTurn();
        }
    }

    private void SpawnRandomEnemies()
    {
        GameObject[] tempEnemies = Resources.LoadAll<GameObject>("EnemyPrefabs");
        List<GameObject> enemies = new List<GameObject>();
        enemies.AddRange(tempEnemies);

        Transform[] enemyBattleStation = { enemyBattleStation1, enemyBattleStation2, enemyBattleStation3, enemyBattleStation4 };

        for (int i = 0; i < 4; i++)
        {
            if (enemyBattleStation[i].childCount != 0)
                enemyBattleStation[i].GetChild(sceneLevel - 1).gameObject.SetActive(false);

            GameObject tempEnemyGO = Instantiate(enemies[Random.Range(0, 4)], enemyBattleStation[i]);
            Units[4 + i] = tempEnemyGO.GetComponent<Unit>();
        }
    }

    public int GetSceneLevel()
    {
        return sceneLevel;
    }

    void KilledUnit(Unit unit)
    {
        if (unit.isHero)
            playerKilled += 1;
        else
            enemyKilled += 1;
    }

    public void ReturnGameplay()
    {
        foreach (GameObject o in objectsInGame)
        {
            o.SetActive(true);
        }
        objectsInGame.Clear();

        specialDamage = BubblePopManager.specialDamage;
        StartCoroutine("PlayerAttack");
    }
}
