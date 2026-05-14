using UnityEngine;

public class lookAtCamera : MonoBehaviour
{


public class FlatBillboard : MonoBehaviour
{
    void LateUpdate()
    {
        
        transform.rotation = Camera.main.transform.rotation;
    }

}
}
