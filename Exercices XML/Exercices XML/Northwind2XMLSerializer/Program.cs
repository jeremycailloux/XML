using Northwind2XMLSerializer.Models;
using Outils.TConsole;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace Northwind2XMLSerializer
{
    class Program
    {
        private static Northwind2Context _context = new Northwind2Context(); // Générer le jeu d’entités et le contexte de données à partir de la base Northwind2

        static void Main(string[] args)
        {
            DisplayMenu();
            Console.ReadKey();
        }

        private static void DisplayMenu()
        {
            // Affiche le menu suivant
            Console.WriteLine("1. Sérialiser les commandes d’un client");
            Console.WriteLine("2. Désérialiser les commandes");
            Console.WriteLine("3. Désérialiser les commandes avec XmlWriter");

            int choix;
            while (!int.TryParse(Console.ReadLine(), out choix) || choix < 1 || choix > 3);

            switch (choix)
            {
                case 1:
                    Console.WriteLine("Veuillez saisir un id client");
                    string id = Console.ReadLine();
                    var listCol = _context.Orders.Where(c => c.CustomerId == id).ToList();
                    ExportXML(listCol);
                    break;
                case 2:
                    var list = ImportXML();
                    ConsoleTable.From(list).Display("Commande(s)");

                    foreach(var item in list)
                    {
                        ConsoleTable.From(new List<Orders> { item }).Display("Commande");
                        ConsoleTable.From(item.OrderDetail).Display("Lignes(s) de commande");
                    }
                    break;
                case 3:
                    var OrderList = _context.Orders.Include(o=> o.OrderDetail).ToList();
                    WriteXml();
                    break;
            }
        }

        // Chargement des données et sérialisation
        private static void ExportXML(List<Orders> listCol)
        {
            // On crée un sérialiseur, en spécifiant le type de l'objet à sérialiser
            // et le nom de l'élément xml racine
            XmlSerializer serializer = new XmlSerializer(typeof(List<Orders>),
                                       new XmlRootAttribute("CustomerOrders"));

            using (var sw = new StreamWriter(@"..\..\Orders.xml"))
            {
                serializer.Serialize(sw, listCol);
            }
        }

        // Désérialisation
        private static List<Orders> ImportXML()
        {
            List<Orders> listCol = null;

            XmlSerializer deserializer = new XmlSerializer(typeof(List<Orders>),
               new XmlRootAttribute("CustomerOrders"));

            using (var sr = new StreamReader(@"..\..\Orders.xml"))
            {
                listCol = (List<Orders>)deserializer.Deserialize(sr);
            }

            return listCol;

        }

        // Désérialisation avec XmlWriter
        private static void WriteXml()
        {
            //Console.WriteLine("id customer :");
            //string id = Console.ReadLine();
            var listCol = _context.Orders.Local.GroupBy(o => o.OrderDate.Year.ToString() + o.OrderDate.Month.ToString()).ToList();
            // Définit les paramètres pour l'indentation du flux xml généré
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";

            // Utilisation d'un XmlWriter avec les paramètres définis précédemment
            // pour écrire un fichier CollectionsBD_Writer.xml
            using (XmlWriter writer = XmlWriter.Create(@"..\..\Orders_Writer.xml",
                                          settings))
            {
                // Ecriture du prologue
                writer.WriteStartDocument();
                writer.WriteStartElement("DatesCommandes");
                foreach (var item in listCol)
                {
                    writer.WriteStartElement("DateCommande");
                    writer.WriteAttributeString("annee", item.Key.Substring(0, 4));
                    writer.WriteAttributeString("mois", item.Key.Substring(4));
                    foreach (var commande in item)
                    {
                        writer.WriteStartElement("Commande");
                        writer.WriteAttributeString("Id", commande.OrderId.ToString());
                        decimal montant = commande.OrderDetail.Sum(od => (decimal)(1 - od.Discount) * od.UnitPrice * od.Quantity) + (decimal)commande.Freight;
                        writer.WriteAttributeString("Montant", montant.ToString());
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();

                }
                // Ecriture de l'élément racine
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }
    }
}
