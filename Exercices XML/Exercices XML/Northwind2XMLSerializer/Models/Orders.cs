using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Northwind2XMLSerializer.Models
{
    public partial class Orders
    {
        public Orders()
        {
            OrderDetail = new HashSet<OrderDetail>();
        }

        [XmlAttribute]
        public int OrderId { get; set; }
        [XmlAttribute]
        public Guid AddressId { get; set; }
        [XmlAttribute]
        public string CustomerId { get; set; }
        [XmlAttribute]
        public int EmployeeId { get; set; }
        [XmlAttribute]
        public int ShipperId { get; set; }
        [XmlElement]
        public DateTime OrderDate { get; set; }
        [XmlElement] // Pour les attributs nullables
        public DateTime? RequiredDate { get; set; }
        [XmlIgnore]
        public DateTime? ShippedDate { get; set; }
        [XmlIgnore]
        public decimal? Freight { get; set; }
        [XmlIgnore]
        public string ShipName { get; set; }

        //[XmlArray("Addresses")]
        //[XmlArrayItem("Address")]
        [XmlIgnore]
        public virtual Address Address { get; set; }
        //[XmlArray("Customers")]
        //[XmlArrayItem("Customer")]
        [XmlIgnore]
        public virtual Customer Customer { get; set; }
        //[XmlArray("Employees")]
        //[XmlArrayItem("Employee")]
        [XmlIgnore]
        public virtual Employee Employee { get; set; }
        //[XmlArray("Shippers")]
        //[XmlArrayItem("Shipper")]
        [XmlIgnore]
        public virtual Shipper Shipper { get; set; }
        [XmlArray("OrderDetails")]
        [XmlArrayItem("OrderDetail")]
        public virtual HashSet<OrderDetail> OrderDetail { get; set; }
    }
}
