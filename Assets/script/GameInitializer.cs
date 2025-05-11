// GameInitializer.cs
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    public ObstacleEditor obstacleEditor;  // Arrastra aquí tu ObstacleEditor
    public Camera gameCamera;              // Arrastra aquí la Main Camera
    public float tileSize = 1f;            // Debe coincidir con tu Grid.cellSize
    public float cameraMargin = 1f;        // Margen extra para la cámara

    void Start()
    {
        int w = GameSettings.GridWidth;
        int h = GameSettings.GridHeight;

        // Establece los límites y activa el editor sin deshabilitarlo
        obstacleEditor.SetGridBounds(w, h);

        // Centra y ajusta la cámara al área de edición
        CenterAndZoomCamera(w, h);
    }

    void CenterAndZoomCamera(int width, int height)
    {
        float cx = (width * tileSize) / 2f - tileSize / 2f;
        float cy = (height * tileSize) / 2f - tileSize / 2f;
        gameCamera.transform.position = new Vector3(cx, cy, gameCamera.transform.position.z);

        float halfH = (height * tileSize) / 2f + cameraMargin;
        float halfW = (width * tileSize) / 2f + cameraMargin;
        float aspect = gameCamera.aspect;
        float sizeY = halfH;
        float sizeX = halfW / aspect;
        gameCamera.orthographicSize = Mathf.Max(sizeY, sizeX);
    }
}
