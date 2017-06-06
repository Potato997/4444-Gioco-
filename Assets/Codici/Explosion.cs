using UnityEngine;

public class Explosion : MonoBehaviour {

  #region Variables

  #endregion
	void Start () {
        Invoke("Destroy", 0.5f);
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}