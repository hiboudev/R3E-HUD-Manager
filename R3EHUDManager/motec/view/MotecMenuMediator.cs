using da2mvc.framework.collection.model;
using da2mvc.framework.collection.view;
using R3EHUDManager.motec.model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.motec.view
{
    class MotecMenuMediator : CollectionMediator<CollectionModel<MotecModel>, MotecModel, MotecMenuView>
    {
        public MotecMenuMediator()
        {
            Debug.WriteLine("toto");
        }
    }
}
