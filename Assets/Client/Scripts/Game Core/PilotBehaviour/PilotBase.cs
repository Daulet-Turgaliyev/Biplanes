
    using UnityEngine;

    public class PilotBase: IBaseObject
    {
        private Rigidbody2D PlaneRigidbodyPlane { get; }
        public PilotData PilotData { get; }

        public PilotMovement PilotMovement { get; private set; }

        public PilotBase(Rigidbody2D planeRigidbody2D, PilotData pilotData)
        {
            PlaneRigidbodyPlane = planeRigidbody2D;
            PilotData = pilotData;

            PilotMovementInit();
        }
        
        private void PilotMovementInit()
        {
            var pilotSettings = new PilotSettings(PilotData);
            PilotMovement = new PilotMovement(pilotSettings, PlaneRigidbodyPlane);
            GameManager.Instance.OpenGameWindow(this);
        }
    }