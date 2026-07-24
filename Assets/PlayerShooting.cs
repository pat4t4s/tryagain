using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Shooting Settings")]
    public Transform firePoint;        // I-assign ang FirePoint child object dito
    public GameObject bulletPrefab;    // Dito i-drag ang BulletSphere Prefab
    public float bulletSpeed = 40f;    // Bilis ng paglipad ng sphere

    [Header("Mouse Aiming")]
    public Camera mainCamera;          // I-assign ang Main Camera
    public LayerMask aimLayerMask;     // Pang-filter ng raycast (optional pero advisable)

    void Start()
    {
        // Kung hindi na-assign sa Inspector, kusa nitong kukunin ang Main Camera
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
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

        // 1. Kunan ng target position mula sa cursor
        Vector3 targetPoint = GetMouseWorldPosition();

        // 2. Mag-spawn ng bullet sa posisyon ng firePoint
        GameObject currentBullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        // 3. Kunan ng Rigidbody para patalsikin
        Rigidbody rb = currentBullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Calculate ang direksyon mula sa FirePoint papunta sa target point ng Mouse
            Vector3 targetDirection = (targetPoint - firePoint.position).normalized;

            // Ilapat ang bilis sa direksyong iyon
            rb.linearVelocity = targetDirection * bulletSpeed;
        }

        // 4. I-destroy ang bullet pagkalipas ng 3 seconds
        Destroy(currentBullet, 3f);
    }

    // Function para makuha ang 3D Position sa mundo gamit ang Mouse Cursor
    Vector3 GetMouseWorldPosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        // Kapag may natamaan ang ray galing sa mouse (lupa, pader, target objects, etc.)
        if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity))
        {
            return raycastHit.point; // Ibabalik ang 3D coordinate ng natamaan ng cursor
        }
        else
        {
            // Kapag nakaturo sa kalawakan/skybox, magse-set lang ng point sa malayo sa harap ng camera
            return ray.GetPoint(100f);
        }
    }
}