using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public static Weapon instance;

    public float fireRate = 0;
    public int Damage = 50;
    public LayerMask whatToHit;
    public Transform HitPrefab;
    float timeToFire = 0;
    float timeToSpawnEffect = 0;
    public float effectSpawnRate = 10;
    Transform firePoint;
    public Transform BulletTrailPrefab;

    public LineRenderer laserLineRenderer;
    public float laserWidth = 0.1f;
    public float laserMaxLength = 5f;
    public Color laserColor;// = Color.red;
    public Transform MuzzleFlashPrefab;

    public float camShakeAmt = 0.1f;
    public float camShakeLength = 0.1f;
    CameraShake camShake;
    //Handle camera shaking
    void Start()
    {
        Cursor.visible = false;
        laserLineRenderer.startWidth = 0.1f;
        laserLineRenderer.endWidth = 0.1f;
        Vector3[] initLaserPositions = new Vector3[2] { Vector3.zero, Vector3.zero };
        laserLineRenderer.SetPositions(initLaserPositions);
        //laserLineRenderer.startColor(c1);
        //laserLineRenderer.SetWidth(laserWidth, laserWidth);

        camShake = GameMaster.gm.GetComponent<CameraShake>();
        if (camShake == null)
        {
            Debug.LogError("No CameraShake script found on GM object");
        }
    }
    // Use this for initialization
    private void Awake()
    {
        laserLineRenderer.enabled = true;
        firePoint = transform.Find("FirePoint");
        
        if (firePoint == null)
        {
            Debug.LogError("No firePoint? What??");
        }
    }
	// Update is called once per frame

	void Update () {
        laserLineRenderer.startColor = laserColor;
        laserLineRenderer.endColor = laserColor;
        Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);
        RaycastHit2D aim = Physics2D.Raycast(firePointPosition, mousePosition - firePointPosition, 100, whatToHit);
        //laserLineRenderer.enabled = true;
        //Invoke("EnableLaser", 3f);
        
        laserLineRenderer.SetPosition(0, firePointPosition);
        laserLineRenderer.SetPosition(1, (mousePosition - firePointPosition) * 100);
        //laserLineRenderer.startColor = laserColor;
        //laserLineRenderer.endColor = laserColor;
        if (fireRate == 0)
        {
            if (Input.GetButtonDown("Fire1")) {
                Shoot();
            }
        }
        else
        {
            if (Input.GetButton ("Fire1") && Time.time > timeToFire)
            {
                timeToFire = Time.time + 1 / fireRate;
                Shoot();
            }
        }
        if (aim.collider != null)
        {
            laserLineRenderer.SetPosition(0, firePointPosition);
            laserLineRenderer.SetPosition(1, aim.point);
        }
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        YourBool = true;
        Invoke("SetBoolBack", YOUR_ELAPSED_TIME);
    }*/
    private void EnableLaser()
    {
        laserLineRenderer.enabled = true;
    }
    private void DisableLaser()
    {
        laserLineRenderer.enabled = false;
    }
    void Shoot()
    {
        Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);
        RaycastHit2D hit = Physics2D.Raycast(firePointPosition, mousePosition-firePointPosition, 100, whatToHit);
        //Invoke("DisableLaser", 3f);
        
        //laserLineRenderer.SetPosition(0, firePointPosition);
        //laserLineRenderer.SetPosition(1, (mousePosition - firePointPosition) * 100);
        //laserLineRenderer.startColor = c1;
        //laserLineRenderer.endColor = c1;
        //Effect();
        if (hit.collider == null)
        {
            //Debug.LogError("No collider? What??");
            //Debug.DrawLine(firePointPosition, (mousePosition - firePointPosition) * 100, Color.red);

        }
        //Debug.DrawLine(firePointPosition, (mousePosition-firePointPosition)*100, Color.red);
        //Debug.DrawLine(firePointPosition, hit.point, Color.red);
 
        if (Time.time >= timeToSpawnEffect)
        {
            Vector3 hitPos;
            Vector3 hitNormal;
            laserLineRenderer.enabled = false;
            Invoke("EnableLaser", .25f);
            if (hit.collider != null)
            {


                //Debug.LogError("We hit " + hit.collider.name + " and did " + Damage + " damage.");
                //Debug.DrawLine(firePointPosition, hit.point, Color.red);
                //Debug.DrawLine(firePointPosition, (mousePosition - firePointPosition) * 100, Color.red);
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.DamageEnemy(Damage);
                }
                //laserLineRenderer.SetPosition(0, firePointPosition);
                //laserLineRenderer.SetPosition(1, hit.point);
            }
            if (hit.collider == null)
            {
                hitPos = (mousePosition - firePointPosition) * 30;
                hitNormal = new Vector3(9999, 9999, 9999);
            }
            else
            {
                hitPos = hit.point;
                hitNormal = hit.normal;
            }
            Effect(hitPos, hitNormal);
            timeToSpawnEffect = Time.time + 1 / effectSpawnRate;
        }
    }
    void Effect(Vector3 hitPos, Vector3 hitNormal)
    {
        Transform trail = Instantiate(BulletTrailPrefab,firePoint.position,firePoint.rotation) as Transform;
        LineRenderer lr = trail.GetComponent<LineRenderer>();

        if (lr != null)
        {
            lr.SetPosition(0, firePoint.position);
            lr.SetPosition(1, hitPos);
        }
        Destroy(trail.gameObject, 0.02f);

        if(hitNormal !=  new Vector3(9999,9999,9999))
        {
            Transform hitParticle = Instantiate(HitPrefab, hitPos, Quaternion.FromToRotation(Vector3.right, hitNormal)) as Transform;
            Destroy(hitParticle.gameObject, 1f);
        }
        Transform clone = (Transform)Instantiate(MuzzleFlashPrefab, firePoint.position, firePoint.rotation); //Make a clone because we are wanting to CHANGE the prefab
        clone.parent = firePoint;
        float size = Random.Range(0.6f, 0.9f);
        clone.localScale = new Vector3(size, size, size);
        Destroy(clone.gameObject, 0.02f);

        //Shake the camera
        camShake.Shake(camShakeAmt, camShakeLength);
    }
}
