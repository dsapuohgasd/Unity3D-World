using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Coin : MonoBehaviour
{
    private Animator _animator;
    private GameObject player;
    private float spawnDistanceMin = 10;
    private float spawnDistanceMax = 20;
    private float coinOffsetY;
    private float respawnTime=1;
    public float restTime;  

    // Start is called before the first frame update
    void Start()
    {
        _animator= GetComponent<Animator>();
        player = GameObject.Find("Player");
        coinOffsetY= this.transform.position.y - Terrain.activeTerrain.SampleHeight(this.transform.position);
        restTime = respawnTime;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        restTime-=Time.deltaTime/10;
        if (restTime <= 0)
        {
            Respawn(spawnDistanceMax,spawnDistanceMin);
            restTime = respawnTime;
        }
        float coinDistance = (this.transform.position - player.transform.position).magnitude;

        if (coinDistance<5)
        {
            _animator.SetBool("IsNear", true);
        }
        else
        {
            _animator.SetBool("IsNear", false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Player") {
            _animator.SetBool("IsPicked", true);

        }
        else
        {
            Debug.Log(other.gameObject.name);
            float coinRadius = transform.localScale.x;
            Respawn(coinRadius,coinRadius);

        }
    }
    public void Picked()
    {

        Respawn(spawnDistanceMax,spawnDistanceMin);
        restTime = 1;
        _animator.SetBool("IsPicked", false);
        Player.CoinCount += GameSettings.CoinValue;
    }

    public void Respawn(float max,float min)
    {
        Vector3 newPosition;
        float distance;
        do
        {
            newPosition = new Vector3(
                this.transform.position.x + Random.Range(-max, min),
                this.transform.position.y,
                this.transform.position.z + Random.Range(-max, max));
            distance = Vector3.Distance(newPosition, this.transform.position);
        } while (distance < min
                 || distance > max
                 || newPosition.x < 15
                 || newPosition.z < 15
                 || newPosition.x > 1000 - 15
                 || newPosition.z > 1000 - 15);

        float y = Terrain.activeTerrain.SampleHeight(newPosition) + coinOffsetY;
        newPosition.y = y;

        this.transform.position = newPosition;
    }
}
