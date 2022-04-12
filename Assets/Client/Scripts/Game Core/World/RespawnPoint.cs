using UnityEngine;

public class RespawnPoint: MonoBehaviour
{
	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.TryGetComponent(out PilotBehaviour pilotBehaviour))
		{
			pilotBehaviour.CmdRespawnPlane();
		}
	}
}
