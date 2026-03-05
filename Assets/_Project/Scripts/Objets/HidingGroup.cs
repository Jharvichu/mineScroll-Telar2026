using UnityEngine;
using Player;

public class HidingGroup : MonoBehaviour
{
    private int _sectionsOccupied = 0;

    public void playerEnteredSection(PlayerController player)
    {
        _sectionsOccupied++;
        UpdatePlayerStatus(player);
    }

    public void playerLeftSection(PlayerController player)
    {
        _sectionsOccupied--;
        // Solo si salió de TODAS las secciones, deja de estar oculto
        if (_sectionsOccupied <= 0)
        {
            _sectionsOccupied = 0;
            player.isHidden = false;
        }
    }

    private void UpdatePlayerStatus(PlayerController player)
    {
        if (player.isCrouching)
        {
            player.isHidden = true;
        }
    }
}
