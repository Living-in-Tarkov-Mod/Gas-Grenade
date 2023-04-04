using BepInEx;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GasGrenadePlugin
{
    [BepInPlugin("com.example.gasgrenade", "Gas Grenade Plugin", "1.0.0")]
    public class GasGrenade : BaseUnityPlugin
    {
        public GameObject smokePrefab;
        public float damagePerSecond = 10f;
        public float gasDuration = 10f;
        public float gasRadius = 10f;
        public AudioClip gasSound;
        public LayerMask gasLayerMask;

        private HashSet<GameObject> objectsInGasCloud = new HashSet<GameObject>();

        void Start()
        {
            // Load the smoke grenade prefab from Escape from Tarkov
            smokePrefab = Resources.Load<GameObject>("Prefabs/Effects/SmokeGrenades/SmokeGrenade1");
        }

        void Update()
        {
            // Check for objects in the gas cloud and apply damage
            foreach (GameObject obj in objectsInGasCloud)
            {
                ApplyGasDamage(obj);
            }
        }

        void OnTriggerEnter(Collider other)
        {
            // Add objects that enter the gas cloud to the set
            if ((gasLayerMask.value & 1 << other.gameObject.layer) != 0)
            {
                objectsInGasCloud.Add(other.gameObject);

                // Play gas sound for AI
                if (other.CompareTag("AI"))
                {
                    AudioSource.PlayClipAtPoint(gasSound, other.transform.position);
                }
            }
        }

        void OnTriggerExit(Collider other)
        {
            // Remove objects that exit the gas cloud from the set
            if ((gasLayerMask.value & 1 << other.gameObject.layer) != 0)
            {
                objectsInGasCloud.Remove(other.gameObject);
            }
        }

        void ApplyGasDamage(GameObject obj)
        {
            // Apply gas damage to objects in the gas cloud
            HealthController healthController = obj.GetComponent<HealthController>();
            if (healthController != null)
            {
                float damage = damagePerSecond * Time.deltaTime;
                healthController.TakeDamage(damage, DamageType.Toxic);

                // Slow down AI movement speed while in gas cloud
                if (obj.CompareTag("AI"))
                {
                    obj.GetComponent<AIController>().speedModifier = 0.5f;
                }
            }
        }

        public void DeployGasGrenade(Vector3 position)
        {
            // Instantiate the smoke grenade prefab at the specified position
            GameObject smoke = Instantiate(smokePrefab, position, Quaternion.identity);

            // Set the smoke grenade duration and radius
            SmokeGrenade smokeGrenade = smoke.GetComponent<SmokeGrenade>();
            smokeGrenade.duration = gasDuration;
            smokeGrenade.radius = gasRadius;

            // Destroy the smoke grenade after it has finished
            Destroy(smoke, gasDuration);
        }
    }
}
