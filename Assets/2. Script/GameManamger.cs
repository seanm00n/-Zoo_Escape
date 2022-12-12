using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManamger : MonoBehaviour
{
    [SerializeField] Transform[] spawnPointA;
    [SerializeField] Transform[] spawnPointB;
    [SerializeField] Transform[] spawnPointC;

    [SerializeField] GameObject monsterPrefA;
    [SerializeField] GameObject monsterPrefB;
    [SerializeField] GameObject monsterPrefC;
    private void Start () {
        if(monsterPrefA != null) {
            for (int i = 0; i < spawnPointA.Length; i++) {
                Instantiate(monsterPrefA, spawnPointA[i]);
            }
        }
        if(monsterPrefB != null) {
            for (int i = 0; i < spawnPointB.Length; i++) {
                Instantiate(monsterPrefB, spawnPointB[i]);
            }
        }
        if (monsterPrefC != null) {
            for (int i = 0; i < spawnPointC.Length; i++) {
                Instantiate(monsterPrefC, spawnPointC[i]);
            }
        }
        
    }
}
