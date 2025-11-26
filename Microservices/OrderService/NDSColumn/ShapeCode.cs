namespace OrderService.NDSColumn
{
    public partial class ShapeCode : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {

        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        private int ShapeIDField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ShapeCodeNameField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private NDSColumn.ShapeParameter[] ShapeParamField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MeshShapeGroupField;

        private bool BendIndicatorField;

        private bool CreepDeductAtMO1Field;

        private bool CreepDeductAtCO1Field;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MOCOField;

        private bool ShapePopUpField;

        private bool IsCappingField;

        private int NoOfBendsField;

        private int MWBendPositionField;

        private int CWBendPositionField;

        private int NoOfMWBendField;

        private int NoOfCWBendField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MWbvbsStringField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CWbvbsStringField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private NDSColumn.ShapeParameter[] ShapeParameterListField;

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
        public int ShapeID
        {
            get
            {
                return this.ShapeIDField;
            }
            set
            {
                if ((this.ShapeIDField.Equals(value) != true))
                {
                    this.ShapeIDField = value;
                    this.RaisePropertyChanged("ShapeID");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
        public string ShapeCodeName
        {
            get
            {
                return this.ShapeCodeNameField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ShapeCodeNameField, value) != true))
                {
                    this.ShapeCodeNameField = value;
                    this.RaisePropertyChanged("ShapeCodeName");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
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

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 3)]
        public string MeshShapeGroup
        {
            get
            {
                return this.MeshShapeGroupField;
            }
            set
            {
                if ((object.ReferenceEquals(this.MeshShapeGroupField, value) != true))
                {
                    this.MeshShapeGroupField = value;
                    this.RaisePropertyChanged("MeshShapeGroup");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 4)]
        public bool BendIndicator
        {
            get
            {
                return this.BendIndicatorField;
            }
            set
            {
                if ((this.BendIndicatorField.Equals(value) != true))
                {
                    this.BendIndicatorField = value;
                    this.RaisePropertyChanged("BendIndicator");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 5)]
        public bool CreepDeductAtMO1
        {
            get
            {
                return this.CreepDeductAtMO1Field;
            }
            set
            {
                if ((this.CreepDeductAtMO1Field.Equals(value) != true))
                {
                    this.CreepDeductAtMO1Field = value;
                    this.RaisePropertyChanged("CreepDeductAtMO1");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 6)]
        public bool CreepDeductAtCO1
        {
            get
            {
                return this.CreepDeductAtCO1Field;
            }
            set
            {
                if ((this.CreepDeductAtCO1Field.Equals(value) != true))
                {
                    this.CreepDeductAtCO1Field = value;
                    this.RaisePropertyChanged("CreepDeductAtCO1");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 7)]
        public string MOCO
        {
            get
            {
                return this.MOCOField;
            }
            set
            {
                if ((object.ReferenceEquals(this.MOCOField, value) != true))
                {
                    this.MOCOField = value;
                    this.RaisePropertyChanged("MOCO");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 8)]
        public bool ShapePopUp
        {
            get
            {
                return this.ShapePopUpField;
            }
            set
            {
                if ((this.ShapePopUpField.Equals(value) != true))
                {
                    this.ShapePopUpField = value;
                    this.RaisePropertyChanged("ShapePopUp");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 9)]
        public bool IsCapping
        {
            get
            {
                return this.IsCappingField;
            }
            set
            {
                if ((this.IsCappingField.Equals(value) != true))
                {
                    this.IsCappingField = value;
                    this.RaisePropertyChanged("IsCapping");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 10)]
        public int NoOfBends
        {
            get
            {
                return this.NoOfBendsField;
            }
            set
            {
                if ((this.NoOfBendsField.Equals(value) != true))
                {
                    this.NoOfBendsField = value;
                    this.RaisePropertyChanged("NoOfBends");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 11)]
        public int MWBendPosition
        {
            get
            {
                return this.MWBendPositionField;
            }
            set
            {
                if ((this.MWBendPositionField.Equals(value) != true))
                {
                    this.MWBendPositionField = value;
                    this.RaisePropertyChanged("MWBendPosition");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 12)]
        public int CWBendPosition
        {
            get
            {
                return this.CWBendPositionField;
            }
            set
            {
                if ((this.CWBendPositionField.Equals(value) != true))
                {
                    this.CWBendPositionField = value;
                    this.RaisePropertyChanged("CWBendPosition");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 13)]
        public int NoOfMWBend
        {
            get
            {
                return this.NoOfMWBendField;
            }
            set
            {
                if ((this.NoOfMWBendField.Equals(value) != true))
                {
                    this.NoOfMWBendField = value;
                    this.RaisePropertyChanged("NoOfMWBend");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 14)]
        public int NoOfCWBend
        {
            get
            {
                return this.NoOfCWBendField;
            }
            set
            {
                if ((this.NoOfCWBendField.Equals(value) != true))
                {
                    this.NoOfCWBendField = value;
                    this.RaisePropertyChanged("NoOfCWBend");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 15)]
        public string MWbvbsString
        {
            get
            {
                return this.MWbvbsStringField;
            }
            set
            {
                if ((object.ReferenceEquals(this.MWbvbsStringField, value) != true))
                {
                    this.MWbvbsStringField = value;
                    this.RaisePropertyChanged("MWbvbsString");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 16)]
        public string CWbvbsString
        {
            get
            {
                return this.CWbvbsStringField;
            }
            set
            {
                if ((object.ReferenceEquals(this.CWbvbsStringField, value) != true))
                {
                    this.CWbvbsStringField = value;
                    this.RaisePropertyChanged("CWbvbsString");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 17)]
        public NDSColumn.ShapeParameter[] ShapeParameterList
        {
            get
            {
                return this.ShapeParameterListField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ShapeParameterListField, value) != true))
                {
                    this.ShapeParameterListField = value;
                    this.RaisePropertyChanged("ShapeParameterList");
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
