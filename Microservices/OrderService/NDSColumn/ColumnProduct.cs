namespace OrderService.NDSColumn
{
    public partial class ColumnProduct : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {

        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        private int ProductMarkIdField;

        private int StructureMarkIdField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ProductMarkingNameField;

        private int ProductCodeIdField;

        private int LinkWidthField;

        private int LinkLengthField;

        private int LinkQtyField;

        private int InvoiceMWQtyField;

        private int InvoiceCWQtyField;

        private double InvoiceMWWeightField;

        private double InvoiceCWWeightField;

        private double InvoiceWeightField;

        private double InvoiceMWLengthField;

        private double InvoiceCWLengthField;

        private int ShapeCodeIdField;

        private int NoofLinksField;

        private int TotalLinksField;

        private int MO1Field;

        private int MO2Field;

        private int CO1Field;

        private int CO2Field;

        private int ProductionMO1Field;

        private int ProductionMO2Field;

        private int ProductionCO1Field;

        private int ProductionCO2Field;

        private double InvoiceAreaField;

        private double ProductionAreaField;

        private double TheoraticalWeightField;

        private int ProductionMWLengthField;

        private int ProductionCWLengthField;

        private int ProductionMWQtyField;

        private int ProductionCWQtyField;

        private double ProductionMWWeightField;

        private double ProductionCWWeightField;

        private double ProductionWeightField;

        private int PinSizeField;

        private int MWSpacingField;

        private int CWSpacingField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ParamValuesField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string BOMDrawingPathField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MWBVBSStringField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CWBVBSStringField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private NDSColumn.ShapeParameter[] ShapeParamField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ShapeCodeField;

        private int QuantityField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string BOMIndicatorField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CWSpacingStringField;

        private bool ProduceIndicatorField;

        private bool BendCheckField;

        private int GenerationStatusField;

        private int ProductValidatorField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MWPitchField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CWPitchField;

        private int MWFlagField;

        private int CWFlagField;

        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true)]
        public int ProductMarkId
        {
            get
            {
                return this.ProductMarkIdField;
            }
            set
            {
                if ((this.ProductMarkIdField.Equals(value) != true))
                {
                    this.ProductMarkIdField = value;
                    this.RaisePropertyChanged("ProductMarkId");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true)]
        public int StructureMarkId
        {
            get
            {
                return this.StructureMarkIdField;
            }
            set
            {
                if ((this.StructureMarkIdField.Equals(value) != true))
                {
                    this.StructureMarkIdField = value;
                    this.RaisePropertyChanged("StructureMarkId");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
        public string ProductMarkingName
        {
            get
            {
                return this.ProductMarkingNameField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ProductMarkingNameField, value) != true))
                {
                    this.ProductMarkingNameField = value;
                    this.RaisePropertyChanged("ProductMarkingName");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 3)]
        public int ProductCodeId
        {
            get
            {
                return this.ProductCodeIdField;
            }
            set
            {
                if ((this.ProductCodeIdField.Equals(value) != true))
                {
                    this.ProductCodeIdField = value;
                    this.RaisePropertyChanged("ProductCodeId");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 4)]
        public int LinkWidth
        {
            get
            {
                return this.LinkWidthField;
            }
            set
            {
                if ((this.LinkWidthField.Equals(value) != true))
                {
                    this.LinkWidthField = value;
                    this.RaisePropertyChanged("LinkWidth");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 5)]
        public int LinkLength
        {
            get
            {
                return this.LinkLengthField;
            }
            set
            {
                if ((this.LinkLengthField.Equals(value) != true))
                {
                    this.LinkLengthField = value;
                    this.RaisePropertyChanged("LinkLength");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 6)]
        public int LinkQty
        {
            get
            {
                return this.LinkQtyField;
            }
            set
            {
                if ((this.LinkQtyField.Equals(value) != true))
                {
                    this.LinkQtyField = value;
                    this.RaisePropertyChanged("LinkQty");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 7)]
        public int InvoiceMWQty
        {
            get
            {
                return this.InvoiceMWQtyField;
            }
            set
            {
                if ((this.InvoiceMWQtyField.Equals(value) != true))
                {
                    this.InvoiceMWQtyField = value;
                    this.RaisePropertyChanged("InvoiceMWQty");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 8)]
        public int InvoiceCWQty
        {
            get
            {
                return this.InvoiceCWQtyField;
            }
            set
            {
                if ((this.InvoiceCWQtyField.Equals(value) != true))
                {
                    this.InvoiceCWQtyField = value;
                    this.RaisePropertyChanged("InvoiceCWQty");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 9)]
        public double InvoiceMWWeight
        {
            get
            {
                return this.InvoiceMWWeightField;
            }
            set
            {
                if ((this.InvoiceMWWeightField.Equals(value) != true))
                {
                    this.InvoiceMWWeightField = value;
                    this.RaisePropertyChanged("InvoiceMWWeight");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 10)]
        public double InvoiceCWWeight
        {
            get
            {
                return this.InvoiceCWWeightField;
            }
            set
            {
                if ((this.InvoiceCWWeightField.Equals(value) != true))
                {
                    this.InvoiceCWWeightField = value;
                    this.RaisePropertyChanged("InvoiceCWWeight");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 11)]
        public double InvoiceWeight
        {
            get
            {
                return this.InvoiceWeightField;
            }
            set
            {
                if ((this.InvoiceWeightField.Equals(value) != true))
                {
                    this.InvoiceWeightField = value;
                    this.RaisePropertyChanged("InvoiceWeight");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 12)]
        public double InvoiceMWLength
        {
            get
            {
                return this.InvoiceMWLengthField;
            }
            set
            {
                if ((this.InvoiceMWLengthField.Equals(value) != true))
                {
                    this.InvoiceMWLengthField = value;
                    this.RaisePropertyChanged("InvoiceMWLength");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 13)]
        public double InvoiceCWLength
        {
            get
            {
                return this.InvoiceCWLengthField;
            }
            set
            {
                if ((this.InvoiceCWLengthField.Equals(value) != true))
                {
                    this.InvoiceCWLengthField = value;
                    this.RaisePropertyChanged("InvoiceCWLength");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 14)]
        public int ShapeCodeId
        {
            get
            {
                return this.ShapeCodeIdField;
            }
            set
            {
                if ((this.ShapeCodeIdField.Equals(value) != true))
                {
                    this.ShapeCodeIdField = value;
                    this.RaisePropertyChanged("ShapeCodeId");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 15)]
        public int NoofLinks
        {
            get
            {
                return this.NoofLinksField;
            }
            set
            {
                if ((this.NoofLinksField.Equals(value) != true))
                {
                    this.NoofLinksField = value;
                    this.RaisePropertyChanged("NoofLinks");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 16)]
        public int TotalLinks
        {
            get
            {
                return this.TotalLinksField;
            }
            set
            {
                if ((this.TotalLinksField.Equals(value) != true))
                {
                    this.TotalLinksField = value;
                    this.RaisePropertyChanged("TotalLinks");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 17)]
        public int MO1
        {
            get
            {
                return this.MO1Field;
            }
            set
            {
                if ((this.MO1Field.Equals(value) != true))
                {
                    this.MO1Field = value;
                    this.RaisePropertyChanged("MO1");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 18)]
        public int MO2
        {
            get
            {
                return this.MO2Field;
            }
            set
            {
                if ((this.MO2Field.Equals(value) != true))
                {
                    this.MO2Field = value;
                    this.RaisePropertyChanged("MO2");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 19)]
        public int CO1
        {
            get
            {
                return this.CO1Field;
            }
            set
            {
                if ((this.CO1Field.Equals(value) != true))
                {
                    this.CO1Field = value;
                    this.RaisePropertyChanged("CO1");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 20)]
        public int CO2
        {
            get
            {
                return this.CO2Field;
            }
            set
            {
                if ((this.CO2Field.Equals(value) != true))
                {
                    this.CO2Field = value;
                    this.RaisePropertyChanged("CO2");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 21)]
        public int ProductionMO1
        {
            get
            {
                return this.ProductionMO1Field;
            }
            set
            {
                if ((this.ProductionMO1Field.Equals(value) != true))
                {
                    this.ProductionMO1Field = value;
                    this.RaisePropertyChanged("ProductionMO1");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 22)]
        public int ProductionMO2
        {
            get
            {
                return this.ProductionMO2Field;
            }
            set
            {
                if ((this.ProductionMO2Field.Equals(value) != true))
                {
                    this.ProductionMO2Field = value;
                    this.RaisePropertyChanged("ProductionMO2");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 23)]
        public int ProductionCO1
        {
            get
            {
                return this.ProductionCO1Field;
            }
            set
            {
                if ((this.ProductionCO1Field.Equals(value) != true))
                {
                    this.ProductionCO1Field = value;
                    this.RaisePropertyChanged("ProductionCO1");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 24)]
        public int ProductionCO2
        {
            get
            {
                return this.ProductionCO2Field;
            }
            set
            {
                if ((this.ProductionCO2Field.Equals(value) != true))
                {
                    this.ProductionCO2Field = value;
                    this.RaisePropertyChanged("ProductionCO2");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 25)]
        public double InvoiceArea
        {
            get
            {
                return this.InvoiceAreaField;
            }
            set
            {
                if ((this.InvoiceAreaField.Equals(value) != true))
                {
                    this.InvoiceAreaField = value;
                    this.RaisePropertyChanged("InvoiceArea");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 26)]
        public double ProductionArea
        {
            get
            {
                return this.ProductionAreaField;
            }
            set
            {
                if ((this.ProductionAreaField.Equals(value) != true))
                {
                    this.ProductionAreaField = value;
                    this.RaisePropertyChanged("ProductionArea");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 27)]
        public double TheoraticalWeight
        {
            get
            {
                return this.TheoraticalWeightField;
            }
            set
            {
                if ((this.TheoraticalWeightField.Equals(value) != true))
                {
                    this.TheoraticalWeightField = value;
                    this.RaisePropertyChanged("TheoraticalWeight");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 28)]
        public int ProductionMWLength
        {
            get
            {
                return this.ProductionMWLengthField;
            }
            set
            {
                if ((this.ProductionMWLengthField.Equals(value) != true))
                {
                    this.ProductionMWLengthField = value;
                    this.RaisePropertyChanged("ProductionMWLength");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 29)]
        public int ProductionCWLength
        {
            get
            {
                return this.ProductionCWLengthField;
            }
            set
            {
                if ((this.ProductionCWLengthField.Equals(value) != true))
                {
                    this.ProductionCWLengthField = value;
                    this.RaisePropertyChanged("ProductionCWLength");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 30)]
        public int ProductionMWQty
        {
            get
            {
                return this.ProductionMWQtyField;
            }
            set
            {
                if ((this.ProductionMWQtyField.Equals(value) != true))
                {
                    this.ProductionMWQtyField = value;
                    this.RaisePropertyChanged("ProductionMWQty");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 31)]
        public int ProductionCWQty
        {
            get
            {
                return this.ProductionCWQtyField;
            }
            set
            {
                if ((this.ProductionCWQtyField.Equals(value) != true))
                {
                    this.ProductionCWQtyField = value;
                    this.RaisePropertyChanged("ProductionCWQty");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 32)]
        public double ProductionMWWeight
        {
            get
            {
                return this.ProductionMWWeightField;
            }
            set
            {
                if ((this.ProductionMWWeightField.Equals(value) != true))
                {
                    this.ProductionMWWeightField = value;
                    this.RaisePropertyChanged("ProductionMWWeight");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 33)]
        public double ProductionCWWeight
        {
            get
            {
                return this.ProductionCWWeightField;
            }
            set
            {
                if ((this.ProductionCWWeightField.Equals(value) != true))
                {
                    this.ProductionCWWeightField = value;
                    this.RaisePropertyChanged("ProductionCWWeight");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 34)]
        public double ProductionWeight
        {
            get
            {
                return this.ProductionWeightField;
            }
            set
            {
                if ((this.ProductionWeightField.Equals(value) != true))
                {
                    this.ProductionWeightField = value;
                    this.RaisePropertyChanged("ProductionWeight");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 35)]
        public int PinSize
        {
            get
            {
                return this.PinSizeField;
            }
            set
            {
                if ((this.PinSizeField.Equals(value) != true))
                {
                    this.PinSizeField = value;
                    this.RaisePropertyChanged("PinSize");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 36)]
        public int MWSpacing
        {
            get
            {
                return this.MWSpacingField;
            }
            set
            {
                if ((this.MWSpacingField.Equals(value) != true))
                {
                    this.MWSpacingField = value;
                    this.RaisePropertyChanged("MWSpacing");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 37)]
        public int CWSpacing
        {
            get
            {
                return this.CWSpacingField;
            }
            set
            {
                if ((this.CWSpacingField.Equals(value) != true))
                {
                    this.CWSpacingField = value;
                    this.RaisePropertyChanged("CWSpacing");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 38)]
        public string ParamValues
        {
            get
            {
                return this.ParamValuesField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ParamValuesField, value) != true))
                {
                    this.ParamValuesField = value;
                    this.RaisePropertyChanged("ParamValues");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 39)]
        public string BOMDrawingPath
        {
            get
            {
                return this.BOMDrawingPathField;
            }
            set
            {
                if ((object.ReferenceEquals(this.BOMDrawingPathField, value) != true))
                {
                    this.BOMDrawingPathField = value;
                    this.RaisePropertyChanged("BOMDrawingPath");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 40)]
        public string MWBVBSString
        {
            get
            {
                return this.MWBVBSStringField;
            }
            set
            {
                if ((object.ReferenceEquals(this.MWBVBSStringField, value) != true))
                {
                    this.MWBVBSStringField = value;
                    this.RaisePropertyChanged("MWBVBSString");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 41)]
        public string CWBVBSString
        {
            get
            {
                return this.CWBVBSStringField;
            }
            set
            {
                if ((object.ReferenceEquals(this.CWBVBSStringField, value) != true))
                {
                    this.CWBVBSStringField = value;
                    this.RaisePropertyChanged("CWBVBSString");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 42)]
        public NDSColumn.ShapeParameter[] ShapeParam
        {
            get
            {
                return this.ShapeParamField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ShapeParamField, value) != true))
                {
                    this.ShapeParamField = value;
                    this.RaisePropertyChanged("ShapeParam");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 43)]
        public string ShapeCode
        {
            get
            {
                return this.ShapeCodeField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ShapeCodeField, value) != true))
                {
                    this.ShapeCodeField = value;
                    this.RaisePropertyChanged("ShapeCode");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 44)]
        public int Quantity
        {
            get
            {
                return this.QuantityField;
            }
            set
            {
                if ((this.QuantityField.Equals(value) != true))
                {
                    this.QuantityField = value;
                    this.RaisePropertyChanged("Quantity");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 45)]
        public string BOMIndicator
        {
            get
            {
                return this.BOMIndicatorField;
            }
            set
            {
                if ((object.ReferenceEquals(this.BOMIndicatorField, value) != true))
                {
                    this.BOMIndicatorField = value;
                    this.RaisePropertyChanged("BOMIndicator");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 46)]
        public string CWSpacingString
        {
            get
            {
                return this.CWSpacingStringField;
            }
            set
            {
                if ((object.ReferenceEquals(this.CWSpacingStringField, value) != true))
                {
                    this.CWSpacingStringField = value;
                    this.RaisePropertyChanged("CWSpacingString");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 47)]
        public bool ProduceIndicator
        {
            get
            {
                return this.ProduceIndicatorField;
            }
            set
            {
                if ((this.ProduceIndicatorField.Equals(value) != true))
                {
                    this.ProduceIndicatorField = value;
                    this.RaisePropertyChanged("ProduceIndicator");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 48)]
        public bool BendCheck
        {
            get
            {
                return this.BendCheckField;
            }
            set
            {
                if ((this.BendCheckField.Equals(value) != true))
                {
                    this.BendCheckField = value;
                    this.RaisePropertyChanged("BendCheck");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 49)]
        public int GenerationStatus
        {
            get
            {
                return this.GenerationStatusField;
            }
            set
            {
                if ((this.GenerationStatusField.Equals(value) != true))
                {
                    this.GenerationStatusField = value;
                    this.RaisePropertyChanged("GenerationStatus");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 50)]
        public int ProductValidator
        {
            get
            {
                return this.ProductValidatorField;
            }
            set
            {
                if ((this.ProductValidatorField.Equals(value) != true))
                {
                    this.ProductValidatorField = value;
                    this.RaisePropertyChanged("ProductValidator");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 51)]
        public string MWPitch
        {
            get
            {
                return this.MWPitchField;
            }
            set
            {
                if ((object.ReferenceEquals(this.MWPitchField, value) != true))
                {
                    this.MWPitchField = value;
                    this.RaisePropertyChanged("MWPitch");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 52)]
        public string CWPitch
        {
            get
            {
                return this.CWPitchField;
            }
            set
            {
                if ((object.ReferenceEquals(this.CWPitchField, value) != true))
                {
                    this.CWPitchField = value;
                    this.RaisePropertyChanged("CWPitch");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 53)]
        public int MWFlag
        {
            get
            {
                return this.MWFlagField;
            }
            set
            {
                if ((this.MWFlagField.Equals(value) != true))
                {
                    this.MWFlagField = value;
                    this.RaisePropertyChanged("MWFlag");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 54)]
        public int CWFlag
        {
            get
            {
                return this.CWFlagField;
            }
            set
            {
                if ((this.CWFlagField.Equals(value) != true))
                {
                    this.CWFlagField = value;
                    this.RaisePropertyChanged("CWFlag");
                }
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
