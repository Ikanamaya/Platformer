using System.Collections;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    [SerializeField] private GameObject objToSpawn;
    [SerializeField] private float timeBetweenSpawn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Spawn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private IEnumerator Spawn()
    {
        yield return new WaitForSeconds(timeBetweenSpawn);
        GameObject temp = Instantiate(objToSpawn, transform.position - new Vector3(1, 0, 0), Quaternion.identity);
        Rigidbody2D rb = temp.GetComponent<Rigidbody2D>();
        rb.AddForce(transform.up * 15, ForceMode2D.Impulse);
        Destroy(temp, 4);
        StartCoroutine(Spawn());
    }
}
