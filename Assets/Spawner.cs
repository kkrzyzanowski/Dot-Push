using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public abstract class Spawner : MonoBehaviour
{
    protected Ray ray;
    protected RaycastHit hit;
    protected Camera cam;

    protected virtual void Start()
    {
        cam = Camera.main;
    }

    protected virtual void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit) && hit.transform == transform)
            {
                OnClicked();
            }
        }
    }

    protected virtual void OnClicked()
    {
        ApplyEffect();
        gameObject.SetActive(false);
    }

    protected abstract void ApplyEffect();

    public virtual void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent<PlayerMove>(out var player))
        {
            player.DeactivateTouch();
            Debug.Log("Collider Enter");
        }
    }
}
