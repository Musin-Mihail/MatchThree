using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchThree : MonoBehaviour
{
    List<Transform> listSphere = new List<Transform>();
    List<Transform> tempListSphere = new List<Transform>();
    List<Transform> newList = new List<Transform>();
    [SerializeField] private GameObject prefab;
    [SerializeField] private List<Material> materials;
    List<int> layers = new List<int>();
    bool addSphere = false;
    void Start()
    {
        layers.Add(7);
        layers.Add(9);
        layers.Add(8);
        Application.targetFrameRate = 60;
        StartCoroutine(CreatePool());
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit _hit;
            Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(_ray, out _hit, Mathf.Infinity))
            {
                int layer = _hit.transform.gameObject.layer;
                SearchNearestSphere(_hit.transform, layer);
                foreach (var sphere in tempListSphere)
                {
                    sphere.gameObject.SetActive(false);
                }
                tempListSphere.Clear();
                StartCoroutine(OnSphere());
            }
        }
    }
    IEnumerator CreatePool()
    {
        yield return new WaitForSeconds(2);
        for (int i = 0; i < 10; i++)
        {
            for (int z = -5; z <= 5; z++)
            {
                for (int x = -5; x <= 5; x++)
                {
                    Vector3 vector = new Vector3(x, 5, z);
                    CreateSphere(vector);
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
        addSphere = true;
        // print("Start");
    }
    void CreateSphere(Vector3 vector)
    {
        GameObject sphere = Instantiate(prefab, vector, Quaternion.identity);
        int random = Random.Range(0, 3);
        sphere.GetComponent<MeshRenderer>().material = materials[random];
        sphere.gameObject.layer = layers[random];
        listSphere.Add(sphere.transform);
    }
    IEnumerator OnSphere()
    {
        if (addSphere == true && newList.Count == 0)
        {
            addSphere = false;
            foreach (var sphere in listSphere)
            {
                if (sphere.gameObject.activeSelf == false)
                {
                    newList.Add(sphere);
                }
            }
            if (newList.Count > 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    if (newList.Count == 0)
                    {
                        break;
                    }
                    for (int z = -5; z <= 5; z++)
                    {
                        if (newList.Count == 0)
                        {
                            break;
                        }
                        for (int x = -5; x <= 5; x++)
                        {
                            if (newList.Count > 0)
                            {
                                Vector3 vector = new Vector3(x, 5, z);
                                int random = Random.Range(0, 3);
                                newList[0].GetComponent<MeshRenderer>().material = materials[random];
                                newList[0].gameObject.layer = layers[random];
                                newList[0].position = vector;
                                newList[0].gameObject.SetActive(true);
                                newList.RemoveAt(0);
                            }
                            else
                            {
                                break;
                            }
                        }

                    }
                    yield return new WaitForSeconds(0.5f);
                }
            }
            addSphere = true;
        }
    }

    void SearchNearestSphere(Transform startPosition, int layer)
    {
        tempListSphere.Add(startPosition);
        foreach (var sphere in listSphere)
        {
            if (sphere.gameObject.layer == layer && sphere.gameObject.activeSelf == true)
            {
                if (Vector3.Distance(startPosition.position, sphere.position) < 1.01f)
                {
                    bool match = false;
                    foreach (var tempSphere in tempListSphere)
                    {
                        if (sphere == tempSphere)
                        {
                            match = true;
                        }
                    }
                    if (match == false)
                    {
                        SearchNearestSphere(sphere, layer);
                    }
                }
            }
        }

    }
    public void Exit()
    {
        Application.Quit();
    }

}