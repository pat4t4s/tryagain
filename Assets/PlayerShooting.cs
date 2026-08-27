using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Shooting Settings")]
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 40f;

    [Header("Automatic / Fire Rate Settings")]
    public float fireRate = 0.15f;     // Oras sa pagitan ng bawat putok (seconds). Mas maliit = mas mabilis pumutok.
    private float nextTimeToFire = 0f; // Cooldown timer

    [Header("Mouse Aiming")]
    public Camera mainCamera;

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    void Update()
    {
        // Pinalitan ng GetButton (tinanggal ang "Down") + Fire Rate timer
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            // I-set ang susunod na oras kung kailan pwedeng pumutok ulit
            nextTimeToFire = Time.time + fireRate;

            Shoot();
        }
    }

    void Shoot()
    {
        if (firePoint == null || bulletPrefab == null)
        {
            Debug.LogError("Error: May hindi pa naka-assign na reference sa Inspector!");
            return;
        }

        Vector3 targetPoint = GetMouseWorldPosition();

        GameObject currentBullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        Rigidbody rb = currentBullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 targetDirection = (targetPoint - firePoint.position).normalized;
            rb.linearVelocity = targetDirection * bulletSpeed;
        }

        Destroy(currentBullet, 3f);
    }

    Vector3 GetMouseWorldPosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity))
        {
            return raycastHit.point;
        }
        else
        {
            return ray.GetPoint(100f);
        }
    }
}