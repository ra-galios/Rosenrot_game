using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour {

	private int currentDiamonds;
	private Button levelButton;
	public Text diamondsInform;
	public int diamondsRequired;

	// Use this for initialization
	void OnEnable () {
		currentDiamonds = Market.Instance.Dimond;
		levelButton = GetComponent<Button>();
		if(currentDiamonds >= diamondsRequired)
		{
			levelButton.interactable = true;
			diamondsInform.text = "";
		}
		else
		{
			levelButton.interactable = false;
			diamondsInform.text = currentDiamonds.ToString() + "/" + diamondsRequired.ToString();
		}
	}
}
