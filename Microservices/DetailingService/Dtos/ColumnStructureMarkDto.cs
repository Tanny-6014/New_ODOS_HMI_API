using DetailingService.Repositories;
using System.Runtime.InteropServices;

namespace DetailingService.Dtos
{
    public class ColumnStructureMarkDto
    {
        public int INTSTRUCTUREMARKID { get; set; }
        public string VCHSTRUCTUREMARKINGNAME { get; set; }
        public int INTMEMBERQTY { get; set; }
        public int NUMCOLUMNWIDTH { get; set; }
        public int NUMCOLUMNLENGTH { get; set; }
        public int NUMCOLUMNHEIGHT { get; set; }
        public int INTNOOFLINKS { get; set; }

        public int INTCOLUMNSHAPEID { get; set; }
        public int INTROWSATLENGTH { get; set; }
        public int INTROWSATWIDTH { get; set; }
        public int INTCLINKPRODUCTCODEIDATLENGTH { get; set; }
        public int INTCLINKPRODUCTCODEIDATWIDTH { get; set; }
        public string CLONLY { get; set; }
        public int INTCOLUMNPRODUCTCODEID { get; set; }
        public int PINSIZE { get; set; }
        public string ISCLINK { get; set; }
        public string COLUMNPRODUCT { get; set; }
        public string COLUMNSHAPE { get; set; }
        public string CLINKPRODUCTLEN { get; set; }
        public string CLINKPRODUCTWIDTH { get; set; }
        public string CLINKSHAPEWIDTH { get; set; }
        public string PRODUCEINDICATOR { get; set; }
        public int SIDEFORCODE { get; set; }
        public int BITBENDINGCHECK { get; set; }


 }
}
