using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class InputManager : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private string _currentControlScheme;
    [SerializeField] private bool _isUsingGamepad;

    private void Awake()
    {
        // Récupérer la référence du PlayerInput
        _playerInput = GetComponent<PlayerInput>();
    }

    //private void Update()
    //{
    //    // Exemple de vérification de l'input
    //    if (IsUsingGamepad())
    //    {
    //        Debug.Log("Utilisation de la manette");
    //    }
    //    else
    //    {
    //        Debug.Log("Utilisation du clavier/souris");
    //    }
    //}

    // Callback pour InputUser.onChange
    //private void OnInputDeviceChanged(InputUser user, InputUserChange change, InputDevice device)
    //{
    //    if (change == InputUserChange.ControlSchemeChanged)
    //    {
    //        UpdateControlScheme(user.controlScheme.ToString());
    //    }
    //}

    // Callback pour PlayerInput.onControlsChanged
    public void OnControlsChanged(PlayerInput input)
    {
        //Debug.Log("Schéma de contrôle changé: " + input.currentControlScheme);
        UpdateControlScheme(input.currentControlScheme);
    }

    // Met à jour le schéma de contrôle actuel et détecte si on utilise une manette ou non
    private void UpdateControlScheme(string controlScheme)
    {
        _currentControlScheme = controlScheme;
        _isUsingGamepad = _currentControlScheme == "Gamepad"; // Remplacez par le nom exact de votre schéma Gamepad
        //Debug.Log("Schéma de contrôle mis à jour: " + _currentControlScheme);
    }

    // Vérifie si l'on utilise actuellement une manette
    public bool IsUsingGamepad()
    {
        return _isUsingGamepad;
    }

    // Retourne le schéma d'input actuel (manette ou clavier/souris)
    public string GetCurrentControlScheme()
    {
        return _currentControlScheme;
    }
}
