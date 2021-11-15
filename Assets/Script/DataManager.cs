using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;


public class DataManager : MonoBehaviour
{
    [Header("Platformes")]
    public Block[] tiles;
    private Dictionary<string,Block> tilesData = new Dictionary<string,Block>();

    [Header("Decoration")]
    public Deco[] decorations;
    private Dictionary<string,Deco> decosData = new Dictionary<string,Deco>();

    void Awake(){

        //creer le dictionnaire pour les plateformes
        for(int i = 0 ; i < tiles.GetLength(0);i++){
            tilesData.Add(tiles[i].id,tiles[i]);
        }

        //creation de dictionnaire pour les decorations
        for(int i = 0 ; i < decorations.GetLength(0);i++){
            decosData.Add(decorations[i].id,decorations[i]);
        }
    }

    //fonction pour obtenir les informations
    public Block GetPlatformFromId(string id){
        if(tilesData.ContainsKey(id)){
            //existe !
            return tilesData[id];
        }else{
            //existe po
            return null;
        }
    }
    public Deco GetDecorationFromId(string id){
        if(decosData.ContainsKey(id)){
            //existe !
            return decosData[id];
        }else{
            //existe po
            return null;
        }
    }
}
