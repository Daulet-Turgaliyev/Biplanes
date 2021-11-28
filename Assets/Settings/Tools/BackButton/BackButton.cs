using System;
using UnityEngine;
using Tools.Updater;

namespace Tools.Back {
    public class BackButton: IUpdateMono
    {
        private static BackButton _instance;

        public static BackButton Instance {
            get {
                if (_instance == null)
                {
                    _instance = new BackButton();
                    UpdaterMono.Instance.Add(_instance);
                }

                return _instance;
            }
        }

        private int _stoppers;

        public event Action OnBack = () => { };

        public void Disable() {
            _stoppers++;
        }

        public void Enable() {
            if (_stoppers > 0) _stoppers--;
        }

        public void EnableAndClear() { _stoppers = 0; }
        
        public void Tick() {
            if (Input.GetKeyUp(KeyCode.Escape)) {
                if (_stoppers > 0) return;
                OnBack();
            }
        }
    }
}