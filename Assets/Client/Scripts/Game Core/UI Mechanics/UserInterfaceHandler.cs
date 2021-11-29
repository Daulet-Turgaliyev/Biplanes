using Client.Scripts.Game_Core.UI_Mechanics.Controllers;
using UnityEngine;

namespace Client.Scripts.Game_Core.UI_Mechanics
{
    public class UserInterfaceHandler
    {
        public AController currentController { private set; get; }
        
        public void SwitchController(AController controller)
        {
            Debug.Log($"Controller has swithed");
            currentController = controller;
        }
    }
}