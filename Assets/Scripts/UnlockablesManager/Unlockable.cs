using ORMish;
using UnityEngine;

public class Unlockable : MonoBehaviour
{
    private UnlockableScriptableObject _unlockableScriptableObject;
    public UnlockableScriptableObject UnlockableScriptableObject => _unlockableScriptableObject;

    public void Initialize(UnlockableScriptableObject unlockableScriptableObject)
    {
        _unlockableScriptableObject = unlockableScriptableObject;
    }
}
