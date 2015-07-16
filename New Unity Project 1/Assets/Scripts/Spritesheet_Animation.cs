using UnityEngine;
using System.Collections;

public class Spritesheet_Animation : MonoBehaviour
{
	public Texture walkEast;
	public Texture walkNorth;
	public Texture walkWest;
	public Texture walkSouth;
	//public Texture jumpRight;
	//public Texture jumpLeft;
	private int rows;
	private int columns;
	public int walkColumns = 5;
	public int walkRows = 6;
//	public int jumpColumns = 5;
//	public int jumpRows = 4;
	public float jumpFPS = 10.0f;
	public float FPS;
	private int index = 0;
	public bool bNorth;
	public bool bSouth;
	public bool bEast;
	public bool bWest; 
	private bool run = false;
	//private bool jump = false;
//	private bool fall = false;
	
	public void SetDirection(string dir)
	{
		switch(dir){
			case "North":
			bNorth = true;
			break;
			case "East":
			bEast = true;
			break;
			case "South":
			bSouth = true;
			break;
			case "West":
			bWest = true;
			break;
		}
		
		
	}
	public void still ()
	{
		GetComponent<Renderer>().material.SetTextureOffset ("_MainTex", new Vector2 (1f, 1f));
		GetComponent<Renderer>().material.SetTextureScale ("_MainTex", new Vector2 (1f / walkColumns, 1f / walkRows));
		
		if (bNorth) {
			this.GetComponent<Renderer>().material.mainTexture = walkNorth;
		} else if (bEast) {
			this.GetComponent<Renderer>().material.mainTexture = walkEast;
		}
		else if (bSouth) {
			this.GetComponent<Renderer>().material.mainTexture = walkSouth;
		}
		else if (bWest) {
			this.GetComponent<Renderer>().material.mainTexture = walkWest;
		}
	}


	//=========================================RUN
	public void StartNorth ()
	{
		//if (!jump) {
			columns = walkColumns;
			rows = walkRows;
			
			bNorth = true;
			bEast = false;
			bSouth = false;
			bWest = false;
		
			this.GetComponent<Renderer>().material.mainTexture = walkNorth;
			run = true;
			index = 0;
			StartCoroutine (AnimateRun ());	
		//}
	}

	public void StartEast ()
	{
		//if (!jump) {
			columns = walkColumns;
			rows = walkRows;
			bNorth = false;
			bEast = true;
			bSouth = false;
			bWest = false;
			//jump = false;
		
			this.GetComponent<Renderer>().material.mainTexture = walkEast;
			run = true;
			index = rows * columns;
			StartCoroutine (AnimateRun ());	
		//}
	}
	public void StartSouth ()
	{
		//if (!jump) {
			columns = walkColumns;
			rows = walkRows;
			bNorth = false;
			bEast = false;
			bSouth = true;
			bWest = false;
			//jump = false;
		
			this.GetComponent<Renderer>().material.mainTexture = walkSouth;
			run = true;
			index = rows * columns;
			StartCoroutine (AnimateRun ());	
		//}
	}
	public void StartWest ()
	{
		//if (!jump) {
			columns = walkColumns;
			rows = walkRows;
			bNorth = false;
			bEast = false;
			bSouth = false;
			bWest = true;
			//jump = false;
		
			this.GetComponent<Renderer>().material.mainTexture = walkWest;
			run = true;
			index = rows * columns;
			StartCoroutine (AnimateRun ());	
		//}
	}

	public void StopRun ()
	{
		run = false;
		still();
	}

	
	public float uIndex;
	public	float vIndex;
	public float offsetX;
	public float offsetY;
	public Vector2 size;
	
	private IEnumerator AnimateRun ()
	{
		while (run) {
			
			
			//Vector2 size;
			//if (bNorth) {
				size = new Vector2 (1f / columns, 1f / rows);
			//} else {
			//	size = new Vector2 (1f / columns, 1f / rows);
			//}
			GetComponent<Renderer>().material.SetTextureScale ("_MainTex", size);
			
			if (index >= rows * columns) {
				index = 0;
			}
				
			uIndex = index % columns;
			vIndex = index / columns;
			int columnStart = 0;
			int rowStart = 0;
			
			offsetX = (uIndex + columnStart) * size.x;
			offsetY = (1.0f - size.y) - (vIndex + rowStart) * size.y;
					
			//split into x and y indexes
			if (bNorth || bEast || bSouth || bWest) {
				index++;
				Vector2 offset = new Vector2 (offsetX, offsetY);
	
				GetComponent<Renderer>().material.SetTextureOffset ("_MainTex", offset);
			} 

			yield return new WaitForSeconds(1f / jumpFPS);
		}			
	}		
}
