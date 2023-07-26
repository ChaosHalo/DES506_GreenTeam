using Mono.Cecil.Cil;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
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
    internal List<Material> originalMaterialInstance=new();
    private Renderer[] renderers;

    [SerializeField]
    internal bool canScale = true;
    [SerializeField]
    private float scaleSpeed = 0.005f;
    private bool isConnected = false;
    public bool isConnectedToStart = false;
    public bool isStartPiece = false;
    internal bool hasCompletedChainCheck = false;
    internal bool GetConnectedStatus() { return isConnected; }
    internal bool IsConnectedToStart() { return isConnectedToStart; }

    internal ObjectData.ObjectType objectType;

    internal bool isSaved = false;
    internal void SetIsSaved(bool b) { isSaved = b; }
    private bool isPlaced = false;
    private bool isDeleted = false;
    private float deleteAnimSpeed = 12.5f;
    private Vector3 placedPos;
    private bool isFalling = false;
    private bool isFallingAnim = false;
    private bool wasPlacedByPlayer = false;
    internal bool GetFallingStatus() { return isFallingAnim; }
    [SerializeField] private float fallAnimSpeed = 12.5f;
    internal int verticalOffset = 100;


    // scale presets
    private float multiLarge = 1.2f;
    private float multiSmall = 1.0f;
    private Vector3 scaleSmall;
    private Vector3 scaleLarge;
    private Vector3 scaleNext;
    private Vector3 scaleOriginal;

    [Header("Additional Track Piece Components")]
    public List<GameObject> additionalObjects = new List<GameObject>();

    private void Awake()
    {
        scaleOriginal = transform.parent.localScale;
        scaleSmall = scaleOriginal * multiSmall;
        scaleSmall.y = scaleOriginal.y;
        scaleLarge = scaleOriginal * multiLarge;
        scaleLarge.y = scaleOriginal.y;
        scaleNext = scaleLarge;

        inputManager = FindObjectOfType<InputManager>();
        renderers = GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
            originalMaterialInstance.Add(renderer.material);
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

        if (isFalling)
        {
            Vector3 targetPosition = Vector3.Lerp(transform.parent.localPosition, placedPos, Time.deltaTime * fallAnimSpeed);

            if (objectType == ObjectData.ObjectType.Terrain)
            {
                targetPosition = wasPlacedByPlayer
                    ? Vector3.Lerp(transform.parent.localPosition, placedPos, Time.deltaTime * fallAnimSpeed / 200)
                    : Vector3.MoveTowards(transform.parent.localPosition, placedPos, Time.deltaTime * fallAnimSpeed);
            }

            transform.parent.localPosition = targetPosition;

            if (Vector3.Distance(transform.parent.localPosition, placedPos) < 0.5f)
            {
                isFalling = false;
                isFallingAnim = false;
                transform.parent.localPosition = placedPos;
                objectPlacer.UpdateTrackConnections();
            }
        }
    }

    private void PrepareConnectionMaterial()
    {
       // originalMaterialInstance=renderer.material;
       // renderer.material = connectionMaterialInstance;
       // Color c = new(0.1f, 0.1f, 0.1f);
       // Color c = Color.red;
       // c.a = 0.75f;
       // connectionMaterialInstance.color = c;

        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = connectionMaterialInstance;
            }
            renderer.materials = materials;
        }
        UpdateConnectionState();
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

    internal void BeginChainCheck()
    {
        ChainCheckAdjacent();
    }

    internal void ChainCheckAdjacent()
    {
        // don't check same piece twice
        if (hasCompletedChainCheck == true)
            return;
        hasCompletedChainCheck = true;

        // check if adjacent piece is start piece or connected to it
        List<PlacableObject> adjacentObjects = autoRotate.GetAdjacentObjects();
        foreach (PlacableObject adjacentObject in adjacentObjects)
            if (adjacentObject != null)
                if (adjacentObject.isStartPiece || adjacentObject.isConnectedToStart == true)
                    isConnectedToStart = true;

        // continue to chain to adjacent pieces
        foreach (PlacableObject adjacentObject in adjacentObjects)
            if (adjacentObject != null)
                adjacentObject.ChainCheckAdjacent();
    }

    internal void UpdateConnectionState()
    {
        if (objectType == ObjectData.ObjectType.Terrain)
            return;

        isConnected = autoRotate.CheckForConnections();

        if (connectionMaterialInstance)
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material = connectionMaterialInstance;
                renderers[i].material = isConnected ? originalMaterialInstance[i] : connectionMaterialInstance;
            }
            //renderer.material = isConnected ? originalMaterialInstance : connectionMaterialInstance;
        }
    }

    internal void SetIsPlaced(bool b)
    {
        isPlaced = b;

        if (objectType == ObjectData.ObjectType.Track)
            if (isPlaced == true)
            {
                transform.tag = "RaceTrackSurface";

                foreach(GameObject obj in additionalObjects)
                {
                    obj.transform.tag = "RaceTrackSurface";
                }
            }

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
        wasPlacedByPlayer = placedByUser;
        float delay = objectType == ObjectData.ObjectType.Terrain ? 0 : 0.5f;
        if (placedByUser == true)
            delay = 0;
        else
            verticalOffset = UnityEngine.Random.Range(300, 500);

        //verticalOffset = objectType == ObjectData.ObjectType.Terrain ? UnityEngine.Random.Range(300, 500) : 175;
        StartCoroutine(DelayFallAnim(delay));
    }

    IEnumerator DelayFallAnim(float delay)
    {
        isFallingAnim = true;
        ToggleRenderes(false);
        transform.parent.localPosition = new Vector3(50, transform.localPosition.y + verticalOffset, 50);
        placedPos = new Vector3(transform.parent.localPosition.x, transform.parent.localPosition.y - verticalOffset, transform.parent.localPosition.z);
        yield return new WaitForSeconds(delay);
        ToggleRenderes(true);
        isFalling = true;
    }

    private void ToggleRenderes(bool b)
    {
        foreach (MeshRenderer renderer in renderers)
            renderer.enabled = b;
    }
}
