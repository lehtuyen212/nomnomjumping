using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecking : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.gameObject.CompareTag(GameTag.PlatForm.ToString())) return;
        var platformLanded = col.gameObject.GetComponent<PlatForm>();
        if (!GameManager.Ins || !GameManager.Ins.player || !platformLanded) return;
        GameManager.Ins.player.PlatformLanded= platformLanded;
        GameManager.Ins.player.Jump();

        if(!GameManager.Ins.IsPlatformLanded(platformLanded.Id))
        {
            int randScore = Random.Range(3, 8);
                GameManager.Ins.AddScore(randScore);
                GameManager.Ins.PlatformLandedIds.Add(platformLanded.Id);
        }
    }
}
