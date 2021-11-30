using System;
using Client.Scripts.Game_Core.UI_Mechanics.Controllers;
using UnityEngine;

namespace Client.Scripts.Game_Core.UI_Mechanics
{
    public class UserInterfaceHandler
    {
        public PlaneController planeController {  get; private set; }

        public void SetPlaneController(PlaneController currentController)
        {
            planeController = currentController;
        }
    }
}