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
    public EndOfGamePanel endOfGamePanel;

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
    private int enemyKilled = 0, playerKilled = 0;

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
        enemyUnit.unitName = enemyUnit.unitName + " 1";
        Units.Add(enemyUnit);

        enemyGO = Instantiate(enemyPrefab, enemyBattleStation2);
        enemyUnit = enemyGO.GetComponent<Unit>();
        enemyUnit.unitName = enemyUnit.unitName + " 2";
        Units.Add(enemyUnit);

        enemyGO = Instantiate(enemyPrefab, enemyBattleStation3);
        enemyUnit = enemyGO.GetComponent<Unit>();
        enemyUnit.unitName = enemyUnit.unitName + " 3";
        Units.Add(enemyUnit);

        enemyGO = Instantiate(enemyPrefab, enemyBattleStation4);
        enemyUnit = enemyGO.GetComponent<Unit>();
        enemyUnit.unitName = enemyUnit.unitName + " 4";
        Units.Add(enemyUnit);

        BattleHUD[] listOfBH = { playerHUD1, playerHUD2, playerHUD3, playerHUD4, enemyHUD1, enemyHUD2, enemyHUD3, enemyHUD4 };
        HUDs.AddRange(listOfBH);

        for (int i = 0; i < Units.Count; i++)
        {
            HUDs[i].SetHUD(Units[i]);
        }

        battleOrderManager.SetupBattleOrder(Units);
        onTurnUnit = battleOrderManager.PeekOnTurnUnit();
        battleStationManager.TurnSelected(onTurnUnit);

        attackButton.GetComponent<Button>();


        NextTurn();
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

        bool isDead = enemyUnit.TakeDamage(myDice.diceResult + playerUnit.damage);

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

            if(++enemyKilled == 4)
            {
                state = BattleState.WON;
                Debug.Log("YOU WON");
                endOfGamePanel.Activate("YOU WON!");
                yield break;
            }
        }

        NextTurn();
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

        bool isDead = playerUnit.TakeDamage(myDice.diceResult + enemyUnit.damage);

        HUDs[correctIndex].SetHP(playerUnit.currHP);

        if (isDead)
        {
            HUDs[correctIndex].NullHP();

            battleOrderManager.UnitDied(playerUnit);

            playerUnit.gameObject.transform.parent.GetComponent<SelectBattleStation>().DoNotTrigger();

            Debug.Log(enemyUnit.unitName + " has died!");

            if (++playerKilled == 4)
            {
                state = BattleState.LOST;
                Debug.Log("YOU LOST");
                endOfGamePanel.Activate("YOU LOST!");
                yield break;
            }
        }



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
}
