using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public abstract class Spawner : MonoBehaviour
{
    protected Ray ray;
    protected RaycastHit hit;
    protected Camera cam;
    public float lifeTime = 5f;
    protected virtual void Start()
    {
        cam = Camera.main;
        StartCoroutine(WaitForDisable());
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
        if(!gameObject.activeSelf)
        {

        }
    }

    protected virtual void OnClicked()
    {
        ApplyEffect();
        UIManager.UIManagerInstance.UpdateScore();
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

    IEnumerator WaitForDisable()
    {
       
        yield return new WaitForSeconds(lifeTime);
        gameObject.SetActive(false);
    }
}
