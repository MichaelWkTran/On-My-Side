using UnityEngine;

public class Observer : MonoBehaviour
{
    public delegate void ObserverDelegate();
    public ObserverDelegate m_onNotify;
}
