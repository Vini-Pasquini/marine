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
        CacheTasks();
    }

    private void LoadTaskCache()
    {
        LoadEnemyTaskCache();
        LoadAnimalTaskCache();
    }

    private void LoadEnemyTaskCache()
    {
        List<EnemyTaskCache> enemyTaskCacheList = Core.GetEnemyTaskCacheList();
        if (enemyTaskCacheList.Count == 0) { Debug.Log($"enemyTaskCacheList.Count == {enemyTaskCacheList.Count}"); return; } // might have to change later
        foreach(EnemyController enemyTask in enemyTaskList)
        {
            int currentIndex = enemyTaskList.IndexOf(enemyTask);
            Debug.Log($"Enemy {currentIndex}");
            //enemyTask.transform.position = enemyTaskCacheList[currentIndex].position;
            //enemyTask.transform.rotation = enemyTaskCacheList[currentIndex].rotation;
            enemyTask.SetEnemyType(enemyTaskCacheList[currentIndex].type);
            enemyTask.SetTaskComplete(enemyTaskCacheList[currentIndex].isComplete);
        }
        Core.ClearEnemyTaskCache();
    }

    private void LoadAnimalTaskCache()
    {
        List<AnimalTaskCache> animalTaskCacheList = Core.GetAnimalTaskCacheList();
        if (animalTaskCacheList.Count == 0) { Debug.Log($"animalTaskCacheList.Count == {animalTaskCacheList.Count}"); return; } // might have to change later
        foreach (AnimalController animalTask in animalTaskList)
        {
            int currentIndex = animalTaskList.IndexOf(animalTask);
            Debug.Log($"Animal {currentIndex}");
            GameObject animalWaypoint = animalTask.GetAnimalWaypoint();
            if (animalWaypoint != null) animalWaypoint.transform.position = animalTaskCacheList[currentIndex].position;
            else animalTask.StageWaypointCacheLoad(animalTaskCacheList[currentIndex].position);
            GameObject animal = animalTask.GetAnimal();
            if (animal != null) animal.transform.position = animalTaskCacheList[currentIndex].position;
            else animalTask.StageAnimalCacheLoad(animalTaskCacheList[currentIndex].position);
            //animalTask.transform.rotation = animalTaskCacheList[currentIndex].rotation;
            animalTask.SetAnimalType(animalTaskCacheList[currentIndex].type);
            animalTask.SetTaskComplete(animalTaskCacheList[currentIndex].isComplete);
        }
        Core.ClearAnimalTaskCache();
    }

    public void CacheTasks()
    {
        CacheEnemyTasks();
        CacheAnimalTasks();
    }

    private void CacheEnemyTasks()
    {
        if (enemyTaskList == null || enemyTaskList.Count == 0) return;
        Core.ClearEnemyTaskCache();
        foreach (EnemyController enemyTask in enemyTaskList)
        {
            Core.CacheEnemyTask(enemyTask.GetEnemyType(), enemyTask.transform.position, enemyTask.transform.rotation, enemyTask.IsTaskComplete());
        }
    }

    private void CacheAnimalTasks()
    {
        if (animalTaskList == null || animalTaskList.Count == 0) return;
        Core.ClearAnimalTaskCache();
        foreach (AnimalController animalTask in animalTaskList)
        {
            GameObject animalWaypoint = animalTask.GetAnimalWaypoint();
            Core.CacheAnimalTask(animalTask.GetAnimalType(), animalWaypoint == null ? animalTask.transform.position : animalWaypoint.transform.position, animalTask.transform.rotation, animalTask.IsTaskComplete());
        }
    }
}
