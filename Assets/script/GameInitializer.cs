// GameInitializer.cs
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    public ObstacleEditor obstacleEditor;  // Arrastra aqu� tu ObstacleEditor
    public Camera gameCamera;              // Arrastra aqu� la Main Camera
    public float tileSize = 1f;            // Debe coincidir con tu Grid.cellSize
    public float cameraMargin = 1f;        // Margen extra para la c�mara

    void Start()
    {
        int w = GameSettings.GridWidth;
        int h = GameSettings.GridHeight;

        // Establece los l�mites y activa el editor sin deshabilitarlo
        obstacleEditor.SetGridBounds(w, h);

        // Centra y ajusta la c�mara al �rea de edici�n
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
