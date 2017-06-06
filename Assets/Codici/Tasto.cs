using System.Collections;
using UnityEngine;

public class Tasto : MonoBehaviour {

    public Renderer rend;
    public Color originale, blu, verde, giallo, rosso;

    private void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray raggio = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit colpo;

            if (Physics.Raycast(raggio, out colpo, Mathf.Infinity))
                StartCoroutine(Touched(colpo.collider));
        }
    }

    private IEnumerator Touched(Collider collider)
    {
        collider.transform.localScale = new Vector3(200, 1, 200);
        yield return new WaitForSeconds(0.25f);
        collider.transform.localScale = new Vector3(190, 1, 190);
    }
}
