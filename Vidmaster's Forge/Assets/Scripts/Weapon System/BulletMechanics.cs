using Kitbashery.Gameplay;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BulletMechanics : MonoBehaviour
{
    [Header("Projectile:")]
    public Rigidbody rigid;
    [Tooltip("Initial velocity of the rigidbody.")]
    public float velocity = 500f;

    [Space]
    [Tooltip("Should the projectile be disabled on impact? (useful for pooling).")]
    public bool disableOnImpact = true;
    [Tooltip("Should the GameObject be deactivated after lifeTime has elapsed.")]
    public bool useLifeTime = true;
    [Tooltip("How long this projectile will live before being deactivated (in seconds).")]
    public float lifeTime = 10f;
    [Min(0)]
    private float life = 0f;
    [Tooltip("Projectile Impact Effect")]
    public ParticleSystem ImpactEffect;

    public GameObject BulletModel;


    private void Awake()
    {
        if (rigid == null)
        {
            rigid = gameObject.GetComponent<Rigidbody>();
        }
    }

    private void OnEnable()
    {
        BulletModel.SetActive(true);
        rigid.velocity = Vector3.zero;
        rigid.AddForce(transform.forward * velocity, ForceMode.Impulse);
    }

    private void OnDisable()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        life = 0;
    }

    private void Update()
    {
        if (useLifeTime == true)
        {
            life += Time.deltaTime;
            if (life >= lifeTime)
            {
                gameObject.SetActive(false);
            }
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject newImpact = ObjectPools.Instance.GetPooledObject(ImpactEffect.gameObject.name);

        newImpact.transform.position = transform.position;
        if(newImpact?.GetComponent<ParticleSystem>()!=null)
        {
            newImpact.SetActive(true);
            newImpact.GetComponent<ParticleSystem>().Play();
            Debug.Log(collision.gameObject.name);
            gameObject.SetActive(false);

        }
               
    }


    
}
