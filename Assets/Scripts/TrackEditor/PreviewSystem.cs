using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PreviewSystem : MonoBehaviour
{
    [SerializeField]
    private float previewYOffset = 0.1f;

    [SerializeField]
    private GameObject previewObject;

    [SerializeField]
    internal AutoRotate previewObjectRotation;

    [SerializeField]
    private Material previewMaterialsPrefab;
    private Material previewMaterialInstance;


    private void Start()
    {
        previewMaterialInstance = new Material(previewMaterialsPrefab);
    }

    public void SetNewPreview(GameObject prefab, Vector2Int size, PlacementSystem placementSystem)
    {
        StartCoroutine(SetNewPreviewDelay(prefab, size, placementSystem));
    }

    private IEnumerator SetNewPreviewDelay(GameObject prefab, Vector2Int size, PlacementSystem placementSystem)
    {
        yield return new WaitForEndOfFrame();

        // set new preview indicator
        previewObject = Instantiate(prefab);
        previewObjectRotation = previewObject.GetComponentInChildren<AutoRotate>();
        PreparePreview(previewObject);
    }

    private void PreparePreview(GameObject previewObject)
    {
        Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = previewMaterialInstance;
            }
            renderer.materials = materials;
        }
    }

    public void StopShowingPreview()
    {
        // check valid
        if (previewObject == null)
            return;

        Destroy(previewObject);
    }

    public void UpdatePreview(Vector3 position, bool validity)
    {
        // check valid
        if (previewObject == null)
            return;

        // position
        previewObject.transform.position = new Vector3(position.x,
                                                       position.y + previewYOffset,
                                                       position.z);
        ApplyFeedbackToPreview(validity);
    }

    private void ApplyFeedbackToPreview(bool validity)
    {
        // colour
        Color c = validity ? Color.white : Color.red;
        c.a = 0.5f;
        previewMaterialInstance.color = c;
    }

    internal void StartShowingRemovePreview()
    {
        // temp
    }

    internal int GetCurrentRotationState()
    {
        return previewObjectRotation.GetRotationState();
    }
}
