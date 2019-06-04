using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenAIO
{
    public class Product
    {

        public Product(string url, int id, float size, string profile, string proxy)
        {
            URL = url;
            Name = id;
            Size = size;
            Profile = profile;
            CProxy = proxy;
        }

        public string URL
        {
            get; private set;
        }
        public string CProxy
        {
            get; private set;
        }


        public int Name
        {
            get; private set;
        }

        public float Size
        {
            get; private set;
        }

        public string Profile
        {
            get; private set;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder(0x40);

            builder.Append("Product:{URL=\"");
            builder.Append(URL);
            builder.Append("\", Name=\"");
            builder.Append(Name);
            builder.Append("\", Size=\"");
            builder.Append(Size);
            builder.Append("\", Profile=\"");
            builder.Append(Profile);
            builder.Append("\"}");
            builder.Append(CProxy);
            builder.Append("\"}");

            return builder.ToString();
        }

        public override int GetHashCode()
        {
            int hash = 17;

            unchecked
            {
                hash = hash * 31 + URL.GetHashCode();
                hash = hash * 31 + Name.GetHashCode();
                hash = hash * 31 + (int) (Size * 2.0F); // multiply by 2 to remove half sizes
                hash = hash * 31 + Profile.GetHashCode();
            }

            return hash;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Product))
                return false;
            else if (obj == this)
                return true;

            Product other = obj as Product;

            return URL == other.URL && Name == other.Name && Size == other.Size && Profile == other.Profile && CProxy == other.CProxy;
        }

    }
}
