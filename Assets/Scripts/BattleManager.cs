using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    public List<GameObject> enemiesList;
    public int points = 0;
    public bool hasNoEnemies = false;
    public bool playerLost = false;

    [SerializeField] private TextMeshProUGUI enemyDisplay;
    
    // Start is called before the first frame update
    void Start()
    {
        //new List<GameObject> enemiesList
        enemyDisplay.text = "ENEMIES LEFT:\n" + (5 - points).ToString();
        //enemiesList.Add() = GameObject.FindGameObjectsWithTag("Enemy");
    }

    // Update is called once per frame
    void Update()
    {

        //if(enemiesList.Count == 0)
        //{
        //    hasNoEnemies = true;
        //}
        //if(hasNoEnemies)
        //{
        //    Debug.Log("voce ganhou");
        //    //voce ganhou;
        //}

        if(Input.GetKeyDown(KeyCode.PageUp)) { AddPoints(1); } // DEBUG< TIRAR DPS

        //condição de vitória
        if(points >= 5)
        {
            hasNoEnemies = true;
            Core.IncrementEnemyCount(-1);
            Core.IncrementPlayerGold(10);

            Core.SetCurrentBattleVictory();
        }

        //condição de derrota
        if(!GameObject.FindGameObjectWithTag("Arpao").GetComponent<CannonController>().hasAmmo)
        {
            playerLost = true;
            Core.IncrementPlayerHealth(-40);
        }

        if (hasNoEnemies || playerLost)
        {
            GameObject.Find("EventSystem").GetComponent<MenuController>().OnChangeSceneButtonPress(Core.GetLastActiveScene());
        }
    }


    public void AddPoints(int point)
    {
        points += point;
        enemyDisplay.text = "ENEMIES LEFT:\n" + (5 - points).ToString();
        Debug.Log(points);
    }

    //public void RemoveEnemy()
    //{
    //    enemiesList.Remove();
    //}
}
