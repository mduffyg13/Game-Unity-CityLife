using UnityEngine;
using System.Collections;

public class NPC_Interact : MonoBehaviour {
	
	public int current_x = 18;
	public int current_z = 15;
	public string name;
	public string start_dir = "South";
	Spritesheet_Animation ssAnimation;
	// Use this for initialization
	void Start () {
	ssAnimation = gameObject.GetComponent<Spritesheet_Animation>();
		ssAnimation.SetDirection(start_dir);
		ssAnimation.still();
	}
	
	// Update is called once per frame
	void Update () {
	if (Input.GetMouseButtonDown (0)) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			
			if (Physics.Raycast (ray, out hit, Mathf.Infinity)) {
				if(hit.collider.name == "NPC_Smith"){
					Debug.Log("SMITH SELECTED");
				}
			}
			
		}
	}
}
