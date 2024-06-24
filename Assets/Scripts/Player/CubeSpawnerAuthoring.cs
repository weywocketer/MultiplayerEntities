using Unity.Entities;
using UnityEngine;

public struct CubeSpawner : IComponentData
{
    public Entity Player1;
    public Entity Player2;
}

[DisallowMultipleComponent]
public class CubeSpawnerAuthoring : MonoBehaviour
{
    public GameObject Player1;
    public GameObject Player2;

    class Baker : Baker<CubeSpawnerAuthoring>
    {
        public override void Bake(CubeSpawnerAuthoring authoring)
        {
            CubeSpawner component = default(CubeSpawner);
            component.Player1 = GetEntity(authoring.Player1, TransformUsageFlags.Dynamic);
            component.Player2 = GetEntity(authoring.Player2, TransformUsageFlags.Dynamic);
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, component);
        }
    }
}