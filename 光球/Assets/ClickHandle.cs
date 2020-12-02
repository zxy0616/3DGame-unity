using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickHandle : MonoBehaviour
{
    public GameObject paricle;

    private int speed = 0;

    // Start is called before the first frame update
    void Start()
    {
        speed = 0;
        paricle = ParticleSystem.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/guangqiu"));
    }

    // Update is called once per frame
    void Update()
    {
        if (paricle != null && paricle.transform.position.y > -3)
        {
            paricle.transform.Translate(new Vector3(0, -2, 0) * speed * Time.deltaTime);
        }
        else if (paricle != null && paricle.GetComponent<ParticleSea>() == null)
        {
            Destroy(paricle);
            this.gameObject.AddComponent<ParticleSea>();
        }
            

        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits;
            hits = Physics.RaycastAll(ray);

            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit hit = hits[i];


                if (hit.collider.gameObject != null)       //射线打中某物体
                {
                    speed = 2;
                }
            }
        }   
    }
}
