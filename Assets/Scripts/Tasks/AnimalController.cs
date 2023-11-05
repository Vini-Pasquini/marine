using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalController : MonoBehaviour
{
    private ANIMAL_TYPE animalType = ANIMAL_TYPE._NoType;
    public ANIMAL_TYPE GetAnimalType() { return animalType; }
    public void SetAnimalType(ANIMAL_TYPE newType) { animalType = newType; }

    private bool isTaskComplete = false;
    public bool IsTaskComplete() { return isTaskComplete; }
    public void SetTaskComplete(bool newValue) { isTaskComplete = newValue; }

    private GameObject animalWaypoint;
    public GameObject GetAnimalWaypoint() { return animalWaypoint; }

    private AnimalAI animal;
    public GameObject GetAnimal() { return animal.gameObject; }

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

    public void AnimalTaskInit()
    {
        animalWaypoint = this.transform.GetChild(0).gameObject;
        animal = this.transform.GetChild(1).GetComponent<AnimalAI>();
        animal.SetWaypointObject(animalWaypoint);
        animal.SetAnimalSpeed(baseAnimalSpeed);
        animal.SetAnimalRotationRate(baseAnimalSpeed * .1f);
    }

    public void LoadAnimal()
    {
        SelectAnimalType();
        Transform animalGameObject = this.transform.GetChild(1);
        Transform animalModelGameObject = animalGameObject.GetChild(0);
        Transform animalHighlightGameObject = animalGameObject.GetChild(1);
        /* animal model init */
        GameObject animalModel = GameObject.Instantiate((GameObject)Resources.Load($"Models/Animals/{animalType}Model"));
        animalModel.transform.SetParent(animalModelGameObject, false);
        animalModel.transform.localPosition = Vector3.zero;
        animalModel.GetComponent<Animator>().runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load($"Animation/{animalType}Anim");
        /* animal highlight init */
        Transform animalModelTransform = animalModel.transform.GetChild(1);
        animalHighlightGameObject.position = animalModelTransform.position;
        animalHighlightGameObject.rotation = animalModelTransform.rotation;
        Mesh animalMesh = animalModel.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh;
        animalHighlightGameObject.GetComponent<MeshFilter>().mesh = animalMesh;
        animalHighlightGameObject.GetComponent<MeshCollider>().sharedMesh = animalMesh;
    }

    private void SelectAnimalType()
    {
        if (animalType != ANIMAL_TYPE._NoType) return;
        animalType = (ANIMAL_TYPE)Random.Range(1 + (int)ANIMAL_TYPE._COMMON, (int)ANIMAL_TYPE._COMMON_END);
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
