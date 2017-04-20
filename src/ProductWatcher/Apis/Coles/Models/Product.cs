using System.Collections.Generic;

namespace ProductWatcher.Apis.Coles.Models
{

    public class A
    {
        //quantity descripton
        public List<string> O3 { get; set; } = new List<string>();

#region WTF like wow coles...who wrote this crap
        public List<string> A4 { get; set; }
        public List<string> B { get; set; }
        public List<string> E1 { get; set; }
        public List<string> F3 { get; set; }
        public List<string> H1 { get; set; }
        public List<string> H2 { get; set; }
        public List<string> I1 { get; set; }
        public List<string> L2 { get; set; }
        public List<string> O { get; set; }
        public List<string> O1 { get; set; }

        public List<string> P { get; set; }
        public List<string> P8 { get; set; }
        public List<string> P9 { get; set; }
        public List<string> S { get; set; }
        public List<string> S5 { get; set; }
        public List<string> S9 { get; set; }
        public List<string> T { get; set; }
        public List<string> T1 { get; set; }
        public List<string> T2 { get; set; }
        public List<string> U { get; set; }
        public List<string> W1 { get; set; }
        public List<string> WEIGHTEDBYEACH { get; set; }
#endregion
    }

    public class Price
    {
        public decimal? l4 { get; set; }
        public decimal? o { get; set; }
    }

    public class CatalogEntryView
    {
        //Only shit we use

        //image
        public string fi { get; set; }
        //Brand
        public string m { get; set; }
        //Image thumb
        public string t { get; set; }
        //This is price per litre
        public string u2 { get; set; }
        //Product Description
        public string n { get; set; }
        //Price dahhh
        public Price P1 { get; set; }
        //Product code
        public string p { get; set; }

#region more absolute Nonsense

        public A a { get; set; }
        public string pl { get; set; }
        public string s { get; set; }
        public string s9 { get; set; }

        public string t1 { get; set; }
        public string u { get; set; }
#endregion
    }

    public class M4
    {
        public string p1 { get; set; }
    }

    public class ShitAsProductModel
    {
        public List<CatalogEntryView> catalogEntryView { get; set; } = new List<CatalogEntryView>();
        public M4 m4 { get; set; }
        public int? recordSetCount { get; set; }
        public int? recordSetStartNumber { get; set; }
        public int? recordSetTotal { get; set; }
    }

}