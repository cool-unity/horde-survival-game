using UnityEngine;

public class ShieldComponent : MonoBehaviour
{
    [SerializeField] private int maxShield = 50; // Maximum shield value
    private int currentShield;

    [Header("Shield Recharge Settings")]
    [SerializeField] private bool enableRecharge = true; // Toggle for automatic recharge
    [SerializeField] private float shieldRechargeDelay = 5f; // Time before shield starts recharging
    [SerializeField] private float shieldRechargeRate = 10f; // Shield recharge amount per second

    private float lastDamageTime; // Tracks the last time damage was taken

    public delegate void OnShieldChanged(int current, int max);
    public event OnShieldChanged ShieldChanged;

    private void Start()
    {
        currentShield = maxShield;
        ShieldChanged?.Invoke(currentShield, maxShield); // Notify listeners of initial shield value
    }

    private void Update()
    {
        if (enableRecharge)
        {
            RechargeShield();
        }
    }

    public int AbsorbDamage(int damage)
    {
        lastDamageTime = Time.time;

        int damageAbsorbed = Mathf.Min(currentShield, damage);
        currentShield -= damageAbsorbed;
        ShieldChanged?.Invoke(currentShield, maxShield);

        Debug.Log($"{gameObject.name} shield absorbed {damageAbsorbed} damage. Current shield: {currentShield}");

        return damage - damageAbsorbed; // Return remaining damage
    }

    private void RechargeShield()
    {
        // Only recharge if enough time has passed since last damage
        if (Time.time - lastDamageTime >= shieldRechargeDelay && currentShield < maxShield)
        {
            currentShield += Mathf.RoundToInt(shieldRechargeRate * Time.deltaTime);
            currentShield = Mathf.Clamp(currentShield, 0, maxShield);
            ShieldChanged?.Invoke(currentShield, maxShield);

            Debug.Log($"{gameObject.name} shield recharging. Current shield: {currentShield}");
        }
    }

    public void SetRechargeEnabled(bool enable)
    {
        enableRecharge = enable;
        Debug.Log($"Shield recharge for {gameObject.name} set to {(enableRecharge ? "enabled" : "disabled")}.");
    }

    public void RechargeFull()
    {
        currentShield = maxShield;
        ShieldChanged?.Invoke(currentShield, maxShield);
        Debug.Log($"{gameObject.name} shield fully recharged.");
    }
}
