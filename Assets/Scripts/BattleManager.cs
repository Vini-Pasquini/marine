using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public List<GameObject> enemiesList;
    public int points = 0;
    public bool hasNoEnemies = false;

    // Start is called before the first frame update
    void Start()
    {
        //enemiesList = GameObject.FindGameObjectsWithTag("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        if(enemiesList.Count == 0)
        {
            hasNoEnemies = true;
        }
        if(hasNoEnemies)
        {
            //voce ganhou;
        }
    }

    public void AddPoints(int point)
    {
        points += point;
        Debug.Log(points);
    }
}
