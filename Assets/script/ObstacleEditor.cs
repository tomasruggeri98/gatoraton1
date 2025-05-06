using UnityEngine;
using UnityEngine.Tilemaps;
using Pathfinding;

public class ObstacleEditor : MonoBehaviour
{
    [Header("Tilemap & Tiles")]
    public Tilemap tilemap;            // Tu Tilemap con TilemapCollider2D + CompositeCollider2D
    public TileBase obstacleTile;      // El Tile que usarás como muro

    [Header("Player (Cat) Settings")]
    public GameObject cat;             // GameObject con Seeker, Rigidbody2D, Collider2D y CatGridChase

    [Header("Controls")]
    public KeyCode startChaseKey = KeyCode.Return;

    // Internos
    private Seeker seeker;
    private CatGridChase gridChase;
    private bool editMode = true;

    void Start()
    {
        // Referencias a la IA del gato
        seeker = cat.GetComponent<Seeker>();
        gridChase = cat.GetComponent<CatGridChase>();

        // Desactiva el Seeker y la persecución mientras editamos
        if (seeker) seeker.enabled = false;
        if (gridChase) gridChase.enabled = false;
    }

    void Update()
    {
        if (!editMode) return;

        HandlePlacement();

        if (Input.GetKeyDown(startChaseKey))
        {
            BeginChase();
        }
    }

    void HandlePlacement()
    {
        // Calcula la celda bajo el cursor
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cell = tilemap.WorldToCell(worldPos);
        TileBase current = tilemap.GetTile(cell);

        // Click izquierdo → coloca si no hay tile
        if (Input.GetMouseButtonDown(0) && current == null)
        {
            tilemap.SetTile(cell, obstacleTile);
            tilemap.RefreshTile(cell);
        }
        // Click derecho → borra si hay tile
        else if (Input.GetMouseButtonDown(1) && current != null)
        {
            tilemap.SetTile(cell, null);
            tilemap.RefreshTile(cell);
        }
    }

    void BeginChase()
    {
        editMode = false;

        // Actualiza todos los colliders del Tilemap
        tilemap.RefreshAllTiles();

        // Re-scan del grafo A* para incluir la capa Obstacles
        // Asegúrate en tu AstarPath → Graphs → Collision Testing 
        // de tener marcada la capa "Obstacles"
        AstarPath.active.Scan();

        // Reactiva la IA del gato
        if (seeker) seeker.enabled = true;
        if (gridChase) gridChase.enabled = true;

        // Inicia la persecución por casillas
        if (gridChase) gridChase.BeginChase();
    }
}
