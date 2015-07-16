using UnityEngine;
using System.Collections;

public class Object_Depth : MonoBehaviour {
	
	public float behind_player_vert;
	public float infront_player_vert;
	public float behind_player_hor;
	public float infront_player_hor;
	
	public bool H_Depth = false;
	public bool V_Depth = false;
	public float loffset  = 0.0f;
	public float roffset  = 0.0f;
	GameObject player;
	private Vector3 depth_position;
	SphereCollider s_col;
	BoxCollider b_col;
	
	Collider boxcollider;
	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player");
		s_col = gameObject.GetComponent<SphereCollider>();
		b_col = gameObject.GetComponent<BoxCollider>();
		s_col.isTrigger = true;
		Vector3 s_col_centre = new Vector3(3.0f, 0.0f, 6.7f);
		s_col.center = s_col_centre;
		s_col.radius = 1.03f;
	
		b_col.isTrigger = true;
		Vector3 b_col_centre = new Vector3(-2.68f, 0.0f, 4.00f);
		b_col.center = b_col_centre;
		Vector3 b_col_size = new Vector3(7.85f,0.49f,4.86f);
		b_col.size = b_col_size;
	
	}
	
	void OnTriggerStay(Collider col){
		Debug.Log("(ENTER) TRIGGERED BY: " + col.name.ToString());
		depth_position = new Vector3(transform.position.x, 0.2f , transform.position.z);
				transform.position = depth_position;
	}
	void OnTriggerExit(Collider col){
		Debug.Log("(EXIT)TRIGGERED BY: " + col.name.ToString());
		depth_position = new Vector3(transform.position.x, 0.4f , transform.position.z);
				transform.position = depth_position;
	}
	// Update is called once per frame
	void Update () {
		if(player == null){
			Debug.Log("PLAYER NOT FOUND");
			
			
		}
		
		
		
		//if(boxcollider.
		
		/*if(V_Depth){
			if(player.transform.position.z > transform.position.z && player.transform.position.x < loffset ||
				player.transform.position.z > transform.position.z && player.transform.position.x > roffset){
				
				depth_position = new Vector3(transform.position.x, 0.4f , transform.position.z);
				transform.position = depth_position;
			}
			else
			{
				depth_position = new Vector3(transform.position.x, 0.2f , transform.position.z);
				transform.position = depth_position;
			}
		}*/
		
		if(H_Depth){
			if(player.transform.position.x > transform.position.x){
				
				depth_position = new Vector3(transform.position.x, 0.4f , transform.position.z);
				transform.position = depth_position;
			}
			else
			{
				depth_position = new Vector3(transform.position.x, 0.2f , transform.position.z);
				transform.position = depth_position;
			}
		}
	}
}
