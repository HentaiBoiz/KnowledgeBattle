using UnityEngine;

public class TriggerOutline : MonoBehaviour
{
    public Outline outline;

    private void Start()
    {
        outline.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            outline.enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            outline.enabled = false;
    }
}
