namespace OrderService.NDSSlab
{
    public partial class ShapeCodeParameterSet : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {

        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        private int ParameterSetValueField;

        private int ParameterSetNumberField;

        private int TransportModeIdField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TransportModeField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TransportModeDescField;

        private int Gap1Field;

        private int Gap2Field;

        private int TopCoverField;

        private int BottomCoverField;

        private int LeftCoverField;

        private int RightCoverField;

        private int HookField;

        private int LegField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DescriptionField;

        private int MaxMWLengthField;

        private int MaxCWLengthField;

        private int MinMWLengthField;

        private int MinCWLengthField;

        private int MachineMaxMWLengthField;

        private int MachineMaxCWLengthField;

        private int MWLapField;

        private int CWLapField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private ProductCode productCodeField;

        private int TransportMaxLengthField;

        private int TransportMaxWidthField;

        private int TransportMaxHeightField;

        private int TransportMaxWeightField;

        private bool ISStandardField;

        private int MinMo1Field;

        private int MinMo2Field;

        private int MinCo1Field;

        private int MinCo2Field;

        private int StructureElementTypeField;

        public int TNTPARAMSETNUMBER;

        public int INTPARAMETESET;

        public int TNTTRANSPORTMODEID;

        public string VCHDESCRIPTION;

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
        public int ParameterSetValue
        {
            get
            {
                return this.ParameterSetValueField;
            }
            set
            {
                if ((this.ParameterSetValueField.Equals(value) != true))
                {
                    this.ParameterSetValueField = value;
                    this.RaisePropertyChanged("ParameterSetValue");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 1)]
        public int ParameterSetNumber
        {
            get
            {
                return this.ParameterSetNumberField;
            }
            set
            {
                if ((this.ParameterSetNumberField.Equals(value) != true))
                {
                    this.ParameterSetNumberField = value;
                    this.RaisePropertyChanged("ParameterSetNumber");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 2)]
        public int TransportModeId
        {
            get
            {
                return this.TransportModeIdField;
            }
            set
            {
                if ((this.TransportModeIdField.Equals(value) != true))
                {
                    this.TransportModeIdField = value;
                    this.RaisePropertyChanged("TransportModeId");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 3)]
        public string TransportMode
        {
            get
            {
                return this.TransportModeField;
            }
            set
            {
                if ((object.ReferenceEquals(this.TransportModeField, value) != true))
                {
                    this.TransportModeField = value;
                    this.RaisePropertyChanged("TransportMode");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 4)]
        public string TransportModeDesc
        {
            get
            {
                return this.TransportModeDescField;
            }
            set
            {
                if ((object.ReferenceEquals(this.TransportModeDescField, value) != true))
                {
                    this.TransportModeDescField = value;
                    this.RaisePropertyChanged("TransportModeDesc");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 5)]
        public int Gap1
        {
            get
            {
                return this.Gap1Field;
            }
            set
            {
                if ((this.Gap1Field.Equals(value) != true))
                {
                    this.Gap1Field = value;
                    this.RaisePropertyChanged("Gap1");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 6)]
        public int Gap2
        {
            get
            {
                return this.Gap2Field;
            }
            set
            {
                if ((this.Gap2Field.Equals(value) != true))
                {
                    this.Gap2Field = value;
                    this.RaisePropertyChanged("Gap2");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 7)]
        public int TopCover
        {
            get
            {
                return this.TopCoverField;
            }
            set
            {
                if ((this.TopCoverField.Equals(value) != true))
                {
                    this.TopCoverField = value;
                    this.RaisePropertyChanged("TopCover");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 8)]
        public int BottomCover
        {
            get
            {
                return this.BottomCoverField;
            }
            set
            {
                if ((this.BottomCoverField.Equals(value) != true))
                {
                    this.BottomCoverField = value;
                    this.RaisePropertyChanged("BottomCover");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 9)]
        public int LeftCover
        {
            get
            {
                return this.LeftCoverField;
            }
            set
            {
                if ((this.LeftCoverField.Equals(value) != true))
                {
                    this.LeftCoverField = value;
                    this.RaisePropertyChanged("LeftCover");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 10)]
        public int RightCover
        {
            get
            {
                return this.RightCoverField;
            }
            set
            {
                if ((this.RightCoverField.Equals(value) != true))
                {
                    this.RightCoverField = value;
                    this.RaisePropertyChanged("RightCover");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 11)]
        public int Hook
        {
            get
            {
                return this.HookField;
            }
            set
            {
                if ((this.HookField.Equals(value) != true))
                {
                    this.HookField = value;
                    this.RaisePropertyChanged("Hook");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 12)]
        public int Leg
        {
            get
            {
                return this.LegField;
            }
            set
            {
                if ((this.LegField.Equals(value) != true))
                {
                    this.LegField = value;
                    this.RaisePropertyChanged("Leg");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 13)]
        public string Description
        {
            get
            {
                return this.DescriptionField;
            }
            set
            {
                if ((object.ReferenceEquals(this.DescriptionField, value) != true))
                {
                    this.DescriptionField = value;
                    this.RaisePropertyChanged("Description");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 14)]
        public int MaxMWLength
        {
            get
            {
                return this.MaxMWLengthField;
            }
            set
            {
                if ((this.MaxMWLengthField.Equals(value) != true))
                {
                    this.MaxMWLengthField = value;
                    this.RaisePropertyChanged("MaxMWLength");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 15)]
        public int MaxCWLength
        {
            get
            {
                return this.MaxCWLengthField;
            }
            set
            {
                if ((this.MaxCWLengthField.Equals(value) != true))
                {
                    this.MaxCWLengthField = value;
                    this.RaisePropertyChanged("MaxCWLength");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 16)]
        public int MinMWLength
        {
            get
            {
                return this.MinMWLengthField;
            }
            set
            {
                if ((this.MinMWLengthField.Equals(value) != true))
                {
                    this.MinMWLengthField = value;
                    this.RaisePropertyChanged("MinMWLength");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 17)]
        public int MinCWLength
        {
            get
            {
                return this.MinCWLengthField;
            }
            set
            {
                if ((this.MinCWLengthField.Equals(value) != true))
                {
                    this.MinCWLengthField = value;
                    this.RaisePropertyChanged("MinCWLength");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 18)]
        public int MachineMaxMWLength
        {
            get
            {
                return this.MachineMaxMWLengthField;
            }
            set
            {
                if ((this.MachineMaxMWLengthField.Equals(value) != true))
                {
                    this.MachineMaxMWLengthField = value;
                    this.RaisePropertyChanged("MachineMaxMWLength");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 19)]
        public int MachineMaxCWLength
        {
            get
            {
                return this.MachineMaxCWLengthField;
            }
            set
            {
                if ((this.MachineMaxCWLengthField.Equals(value) != true))
                {
                    this.MachineMaxCWLengthField = value;
                    this.RaisePropertyChanged("MachineMaxCWLength");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 20)]
        public int MWLap
        {
            get
            {
                return this.MWLapField;
            }
            set
            {
                if ((this.MWLapField.Equals(value) != true))
                {
                    this.MWLapField = value;
                    this.RaisePropertyChanged("MWLap");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 21)]
        public int CWLap
        {
            get
            {
                return this.CWLapField;
            }
            set
            {
                if ((this.CWLapField.Equals(value) != true))
                {
                    this.CWLapField = value;
                    this.RaisePropertyChanged("CWLap");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 22)]
        public ProductCode productCode
        {
            get
            {
                return this.productCodeField;
            }
            set
            {
                if ((object.ReferenceEquals(this.productCodeField, value) != true))
                {
                    this.productCodeField = value;
                    this.RaisePropertyChanged("productCode");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 23)]
        public int TransportMaxLength
        {
            get
            {
                return this.TransportMaxLengthField;
            }
            set
            {
                if ((this.TransportMaxLengthField.Equals(value) != true))
                {
                    this.TransportMaxLengthField = value;
                    this.RaisePropertyChanged("TransportMaxLength");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 24)]
        public int TransportMaxWidth
        {
            get
            {
                return this.TransportMaxWidthField;
            }
            set
            {
                if ((this.TransportMaxWidthField.Equals(value) != true))
                {
                    this.TransportMaxWidthField = value;
                    this.RaisePropertyChanged("TransportMaxWidth");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 25)]
        public int TransportMaxHeight
        {
            get
            {
                return this.TransportMaxHeightField;
            }
            set
            {
                if ((this.TransportMaxHeightField.Equals(value) != true))
                {
                    this.TransportMaxHeightField = value;
                    this.RaisePropertyChanged("TransportMaxHeight");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 26)]
        public int TransportMaxWeight
        {
            get
            {
                return this.TransportMaxWeightField;
            }
            set
            {
                if ((this.TransportMaxWeightField.Equals(value) != true))
                {
                    this.TransportMaxWeightField = value;
                    this.RaisePropertyChanged("TransportMaxWeight");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 27)]
        public bool ISStandard
        {
            get
            {
                return this.ISStandardField;
            }
            set
            {
                if ((this.ISStandardField.Equals(value) != true))
                {
                    this.ISStandardField = value;
                    this.RaisePropertyChanged("ISStandard");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 28)]
        public int MinMo1
        {
            get
            {
                return this.MinMo1Field;
            }
            set
            {
                if ((this.MinMo1Field.Equals(value) != true))
                {
                    this.MinMo1Field = value;
                    this.RaisePropertyChanged("MinMo1");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 29)]
        public int MinMo2
        {
            get
            {
                return this.MinMo2Field;
            }
            set
            {
                if ((this.MinMo2Field.Equals(value) != true))
                {
                    this.MinMo2Field = value;
                    this.RaisePropertyChanged("MinMo2");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 30)]
        public int MinCo1
        {
            get
            {
                return this.MinCo1Field;
            }
            set
            {
                if ((this.MinCo1Field.Equals(value) != true))
                {
                    this.MinCo1Field = value;
                    this.RaisePropertyChanged("MinCo1");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 31)]
        public int MinCo2
        {
            get
            {
                return this.MinCo2Field;
            }
            set
            {
                if ((this.MinCo2Field.Equals(value) != true))
                {
                    this.MinCo2Field = value;
                    this.RaisePropertyChanged("MinCo2");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 32)]
        public int StructureElementType
        {
            get
            {
                return this.StructureElementTypeField;
            }
            set
            {
                if ((this.StructureElementTypeField.Equals(value) != true))
                {
                    this.StructureElementTypeField = value;
                    this.RaisePropertyChanged("StructureElementType");
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
