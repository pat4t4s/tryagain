using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Shooting Setup")]
    public Transform firePoint;      // I-assign ang FirePoint child object dito
    public float range = 100f;       // Gaano kalayo ang abot ng baril
    public float damage = 10f;       // Bawas sa kalaban

    [Header("Visual Effects (Optional)")]
    public ParticleSystem muzzleFlash; // Fire effect sa dulo ng baril
    public GameObject impactEffect;   // Spark/Hole effect kung saan tumama ang bala

    void Update()
    {
        // Kapag pinindot ang Left Mouse Button
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Mag-play ng Muzzle Flash effect kung may nakalagay
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

        RaycastHit hit;

        // Gumawa ng Raycast mula sa FirePoint papunta sa harap (forward)
        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, range))
        {
            Debug.Log("Napatamaan: " + hit.transform.name);

            // OPTIONAL: Dito pwedeng bawasan ang buhay ng Target/Enemy
            // EnemyTarget enemy = hit.transform.GetComponent<EnemyTarget>();
            // if (enemy != null) { enemy.TakeDamage(damage); }

            // OPTIONAL: Mag-spawn ng Impact Effect kung saan tumama ang bala
            if (impactEffect != null)
            {
                GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactGO, 2f); // Burahin ang effect pagkatapos ng 2 seconds
            }
        }
    }
}