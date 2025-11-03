using System.Collections;
using UnityEngine;

public class Pistol : Weapon
{
    [Header("Pistol References")]
    public float maxRange = 100f;
    private LineRenderer lineRenderer;
    private Coroutine laserDisplayCoroutine;

    void Awake()
    {
        GameObject cam = GameObject.FindGameObjectWithTag("PlayerCamera");
        if (cam != null)
        {
            playerCamera = cam.transform;
        }

        if (stats == null)
        {
            stats = GetComponent<WeaponStats>();
        }
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            Debug.LogError("LineRenderer component missing on Pistol!", this);
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    protected override void Fire()
    {
        if (playerCamera == null || stats == null)
        {
            Debug.LogError("Pistol firing failed: Camera or WeaponStats missing.", this);
            return;
        }

        Camera camComponent = playerCamera.GetComponent<Camera>();

        if (camComponent == null)
        {
            Debug.LogError(
                "Pistol firing failed: Player Camera Transform is missing the Camera component.",
                this
            );
            return;
        }

        Ray ray = camComponent.ScreenPointToRay(
            new Vector3(Screen.width / 2f, Screen.height / 2f, 0f)
        );
        RaycastHit hit;
        Vector3 hitPoint = firePoint.position + ray.direction * maxRange;

        int enemyLayer = LayerMask.NameToLayer("Enemy");

        if (Physics.Raycast(ray, out hit, maxRange))
        {
            hitPoint = hit.point;
            if (hit.collider.gameObject.layer == enemyLayer)
            {
                if (hit.collider.TryGetComponent<EnemyHealth>(out EnemyHealth enemyHealth))
                {
                    float currentDamage = stats.currentDamage;
                    enemyHealth.TakeDamage(currentDamage, hit.point);
                }
            }
            else
            {
                Debug.Log(
                    $"Hit non-enemy layer: {LayerMask.LayerToName(hit.collider.gameObject.layer)}"
                );
            }
        }
        if (lineRenderer != null)
        {
            if (laserDisplayCoroutine != null)
            {
                StopCoroutine(laserDisplayCoroutine);
            }

            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, hitPoint);

            laserDisplayCoroutine = StartCoroutine(ShowLaser());
        }
    }

    private IEnumerator ShowLaser()
    {
        if (lineRenderer != null)
        {
            lineRenderer.enabled = true;
            yield return new WaitForSeconds(0.05f);
            lineRenderer.enabled = false;
            laserDisplayCoroutine = null;
        }
    }
}
