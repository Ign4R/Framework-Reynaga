using System;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    [Header("||--Line Of View--||")]
    public float _radiusView;    // Radio en el cual la ara�a puede detectar al objetivo.
    public float _angleView;     // �ngulo de visi�n de la ara�a.
    public LayerMask _ignoreMask; // Capas que deben ser ignoradas en los rayos de visi�n.
    public Transform _target;  // Referencia al objetivo (por ejemplo, el jugador). [Tambien podria ser tratada como un Vector 3]

    /// <summary>
    /// Verifica si el objetivo est� dentro del rango de visi�n definido.
    /// </summary>
    /// <param name="target">Transform del objetivo (jugador u otro).</param>
    /// <returns>Devuelve true si el objetivo est� dentro del rango, false en caso contrario.</returns>
    public bool CheckRange(Transform target)
    {
        // Calcula la distancia entre la ara�a y el objetivo
        float distance = Vector3.Distance(transform.position, target.position);

        // Comprueba si la distancia es menor al radio de visi�n de la ara�a
        return distance < _radiusView;
    }

    /// <summary>
    /// Verifica si el objetivo est� dentro del �ngulo de visi�n de la ara�a.
    /// </summary>
    /// <param name="target">Transform del objetivo (jugador u otro).</param>
    /// <returns>Devuelve true si el objetivo est� dentro del �ngulo de visi�n, false en caso contrario.</returns>
    public bool CheckAngle(Transform target)
    {
        // Direcci�n hacia el objetivo desde la posici�n de la ara�a
        Vector3 forward = transform.forward;
        Vector3 dirToTarget = (target.position - transform.position).normalized;

        // Calcula el �ngulo entre la direcci�n hacia el objetivo y la orientaci�n actual de la ara�a
        float angleToTarget = Vector3.Angle(forward, dirToTarget);

        // Comprueba si el �ngulo es menor a la mitad del �ngulo de visi�n
        return _angleView / 2 > angleToTarget;
    }

    /// <summary>
    /// Verifica si el objetivo es visible directamente desde la posici�n de la ara�a.
    /// </summary>
    /// <param name="target">Transform del objetivo (jugador u otro).</param>
    /// <returns>Devuelve true si el objetivo es visible, false en caso contrario.</returns>
    public bool CheckView(Transform target)
    {
        // Calcula la direcci�n y la distancia al objetivo
        Vector3 diff = target.position - transform.position;
        float distanceToTarget = diff.magnitude;
        Vector3 dirToTarget = diff.normalized;

        // Ajusta la posici�n de origen del rayo para que sea a nivel del suelo
        Vector3 fixedOriginY = transform.position;

        RaycastHit hit;

        // Lanza un rayo desde la ara�a hacia el objetivo, ignorando ciertas capas
        return !Physics.Raycast(fixedOriginY, dirToTarget, out hit, distanceToTarget, _ignoreMask);
    }
}
