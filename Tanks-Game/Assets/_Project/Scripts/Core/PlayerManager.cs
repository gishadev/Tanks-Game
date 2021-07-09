using System.Collections;
using UnityEngine;

namespace Gisha.TanksGame.Core
{
    public class PlayerManager : MonoBehaviour
    {
        [Header("Respawn")]
        [SerializeField] private GameObject vehiclePrefab;
        [SerializeField] private Transform[] spawnpoints;
        [SerializeField] private float respawnTime;

        VehicleController _vehicleController;

        private void Start()
        {
            RespawnVehicle();
        }

        private void RespawnVehicle()
        {
            var randomSpawnpoint = spawnpoints[Random.Range(0, spawnpoints.Length)];
            var obj = Instantiate(vehiclePrefab, randomSpawnpoint.position, Quaternion.identity);

            _vehicleController = obj.GetComponent<VehicleController>();
            _vehicleController.OnDestroyed += StartDelayedRespawn;
        }

        private void StartDelayedRespawn()
        {
            StartCoroutine(DelayedRespawnCoroutine());
        }

        private IEnumerator DelayedRespawnCoroutine()
        {
            yield return new WaitForSeconds(1f);
            RespawnVehicle();
        }
    }
}
