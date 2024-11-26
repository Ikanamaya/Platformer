using UnityEngine;
using UnityEngine.SceneManagement;


public class TrapController : MonoBehaviour
{
    [SerializeField] float trapDamage;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<PlayerController>())
        {
            collision.collider.GetComponent<PlayerController>().TakeDamage(trapDamage);
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    
}
