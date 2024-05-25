using UnityEngine;

public class PlaceFinishPrefab : MonoBehaviour
{
    public GameObject cubeObject; // Küp objesi
    public GameObject finishPrefab; // Yerleştirilecek finish prefab

    void Start()
    {
        // Küp objesinin boyutlarını al
        float cubeDepth = cubeObject.transform.localScale.z;

        // finishPrefab objesinin boyutlarını al
        Vector3 finishBounds = finishPrefab.GetComponent<MeshRenderer>().bounds.size;
        float finishDepth = finishBounds.z;

        // Yeni pozisyonu hesapla
        Vector3 newPosition = new Vector3(
            cubeObject.transform.position.x,
            cubeObject.transform.position.y,
            cubeObject.transform.position.z +
            (cubeDepth * 3) + // 3 küp boyu ileride
            (cubeDepth / 2) + // küpün yarısı kadar daha
            (finishDepth / 2) // finishPrefab'in yarısı kadar daha
        );

        // finishPrefab'i yeni pozisyona yerleştir
        finishPrefab.transform.position = newPosition;
    }
}