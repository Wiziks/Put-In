using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class DisclaimerDownloadPlayScene : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        //SceneManager.LoadScene(1);
        foreach (GameObject gameObject in DisclaimerScript.Instance.StartGameObjects)
        {
            gameObject.SetActive(true);
        }
        gameObject.SetActive(false);
    }
}
