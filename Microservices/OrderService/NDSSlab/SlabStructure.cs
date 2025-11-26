namespace OrderService.NDSSlab
{
    public partial class SlabStructure : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {

        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        private int SEDetailingIDField;

        private int StructureMarkIdField;

        private int ParentStructureMarkIdField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string StructureMarkingNameField;

        private int ParamSetNumberField;

        private int MainWireLengthField;

        private int CrossWireLengthField;

        private int MemberQtyField;

        private bool BendingCheckField;

        private bool MachineCheckField;

        private bool TransportCheckField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private ProductCode ProductCodeField;

        private bool MultiMeshField;

        private bool ProduceIndicatorField;

        private int PinSizeField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private List<SlabProduct> SlabProductField;

        private bool ProductGenerationStatusField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private ShapeCodeParameterSet ParameterSetField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SideForCodeField;

        private bool ProductSplitUpField;

        public ShapeCode Shapecode;

        public int CO1;

        public int CO2;

        public int MO1;

        public int MO2;

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
        public int SEDetailingID
        {
            get
            {
                return this.SEDetailingIDField;
            }
            set
            {
                if ((this.SEDetailingIDField.Equals(value) != true))
                {
                    this.SEDetailingIDField = value;
                    this.RaisePropertyChanged("SEDetailingID");
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 2)]
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

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 3)]
        public string StructureMarkingName
        {
            get
            {
                return this.StructureMarkingNameField;
            }
            set
            {
                if ((object.ReferenceEquals(this.StructureMarkingNameField, value) != true))
                {
                    this.StructureMarkingNameField = value;
                    this.RaisePropertyChanged("StructureMarkingName");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 4)]
        public int ParamSetNumber
        {
            get
            {
                return this.ParamSetNumberField;
            }
            set
            {
                if ((this.ParamSetNumberField.Equals(value) != true))
                {
                    this.ParamSetNumberField = value;
                    this.RaisePropertyChanged("ParamSetNumber");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 5)]
        public int MainWireLength
        {
            get
            {
                return this.MainWireLengthField;
            }
            set
            {
                if ((this.MainWireLengthField.Equals(value) != true))
                {
                    this.MainWireLengthField = value;
                    this.RaisePropertyChanged("MainWireLength");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 6)]
        public int CrossWireLength
        {
            get
            {
                return this.CrossWireLengthField;
            }
            set
            {
                if ((this.CrossWireLengthField.Equals(value) != true))
                {
                    this.CrossWireLengthField = value;
                    this.RaisePropertyChanged("CrossWireLength");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 7)]
        public int MemberQty
        {
            get
            {
                return this.MemberQtyField;
            }
            set
            {
                if ((this.MemberQtyField.Equals(value) != true))
                {
                    this.MemberQtyField = value;
                    this.RaisePropertyChanged("MemberQty");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 8)]
        public bool BendingCheck
        {
            get
            {
                return this.BendingCheckField;
            }
            set
            {
                if ((this.BendingCheckField.Equals(value) != true))
                {
                    this.BendingCheckField = value;
                    this.RaisePropertyChanged("BendingCheck");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 9)]
        public bool MachineCheck
        {
            get
            {
                return this.MachineCheckField;
            }
            set
            {
                if ((this.MachineCheckField.Equals(value) != true))
                {
                    this.MachineCheckField = value;
                    this.RaisePropertyChanged("MachineCheck");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 10)]
        public bool TransportCheck
        {
            get
            {
                return this.TransportCheckField;
            }
            set
            {
                if ((this.TransportCheckField.Equals(value) != true))
                {
                    this.TransportCheckField = value;
                    this.RaisePropertyChanged("TransportCheck");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 11)]
        public ProductCode ProductCode
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 12)]
        public bool MultiMesh
        {
            get
            {
                return this.MultiMeshField;
            }
            set
            {
                if ((this.MultiMeshField.Equals(value) != true))
                {
                    this.MultiMeshField = value;
                    this.RaisePropertyChanged("MultiMesh");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 13)]
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 14)]
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

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 15)]
        public List<NDSSlab.SlabProduct> SlabProduct
        {
            get
            {
                return this.SlabProductField;
            }
            set
            {
                if ((object.ReferenceEquals(this.SlabProductField, value) != true))
                {
                    this.SlabProductField = value;
                    this.RaisePropertyChanged("SlabProduct");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 16)]
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

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 17)]
        public NDSSlab.ShapeCodeParameterSet ParameterSet
        {
            get
            {
                return this.ParameterSetField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ParameterSetField, value) != true))
                {
                    this.ParameterSetField = value;
                    this.RaisePropertyChanged("ParameterSet");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 18)]
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 19)]
        public bool ProductSplitUp
        {
            get
            {
                return this.ProductSplitUpField;
            }
            set
            {
                if ((this.ProductSplitUpField.Equals(value) != true))
                {
                    this.ProductSplitUpField = value;
                    this.RaisePropertyChanged("ProductSplitUp");
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
