using UnityEngine;
using UnityEngine.UI;

public class cutscene : MonoBehaviour
{
    public int cutscene_id; // 0 = first cutscene, 1 = second cutscene
    public GameObject cutscene_object;
    public inputcontroller input_controller;
    public Image cutscene_renderer;
    public Sprite firstpicture;
    public Sprite secondpicture;
    public Sprite thirdpicture;
    public Sprite fourthpicture;
    public Sprite fifthpicture;
    public Sprite sixthpicture;
    public Sprite seventhpicture;
    public Sprite eightpicture;
    public Sprite ninethpicture;
    public Sprite tenthpicture;
    public Sprite Firstpicture;
    public Sprite Secondpicture;
    public Sprite Thirdpicture;
    public Sprite Fourthpicture;
    public Sprite Fifthpicture;
    void Start()
    {
        input_controller.gamestate = 2; // Set the game state to cutscene
        cutscene_object.SetActive(true);
        cutscene_id = 0; // Set the cutscene ID to the first cutscene
        cutscene_renderer.sprite = firstpicture;
    }
    public void start_cutscene()
    {
        input_controller.gamestate = 2; // Set the game state to cutscene
        cutscene_object.SetActive(true);
        cutscene_id = 1; // Set the cutscene ID to the second cutscene
        cutscene_renderer.sprite = Firstpicture;
        Debug.Log("Cutscene started: ID = " + cutscene_id);
    }

    public void next_frame()
    {
        if (cutscene_id == 0)
        {
            AdvanceFirstCutscene();
        }
        else if (cutscene_id == 1)
        {
            AdvanceSecondCutscene();
        }
    }

    private void AdvanceFirstCutscene()
    {
        if (cutscene_renderer.sprite == firstpicture)
        {
            cutscene_renderer.sprite = secondpicture;
        }
        else if (cutscene_renderer.sprite == secondpicture)
        {
            cutscene_renderer.sprite = thirdpicture;
        }
        else if (cutscene_renderer.sprite == thirdpicture)
        {
            cutscene_renderer.sprite = fourthpicture;
        }
        else if (cutscene_renderer.sprite == fourthpicture)
        {
            cutscene_renderer.sprite = fifthpicture;
        }
        else if (cutscene_renderer.sprite == fifthpicture)
        {
            cutscene_renderer.sprite = sixthpicture;
        }
        else if (cutscene_renderer.sprite == sixthpicture)
        {
            cutscene_renderer.sprite = seventhpicture;
        }
        else if (cutscene_renderer.sprite == seventhpicture)
        {
            cutscene_renderer.sprite = eightpicture;
        }
        else if (cutscene_renderer.sprite == eightpicture)
        {
            cutscene_renderer.sprite = ninethpicture;
        }
        else if (cutscene_renderer.sprite == ninethpicture)
        {
            cutscene_renderer.sprite = tenthpicture;
        }
        else if (cutscene_renderer.sprite == tenthpicture)
        {
            cutscene_object.SetActive(false);
            input_controller.gamestate = 0; // Return to wonder state
        }
    }

    private void AdvanceSecondCutscene()
    {
        if (cutscene_renderer.sprite == Firstpicture)
        {
            cutscene_renderer.sprite = Secondpicture;
        }
        else if (cutscene_renderer.sprite == Secondpicture)
        {
            cutscene_renderer.sprite = Thirdpicture;
        }
        else if (cutscene_renderer.sprite == Thirdpicture)
        {
            cutscene_renderer.sprite = Fourthpicture;
        }
        else if (cutscene_renderer.sprite == Fourthpicture)
        {
            cutscene_renderer.sprite = Fifthpicture;
        }
        else if (cutscene_renderer.sprite == Fifthpicture)
        {
            cutscene_object.SetActive(false);
            input_controller.gamestate = 0; // Return to wonder state
            Application.Quit();
        }
    }
}
