using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalController : MonoBehaviour
{
    private ANIMAL_TYPE animalType;
    public ANIMAL_TYPE GetAnimalType() { return animalType; }
    public void SetAnimalType(ANIMAL_TYPE newType) { animalType = newType; }

    private bool isTaskComplete = false;
    public bool IsTaskComplete() { return isTaskComplete; }
    public void SetTaskComplete(bool newValue) { isTaskComplete = newValue; }

    private GameObject animalWaypoint;
    public GameObject GetAnimalWaypoint() { return animalWaypoint; }

    private Vector3 waypointSatgedPosition;
    private bool isWaypointCacheLoadStaged = false;
    public void StageWaypointCacheLoad(Vector3 cachedPosition) { waypointSatgedPosition = cachedPosition; isWaypointCacheLoadStaged = true; }

    private AnimalAI animal;
    public GameObject GetAnimal() { return animal != null ? animal.gameObject : null; }

    private Vector3 animalSatgedPosition;
    private bool isAnimalCacheLoadStaged = false;
    public void StageAnimalCacheLoad(Vector3 cachedPosition) { animalSatgedPosition = cachedPosition; isAnimalCacheLoadStaged = true; }

    GameObject targetObject = null;

    public bool IsFollowingBoat()
    {
        return targetObject != null;
    }

    private float baseAnimalSpeed = 5f;

    private bool isSafe = false;

    public bool GetIsSafe()
    {
        return isSafe;
    }

    private void Start()
    {
        Transform animalGameObject = this.transform.GetChild(1);
        Transform animalModelGameObject = animalGameObject.GetChild(0);
        Transform animalHighlightGameObject = animalGameObject.GetChild(1);
        /* animal init */
        animalType = (ANIMAL_TYPE)Random.Range(0, (int)ANIMAL_TYPE._COUNT);
        GameObject animalModel = GameObject.Instantiate((GameObject)Resources.Load($"Models/Animals/{animalType}Model"));
        animalModel.transform.SetParent(animalModelGameObject, false);
        animalModel.transform.localPosition = Vector3.zero;
        /* animal highlight init */
        animalHighlightGameObject.rotation = animalModel.transform.rotation;
        Mesh animalMesh = animalModel.GetComponent<MeshFilter>().mesh;
        animalHighlightGameObject.GetComponent<MeshFilter>().mesh = animalMesh;
        animalHighlightGameObject.GetComponent<MeshCollider>().sharedMesh = animalMesh;
        /*  */
        animalWaypoint = this.transform.GetChild(0).gameObject;
        if (isWaypointCacheLoadStaged)
        {
            animalWaypoint.transform.position = waypointSatgedPosition;
            isWaypointCacheLoadStaged = false;
        }

        animal = this.transform.GetChild(1).GetComponent<AnimalAI>();
        animal.SetWaypointObject(animalWaypoint);
        animal.SetAnimalSpeed(baseAnimalSpeed);
        animal.SetAnimalRotationRate(baseAnimalSpeed * .1f);
        if (isAnimalCacheLoadStaged)
        {
            animal.transform.position = animalSatgedPosition;
            isAnimalCacheLoadStaged = false;
        }
    }

    private void Update()
    {
        TrackTargetObject();
    }

    private void TrackTargetObject()
    {
        if (targetObject == null) return;
        animalWaypoint.transform.position = targetObject.transform.position;
    }

    public void SetTargetObject(GameObject newTarget)
    {
        animal.SetAnimalSpeed(newTarget != null ? baseAnimalSpeed * 3 : baseAnimalSpeed);
        animal.SetAnimalRotationRate(newTarget != null ? baseAnimalSpeed * .3f : baseAnimalSpeed * .1f);
        RaycastHit hit;
        if (newTarget == null && Physics.Raycast((animalWaypoint.transform.position + Vector3.up), Vector3.down, out hit, Mathf.Infinity, 1 << (int)LAYERS.RescueArea))
        {
            isTaskComplete = true;

            isSafe = true; // old, delete later
            Core.IncrementPlayerGold(10);
            Core.IncrementAnimalCount(-1);
        }
        targetObject = newTarget;
    }
}
