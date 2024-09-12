using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Administrador que gestiona la actualizaci�n de objetos a una tasa fija usando deltaTime.
/// </summary>
public class UpdateManager : MonoBehaviour
{
    private List<Updateable> _objsToUpdate = new List<Updateable>(); // Lista de objetos que necesitan ser actualizados.
    public static UpdateManager Instance { get; private set; } // Instancia �nica del UpdateManager.

    private void Awake()
    {
        // Configura el patr�n Singleton.
        if (Instance != null && Instance != this)
        {
            Destroy(this);  // Destruye el objeto si ya existe una instancia.
        }
        else
        {
            Instance = this;  // Establece la instancia actual como la �nica.
        }
    }

    private void Update()
    {
        // Recorre la lista de objetos que necesitan ser actualizados y llama a su m�todo Tick.
        foreach (var obj in _objsToUpdate)
        {
            obj.Tick();  // Llama al m�todo Tick de cada objeto en la lista.
        }
    }

    /// <summary>
    /// A�ade un objeto a la lista de objetos que necesitan ser actualizados.
    /// </summary>
    /// <param name="obj">Objeto que implementa la interfaz Updateable.</param>
    public void Add(Updateable obj)
    {
        // A�ade el objeto solo si no est� ya en la lista.
        if (!_objsToUpdate.Contains(obj))
        {
            _objsToUpdate.Add(obj);
        }
    }

    /// <summary>
    /// Elimina un objeto de la lista de objetos que necesitan ser actualizados.
    /// </summary>
    /// <param name="obj">Objeto que implementa la interfaz Updateable.</param>
    public void Remove(Updateable obj)
    {
        // Elimina el objeto solo si est� en la lista.
        if (_objsToUpdate.Contains(obj))
        {
            _objsToUpdate.Remove(obj);
        }
    }
}

/// <summary>
/// Base para objetos que necesitan ser actualizados por el UpdateManager.
/// </summary>
public class Updateable : MonoBehaviour
{
    public float updateInterval = 1.0f / 60.0f; // Intervalo de actualizaci�n en segundos (60 FPS).
    private float _elapsedTime = 0f; // Tiempo acumulado desde la �ltima actualizaci�n.

    protected virtual void Awake()
    {
        // Configuraciones iniciales si es necesario.
    }

    protected virtual void Start()
    {
        UpdateManager.Instance.Add(this); // A�ade el objeto al UpdateManager.
    }

    /// <summary>
    /// M�todo llamado en cada frame por el UpdateManager.
    /// </summary>
    public void Tick()
    {
        _elapsedTime += Time.deltaTime; // Acumula el tiempo transcurrido desde el �ltimo frame.

        // Verifica si el tiempo acumulado ha alcanzado el intervalo de actualizaci�n.
        if (_elapsedTime >= updateInterval)
        {
            CustomUpdate();  // Llama al m�todo CustomUpdate de cada objeto.
            _elapsedTime -= updateInterval; // Resta el intervalo para mantener el tiempo restante.
        }
    }

    /// <summary>
    /// M�todo virtual que puede ser sobreescrito por las clases derivadas.
    /// </summary>
    protected virtual void CustomUpdate()
    {
        // Se deja este m�todo vac�o para que las clases derivadas incorporen distintas funcionalidades en el Update
    }

    protected virtual void OnDestroy()
    {
        UpdateManager.Instance.Remove(this); // Elimina el objeto del UpdateManager al destruirlo.
    }
}
