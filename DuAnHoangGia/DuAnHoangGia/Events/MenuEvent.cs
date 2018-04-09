using Prism.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace DuAnHoangGia.Events
{
    public class MenuEvent : PubSubEvent<MenuMessage>
    {
    }
    public class MenuMessage
    {
        public bool IsPresented { get; set; }
    }
}
