using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    public GameObject dronePrefab;
    public GameObject leaderDrone;
    public Leader leader;
    public int nombreDrones = 10;
    readonly float leaderVitesse = 5.0f;
    readonly float camVitesse = 2.0f;
    public GameObject[] tousLesDrones;
    public Vector3 flyLimit = new(5, 5, 5);
    public Vector3 goalPos;
    bool tourner = false;

    [Header("Parametres du drone")]
    [Range(0.0f, 5.0f)]
    public float vitesseMinimale;
    [Range(0.0f, 5.0f)]
    public float vitesseMaximale;
    [Range(0.0f, 10.0f)]
    public float voisinageDistance;
    [Range(0.0f, 10.0f)]
    public float vitesseDeRotation;

    // Start is called before the first frame update
    void Start()
    {
        tousLesDrones = new GameObject[nombreDrones];
        for (int i = 0; i < nombreDrones; i++)
        {
            Vector3 pos = this.transform.position + new Vector3(
                Random.Range(-flyLimit.x, flyLimit.x),
                Random.Range(-flyLimit.y, flyLimit.y),
                Random.Range(-flyLimit.z, flyLimit.z)
            );
            tousLesDrones[i] = Instantiate(dronePrefab, pos, Quaternion.identity);
            tousLesDrones[i].GetComponent<Flock>().myManager = this;
        }
        goalPos = transform.position;
        leaderDrone = leader.SetupLeader(goalPos);

    }

    // Update is called once per frame
    void Update()
    {
        Bounds bound = new(transform.position, flyLimit * 2);
        if (!bound.Contains(leaderDrone.transform.position))
        {
            tourner = true;
        }
        else
        {
            tourner = false;
        }


        if (tourner)
        {
            Vector3 direction = transform.position - leaderDrone.transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), vitesseDeRotation * Time.deltaTime);
            leaderDrone.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), vitesseDeRotation * Time.deltaTime);
        }
        else
        {
            if (Random.Range(0, 100) < 10)
            {
                goalPos = leaderDrone.transform.position * 0.5f + new Vector3(
                    Random.Range(-flyLimit.x * 3.0f, flyLimit.x * 3.0f),
                    Random.Range(-flyLimit.y * 0.2f, flyLimit.y),
                    Random.Range(-flyLimit.z * 3.0f, flyLimit.z * 3.0f)
                );
                transform.position = goalPos;
            }
        }
        leaderDrone.transform.position = Vector3.MoveTowards(leaderDrone.transform.position, goalPos, Time.deltaTime * leaderVitesse * 2.0f);
        leader.MoveCamera(Camera.main, tousLesDrones, leaderDrone, camVitesse);
    }
}
