namespace OrderService.NDSCAB
{
    public partial class ShapeParameter : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {

        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        private int ShapeIdField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ParameterNameField;

        private int ParameterValueField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CriticalIndiacatorField;

        private int SequenceNumberField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ShapeCodeImageField;

        private bool EditFlagField;

        private bool VisibleFlagField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string symmetricIndexField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string HeghtAngleFormulaField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string HeghtSuceedAngleFormulaField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ParameterField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ValueField;

        private bool IsVisibleField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string AngleTypeField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string WireTypeField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string OHDtlsField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PrintFlagField;

        private int PrintValueField;

        private int EvenMo1Field;

        private int EvenMo2Field;

        private int OddMo1Field;

        private int OddMo2Field;

        private int EvenCo1Field;

        private int EvenCo2Field;

        private int OddCo1Field;

        private int OddCo2Field;

        private bool OHIndicatorField;

        private int AngleDirField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string AngleField;

        private bool IsParamUsedField;

        private int IntXCordField;

        private int IntYCordField;

        private int IntZCordField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CustFormulaField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string OffsetAngleFormulaField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string OffsetSuceedAngleFormulaField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ParameterValueCabField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CouplerTypeField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ParameterViewField;

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
        public int ShapeId
        {
            get
            {
                return this.ShapeIdField;
            }
            set
            {
                if ((this.ShapeIdField.Equals(value) != true))
                {
                    this.ShapeIdField = value;
                    this.RaisePropertyChanged("ShapeId");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
        public string ParameterName
        {
            get
            {
                return this.ParameterNameField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ParameterNameField, value) != true))
                {
                    this.ParameterNameField = value;
                    this.RaisePropertyChanged("ParameterName");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 2)]
        public int ParameterValue
        {
            get
            {
                return this.ParameterValueField;
            }
            set
            {
                if ((this.ParameterValueField.Equals(value) != true))
                {
                    this.ParameterValueField = value;
                    this.RaisePropertyChanged("ParameterValue");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 3)]
        public string CriticalIndiacator
        {
            get
            {
                return this.CriticalIndiacatorField;
            }
            set
            {
                if ((object.ReferenceEquals(this.CriticalIndiacatorField, value) != true))
                {
                    this.CriticalIndiacatorField = value;
                    this.RaisePropertyChanged("CriticalIndiacator");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 4)]
        public int SequenceNumber
        {
            get
            {
                return this.SequenceNumberField;
            }
            set
            {
                if ((this.SequenceNumberField.Equals(value) != true))
                {
                    this.SequenceNumberField = value;
                    this.RaisePropertyChanged("SequenceNumber");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 5)]
        public string ShapeCodeImage
        {
            get
            {
                return this.ShapeCodeImageField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ShapeCodeImageField, value) != true))
                {
                    this.ShapeCodeImageField = value;
                    this.RaisePropertyChanged("ShapeCodeImage");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 6)]
        public bool EditFlag
        {
            get
            {
                return this.EditFlagField;
            }
            set
            {
                if ((this.EditFlagField.Equals(value) != true))
                {
                    this.EditFlagField = value;
                    this.RaisePropertyChanged("EditFlag");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 7)]
        public bool VisibleFlag
        {
            get
            {
                return this.VisibleFlagField;
            }
            set
            {
                if ((this.VisibleFlagField.Equals(value) != true))
                {
                    this.VisibleFlagField = value;
                    this.RaisePropertyChanged("VisibleFlag");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 8)]
        public string symmetricIndex
        {
            get
            {
                return this.symmetricIndexField;
            }
            set
            {
                if ((object.ReferenceEquals(this.symmetricIndexField, value) != true))
                {
                    this.symmetricIndexField = value;
                    this.RaisePropertyChanged("symmetricIndex");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 9)]
        public string HeghtAngleFormula
        {
            get
            {
                return this.HeghtAngleFormulaField;
            }
            set
            {
                if ((object.ReferenceEquals(this.HeghtAngleFormulaField, value) != true))
                {
                    this.HeghtAngleFormulaField = value;
                    this.RaisePropertyChanged("HeghtAngleFormula");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 10)]
        public string HeghtSuceedAngleFormula
        {
            get
            {
                return this.HeghtSuceedAngleFormulaField;
            }
            set
            {
                if ((object.ReferenceEquals(this.HeghtSuceedAngleFormulaField, value) != true))
                {
                    this.HeghtSuceedAngleFormulaField = value;
                    this.RaisePropertyChanged("HeghtSuceedAngleFormula");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 11)]
        public string Parameter
        {
            get
            {
                return this.ParameterField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ParameterField, value) != true))
                {
                    this.ParameterField = value;
                    this.RaisePropertyChanged("Parameter");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 12)]
        public string Value
        {
            get
            {
                return this.ValueField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ValueField, value) != true))
                {
                    this.ValueField = value;
                    this.RaisePropertyChanged("Value");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 13)]
        public bool IsVisible
        {
            get
            {
                return this.IsVisibleField;
            }
            set
            {
                if ((this.IsVisibleField.Equals(value) != true))
                {
                    this.IsVisibleField = value;
                    this.RaisePropertyChanged("IsVisible");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 14)]
        public string AngleType
        {
            get
            {
                return this.AngleTypeField;
            }
            set
            {
                if ((object.ReferenceEquals(this.AngleTypeField, value) != true))
                {
                    this.AngleTypeField = value;
                    this.RaisePropertyChanged("AngleType");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 15)]
        public string WireType
        {
            get
            {
                return this.WireTypeField;
            }
            set
            {
                if ((object.ReferenceEquals(this.WireTypeField, value) != true))
                {
                    this.WireTypeField = value;
                    this.RaisePropertyChanged("WireType");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 16)]
        public string OHDtls
        {
            get
            {
                return this.OHDtlsField;
            }
            set
            {
                if ((object.ReferenceEquals(this.OHDtlsField, value) != true))
                {
                    this.OHDtlsField = value;
                    this.RaisePropertyChanged("OHDtls");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 17)]
        public string PrintFlag
        {
            get
            {
                return this.PrintFlagField;
            }
            set
            {
                if ((object.ReferenceEquals(this.PrintFlagField, value) != true))
                {
                    this.PrintFlagField = value;
                    this.RaisePropertyChanged("PrintFlag");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 18)]
        public int PrintValue
        {
            get
            {
                return this.PrintValueField;
            }
            set
            {
                if ((this.PrintValueField.Equals(value) != true))
                {
                    this.PrintValueField = value;
                    this.RaisePropertyChanged("PrintValue");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 19)]
        public int EvenMo1
        {
            get
            {
                return this.EvenMo1Field;
            }
            set
            {
                if ((this.EvenMo1Field.Equals(value) != true))
                {
                    this.EvenMo1Field = value;
                    this.RaisePropertyChanged("EvenMo1");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 20)]
        public int EvenMo2
        {
            get
            {
                return this.EvenMo2Field;
            }
            set
            {
                if ((this.EvenMo2Field.Equals(value) != true))
                {
                    this.EvenMo2Field = value;
                    this.RaisePropertyChanged("EvenMo2");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 21)]
        public int OddMo1
        {
            get
            {
                return this.OddMo1Field;
            }
            set
            {
                if ((this.OddMo1Field.Equals(value) != true))
                {
                    this.OddMo1Field = value;
                    this.RaisePropertyChanged("OddMo1");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 22)]
        public int OddMo2
        {
            get
            {
                return this.OddMo2Field;
            }
            set
            {
                if ((this.OddMo2Field.Equals(value) != true))
                {
                    this.OddMo2Field = value;
                    this.RaisePropertyChanged("OddMo2");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 23)]
        public int EvenCo1
        {
            get
            {
                return this.EvenCo1Field;
            }
            set
            {
                if ((this.EvenCo1Field.Equals(value) != true))
                {
                    this.EvenCo1Field = value;
                    this.RaisePropertyChanged("EvenCo1");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 24)]
        public int EvenCo2
        {
            get
            {
                return this.EvenCo2Field;
            }
            set
            {
                if ((this.EvenCo2Field.Equals(value) != true))
                {
                    this.EvenCo2Field = value;
                    this.RaisePropertyChanged("EvenCo2");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 25)]
        public int OddCo1
        {
            get
            {
                return this.OddCo1Field;
            }
            set
            {
                if ((this.OddCo1Field.Equals(value) != true))
                {
                    this.OddCo1Field = value;
                    this.RaisePropertyChanged("OddCo1");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 26)]
        public int OddCo2
        {
            get
            {
                return this.OddCo2Field;
            }
            set
            {
                if ((this.OddCo2Field.Equals(value) != true))
                {
                    this.OddCo2Field = value;
                    this.RaisePropertyChanged("OddCo2");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 27)]
        public bool OHIndicator
        {
            get
            {
                return this.OHIndicatorField;
            }
            set
            {
                if ((this.OHIndicatorField.Equals(value) != true))
                {
                    this.OHIndicatorField = value;
                    this.RaisePropertyChanged("OHIndicator");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 28)]
        public int AngleDir
        {
            get
            {
                return this.AngleDirField;
            }
            set
            {
                if ((this.AngleDirField.Equals(value) != true))
                {
                    this.AngleDirField = value;
                    this.RaisePropertyChanged("AngleDir");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 29)]
        public string Angle
        {
            get
            {
                return this.AngleField;
            }
            set
            {
                if ((object.ReferenceEquals(this.AngleField, value) != true))
                {
                    this.AngleField = value;
                    this.RaisePropertyChanged("Angle");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 30)]
        public bool IsParamUsed
        {
            get
            {
                return this.IsParamUsedField;
            }
            set
            {
                if ((this.IsParamUsedField.Equals(value) != true))
                {
                    this.IsParamUsedField = value;
                    this.RaisePropertyChanged("IsParamUsed");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 31)]
        public int IntXCord
        {
            get
            {
                return this.IntXCordField;
            }
            set
            {
                if ((this.IntXCordField.Equals(value) != true))
                {
                    this.IntXCordField = value;
                    this.RaisePropertyChanged("IntXCord");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 32)]
        public int IntYCord
        {
            get
            {
                return this.IntYCordField;
            }
            set
            {
                if ((this.IntYCordField.Equals(value) != true))
                {
                    this.IntYCordField = value;
                    this.RaisePropertyChanged("IntYCord");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 33)]
        public int IntZCord
        {
            get
            {
                return this.IntZCordField;
            }
            set
            {
                if ((this.IntZCordField.Equals(value) != true))
                {
                    this.IntZCordField = value;
                    this.RaisePropertyChanged("IntZCord");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 34)]
        public string CustFormula
        {
            get
            {
                return this.CustFormulaField;
            }
            set
            {
                if ((object.ReferenceEquals(this.CustFormulaField, value) != true))
                {
                    this.CustFormulaField = value;
                    this.RaisePropertyChanged("CustFormula");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 35)]
        public string OffsetAngleFormula
        {
            get
            {
                return this.OffsetAngleFormulaField;
            }
            set
            {
                if ((object.ReferenceEquals(this.OffsetAngleFormulaField, value) != true))
                {
                    this.OffsetAngleFormulaField = value;
                    this.RaisePropertyChanged("OffsetAngleFormula");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 36)]
        public string OffsetSuceedAngleFormula
        {
            get
            {
                return this.OffsetSuceedAngleFormulaField;
            }
            set
            {
                if ((object.ReferenceEquals(this.OffsetSuceedAngleFormulaField, value) != true))
                {
                    this.OffsetSuceedAngleFormulaField = value;
                    this.RaisePropertyChanged("OffsetSuceedAngleFormula");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 37)]
        public string ParameterValueCab
        {
            get
            {
                return this.ParameterValueCabField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ParameterValueCabField, value) != true))
                {
                    this.ParameterValueCabField = value;
                    this.RaisePropertyChanged("ParameterValueCab");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 38)]
        public string CouplerType
        {
            get
            {
                return this.CouplerTypeField;
            }
            set
            {
                if ((object.ReferenceEquals(this.CouplerTypeField, value) != true))
                {
                    this.CouplerTypeField = value;
                    this.RaisePropertyChanged("CouplerType");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 39)]
        public string ParameterView
        {
            get
            {
                return this.ParameterViewField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ParameterViewField, value) != true))
                {
                    this.ParameterViewField = value;
                    this.RaisePropertyChanged("ParameterView");
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
