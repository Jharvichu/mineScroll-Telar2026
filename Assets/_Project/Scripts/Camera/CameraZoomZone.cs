using UnityEngine;
using Unity.Cinemachine;

public class CameraZoomZone : MonoBehaviour
{
    public enum CameraType { Ninguna, CamaraCerca, CamaraMedia, CamaraLejana }

    [Header("Camaras")]
    [SerializeField] private GameObject camCerca;
    [SerializeField] private GameObject camMedia;
    [SerializeField] private GameObject camLejana;

    [Header("Configuracion de esta Zona")]
    [SerializeField] private CameraType cameraSeleccionada;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Character"))
        {
            SwitchPriority(cameraSeleccionada);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

    }

    private void SwitchPriority(CameraType camera)
    {
        if (camCerca != null) camCerca.SetActive(false);
        if (camMedia != null) camMedia.SetActive(false);
        if (camLejana != null) camLejana.SetActive(false);
        
        switch (camera)
        {
            case CameraType.CamaraCerca:
                if (camCerca != null) camCerca.SetActive(true);
                break;
            case CameraType.CamaraMedia:
                if (camMedia != null) camMedia.SetActive(true);
                break;
            case CameraType.CamaraLejana:
                if (camLejana != null) camLejana.SetActive(true);
                break;
        }
    }
}
