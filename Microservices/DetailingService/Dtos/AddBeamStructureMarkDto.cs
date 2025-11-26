using DetailingService.Repositories;

namespace DetailingService.Dtos
{
    public class AddBeamStructureMarkDto
    {
        public int _structureMarkId { get; set; }
        public string _structureMarkName { get; set; }
        public ProductCode _productCode { get; set; }
        public ShapeCode _shape { get; set; }
        public int _width { get; set; }
        public int _depth { get; set; }
        public int _slope { get; set; }
        public int _stirupps { get; set; }
        public int _qty { get; set; }
        public int _span { get; set; }
        public bool _iscap { get; set; }
        public ProductCode _capProduct { get; set; }
        public bool _produceInd { get; set; }
        public int _pinSize { get; set; }
        public int _parentStructureMarkId { get; set; }

      //  _structureMarkId:0,
      //_structureMarkName: this.pushElement.StructureMarkName,
      //_productCode: this.selectedproductCode,
      //_shape: this.selectedShape,
      //_depth: this.pushElement.Depth,
      //_width: this.pushElement.Width,
      //_slope: this.pushElement.Slope,
      //_stirupps: this.pushElement.Stirupps,    
      //_qty: this.pushElement.Qty,
      //_span: this.pushElement.Span,
      //_iscap: this.pushElement.IsCap,
      //_capProduct: this.selectedCapProduct,
      //_produceInd: this.pushElement.ProduceInd,
      //_pinSize: this.pushElement.PinSize ,     
      //_parentStructureMarkId: 0,
    }
}
