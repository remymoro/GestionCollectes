using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCollectes.Presentation.Messages
{
    public class CentresChangedMessage : ValueChangedMessage<object>
    {
        public CentresChangedMessage() : base(null) { }
    }
}
