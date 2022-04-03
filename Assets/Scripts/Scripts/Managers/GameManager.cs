using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : GameBehaviour<GameManager>
{
    #region inspector stuff
    [Header("References")]
    public GameObject Card1;
    public GameObject Card2;
    public GameObject PlayerArea;
    public GameObject EnemyArea;

    [Header("Arrays")]
    public GameObject[] PlayerCardAreas;
    public GameObject[] EnemyCardAreas;

    [Header("Lists")]
    public List<GameObject> PlayerDeck = new List<GameObject>();
    public List<GameObject> EnemyDeck = new List<GameObject>();
    public List<GameObject> PlayerDealtCards = new List<GameObject>();
    public List<GameObject> EnemyDealtCards = new List<GameObject>();

    public bool cardPlayed = false;
    #endregion

    public int playerHealth;
    public int enemyHealth;

    public CardObject enemyCardToPlay;
    private int enemyDamage;
    private string[] enemyAttackColours = { };
    private string[] enemyDefenseColours = { };

    //private string enemyAttackColour;
    //private string enemyDefenseColour;
    public void Start()
    {
        playerHealth = 30;
        enemyHealth = 30;
    }

    public void DealCards()
    {
        for (int i = 0; i < 3; i++)
        {
            int rand1 = Random.Range(0,PlayerDeck.Count);
            Debug.Log(rand1);
            GameObject playerCard = Instantiate(PlayerDeck[rand1], PlayerCardAreas[i].transform);
            PlayerDealtCards.Add(playerCard);
            //PlayerDeck.Remove(PlayerDeck[rand]);

            int rand2 = Random.Range(0, PlayerDeck.Count);
            Debug.Log(rand1);
            GameObject enemyCard = Instantiate(EnemyDeck[rand2], EnemyCardAreas[i].transform);
            enemyCard.transform.rotation = EnemyCardAreas[i].transform.rotation;
            EnemyDealtCards.Add(enemyCard);
 
        }
    }

    public void PlayPlayerCard(int playerDamage, string[] playerAttackColours, string[] playerDefenseColours)
    {
        _GM.PlayEnemyCard(playerDamage, playerAttackColours, playerAttackColours);
    }

    public void PlayEnemyCard(int playerDamage, string[] playerAttackColours, string[] playerDefenseColours)
    {
        int rand = Random.Range(1, 3);
        Debug.Log(rand);
        enemyCardToPlay = EnemyDealtCards[rand].GetComponent<CardObject>();
        enemyCardToPlay.transform.position = _ECS.transform.position;
        enemyCardToPlay.transform.rotation = _ECS.transform.rotation;

        enemyDamage = enemyCardToPlay.card.damage;
        //enemyAttackColour = enemyCardToPlay.card.attackColour;
        //enemyDefenseColour = enemyCardToPlay.card.defenseColour;
        enemyAttackColours = enemyCardToPlay.card.attackColours;
        enemyDefenseColours = enemyCardToPlay.card.defenseColours;

        BattlePhase(playerDamage, playerAttackColours, playerDefenseColours,  enemyDamage, enemyAttackColours, enemyDefenseColours);
    }

    public void BattlePhase(int playerDamage, string[] playerAttackColours, string[] playerDefenseColours,
                            int enemyDamage, string[] enemyAttackColours, string[] enemyDefenseColours)
    {
        if (playerDamage != 0)
        {
            Attack(playerDamage, playerAttackColours, enemyDefenseColours, "Enemy", enemyHealth, playerAttackColours);
        }

        if (enemyDamage != 0)
        {
            Attack(enemyDamage, enemyAttackColours, playerDefenseColours, "Player", playerHealth, enemyAttackColours);
        }
    }

    public void Attack(int damage, string[] attackColours, string[] defenseColours, string target, int targetHealth, string[] AttackColours)
    {
        for(int d=0; d <defenseColours.Length; d++)
        {
            for (int a = 0; a < AttackColours.Length; a++)
            {
                if (AttackColours[a] != defenseColours[d])
                {
                    targetHealth -= damage;
                    _UI.UpdateHP(target, targetHealth);
                    return;
                }
            }
        }
        
    }

}
