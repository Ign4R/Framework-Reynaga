/// <summary>
/// Una implementaci�n gen�rica de una m�quina de estados finitos (FSM).
/// Permite gestionar la transici�n entre diferentes estados basados en entradas y actualiza el estado actual.
/// </summary>
/// <typeparam name="T">Tipo de entrada que se usa para las transiciones entre estados.</typeparam>

public class FSM<T>
{
    private IState<T> _current; // Estado actual de la m�quina de estados.

    /// <summary>
    /// Propiedad que obtiene o establece el estado actual de la m�quina de estados.
    /// </summary>
    public IState<T> Current
    {
        get => _current;
        set => _current = value;
    }

    /// <summary>
    /// Constructor por defecto para la m�quina de estados finitos.
    /// </summary>
    public FSM()
    {
    }

    /// <summary>
    /// Constructor que inicializa la m�quina de estados con un estado inicial.
    /// </summary>
    /// <param name="init">Estado inicial de la m�quina de estados.</param>
    public FSM(IState<T> init)
    {
        SetInit(init);
    }

    /// <summary>
    /// Establece el estado inicial y llama al m�todo Awake del estado.
    /// </summary>
    /// <param name="init">Estado inicial que se va a establecer.</param>
    public void SetInit(IState<T> init)
    {
        _current = init;  // Establece el estado actual.
        _current.Awake();  // Llama al m�todo Awake del estado inicial.
    }

    /// <summary>
    /// Actualiza el estado actual llamando a su m�todo Execute.
    /// Se debe llamar a este m�todo en cada ciclo de actualizaci�n del juego.
    /// </summary>
    public void OnUpdate()
    {
        if (_current != null)
            _current.Execute();  // Ejecuta la l�gica del estado actual.
    }

    /// <summary>
    /// Realiza una transici�n al siguiente estado basado en la entrada proporcionada.
    /// </summary>
    /// <param name="input">Entrada usada para determinar el nuevo estado.</param>
    public void Transitions(T input)
    {
        IState<T> newState = _current.GetTransition(input);  // Obtiene el nuevo estado basado en la entrada.

        // Si no hay un nuevo estado y la entrada no es el valor por defecto, no realiza la transici�n.
        // Esto evita hacer cambios de estado innecesarios cuando no hay un estado v�lido al que cambiar.
        // Tambi�n previene transiciones basadas en entradas que no tienen valor significativo.
        if (newState == null && !input.Equals(default(T))) return;

        _current.Sleep();  // Llama al m�todo Sleep del estado actual.
        _current = newState;  // Establece el nuevo estado.
        if (_current == null) return;  // Si el nuevo estado es nulo, no realiza ninguna acci�n adicional.
        _current.Awake();  // Llama al m�todo Awake del nuevo estado.
    }
}


