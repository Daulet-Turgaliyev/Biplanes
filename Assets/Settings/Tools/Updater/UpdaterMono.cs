using System.Collections.Generic;
using Tools.Singletons;

namespace Tools.Updater {
    public class UpdaterMono: SingletoneMonoBehaviour<UpdaterMono>
    {
        private readonly List<IUpdateMono> _updateList = new List<IUpdateMono>();

        public void Add(IUpdateMono updateItem) { _updateList.Add(updateItem); }
        public void Remove(IUpdateMono updateItem) { _updateList.Remove(updateItem); }

        private void Update()
        {
            foreach (var updateMono in _updateList)
            {
                updateMono.Tick();
            }
        }
    }
}