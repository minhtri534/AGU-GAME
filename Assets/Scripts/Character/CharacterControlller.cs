using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public Rigidbody rb;
    public RuntimeCharacterStats stats;

    protected void TakeDamageAnimation()
    {
        gameObject.GetComponent<Animator>().SetTrigger("TakeDamage");
    }
}