using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Northwind2XMLSerializer.Models
{
    public partial class OrderDetail
    {
        [XmlAttribute]
        public int OrderId { get; set; }
        [XmlText]
        public int ProductId { get; set; }
        [XmlAttribute]
        public decimal UnitPrice { get; set; }
        [XmlAttribute]
        public short Quantity { get; set; }
        [XmlAttribute]
        public float Discount { get; set; }

        [XmlIgnore]
        public virtual Orders Order { get; set; }
        [XmlIgnore]
        public virtual Product Product { get; set; }
    }
}
