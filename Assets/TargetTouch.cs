using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TargetTouch : MonoBehaviour
{

    Ray ray;
    RaycastHit hit;
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        ray = cam.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction, Color.red);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.transform == transform)
            {
                PlayerMove.playerMoveInstance.ActiveTouch(transform.position);
                Destroy(this.gameObject);
                ConfigurationGame.ConfigurationGameInstance.AddPoints();
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.GetComponent<PlayerMove>() != null)
        {
            var o = other.gameObject.GetComponent<PlayerMove>();
            o.DeactivateTouch();
            Debug.Log("Collider Enter");
        }
    }

}
