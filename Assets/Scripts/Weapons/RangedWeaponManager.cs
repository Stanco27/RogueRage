using UnityEngine;

public class RangedWeaponManager : MonoBehaviour
{
    [Header("Weapon Slots")]
    public GameObject manualRangedWeaponObject;

    public Weapon[] autoRangedWeapons;

    private Weapon _manualRangedWeaponScript;

    void Start()
    {
        if (manualRangedWeaponObject != null)
        {
            _manualRangedWeaponScript = manualRangedWeaponObject.GetComponent<Weapon>();

            if (_manualRangedWeaponScript == null)
            {
                Debug.LogError(
                    "The assigned Manual Ranged Weapon GameObject is missing a Weapon script!",
                    manualRangedWeaponObject
                );
            }
        }
    }

    public void CheckAndFireManualWeapon()
    {
        if (_manualRangedWeaponScript != null)
        {
            _manualRangedWeaponScript.TryFire();
        }
    }

    public void TryManualFire()
    {
        if (_manualRangedWeaponScript != null)
        {
            _manualRangedWeaponScript.TryFire();
        }
    }
}
