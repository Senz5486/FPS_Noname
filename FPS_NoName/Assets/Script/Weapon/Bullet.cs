using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int BulletDamage;
    public int Bullet_Damage { set{ BulletDamage = value; } }

    private GameObject owner;

    public GameObject Owner { get { return owner; } set { owner = value; } }

    private Vector3 BeforePosition;

    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private GameObject HitParticle;
    [SerializeField] private LayerMask Layer;

    private void Start()
    {
        BeforePosition = transform.position;
    }

    private void Update()
    {
        HitCheck();
        BeforePosition = transform.position;
    }

    //何かに当たった時の判定
    //HitParticleのためにRayに変更 <- Colliderが要らないかも
    private void HitCheck()
    {
        RaycastHit Hit;

        bool rayCheck = Physics.Raycast(BeforePosition, transform.forward, out Hit ,Vector3.Distance(BeforePosition,transform.position) + rigidbody.velocity.magnitude * Time.deltaTime, Layer);
        Debug.DrawRay(BeforePosition, transform.forward * (Vector3.Distance(BeforePosition, transform.position) + rigidbody.velocity.magnitude * Time.deltaTime), Color.green,1);

        if (!rayCheck) return;
        if (Hit.collider.gameObject.tag != "Enemy")
        {
            Debug.Log(Hit.collider.gameObject.name);
        }
        else
        {
            Debug.Log("敵に命中");
            Hit.collider.gameObject.GetComponent<IDamageable>().TakeDamage(BulletDamage);
            GameObject obj = Instantiate(HitParticle, Hit.point, Quaternion.identity);
            obj.transform.LookAt(Camera.main.transform.position);
        }
        
        Destroy(this.gameObject);
    }
}
