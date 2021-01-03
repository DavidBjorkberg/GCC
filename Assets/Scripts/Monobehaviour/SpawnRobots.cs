using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class SpawnRobots : MonoBehaviour
{
    //public GameObject robotPrefab;
    //public float minSpawnDistance;
    //public float maxSpawnDistance;
    //private Construct construct;
    //private void Awake()
    //{
    //    construct = GetComponent<Construct>();
    //}
    //void Start()
    //{
    //    var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
    //    var prefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(robotPrefab, settings);
    //    var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

    //    int totalNrOfPolygons = construct.GetTotalNrOfPolygons();
    //    for (int i = 0; i < totalNrOfPolygons; i++)
    //    {
    //        Vector2 randomDir;
    //        float randomDistance;
    //        Vector2 randomPoint;
    //        Vector3 randomPointAroundConstruction;
    //        randomDir = Random.insideUnitCircle.normalized;
    //        randomDistance = Random.Range(minSpawnDistance, maxSpawnDistance);
    //        randomPoint = randomDir * randomDistance;
    //        var instance = entityManager.Instantiate(prefab);

    //        randomPointAroundConstruction = new Vector3(transform.position.x + randomPoint.x, transform.position.y, transform.position.z + randomPoint.y);


    //        int claimedPolygon = construct.GetAndClaimNextFreePolygon();
    //        RobotMovementData data;
    //        data.movementSpeed = 5;
    //        data.target = transform.position + construct.GetCenterOfPolygon(claimedPolygon);
    //        data.startPos = randomPointAroundConstruction;
    //        data.lerpValue = 0;
    //        data.targetNormal = construct.GetNormalOfPolygon(claimedPolygon);

    //        entityManager.SetComponentData(instance, data);
    //        entityManager.SetComponentData(instance, new Translation { Value = randomPointAroundConstruction });
    //    }
    //}
}
