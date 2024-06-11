using UnityEngine;

public class GameManager : MonoBehaviour
{
    uint m_totalEarnedMoney;
    uint m_money;
    public delegate void OnMoneyUpdateDelegate(uint _money);
    public OnMoneyUpdateDelegate m_onMoneyUpdate;
}
