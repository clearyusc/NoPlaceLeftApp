using System;
using System.Collections.Generic;
using System.Text;

namespace App2.Models
{
    interface IInteraction {
        IList<MinistryAction> MinistryActions { get; set; }
    }
    class Interaction
    {
        public IList<MinistryAction> MinistryActions { get; set; }

        public Interaction()
        {
            MinistryActions = new List<MinistryAction>();
        }
    }
}
