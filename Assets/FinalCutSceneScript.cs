using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class FinalCutSceneScript : MonoBehaviour
{
    [SerializeField] List<GameObject> FriendGroup;
    [SerializeField] private Transform Portal;
    CinemachineCamera maincam;
    
    [SerializeField] List<GameObject> ThingsToHide;
    [SerializeField] List<GameObject> ThingsToShow;
    
    public void StartScene()
    {
        GameManagerScript.instance.isFinalSceneRuning = true;
        maincam = FindFirstObjectByType<CinemachineCamera>();
        foreach (var thing in ThingsToHide) thing.SetActive(false);
        foreach (var thing in ThingsToShow) thing.SetActive(true);
        StartCoroutine(FinalScene());
    }

    IEnumerator FinalScene()
    {
        int i = 0;
        foreach (var friend in FriendGroup)
        {
            maincam.Lens.OrthographicSize = 5f;
            maincam.Follow = friend.transform;
            friend.GetComponent<SpriteRenderer>().flipX = false;
            if (i == 6) Portal.position = new Vector3(Portal.position.x, Portal.position.y + 0.5f);
            LeanTween.move(friend, Portal.position, 1.5f);
            yield return new WaitForSeconds(1.25f);
            LeanTween.alpha(friend, 0f, 0.25f);
            yield return new WaitForSeconds(0.25f);
            i++;
            //SoundFXManagerScript.instance.Play3DSFXSound();
        }
        ThingsToShow[0].SetActive(false);
        GameManagerScript.instance.isFinalSceneRuning = false;
        GameManagerScript.instance.GoToNextAct();
    }
}
