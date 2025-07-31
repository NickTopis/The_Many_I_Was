using UnityEngine;
using UnityEngine.Events;  // for UnityEvent

public class Ammo : MonoBehaviour
{
    [SerializeField] public AmmoSlot[] ammoSlots;

    [System.Serializable]
    public class AmmoSlot
    {
        public AmmoType ammoType;
        public int ammoAmount;
        public int maxAmmoAmount;  // configurable max capacity
    }

    public void IncreaseCurrentAmmo(AmmoType ammoType, int ammoAmount)
    {
        var slot = GetAmmoSlot(ammoType);
        if (slot != null)
        {
            int newAmount = slot.ammoAmount + ammoAmount;
            if (newAmount >= slot.maxAmmoAmount)
            {
                slot.ammoAmount = slot.maxAmmoAmount;
            }
            else
            {
                slot.ammoAmount = newAmount;
            }
        }
    }

    public int GetCurrentAmmo(AmmoType ammoType)
    {
        return GetAmmoSlot(ammoType)?.ammoAmount ?? 0;
    }

    public void ReduceCurrentAmmo(AmmoType ammoType)
    {
        var slot = GetAmmoSlot(ammoType);
        if (slot != null && slot.ammoAmount > 0)
        {
            slot.ammoAmount--;
        }
    }

    public AmmoSlot[] GetAllAmmoSlots()
    {
        return ammoSlots;
    }

    public void SetAmmoSlot(AmmoType ammoType, int amount)
    {
        var slot = GetAmmoSlot(ammoType);
        if (slot != null)
        {
            slot.ammoAmount = Mathf.Clamp(amount, 0, slot.maxAmmoAmount);
        }
    }

    private AmmoSlot GetAmmoSlot(AmmoType ammoType)
    {
        foreach (AmmoSlot slot in ammoSlots)
        {
            if (slot.ammoType == ammoType)
            {
                return slot;
            }
        }
        return null;
    }

    public bool IsAmmoFull(AmmoType ammoType)
    {
        var slot = GetAmmoSlot(ammoType);
        if (slot == null) return false;

        return slot.ammoAmount >= slot.maxAmmoAmount;
    }
}
