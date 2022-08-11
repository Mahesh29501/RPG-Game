using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core

{
    public class ActionScheduler : MonoBehaviour
    {
        IAction currentaction;
        public void startAction(IAction action)
        {
            if (currentaction == action) return;
            if (currentaction != null) { 
                currentaction.Cancel();
            }
            currentaction = action;
        }

        public void CancelCurrentAction()
        {
            startAction(null);
        }
    }
}
