namespace OrderService.NDSSlab
{
    public partial class ProductCode : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {

        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        private int ProductCodeIdField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ProductCodeNameField;

        private int StructureElementTypeIdField;

        private int ProductTypeIdField;

        private int MainWireDiaField;

        private int MainWireSpacingField;

        private int CrossWireSpacingField;

        private double WeightAreaField;

        private double WeightPerMeterRunField;

        private int MinLinkFactorField;

        private int MaxLinkFactorField;

        private double CwDiaField;

        private double CwWeightPerMeterRunField;

        private double DECMWLengthField;

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

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string ProductCodeName
        {
            get
            {
                return this.ProductCodeNameField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ProductCodeNameField, value) != true))
                {
                    this.ProductCodeNameField = value;
                    this.RaisePropertyChanged("ProductCodeName");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true)]
        public int StructureElementTypeId
        {
            get
            {
                return this.StructureElementTypeIdField;
            }
            set
            {
                if ((this.StructureElementTypeIdField.Equals(value) != true))
                {
                    this.StructureElementTypeIdField = value;
                    this.RaisePropertyChanged("StructureElementTypeId");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 3)]
        public int ProductTypeId
        {
            get
            {
                return this.ProductTypeIdField;
            }
            set
            {
                if ((this.ProductTypeIdField.Equals(value) != true))
                {
                    this.ProductTypeIdField = value;
                    this.RaisePropertyChanged("ProductTypeId");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 4)]
        public int MainWireDia
        {
            get
            {
                return this.MainWireDiaField;
            }
            set
            {
                if ((this.MainWireDiaField.Equals(value) != true))
                {
                    this.MainWireDiaField = value;
                    this.RaisePropertyChanged("MainWireDia");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 5)]
        public int MainWireSpacing
        {
            get
            {
                return this.MainWireSpacingField;
            }
            set
            {
                if ((this.MainWireSpacingField.Equals(value) != true))
                {
                    this.MainWireSpacingField = value;
                    this.RaisePropertyChanged("MainWireSpacing");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 6)]
        public int CrossWireSpacing
        {
            get
            {
                return this.CrossWireSpacingField;
            }
            set
            {
                if ((this.CrossWireSpacingField.Equals(value) != true))
                {
                    this.CrossWireSpacingField = value;
                    this.RaisePropertyChanged("CrossWireSpacing");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 7)]
        public double WeightArea
        {
            get
            {
                return this.WeightAreaField;
            }
            set
            {
                if ((this.WeightAreaField.Equals(value) != true))
                {
                    this.WeightAreaField = value;
                    this.RaisePropertyChanged("WeightArea");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 8)]
        public double WeightPerMeterRun
        {
            get
            {
                return this.WeightPerMeterRunField;
            }
            set
            {
                if ((this.WeightPerMeterRunField.Equals(value) != true))
                {
                    this.WeightPerMeterRunField = value;
                    this.RaisePropertyChanged("WeightPerMeterRun");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 9)]
        public int MinLinkFactor
        {
            get
            {
                return this.MinLinkFactorField;
            }
            set
            {
                if ((this.MinLinkFactorField.Equals(value) != true))
                {
                    this.MinLinkFactorField = value;
                    this.RaisePropertyChanged("MinLinkFactor");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 10)]
        public int MaxLinkFactor
        {
            get
            {
                return this.MaxLinkFactorField;
            }
            set
            {
                if ((this.MaxLinkFactorField.Equals(value) != true))
                {
                    this.MaxLinkFactorField = value;
                    this.RaisePropertyChanged("MaxLinkFactor");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 11)]
        public double CwDia
        {
            get
            {
                return this.CwDiaField;
            }
            set
            {
                if ((this.CwDiaField.Equals(value) != true))
                {
                    this.CwDiaField = value;
                    this.RaisePropertyChanged("CwDia");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 12)]
        public double CwWeightPerMeterRun
        {
            get
            {
                return this.CwWeightPerMeterRunField;
            }
            set
            {
                if ((this.CwWeightPerMeterRunField.Equals(value) != true))
                {
                    this.CwWeightPerMeterRunField = value;
                    this.RaisePropertyChanged("CwWeightPerMeterRun");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 13)]
        public double DECMWLength
        {
            get
            {
                return this.DECMWLengthField;
            }
            set
            {
                if ((this.DECMWLengthField.Equals(value) != true))
                {
                    this.DECMWLengthField = value;
                    this.RaisePropertyChanged("DECMWLength");
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
