using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public List<GameObject> enemiesList;
    public int points = 0;
    public bool hasNoEnemies = false;
    public bool playerLost = false;

    // Start is called before the first frame update
    void Start()
    {
        //new List<GameObject> enemiesList

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
        //    Debug.Log("me caguei");
        //    //voce ganhou;
        //}


        //condi��o de vit�ria
        if(points >= 5)
        {
            hasNoEnemies = true;
        }

        //condi��o de derrota
        playerLost = GameObject.FindGameObjectWithTag("Arpao").GetComponent<CannonController>().hasAmmo;
    }

    public void AddPoints(int point)
    {
        points += point;
        Debug.Log(points);
    }

    //public void RemoveEnemy()
    //{
    //    enemiesList.Remove();
    //}
}
