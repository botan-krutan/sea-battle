using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingManager : MonoBehaviour
{
    public static ShootingManager Instance;
    [SerializeField] GameObject _rocketPrefab, _crossPrefab, _dotPrefab;
    [SerializeField] Transform playerParent;
    [SerializeField] Transform aiParent;
    [SerializeField] AudioClip waterSound, hitSound, explosionSound;
    // Start is called before the first frame update
    void Start()
    {

    }
    void Awake() => Instance = this;
    // Update is called once per frame
    void Update()
    {

    }
    public int ShootTile(float[] coordinates, bool player)
    {
        //float x = coordinates[0];
        //float y = coordinates[1];

        //RaycastHit2D[] results = Physics2D.RaycastAll(new Vector3(x,y,0), Vector2.zero);
        //foreach (var result in results)
        //{
        //    if (result && result.collider.gameObject.TryGetComponent(out Tile tileComp))
        //    {
        //        if(player)
        //        {
        //            if(tileComp.aiShip)
        //            {
        //                tileComp.aiShip.hp -= 1;

        //                if (tileComp.aiShip.hp <= 0)
        //                {
        //                    Instantiate(_crossPrefab, tileComp.gameObject.transform).transform.parent = aiParent;
        //                    tileComp.aiShip.GetComponent<DG_Animations>().DeathAnimation();
        //                    MarkShotsAround(tileComp.aiShip.currentlyOccupiedTiles, player);
        //                    tileComp.aiShip = null;
        //                    bool stopGame = true;
        //                    foreach (var item in GameObject.Find("AI Ships").GetComponentsInChildren<ShipBase>())
        //                    {
        //                        if (item.hp > 0)
        //                        {
        //                            stopGame = false;
        //                        }

        //                    }
        //                    if (stopGame) GameManager.Instance.UpdateState(GameManager.GameState.PlayerWin);
        //                    AudioPlayer.Instance.PlaySound(explosionSound);
        //                    return 1;
        //                }
        //                else
        //                {
        //                    Instantiate(_crossPrefab, tileComp.gameObject.transform).transform.parent = aiParent;
        //                    tileComp.aiShip = null;
        //                    AudioPlayer.Instance.PlaySound(hitSound);
        //                    return 0;
        //                }


        //            }
        //            Instantiate(_dotPrefab, tileComp.gameObject.transform).transform.parent = aiParent;
        //            AudioPlayer.Instance.PlaySound(waterSound);
        //            return -1;
        //        }
        //        else
        //        {
        //            if (tileComp.playerShip)
        //            {
        //                tileComp.playerShip.hp -= 1;

        //                if (tileComp.playerShip.hp <= 0)
        //                {
        //                    Instantiate(_crossPrefab, tileComp.gameObject.transform).transform.parent = playerParent;
        //                    tileComp.playerShip.GetComponent<DG_Animations>().DeathAnimation();
        //                    MarkShotsAround(tileComp.playerShip.currentlyOccupiedTiles, player);
        //                    tileComp.playerShip = null;
        //                    AudioPlayer.Instance.PlaySound(explosionSound);
        //                    return 1;
        //                }
        //                else {
        //                    Instantiate(_crossPrefab, tileComp.gameObject.transform).transform.parent = playerParent;
        //                    tileComp.playerShip = null;
        //                    AudioPlayer.Instance.PlaySound(hitSound);
        //                    return 0;
        //                }   

        //            }
        //            Instantiate(_dotPrefab, tileComp.gameObject.transform).transform.parent = playerParent;
        //            AudioPlayer.Instance.PlaySound(waterSound);
        //            return -1;
        //        }
        //    }
        //}
        return -1;
    }
    //void MarkShotsAround(List<Tile> listCells, bool player)
    //{
    //    foreach (var cell in listCells) // each cell beneath the new settled ship
    //    {
    //        int x = int.Parse(cell.gameObject.name.Replace($"Tile ", "")[0].ToString());
    //        int y = int.Parse(cell.gameObject.name.Replace($"Tile ", "")[1].ToString());
    //        for (int i = -1; i <= 1; i++)
    //        {
    //            for (int j = -1; j <= 1; j++)
    //            {
    //                if ((x + i >= 0) && (x + i <= 9) && (y + j >= 0) && (y + j <= 9))
    //                {
    //                    if (!listCells.Contains(TileManager.Instance.GetTile(x + i, y + j)))
    //                    {
    //                        if (player)
    //                        {
    //                            Instantiate(_dotPrefab, TileManager.Instance.GetTile(x + i, y + j).gameObject.transform).transform.parent = aiParent;
    //                        }
    //                        else Instantiate(_dotPrefab, TileManager.Instance.GetTile(x + i, y + j).gameObject.transform).transform.parent = playerParent;

    //                    }
    //                }
    //            }
    //        }
    //    }
    //}
}


