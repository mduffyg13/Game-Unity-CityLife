using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player_Movement : MonoBehaviour
{
	public LayerMask convo_layer;
	
	Spritesheet_Animation ssAnimation;
	bool bConvo = false;
	//GameObject ssAnimation;
	GameObject conversation_component;
	public Vector3 move_to;
	private float moveSpeed = 3f;
	private float gridSize = 1f;
	public Vector2 input;
	public bool isMoving = false;
	bool isConvo = false;
	public float char_offset = 0.0f;
	//public float character_offset = 0.0f;
	GameObject ground;
	Grid levelGrid;
	public int xMoves;
	public int zMoves;
	Tile current_tile;
	Tile destination_tile;
	Renderer[] c_renderer;
	GameObject selected_npc;
	Conversation convo;
	public string npc_name;
	//GameObject proto_dialog;
	//bool show_proto;
	//public bool npc_convo;
	
	void Start ()
	{
		ground = GameObject.FindGameObjectWithTag ("Ground"); //Find plane representing the ground
		levelGrid = (Grid)ground.GetComponent (typeof(Grid)); //Get the levelGrid
		current_tile = levelGrid.iso_array [19, 19];
		Vector3 startpos = new Vector3(current_tile.centre.x,current_tile.centre.y,current_tile.centre.z + char_offset);
		//npc_convo = true;
		//ground = GameObject.FindGameObjectWithTag ("Ground"); //Find plane representing the ground
		//levelGrid = (Grid)ground.GetComponent (typeof(Grid)); //Get the levelGrid
		transform.position = startpos;//levelGrid.iso_array [19, 19].centre; //Set starting position
		//current_tile = levelGrid.iso_array [19, 19]; //Get the current tile
		conversation_component = (GameObject)GameObject.Find ("Conversation_Component");
		c_renderer = conversation_component.GetComponentsInChildren<Renderer> ();
		convo = conversation_component.GetComponent<Conversation>();
		conversation_component.active = false;
		foreach (Renderer r in c_renderer) {		
			r.enabled = false;
		}	
		
	
		
		ssAnimation = gameObject.GetComponent<Spritesheet_Animation>();
		ssAnimation.still();
	}

	void Update ()
	{
		
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;

		if (Input.GetMouseButtonDown (0)) {
			
			if (Physics.Raycast (ray, out hit, Mathf.Infinity)){
				if(hit.collider.tag == "Reset"){
					Application.LoadLevel(0);
				}
			}
			
			if(!isMoving){
				//	Debug.Log("=========================================NEW MOVE!!!================================");
				//Debug.Log ("Current Tile: " + "[" + current_tile.x_pos + "," + current_tile.z_pos +"] " + "C " + current_tile.centre.ToString());
				//Cast ray from camera to mouse pointer
				//Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				//RaycastHit hit;
				//	selected_npc = null;
				bConvo = false;
				
			
				if(!convo.isConvo){
				
					//If ray hits plane object
					if (Physics.Raycast (ray, out hit, Mathf.Infinity, convo_layer)) {
					
						
					/*if(hit.collider.tag == "NPC_CONVO"){	
						Debug.Log("Conversation Hit");
					}*/
					
					//If click on npc character with speach
						if (hit.collider.tag == "NPC_CONVO") {
							bConvo = true;
							//	Debug.Log ("CONVO INIT " + hit.collider.name);
							//Debug.Log ("HIT: " + hit.collider.name.ToString ());
						
							selected_npc = hit.collider.gameObject;
							//GameObject..Find (hit.collider.name.ToString ());
						
						
						
							if (selected_npc != null) {
								//Debug.Log ("NPC FOUND");
							
							
								NPC_Interact npc = selected_npc.GetComponent<NPC_Interact>();
								move_to = levelGrid.getTile(npc.current_x-1, npc.current_z).centre;
								npc_name = npc.name;
								Debug.Log("NPC NAME: " + npc.name);
								//Debug.Log("MOVE TO: " + move_to.x.ToString() + ", " + move_to.z.ToString());
						
								move(move_to);
						
							
							}
						
	
						} else {
					
					
							move_to = new Vector3 (hit.point.x, 0, hit.point.z);
							move(move_to);
						
						}
			
					}
			
				}
			}
		}
	}
	public void switchConvo(){
			if (conversation_component.active) {
				conversation_component.active = false;
				
				foreach (Renderer r in c_renderer) {		
					r.enabled = false;
				}
			} else {
				conversation_component.active = true;
				
				foreach (Renderer r in c_renderer) {		
					r.enabled = true;
				}
				
				convo.initConversation(npc_name);
			}
		
	}
	public void move (Vector3 move_to)
	{
			//Create temp tile
					Tile tempTile = new Tile ();
					
					//Check each tile in array
					foreach (Tile aTile in levelGrid.iso_array) {
							
						//Calculate distances
						float distance = Vector3.Distance (move_to, aTile.centre);
						float tempDistance = Vector3.Distance (tempTile.centre, move_to);
					
						//if movex closer to aTile centre than tempTile AND movez closer to aTile centre than tempTile
						//then save aTile as tempTile
						if (distance < tempDistance) {

							tempTile = aTile;			
						}		
					}
					
					//Check closest tile
					//Debug.Log ("Closest Tile: " + "[" + tempTile.x_pos + "," + tempTile.z_pos + "]");
					destination_tile = tempTile;
				
					//Calculate distance to target
					xMoves = destination_tile.x_pos - current_tile.x_pos;
					zMoves = destination_tile.z_pos - current_tile.z_pos;	
			
				if (current_tile != destination_tile) {
				
		
					StartCoroutine (move (transform, astarPath (destination_tile)));
				
				}	
				current_tile = levelGrid.getTile (current_tile.x_pos + xMoves, current_tile.z_pos + zMoves);
		
	}
	
	public IEnumerator move (Transform transform, List<Vector3> path)
	{
		Vector3 startPosition;
		Vector3 endPosition;
		//Start moving
		isMoving = true;							
		startPosition = current_tile.centre;			//Current tile position
		float t = 0;
		//ssAnimation.StartSouth();
		//for each vector3 in list move to centre
		//int m = 0;#
		bool change_in_movement = false;
		string last_move = "";
		foreach (Vector3 v in path) {
			
			//Debug.Log("MOVE LOOP_> " + v.ToString() );
			t = 0;
			startPosition = transform.position;
			Vector3 endpos = new Vector3(v.x, v.y , v.z+ char_offset);
			endPosition = endpos;  
			
			/*Debug.Log("MOVE_ " + m.ToString());
			Debug.Log("Start - x: " + startPosition.x.ToString() + " z: " + startPosition.z.ToString());
			Debug.Log("End - x: " + endPosition.x.ToString() + " z: " + endPosition.z.ToString());
			m++;
			
			Debug.Log("CALC 1: " + (endPosition.x - startPosition.x).ToString());
			Debug.Log("CALC 2: " + (endPosition.z - startPosition.z).ToString());
			*/
			//if 1, -1, 0.5, -0.5
			//north = "10.5"
			//east =  "1-0.5"
			//south = "-1-0.5"
			//west = "-10.5"
			
			string movement = (endPosition.x - startPosition.x).ToString() + (endPosition.z - startPosition.z).ToString(); 
			//Debug.Log("MOVEMENT STRING: " + movement);
			change_in_movement = false;
			if( movement != last_move){
				change_in_movement = true;
			}
			
			last_move = movement;
			
			//IF A CHANGE IN DIRECTION OCCURS RESTAR ANIMATION
			int i = 0;
			if ((endPosition.x - startPosition.x) == 1 &&  (endPosition.z - startPosition.z) == 0.5)
			{
				
				//Move North
				if(change_in_movement)
					ssAnimation.StartNorth();
				
				
			}
			else if ((endPosition.x - startPosition.x) == 1 &&  (endPosition.z - startPosition.z) == -0.5){
				//Move East
				if(change_in_movement)
				ssAnimation.StartEast();
			}
			else if ((endPosition.x - startPosition.x) == -1 &&  (endPosition.z - startPosition.z) == -0.5){
				//Move South
				if(change_in_movement)
				ssAnimation.StartSouth();
			}
			else if ((endPosition.x - startPosition.x) == -1 &&  (endPosition.z - startPosition.z) == 0.5){
				//Move West
				if(change_in_movement)
				ssAnimation.StartWest();
				
			}
			
			while (transform.position != endPosition) {                                 
				t += Time.deltaTime * (moveSpeed / gridSize);
				transform.position = Vector3.Lerp (startPosition, endPosition, t);

				yield return null;
			}
		}
		
		if(bConvo)
			switchConvo();
		
		isMoving = false;
		ssAnimation.StopRun();
		
		yield return 0;
	}

	public List<Vector3> astarPath (Tile endIn)
	{
		
		List<Vector3> path = new List<Vector3> ();
		;
		
		//Tile start= new Tile();
		Tile start = current_tile; //start tile in
		
		//Tile end = new Tile();
		Tile end = endIn;  //levelGrid.getTile(10,10); //target tile in
		
		Tile current_check_tile = new Tile ();  
		
		List<Tile> open_list = new List<Tile> ();
		List<Tile> closed_list = new List<Tile> ();
		
		
		int g_score = 0; 				// Cost from start along best known path.
		int h_score; 					//heuristic score
		int f_score; 					// Estimated total cost from start to goal through y.
		 
		start.setGScore (g_score);
		h_score = Mathf.Abs (end.x_pos - start.x_pos) + Mathf.Abs (end.z_pos - start.z_pos); 
		start.setFScore (g_score + h_score);
		
		//Debug.Log("START TILE SCORE: G: " + start.getGScore().ToString() + ", F: " + start.getFScore().ToString());
		
		int loop_counter = 0;
		
		open_list.Add (start); 					//Add start tile to open list
		
		//while open list is not empty
		while (open_list.Count > 0) {
			
			//	Debug.Log("Loop counter: " + loop_counter.ToString());
			loop_counter++;
			
			if (loop_counter == 30)
				break;
			
			current_check_tile = getBestTile (open_list);			 //Get tile with lowest f value	
			//Debug.Log("BEST TILE: " + current_check_tile.x_pos.ToString() + "," + current_check_tile.z_pos.ToString());
			
			//if the current_tile in the target
			if (current_check_tile.centre == end.centre) {
				//	 Debug.Log("Goal reached");
				closed_list.Add (current_check_tile);
				path = ConstructPath (closed_list);		//Calculate the path
				break;							//exit while loop
			}
			
			
			open_list.Remove (current_check_tile);		//remove current_tile from open list
			closed_list.Add (current_check_tile);		//add current_tile to closed list
			
			//	Debug.Log("CURRENT TILE: " + current_check_tile.x_pos.ToString() + "," + current_check_tile.z_pos.ToString());
			//get tiles connected to current_tile
			
			List<Tile> neighbors = getNeighbours (current_check_tile);
			foreach (Tile nt in neighbors) {
				//Debug.Log("NEIGHBOR TILE SCORE: G: " + nt.getGScore().ToString() + ", F: " + nt.getFScore().ToString());
			
			}
			foreach (Tile nt in neighbors) {
				//Debug.Log("NEIBOUR X: " + nt.x_pos.ToString() + " Z: " + nt.z_pos.ToString());
				
			
				
				if (closed_list.Contains (nt) || nt.getCollision () == 1)		//if tile is on closed list, move to next tile
					continue;
				
				
				//G score of current tile
				g_score = current_check_tile.getGScore () + 1;
				//heuristic sore of current neighbor tile
				h_score = Mathf.Abs (end.x_pos - nt.x_pos) + Mathf.Abs (end.z_pos - nt.z_pos); 
				//total movement score of current neighbor tile
				f_score = g_score + h_score;
				
				
			
				
				//if tile not already on open list OR f score of current n tile 
				if (!open_list.Contains (nt) || f_score < nt.getFScore ()) {
					//Set scores for neighbour tiles
					nt.setParent (current_check_tile);
					nt.setGScore (g_score);
					nt.setFScore (f_score);
                	
					//If current neighbour tile is not on open list, add
					if (!open_list.Contains (nt)) {
						
						open_list.Add (nt);
					}
				}
			}
		}
		return path;
	}

	public Tile getBestTile (List<Tile> open_list)
	{
		
		/*foreach(Tile t in open_list){
		//Debug.Log("OPENLIST TILE SCORE:" + " [" + t.x_pos.ToString() + ","+ t.z_pos.ToString() + "]" + " G: " + t.getGScore().ToString() + ", F: " + t.getFScore().ToString());
		}*/
	
		Tile temp = new Tile ();
		foreach (Tile t in open_list) {
			//Debug.Log("Get best tile from open list -> Tile: " + t.x_pos.ToString() +"," + t.z_pos.ToString() + " f score: " + t.getFScore().ToString());
			
			if (t.getFScore () <= temp.getFScore () || temp.getFScore () == 0) {
				// 
				
				temp = t;
			}
			
		}
		//Debug.Log("RETURNED BEST: " + temp.x_pos.ToString() + "," + temp.z_pos.ToString());
		return temp;
	}

	public List<Tile> getNeighbours (Tile tile)
	{
		//get tile
		List<Tile> neighbours = new List<Tile> ();
		
		int xpos = tile.x_pos;
		int zpos = tile.z_pos;
		
		neighbours.Add (levelGrid.getTile ((xpos + 1), zpos));
		neighbours.Add (levelGrid.getTile ((xpos - 1), zpos));
		neighbours.Add (levelGrid.getTile (xpos, (zpos + 1)));
		neighbours.Add (levelGrid.getTile (xpos, (zpos - 1)));
		
		return neighbours;
		
	}

	public List<Vector3> ConstructPath (List<Tile> closed_list)
	{
		List<Vector3> path = new List<Vector3> ();
		
		foreach (Tile t in closed_list) {
			//	Debug.Log("PATH X: " + t.x_pos.ToString() + " Z: " + t.z_pos.ToString());
			path.Add (t.centre);
		}
		
		return path;
		
	}
	
	
}
