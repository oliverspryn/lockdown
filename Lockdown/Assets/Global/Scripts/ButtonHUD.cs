using UnityEngine;

/// <summary>
/// This class is used to control the HUD which will appear
/// when the user should press a specific PS3 controller
/// button, and any directions associated with it.
/// </summary>
public class ButtonHUD : MonoBehaviour {
	#region Fields

/// <summary>
/// Whether or not to display this UI element on the HUD.
/// </summary>
	public bool Active {
		get { return Camera.enabled; }
		set { Camera.enabled = value; }
	}

/// <summary>
/// The PS3 button to display as part of the HUD.
/// </summary>
	public Buttons Button {
		set {
			switch(value) {
				case Buttons.Circle:
					ObjectCircle.SetActive(true);
					ObjectSquare.SetActive(false);
					ObjectTriangle.SetActive(false);
					ObjectX.SetActive(false);
					break;

				case Buttons.Square:
					ObjectCircle.SetActive(false);
					ObjectSquare.SetActive(true);
					ObjectTriangle.SetActive(false);
					ObjectX.SetActive(false);
					break;

				case Buttons.Triangle:
					ObjectCircle.SetActive(false);
					ObjectSquare.SetActive(false);
					ObjectTriangle.SetActive(true);
					ObjectX.SetActive(false);
					break;

				case Buttons.X:
					ObjectCircle.SetActive(false);
					ObjectSquare.SetActive(false);
					ObjectTriangle.SetActive(false);
					ObjectX.SetActive(true);
					break;
			}
		}
	}

/// <summary>
/// The camera which is used to display this part of the HUD.
/// </summary>
	public Camera Camera;

/// <summary>
/// The text which will display as the directions in the HUD.
/// </summary>
	public GameObject DirectionsText;

/// <summary>
/// The object which contains the graphic for the circle button.
/// </summary>
	public GameObject ObjectCircle;

/// <summary>
/// The object which contains the graphic for the square button.
/// </summary>
	public GameObject ObjectSquare;

/// <summary>
/// The object which contains the graphic for the triangle button.
/// </summary>
	public GameObject ObjectTriangle;

/// <summary>
/// The object which contains the graphic for the X button.
/// </summary>
	public GameObject ObjectX;

/// <summary>
/// The text to display as part of the directions.
/// </summary>
	public string Text {
		get { return DirectionsText.GetComponent<TextMesh>().text; }
		set { DirectionsText.GetComponent<TextMesh>().text = value; }
	}

	#endregion
}