using UnityEngine;
using UnityEngine.Events;

public abstract class BasePopup : MonoBehaviour
{
    public abstract void Open(UnityAction done);
    public abstract void Close(UnityAction done);
}

public class Popup : MonoBehaviour, IPlug
{
    public StagePopup stagePopup;
    public PausePopup pausePopup;
    public CompletePopup completePopup;
    public LosePopup losePopup;

}
