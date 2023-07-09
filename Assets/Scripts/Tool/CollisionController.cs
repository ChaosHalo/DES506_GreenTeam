using UnityEngine;

public static class CollisionController
{
    /// <summary>
    /// 启用物体之间的碰撞
    /// </summary>
    /// <param name="object1"></param>
    /// <param name="object2"></param>
    public static void EnableCollisionBetweenGameObjects(GameObject object1, GameObject object2)
    {
        Physics.IgnoreCollision(object1.GetComponent<Collider>(), object2.GetComponent<Collider>(), false);
    }
    /// <summary>
    /// 禁用物体之间的碰撞
    /// </summary>
    /// <param name="object1"></param>
    /// <param name="object2"></param>
    public static void DisableCollisionBetweenGameObjects(GameObject object1, GameObject object2)
    {
        Physics.IgnoreCollision(object1.GetComponent<Collider>(), object2.GetComponent<Collider>(), true);
    }
}
