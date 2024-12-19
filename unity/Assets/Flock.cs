using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockManager myManager;
    public float speed;
    bool tourner = false;
    public AudioSource droneSon;

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(myManager.vitesseMinimale, myManager.vitesseMaximale);
        droneSon.PlayDelayed(2f);

    }

    // Update is called once per frame
    void Update()
    {
        Bounds bounds = new(myManager.leaderDrone.transform.position, myManager.flyLimit * 2);
        if (!bounds.Contains(transform.position))
        {
            tourner = true;
        }
        else
        {
            tourner = false;
        }

        if (tourner)
        {
            Vector3 direction = myManager.leaderDrone.transform.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), myManager.vitesseDeRotation * Time.deltaTime);
        }
        else
        {
            if (Random.Range(0, 100) < 10)
                speed = Random.Range(myManager.vitesseMinimale, myManager.vitesseMaximale);
            if (Random.Range(0, 100) < 20)
                ApplyRules();
        }

        transform.LookAt(myManager.leaderDrone.transform);
        FollowLeader();

    }

    void ApplyRules() {
        GameObject[] gos;
        gos = myManager.tousLesDrones;
        Vector3 vcentre = Vector3.zero; 
        Vector3 vavoid = Vector3.zero;
        float gSpeed = 0.01f;
        float nDistance;
        int groupSize = 0;

        foreach (GameObject go in gos)
        {
            if (go != this.gameObject)
            {
                nDistance = Vector3.Distance(go.transform.position, transform.position);
                if (nDistance <= myManager.voisinageDistance)
                {
                    vcentre += go.transform.position;
                    groupSize++;
                    if (nDistance < 1.0f)
                    {
                        vavoid += this.transform.position - go.transform.position;
                    }
                    Flock anotherFlock = go.GetComponent<Flock>();
                    gSpeed += anotherFlock.speed;
                }
            }
        }

        if (groupSize > 0)
        {
            vcentre = vcentre / groupSize + (myManager.leaderDrone.transform.position - transform.position);
            Vector3 direction = (vcentre + vavoid) - transform.position;
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), myManager.vitesseDeRotation * Time.deltaTime);
            }
        }
    }

    void FollowLeader()
    {
        Vector3 r = myManager.leaderDrone.transform.position - transform.position;
        if (Vector3.Dot(transform.forward, r) > 0.0f)
        {
            transform.Translate(speed * Time.deltaTime * myManager.leaderDrone.transform.forward * 1.5f);
        }
        else
        {
            transform.Translate(speed * Time.deltaTime * -myManager.leaderDrone.transform.forward);
        }
    }
}
