using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private List<EnemyController> enemyTaskList;
    private List<AnimalController> animalTaskList;

    private void Start()
    {
        enemyTaskList = FindObjectsOfType<EnemyController>().ToList();
        animalTaskList = FindObjectsOfType<AnimalController>().ToList();
        LoadTaskCache();
    }

    private void LoadTaskCache()
    {
        LoadEnemyTaskCache();
        LoadAnimalTaskCache();
    }

    private void LoadEnemyTaskCache()
    {
        List<EnemyTaskCache> enemyTaskCacheList = Core.GetEnemyTaskCacheList();
        if (enemyTaskCacheList == null) return;
        foreach(EnemyController enemyTask in enemyTaskList)
        {
            int currentIndex = enemyTaskList.IndexOf(enemyTask);
            //enemyTask.transform.position = enemyTaskCacheList[currentIndex].position;
            //enemyTask.transform.rotation = enemyTaskCacheList[currentIndex].rotation;
            enemyTask.SetEnemyType(enemyTaskCacheList[currentIndex].type);
            enemyTask.SetTaskComplete(enemyTaskCacheList[currentIndex].isComplete);
        }
    }

    private void LoadAnimalTaskCache()
    {
        List<AnimalTaskCache> animalTaskCacheList = Core.GetAnimalTaskCacheList();
        if (animalTaskCacheList == null) return;
        foreach (AnimalController animalTask in animalTaskList)
        {
            int currentIndex = animalTaskList.IndexOf(animalTask);
            animalTask.GetAnimalWaypoint().transform.position = animalTaskCacheList[currentIndex].position;
            animalTask.GetAnimal().transform.position = animalTaskCacheList[currentIndex].position;
            //animalTask.transform.rotation = animalTaskCacheList[currentIndex].rotation;
            animalTask.SetAnimalType(animalTaskCacheList[currentIndex].type);
            animalTask.SetTaskComplete(animalTaskCacheList[currentIndex].isComplete);
        }
    }
}
