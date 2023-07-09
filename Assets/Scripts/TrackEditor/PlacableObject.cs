using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacableObject : MonoBehaviour
{
    [SerializeField]
    internal InputManager inputManager;
    [SerializeField]
    internal AutoRotate autoRotate;
    [SerializeField]
    private GameObject lockedIndicator;
    [SerializeField]
    private GameObject unconnectedIndicator;
    [SerializeField]
    internal ObjectPlacer objectPlacer;

    [SerializeField]
    private Material connectionMaterialPrefab;
    private Material connectionMaterialInstance;
    private Material originalMaterialInstance;
    private Renderer renderer;

    [SerializeField]
    internal bool canScale = true;
    [SerializeField]
    private float scaleSpeed = 0.005f;
    [SerializeField]
    private bool isConnected = false;
    internal bool GetConnectedStatus() { return isConnected; }

    internal ObjectData.ObjectType objectType;

    private bool isPlaced = false;
    private bool isDeleted = false;
    private float deleteAnimSpeed = 12.5f;
    private Vector3 placedPos;
    private bool isFalling = false;
    [SerializeField] private float fallAnimSpeed = 12.5f;
    internal int verticalOffset = 100;


    // scale presets
    private float multiLarge = 1.2f;
    private float multiSmall = 1.0f;
    private Vector3 scaleSmall;
    private Vector3 scaleLarge;
    private Vector3 scaleNext;
    private Vector3 scaleOriginal;

    private void Awake()
    {
        scaleOriginal = transform.parent.localScale;
        scaleSmall = scaleOriginal * multiSmall;
        scaleSmall.y = scaleOriginal.y;
        scaleLarge = scaleOriginal * multiLarge;
        scaleLarge.y = scaleOriginal.y;
        scaleNext = scaleLarge;

        inputManager = FindObjectOfType<InputManager>();
        renderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        if (objectType == ObjectData.ObjectType.Track)
        {
            connectionMaterialInstance = new Material(connectionMaterialPrefab);
            PrepareConnectionMaterial();
        }

    }

    private void Update()
    {
        UpdateScale();

        if (isDeleted)
            transform.parent.localScale = Vector3.Lerp(transform.parent.localScale, Vector3.zero, Time.deltaTime * deleteAnimSpeed);

        if (isFalling == true)
        {
            transform.parent.localPosition = Vector3.Lerp(transform.parent.localPosition, placedPos, Time.deltaTime * fallAnimSpeed);
            if (Vector3.Distance(transform.parent.localPosition, placedPos) < 0.5f)
            {
                isFalling = false;
                transform.parent.localPosition = placedPos;
                objectPlacer.UpdateTrackConnections();
            }
        }
    }

    private void PrepareConnectionMaterial()
    {
        originalMaterialInstance=renderer.material;
        renderer.material = connectionMaterialInstance;
       // Color c = new(0.1f, 0.1f, 0.1f);
       // Color c = Color.red;
       // c.a = 0.75f;
       // connectionMaterialInstance.color = c;
        UpdateConnectionState();
        //Renderer[] renderers = GetComponentsInChildren<Renderer>();
        //foreach (Renderer renderer in renderers)
        //{
        //    Material[] materials = renderer.materials;
        //    for (int i = 0; i < materials.Length; i++)
        //    {
        //        materials[i] = connectionMaterialInstance;
        //    }
        //    renderer.materials = materials;
        //}
    }

    internal virtual void UpdateScale()
    {
        //if (canScale == false)
        //    return;

        //transform.parent.localScale = Vector3.MoveTowards(transform.parent.localScale, scaleNext, scaleSpeed * Time.deltaTime);

        //if (transform.parent.localScale == scaleLarge)
        //    scaleNext = scaleSmall;
        //if (transform.parent.localScale == scaleSmall)
        //    scaleNext = scaleLarge;
    }

    internal void StopScaling()
    {
        canScale = false;
        transform.parent.localScale = scaleOriginal;
    }

    internal void SetModifyable(bool canModify)
    {
        if (canModify == false)
            lockedIndicator.SetActive(true);
    }

    internal void UpdateConnectionState()
    {
        if (objectType == ObjectData.ObjectType.Terrain)
            return;

        isConnected = autoRotate.CheckForConnections();
        //unconnectedIndicator.SetActive(!isConnected);

        if (connectionMaterialInstance)
        {
            renderer.material = isConnected ? originalMaterialInstance : connectionMaterialInstance;
        }
    }

    internal void SetIsPlaced(bool b)
    {
        isPlaced = b;

        if (objectType == ObjectData.ObjectType.Track)
            if (isPlaced == true)
                transform.tag = "RaceTrackSurface";

        if (objectType == ObjectData.ObjectType.Terrain)
            GetComponentInChildren<TerrainObject>().GenerateObjects();
    }

    internal void OnDelete()
    {
        BoxCollider[] allColliders =GetComponentsInParent<BoxCollider>();
        foreach (BoxCollider collider in allColliders)
            collider.enabled = false;
        isDeleted = true;
    }

    internal void TriggerFallAnim(bool placedByUser)
    {
        float delay = objectType == ObjectData.ObjectType.Terrain ? 0 : 1f;
        if (placedByUser == true)
            delay = 0;
        else
            verticalOffset = UnityEngine.Random.Range(300, 500);

        StartCoroutine(DelayFallAnim(delay));
    }

    IEnumerator DelayFallAnim(float delay)
    {
        GetComponent<MeshRenderer>().enabled = false;
        transform.parent.localPosition = new Vector3(transform.localPosition.x + 50, transform.localPosition.y + verticalOffset, transform.localPosition.z + 50);
        placedPos = new Vector3(transform.parent.localPosition.x, transform.parent.localPosition.y - verticalOffset, transform.parent.localPosition.z);
        yield return new WaitForSeconds(delay);
        GetComponent<MeshRenderer>().enabled = true;
        isFalling = true;
    }
}
