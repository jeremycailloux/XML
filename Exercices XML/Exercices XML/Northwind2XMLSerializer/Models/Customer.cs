using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Northwind2XMLSerializer.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Orders = new HashSet<Orders>();
        }

        [XmlAttribute]
        public string CustomerId { get; set; }
        [XmlAttribute]
        public Guid AddressId { get; set; }
        [XmlAttribute]
        public string CompanyName { get; set; }
        [XmlIgnore]
        public string ContactName { get; set; }
        [XmlIgnore]
        public string ContactTitle { get; set; }

        //[XmlArray("Addresses")]
        //[XmlArrayItem("Address")]
        [XmlIgnore]
        public virtual Address Address { get; set; }

        [XmlArray("Orders")]
        [XmlArrayItem("Order")]
        public virtual HashSet<Orders> Orders { get; set; }
    }
}
