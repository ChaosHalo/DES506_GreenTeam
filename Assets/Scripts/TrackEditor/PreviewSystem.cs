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
    private PlacableObject previewObjectClass;

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
        previewObjectClass = previewObject.GetComponentInChildren<PlacableObject>();
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

        if(previewObjectClass != null)
        {
            if(previewObjectClass.placementIndicator != null)
            {
                previewObjectClass.placementIndicator.gameObject.SetActive(true);
            }
        }
    }

    public void StopShowingPreview()
    {
        // check valid
        if (previewObject == null)
            return;

        previewObjectClass = null;

        Destroy(previewObject);
    }

    public void UpdatePreview(Vector3 objectPosition, Vector3 indicatorPosition, bool validity)
    {
        // check valid
        if (previewObject == null)
            return;

        // object position
        previewObject.transform.position = new Vector3(objectPosition.x,
                                                       objectPosition.y + previewYOffset,
                                                       objectPosition.z);

        // preview indicator positon
        if (previewObjectClass != null)
        {
            if (previewObjectClass.placementIndicator != null)
            {
                previewObjectClass.placementIndicator.transform.position = new Vector3(indicatorPosition.x,
                                                               indicatorPosition.y + previewYOffset + previewObjectClass.indicatorYOffset,
                                                               indicatorPosition.z);
            }
        }


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
