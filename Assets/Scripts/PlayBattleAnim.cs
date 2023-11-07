using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBattleAnim : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.transform.GetChild(0).GetComponent<Animator>().Play("Static");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
