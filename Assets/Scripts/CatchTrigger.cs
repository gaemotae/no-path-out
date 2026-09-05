using UnityEngine;

public class CatchTrigger : MonoBehaviour
{
    public WatcherFSM watcher;

    void Start()
    {
        if (watcher == null)
            watcher = GetComponentInParent<WatcherFSM>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && watcher != null)
        {
            watcher.ForceGameOver();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && watcher != null)
        {
            watcher.ForceGameOver(); 
        }
    }
}