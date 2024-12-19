using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leader : MonoBehaviour
{
    public GameObject leaderPrefab;
    private GameObject leaderDrone;

    public GameObject SetupLeader(Vector3 goalPos)
    {
        leaderDrone = Instantiate(leaderPrefab, goalPos, Quaternion.identity);
        return leaderDrone;

    }

    public void MoveCamera(Camera camera, GameObject[] allDrones, GameObject leader, float vitesse)
    {
        Vector3 allPos = Vector3.zero;
        foreach (GameObject gameObject in allDrones)
        {
            allPos += gameObject.transform.position;
        }
        allPos += leader.transform.position;
        Vector3 center = allPos / allDrones.Length;

        camera.transform.LookAt(center);
        // Vector3 distance = leader.transform.position - camera.transform.position;
        if (IsCameraAheadOfLeader(cam: camera, leader: leader.transform.position))
        {
            camera.transform.position = Vector3.MoveTowards(camera.transform.position, center, Time.deltaTime * vitesse);
        }
        else
        {
            camera.transform.position = Vector3.MoveTowards(camera.transform.position, -center, Time.deltaTime * vitesse);
        }
    }

    bool IsCameraAheadOfLeader(Vector3 leader, Camera cam)
    {
        Vector3 r = leader - cam.transform.position;
        if (Vector3.Dot(cam.transform.position, r) > 0.0f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
