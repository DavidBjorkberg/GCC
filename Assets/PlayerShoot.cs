using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform gunPoint;
    BlobAssetStore blob;
    // Start is called before the first frame update
    void Start()
    {
        blob = new BlobAssetStore();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blob);
            var prefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(bulletPrefab, settings);
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            var instance = entityManager.Instantiate(prefab);

            var position = transform.TransformPoint(Vector3.zero);

            BulletData bulletData = entityManager.GetComponentObject<BulletData>(instance);
            bulletData.direction = Camera.main.transform.forward;
            entityManager.SetComponentData(instance, bulletData);
            entityManager.SetComponentData(instance, new Translation { Value = position });
        }
    }
    private void OnDestroy()
    {
        blob.Dispose();
    }
}
