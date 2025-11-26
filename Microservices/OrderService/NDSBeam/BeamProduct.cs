namespace OrderService.NDSBeam
{
    public partial class BeamProduct : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {

        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        private int ProductMarkIDField;

        private int StructureMarkIDField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ProductMarkingNameField;

        private int BeamWidthField;

        private int BeamDepthField;

        private int BeamSlopeField;

        private int CageWidthField;

        private int CageDepthField;

        private int CageSlopeField;

        private int ProductCodeIdField;

        private int MWSpacingField;

        private int CWSpacingField;

        private int ShapeCodeIdField;

        private int QuantityField;

        private int InvoiceMWLengthField;

        private int InvoiceCWLengthField;

        private int InvoiceMWQtyField;

        private int InvoiceCWQtyField;

        private int InvoiceMWWeightField;

        private int InvoiceCWWeightField;

        private double InvoiceAreaField;

        private int MO1Field;

        private int MO2Field;

        private int CO1Field;

        private int CO2Field;

        private double InvoiceWeightField;

        private double TheoraticalWeightField;

        private int ProductionAreaField;

        private int ProductionMO1Field;

        private int ProductionMO2Field;

        private int ProductionCO1Field;

        private int ProductionCO2Field;

        private int ProductionMWLengthField;

        private int ProductionCWLengthField;

        private int ProductionMWWeightField;

        private int ProductionCWWeightField;

        private double ProductionWeightField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ShapeCodeField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ParametersField;

        private int NoofLinksField;

        private int PinSizeField;

        private int GenerationStatusField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ParamValuesField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string BOMDrawingPathField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MWBVBSStringField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CWBVBSStringField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string BOMIndicatorField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private NDSBeam.ShapeParameter[] ShapeParamField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MWPitchField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CWPitchField;

        private int MWFlagField;

        private int CWFlagField;

        private int ProductValidatorField;

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
        public int ProductMarkID
        {
            get
            {
                return this.ProductMarkIDField;
            }
            set
            {
                if ((this.ProductMarkIDField.Equals(value) != true))
                {
                    this.ProductMarkIDField = value;
                    this.RaisePropertyChanged("ProductMarkID");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true)]
        public int StructureMarkID
        {
            get
            {
                return this.StructureMarkIDField;
            }
            set
            {
                if ((this.StructureMarkIDField.Equals(value) != true))
                {
                    this.StructureMarkIDField = value;
                    this.RaisePropertyChanged("StructureMarkID");
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
        public int BeamWidth
        {
            get
            {
                return this.BeamWidthField;
            }
            set
            {
                if ((this.BeamWidthField.Equals(value) != true))
                {
                    this.BeamWidthField = value;
                    this.RaisePropertyChanged("BeamWidth");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 4)]
        public int BeamDepth
        {
            get
            {
                return this.BeamDepthField;
            }
            set
            {
                if ((this.BeamDepthField.Equals(value) != true))
                {
                    this.BeamDepthField = value;
                    this.RaisePropertyChanged("BeamDepth");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 5)]
        public int BeamSlope
        {
            get
            {
                return this.BeamSlopeField;
            }
            set
            {
                if ((this.BeamSlopeField.Equals(value) != true))
                {
                    this.BeamSlopeField = value;
                    this.RaisePropertyChanged("BeamSlope");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 6)]
        public int CageWidth
        {
            get
            {
                return this.CageWidthField;
            }
            set
            {
                if ((this.CageWidthField.Equals(value) != true))
                {
                    this.CageWidthField = value;
                    this.RaisePropertyChanged("CageWidth");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 7)]
        public int CageDepth
        {
            get
            {
                return this.CageDepthField;
            }
            set
            {
                if ((this.CageDepthField.Equals(value) != true))
                {
                    this.CageDepthField = value;
                    this.RaisePropertyChanged("CageDepth");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 8)]
        public int CageSlope
        {
            get
            {
                return this.CageSlopeField;
            }
            set
            {
                if ((this.CageSlopeField.Equals(value) != true))
                {
                    this.CageSlopeField = value;
                    this.RaisePropertyChanged("CageSlope");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 9)]
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 10)]
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 11)]
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 12)]
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 13)]
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 14)]
        public int InvoiceMWLength
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 15)]
        public int InvoiceCWLength
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 16)]
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 17)]
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 18)]
        public int InvoiceMWWeight
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 19)]
        public int InvoiceCWWeight
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 20)]
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 21)]
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 22)]
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 23)]
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 24)]
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 25)]
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 26)]
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 27)]
        public int ProductionArea
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 28)]
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 29)]
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 30)]
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 31)]
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 32)]
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 33)]
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 34)]
        public int ProductionMWWeight
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 35)]
        public int ProductionCWWeight
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 36)]
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

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 37)]
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

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 38)]
        public string Parameters
        {
            get
            {
                return this.ParametersField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ParametersField, value) != true))
                {
                    this.ParametersField = value;
                    this.RaisePropertyChanged("Parameters");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 39)]
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 40)]
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 41)]
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

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 42)]
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

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 43)]
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

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 44)]
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

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 45)]
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

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 46)]
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

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 47)]
        public NDSBeam.ShapeParameter[] ShapeParam
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

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 48)]
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

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 49)]
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 50)]
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 51)]
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 52)]
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
