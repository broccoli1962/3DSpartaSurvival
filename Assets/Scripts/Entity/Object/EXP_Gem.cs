using UnityEngine;

public class EXP_Gem : MonoBehaviour
{
    public int experienceValue = 10;
    public float moveSpeed = 15f;

    private Transform playerTarget;
    private bool isFollowing = false;
    void OnEnable()
    {
        isFollowing = false;
        playerTarget = null;
    }

    void Update()
    {
        if (isFollowing && playerTarget != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTarget.position, moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                int finalExperience = (int)(experienceValue * player.expGain);
                player.AddExperience(finalExperience);
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.ShowFloatingText("+" + finalExperience, transform.position);
                }
            }
            GameManager.Instance.ReturnToPool("EXP_Gem", gameObject);
        }
    }

    public void StartFollowing(Transform target)
    {
        playerTarget = target;
        isFollowing = true;
    }
}