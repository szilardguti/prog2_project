using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }
public class BattleSystem : MonoBehaviour
{
    public Dice myDice;
    public Button attackButton;
    public ManageBattleStations battleStationManager;
    public BattleOrderManager battleOrderManager;

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
    private int previousSelect = 4;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        SetupBattle();
    }

    void SetupBattle()
    {
        GameObject playerGO = Instantiate(wizardPrefab, playerBattleStation1);
        playerUnit = playerGO.GetComponent<Unit>();
        Units.Add(playerUnit);

        playerGO = Instantiate(rangerPrefab, playerBattleStation2);
        playerUnit = playerGO.GetComponent<Unit>();
        Units.Add(playerUnit);

        playerGO = Instantiate(rougePrefab, playerBattleStation3);
        playerUnit = playerGO.GetComponent<Unit>();
        Units.Add(playerUnit);

        playerGO = Instantiate(warriorPrefab, playerBattleStation4);
        playerUnit = playerGO.GetComponent<Unit>();
        Units.Add(playerUnit);

        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation1);
        enemyUnit = enemyGO.GetComponent<Unit>();
        Units.Add(enemyUnit);

        enemyGO = Instantiate(enemyPrefab, enemyBattleStation2);
        enemyUnit = enemyGO.GetComponent<Unit>();
        Units.Add(enemyUnit);

        enemyGO = Instantiate(enemyPrefab, enemyBattleStation3);
        enemyUnit = enemyGO.GetComponent<Unit>();
        Units.Add(enemyUnit);

        enemyGO = Instantiate(enemyPrefab, enemyBattleStation4);
        enemyUnit = enemyGO.GetComponent<Unit>();
        Units.Add(enemyUnit);

        BattleHUD[] listOfBH = { playerHUD1, playerHUD2, playerHUD3, playerHUD4, enemyHUD1, enemyHUD2, enemyHUD3, enemyHUD4 };
        HUDs.AddRange(listOfBH);

        for (int i = 0; i < Units.Count; i++)
        {
            HUDs[i].SetHUD(Units[i]);
        }

        battleOrderManager.SetupBattleOrder(Units);
        onTurnUnit = battleOrderManager.GetOnTurnUnit();
        battleStationManager.TurnSelected(onTurnUnit);

        attackButton.GetComponent<Button>();


        if(Units.IndexOf(onTurnUnit) < 4)
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

    void PlayerTurn()
    {
        Debug.Log(onTurnUnit.unitName + "'s turn!");
        attackButton.interactable = true;

        battleStationManager.TurnSelected(onTurnUnit);
        battleStationManager.Selected(Units[previousSelect].transform.parent.gameObject.GetComponent<SelectBattleStation>());
    }

    void EnemyTurn()
    {
        Debug.Log(onTurnUnit.unitName + "'s turn!");
        StartCoroutine("EnemyAttack");
    }

    public void OnAttackButton()
    {
        attackButton.interactable = false;
        if (state != BattleState.PLAYERTURN)
            return;
        StartCoroutine("PlayerAttack");
    }

    IEnumerator PlayerAttack()
    {
        playerUnit = onTurnUnit;

        enemyUnit = battleStationManager.GetActiveUnit();

        myDice.Roll();

        int correctIndex = Units.IndexOf(enemyUnit);

        previousSelect = correctIndex;

        yield return new WaitForSeconds(1.5f);

        bool isDead = enemyUnit.takeDamage(myDice.diceResult + playerUnit.damage);

        if (isDead)
        {
            HUDs[correctIndex].NullHP();
            //state = BattleState.WON;
            Debug.Log("you killed a " + enemyUnit.unitName);

            state = BattleState.ENEMYTURN;
            EnemyTurn();
            yield break;
        }

        HUDs[correctIndex].SetHP(enemyUnit.currHP);

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

    IEnumerator EnemyAttack()
    {
        enemyUnit = onTurnUnit;
        playerUnit = Units[Random.Range(0,4)];

        battleStationManager.TurnSelected(onTurnUnit);
        battleStationManager.Selected(playerUnit.transform.parent.gameObject.GetComponent<SelectBattleStation>());

        yield return new WaitForSeconds(0.5f);

        myDice.Roll();

        yield return new WaitForSeconds(1.5f);

        int correctIndex = Units.IndexOf(playerUnit);

        bool isDead = playerUnit.takeDamage(myDice.diceResult + enemyUnit.damage);

        if (isDead)
        {
            HUDs[correctIndex].NullHP();
            /*
            state = BattleState.LOST;
            Debug.Log("YOU LOST");
            */
            yield break;
        }

        HUDs[correctIndex].SetHP(playerUnit.currHP);

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
}
