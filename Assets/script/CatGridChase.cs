using System.Collections;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Seeker))]
public class CatGridChase : MonoBehaviour
{
    [Tooltip("El transform del ratón")]
    public Transform target;
    [Tooltip("Duración en segundos de mover una casilla")]
    public float moveDuration = 0.3f;

    private Seeker seeker;
    private Path path;
    private int currentWaypoint;

    void Awake()
    {
        seeker = GetComponent<Seeker>();
    }

    /// <summary>
    /// Lanza la búsqueda de ruta desde la posición actual al target.
    /// </summary>
    public void BeginChase()
    {
        if (target == null) return;
        // StartPath calcula la ruta asíncronamente y llama a OnPathComplete
        seeker.StartPath(transform.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
            StopAllCoroutines();
            StartCoroutine(FollowPath());
        }
    }

    IEnumerator FollowPath()
    {
        // Recorre todos los waypoints (centros de casilla)
        while (currentWaypoint < path.vectorPath.Count)
        {
            Vector3 nextPos = path.vectorPath[currentWaypoint];
            // Moverse interpolando entre celdas
            yield return StartCoroutine(MoveOneCell(transform.position, nextPos));
            currentWaypoint++;
        }
        // Al llegar al final, espera un momento y recalcula
        yield return new WaitForSeconds(0.5f);
        BeginChase();
    }

    IEnumerator MoveOneCell(Vector3 from, Vector3 to)
    {
        float t = 0f;
        while (t < moveDuration)
        {
            transform.position = Vector3.Lerp(from, to, t / moveDuration);
            t += Time.deltaTime;
            yield return null;
        }
        transform.position = to;
    }
}
