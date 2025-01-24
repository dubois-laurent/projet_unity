using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBehaviour : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletSpeed;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        RaycastHit hit;
        Ray aimRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(aimRay, out hit))
        {
            Vector3 hitPoint = hit.point - bulletSpawn.position;
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
            bullet.GetComponent<Rigidbody>().AddForce(hitPoint.normalized * bulletSpeed);
			
        }
    }
}
