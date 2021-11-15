using UnityEngine;
using UnityEngine.Tilemaps;

public enum Tool{
	view,
	createPlatform,
	createDecoration,
	delete
}

public class MapManager : MonoBehaviour{
	[Header("variables principales")]
	public Tilemap baseTilemap;
	public Tilemap nodeTilemap;
	[Space()]
	public Tilemap decoBaseTilemap;
	public Tilemap decoNodeTilemap;
	[Space()]
	public Tile node;
	public Tile decorationNode;

	[Header("outils")]
	public Tool curTool;
	public string currentSelectedPlatform = "wood";
	public string currentSelectedDecoration = "torch";

	[Header("limites")]
	public int maxTileCount = 10;
	int tileCount = 0;


	//partie import des autres scriptes
	DataManager dataManager;

	void Awake(){
		dataManager = GetComponent<DataManager>();
	}

	void Start(){
		ForceSetTile(0,0,dataManager.GetPlatformFromId("wood"));
	}

    void Update(){
    	if(curTool == Tool.createPlatform && tileCount <maxTileCount){
    		nodeTilemap.gameObject.SetActive(true);
				decoNodeTilemap.gameObject.SetActive(false);
    	}else if(curTool == Tool.createDecoration){
    		nodeTilemap.gameObject.SetActive(false);
				decoNodeTilemap.gameObject.SetActive(true);
    	}else{
				nodeTilemap.gameObject.SetActive(false);
				decoNodeTilemap.gameObject.SetActive(false);
			}

        if(Input.GetMouseButtonDown(0)){
        	Vector2 mpos= Camera.main.ScreenToWorldPoint(Input.mousePosition);
	        Vector3Int clickPos=  baseTilemap.WorldToCell(mpos);

        	if(curTool == Tool.createPlatform){
	        	SetTile(clickPos.x, clickPos.y, dataManager.GetPlatformFromId(currentSelectedPlatform));
        	}else if(curTool == Tool.createDecoration){
	        	SetDecoration(clickPos.x, clickPos.y, dataManager.GetDecorationFromId(currentSelectedDecoration));
        	}else if(curTool == Tool.delete){
        		DeleteTile(clickPos.x, clickPos.y);
        	}
        }
    }

		//fonctions pour la decoration
		void SetDecoration(int x, int y,Deco newTile){
			if(newTile != null && // test si l'id est valide
    	   decoBaseTilemap.GetTile(new Vector3Int(x,y,0)) == null && // test si tile est vide
    	   decoNodeTilemap.GetTile(new Vector3Int(x,y,0)) == decorationNode){ // test si node existe

					 //ajout de la tile de la decoration
					 decoBaseTilemap.SetTile(new Vector3Int(x,y,0),newTile.data);
		     	 decoNodeTilemap.SetTile(new Vector3Int(x,y,0),null);
    	}
		}

		void DeleteDecoration(int x, int y){
			decoBaseTilemap.SetTile(new Vector3Int(x,y,0),null);
			decoNodeTilemap.SetTile(new Vector3Int(x,y,0),decorationNode);
		}

    //pour suprimer des tiles
    void DeleteTile(int x, int y){
			if(decoBaseTilemap.GetTile(new Vector3Int(x,y,0)) != null){
				DeleteDecoration(x,y);
				return;
			}
    	if (x == 0 && y ==0){return;}

			//etape 0, supression node de decoration (qu'il y en est ou non);
			decoNodeTilemap.SetTile(new Vector3Int(x,y,0),null);

    	//detection si tile n'est pas vide
    	if(baseTilemap.GetTile(new Vector3Int(x,y,0)) != null){
    		tileCount --;

    		//etape 1 , supression du tile
    		baseTilemap.SetTile(new Vector3Int(x,y,0),null);

    		//etape 2 , supression des nodes innutiles
    		if(TestNeighbour(x+1,y)){nodeTilemap.SetTile(new Vector3Int(x+1,y,0),null);}
    		if(TestNeighbour(x-1,y)){nodeTilemap.SetTile(new Vector3Int(x-1,y,0),null);}
    		if(TestNeighbour(x,y+1)){nodeTilemap.SetTile(new Vector3Int(x,y+1,0),null);}
    		if(TestNeighbour(x,y-1)){nodeTilemap.SetTile(new Vector3Int(x,y-1,0),null);}

    		//étape 3 , ajout d'une node a la position
    		if(!TestNeighbour(x,y)){nodeTilemap.SetTile(new Vector3Int(x,y,0),node);}
    	}
    }

    bool TestNeighbour(int x, int y){
    	if(baseTilemap.GetTile(new Vector3Int(x+1,y,0)) != null){return false;}
    	if(baseTilemap.GetTile(new Vector3Int(x-1,y,0)) != null){return false;}
    	if(baseTilemap.GetTile(new Vector3Int(x,y+1,0)) != null){return false;}
    	if(baseTilemap.GetTile(new Vector3Int(x,y-1,0)) != null){return false;}
    	return true;
    }

    //pour placer des tiles
    void SetTile(int x, int y, Block newTile){
    	if(newTile != null && // test si l'id est valide
    	   baseTilemap.GetTile(new Vector3Int(x,y,0)) == null && // test si tile est vide
    	   nodeTilemap.GetTile(new Vector3Int(x,y,0)) == node && // test si node existe
    	   tileCount < maxTileCount){ //test si limite pas depassé
    		ForceSetTile(x,y,newTile);
    	}
    }

    void ForceSetTile(int x, int y, Block newTile){
    	//augmentation de 1 de la limite
    	tileCount ++;

			//etape 0 , placement de la node pour la decoration si nessesaire
			if(newTile.Decorable){
				decoNodeTilemap.SetTile(new Vector3Int(x,y,0),decorationNode);
			}

    	//etape 1 placement tile centrale
    	baseTilemap.SetTile(new Vector3Int(x,y,0),newTile.data);
    	nodeTilemap.SetTile(new Vector3Int(x,y,0),null);

    	//etape 2 placement des node si nessesaire
    	if(baseTilemap.GetTile(new Vector3Int(x+1,y,0)) == null){
    		nodeTilemap.SetTile(new Vector3Int(x+1,y,0),node);
    	}

    	if(baseTilemap.GetTile(new Vector3Int(x-1,y,0)) == null){
    		nodeTilemap.SetTile(new Vector3Int(x-1,y,0),node);
    	}

    	if(baseTilemap.GetTile(new Vector3Int(x,y+1,0)) == null){
    		nodeTilemap.SetTile(new Vector3Int(x,y+1,0),node);
    	}

    	if(baseTilemap.GetTile(new Vector3Int(x,y-1,0)) == null){
    		nodeTilemap.SetTile(new Vector3Int(x,y-1,0),node);
    	}
    }
}
