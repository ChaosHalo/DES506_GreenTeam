using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject previewIndicator;
    private MeshRenderer previewRenderer;


    public void SetNewPreviewIndicator(GameObject prefab)
    {
        // set new preview indicator
        GameObject newObject = Instantiate(prefab);
        previewIndicator = newObject;
        previewRenderer = previewIndicator.GetComponentInChildren<MeshRenderer>();
    }

    public void ClearPreviewIndicator()
    {
        Destroy(previewIndicator);
        previewIndicator = null;
    }

    public void UpdatePreviewIndicatorPosition(Vector3 position)
    {
        if (previewIndicator)
            previewIndicator.transform.position = position;
    }
}
