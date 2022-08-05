using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HoverTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image leftSelector;
    public Image rightSelector;
    public GameObject mainMenuPart;
    public GameObject levelSelectionPart;

    public void OnPointerEnter(PointerEventData eventData)
    {
        gameObject.GetComponent<Animator>().Play("Hover On");
        leftSelector.gameObject.SetActive(true);
        rightSelector.gameObject.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        gameObject.GetComponent<Animator>().Play("Hover Off");
        leftSelector.gameObject.SetActive(false);
        rightSelector.gameObject.SetActive(false);
    }






    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
    public void RetryStage()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
