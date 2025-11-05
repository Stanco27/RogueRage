using System.Collections;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public WeaponStats stats;
    public Transform firePoint;

    protected float nextFireTimeUnscaled;
    protected Transform playerCamera;

    private Coroutine reloadCoroutine;

    protected abstract void Fire();

    public virtual void TryFire()
    {
        if (stats == null)
            return;

        if (Time.time < stats.nextReloadTime)
        {
            return;
        }

        if (stats.currentAmmo <= 0)
        {
            StartReload();
            return;
        }

        if (Time.unscaledTime >= nextFireTimeUnscaled)
        {
            nextFireTimeUnscaled = Time.unscaledTime + (1f / stats.currentFireRate);

            Fire();
            stats.currentAmmo--;

            if (stats.currentAmmo <= 0)
            {
                if (!IsReloading)
                {
                    StartReload();
                }
            }
        }
    }

    public virtual void StartReload()
    {
        if (stats == null)
            return;

        if (reloadCoroutine != null)
        {
            Debug.Log("Reload already in progress. Ignoring input.");
            return;
        }

        if (IsReloading)
        {
            return;
        }

        if (stats.currentAmmo < stats.currentMagazineSize)
        {
            stats.nextReloadTime = Time.time + stats.currentReloadTime;

            reloadCoroutine = StartCoroutine(ReloadRoutine());

            Debug.Log($"Reloading... Ready in {stats.currentReloadTime} seconds.");
        }
    }

    protected virtual IEnumerator ReloadRoutine()
    {
        yield return new WaitForSeconds(stats.currentReloadTime);

        stats.currentAmmo = stats.currentMagazineSize;

        reloadCoroutine = null;
        Debug.Log("Reload complete! Ammo reset to " + stats.currentAmmo);
    }

    public bool IsReloading => Time.time < stats.nextReloadTime;
}
