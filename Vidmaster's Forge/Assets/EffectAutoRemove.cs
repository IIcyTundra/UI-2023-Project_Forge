using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectAutoRemove : MonoBehaviour
{
    [Tooltip("How long this effect live before being deactivated (in seconds).")]
    public float lifeTime = 1f;
    [Min(0)]
    private float life = 0f;


    private void OnDisable()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        life = 0;
    }

    private void Update()
    {
        life += Time.deltaTime;
        if (life >= lifeTime)
        {
            gameObject.SetActive(false);
        }

    }
}
