namespace OrderService.NDSBeam
{
    public partial class BeamStructure : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {

        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string _sideForCodeField;

        private bool _bendCheckStatusField;

        private bool _bendingCheckIndField;

        private int ParentStructureMarkIdField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string StructureMarkNameField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private NDSBeam.ProductCode ProductCodeField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private NDSBeam.ShapeCode ShapeField;

        private int WidthField;

        private int DepthField;

        private int SlopeField;

        private int StiruppsField;

        private int QtyField;

        private int SpanField;

        private bool IsCapField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private NDSBeam.ProductCode CapProductField;

        private bool ProduceIndField;

        private int PinSizeField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private NDSBeam.BeamProduct[] ProductMarkField;

        private int StructureMarkIdField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SideForCodeField;

        private bool ProductGenerationStatusField;

        private bool BendingCheckStatusField;

        private bool BendingCheckIndField;

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

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string _sideForCode
        {
            get
            {
                return this._sideForCodeField;
            }
            set
            {
                if ((object.ReferenceEquals(this._sideForCodeField, value) != true))
                {
                    this._sideForCodeField = value;
                    this.RaisePropertyChanged("_sideForCode");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 1)]
        public bool _bendCheckStatus
        {
            get
            {
                return this._bendCheckStatusField;
            }
            set
            {
                if ((this._bendCheckStatusField.Equals(value) != true))
                {
                    this._bendCheckStatusField = value;
                    this.RaisePropertyChanged("_bendCheckStatus");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 2)]
        public bool _bendingCheckInd
        {
            get
            {
                return this._bendingCheckIndField;
            }
            set
            {
                if ((this._bendingCheckIndField.Equals(value) != true))
                {
                    this._bendingCheckIndField = value;
                    this.RaisePropertyChanged("_bendingCheckInd");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 3)]
        public int ParentStructureMarkId
        {
            get
            {
                return this.ParentStructureMarkIdField;
            }
            set
            {
                if ((this.ParentStructureMarkIdField.Equals(value) != true))
                {
                    this.ParentStructureMarkIdField = value;
                    this.RaisePropertyChanged("ParentStructureMarkId");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 4)]
        public string StructureMarkName
        {
            get
            {
                return this.StructureMarkNameField;
            }
            set
            {
                if ((object.ReferenceEquals(this.StructureMarkNameField, value) != true))
                {
                    this.StructureMarkNameField = value;
                    this.RaisePropertyChanged("StructureMarkName");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 5)]
        public NDSBeam.ProductCode ProductCode
        {
            get
            {
                return this.ProductCodeField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ProductCodeField, value) != true))
                {
                    this.ProductCodeField = value;
                    this.RaisePropertyChanged("ProductCode");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 6)]
        public NDSBeam.ShapeCode Shape
        {
            get
            {
                return this.ShapeField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ShapeField, value) != true))
                {
                    this.ShapeField = value;
                    this.RaisePropertyChanged("Shape");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 7)]
        public int Width
        {
            get
            {
                return this.WidthField;
            }
            set
            {
                if ((this.WidthField.Equals(value) != true))
                {
                    this.WidthField = value;
                    this.RaisePropertyChanged("Width");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 8)]
        public int Depth
        {
            get
            {
                return this.DepthField;
            }
            set
            {
                if ((this.DepthField.Equals(value) != true))
                {
                    this.DepthField = value;
                    this.RaisePropertyChanged("Depth");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 9)]
        public int Slope
        {
            get
            {
                return this.SlopeField;
            }
            set
            {
                if ((this.SlopeField.Equals(value) != true))
                {
                    this.SlopeField = value;
                    this.RaisePropertyChanged("Slope");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 10)]
        public int Stirupps
        {
            get
            {
                return this.StiruppsField;
            }
            set
            {
                if ((this.StiruppsField.Equals(value) != true))
                {
                    this.StiruppsField = value;
                    this.RaisePropertyChanged("Stirupps");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 11)]
        public int Qty
        {
            get
            {
                return this.QtyField;
            }
            set
            {
                if ((this.QtyField.Equals(value) != true))
                {
                    this.QtyField = value;
                    this.RaisePropertyChanged("Qty");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 12)]
        public int Span
        {
            get
            {
                return this.SpanField;
            }
            set
            {
                if ((this.SpanField.Equals(value) != true))
                {
                    this.SpanField = value;
                    this.RaisePropertyChanged("Span");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 13)]
        public bool IsCap
        {
            get
            {
                return this.IsCapField;
            }
            set
            {
                if ((this.IsCapField.Equals(value) != true))
                {
                    this.IsCapField = value;
                    this.RaisePropertyChanged("IsCap");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 14)]
        public NDSBeam.ProductCode CapProduct
        {
            get
            {
                return this.CapProductField;
            }
            set
            {
                if ((object.ReferenceEquals(this.CapProductField, value) != true))
                {
                    this.CapProductField = value;
                    this.RaisePropertyChanged("CapProduct");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 15)]
        public bool ProduceInd
        {
            get
            {
                return this.ProduceIndField;
            }
            set
            {
                if ((this.ProduceIndField.Equals(value) != true))
                {
                    this.ProduceIndField = value;
                    this.RaisePropertyChanged("ProduceInd");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 16)]
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

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 17)]
        public NDSBeam.BeamProduct[] ProductMark
        {
            get
            {
                return this.ProductMarkField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ProductMarkField, value) != true))
                {
                    this.ProductMarkField = value;
                    this.RaisePropertyChanged("ProductMark");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 18)]
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

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 19)]
        public string SideForCode
        {
            get
            {
                return this.SideForCodeField;
            }
            set
            {
                if ((object.ReferenceEquals(this.SideForCodeField, value) != true))
                {
                    this.SideForCodeField = value;
                    this.RaisePropertyChanged("SideForCode");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 20)]
        public bool ProductGenerationStatus
        {
            get
            {
                return this.ProductGenerationStatusField;
            }
            set
            {
                if ((this.ProductGenerationStatusField.Equals(value) != true))
                {
                    this.ProductGenerationStatusField = value;
                    this.RaisePropertyChanged("ProductGenerationStatus");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 21)]
        public bool BendingCheckStatus
        {
            get
            {
                return this.BendingCheckStatusField;
            }
            set
            {
                if ((this.BendingCheckStatusField.Equals(value) != true))
                {
                    this.BendingCheckStatusField = value;
                    this.RaisePropertyChanged("BendingCheckStatus");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 22)]
        public bool BendingCheckInd
        {
            get
            {
                return this.BendingCheckIndField;
            }
            set
            {
                if ((this.BendingCheckIndField.Equals(value) != true))
                {
                    this.BendingCheckIndField = value;
                    this.RaisePropertyChanged("BendingCheckInd");
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
