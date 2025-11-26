namespace OrderService.Models
{
    public partial class PostGroupMark : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {

        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        private int PostGroupMarkingDetailsIdField;

        private int PostHeaderIdField;

        private int GroupMarkIdField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string GroupMarkingNameField;

        private int GroupMarkingRevisionNumberField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string RemarksField;

        private int GroupQtyField;

        private int NoOfStructureMarkField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string BBSNoField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string BBSRemarksField;

        private int PostedQtyField;

        private decimal PostedWeightField;

        private int ProjectIdField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ResultField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ToolTipValueField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string BOMPathField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string isModularField;

        private int isCABDEField;

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
        public int PostGroupMarkingDetailsId
        {
            get
            {
                return this.PostGroupMarkingDetailsIdField;
            }
            set
            {
                if ((this.PostGroupMarkingDetailsIdField.Equals(value) != true))
                {
                    this.PostGroupMarkingDetailsIdField = value;
                    this.RaisePropertyChanged("PostGroupMarkingDetailsId");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true)]
        public int PostHeaderId
        {
            get
            {
                return this.PostHeaderIdField;
            }
            set
            {
                if ((this.PostHeaderIdField.Equals(value) != true))
                {
                    this.PostHeaderIdField = value;
                    this.RaisePropertyChanged("PostHeaderId");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 2)]
        public int GroupMarkId
        {
            get
            {
                return this.GroupMarkIdField;
            }
            set
            {
                if ((this.GroupMarkIdField.Equals(value) != true))
                {
                    this.GroupMarkIdField = value;
                    this.RaisePropertyChanged("GroupMarkId");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 3)]
        public string GroupMarkingName
        {
            get
            {
                return this.GroupMarkingNameField;
            }
            set
            {
                if ((object.ReferenceEquals(this.GroupMarkingNameField, value) != true))
                {
                    this.GroupMarkingNameField = value;
                    this.RaisePropertyChanged("GroupMarkingName");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 4)]
        public int GroupMarkingRevisionNumber
        {
            get
            {
                return this.GroupMarkingRevisionNumberField;
            }
            set
            {
                if ((this.GroupMarkingRevisionNumberField.Equals(value) != true))
                {
                    this.GroupMarkingRevisionNumberField = value;
                    this.RaisePropertyChanged("GroupMarkingRevisionNumber");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 5)]
        public string Remarks
        {
            get
            {
                return this.RemarksField;
            }
            set
            {
                if ((object.ReferenceEquals(this.RemarksField, value) != true))
                {
                    this.RemarksField = value;
                    this.RaisePropertyChanged("Remarks");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 6)]
        public int GroupQty
        {
            get
            {
                return this.GroupQtyField;
            }
            set
            {
                if ((this.GroupQtyField.Equals(value) != true))
                {
                    this.GroupQtyField = value;
                    this.RaisePropertyChanged("GroupQty");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 7)]
        public int NoOfStructureMark
        {
            get
            {
                return this.NoOfStructureMarkField;
            }
            set
            {
                if ((this.NoOfStructureMarkField.Equals(value) != true))
                {
                    this.NoOfStructureMarkField = value;
                    this.RaisePropertyChanged("NoOfStructureMark");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 8)]
        public string BBSNo
        {
            get
            {
                return this.BBSNoField;
            }
            set
            {
                if ((object.ReferenceEquals(this.BBSNoField, value) != true))
                {
                    this.BBSNoField = value;
                    this.RaisePropertyChanged("BBSNo");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 9)]
        public string BBSRemarks
        {
            get
            {
                return this.BBSRemarksField;
            }
            set
            {
                if ((object.ReferenceEquals(this.BBSRemarksField, value) != true))
                {
                    this.BBSRemarksField = value;
                    this.RaisePropertyChanged("BBSRemarks");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 10)]
        public int PostedQty
        {
            get
            {
                return this.PostedQtyField;
            }
            set
            {
                if ((this.PostedQtyField.Equals(value) != true))
                {
                    this.PostedQtyField = value;
                    this.RaisePropertyChanged("PostedQty");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 11)]
        public decimal PostedWeight
        {
            get
            {
                return this.PostedWeightField;
            }
            set
            {
                if ((this.PostedWeightField.Equals(value) != true))
                {
                    this.PostedWeightField = value;
                    this.RaisePropertyChanged("PostedWeight");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 12)]
        public int ProjectId
        {
            get
            {
                return this.ProjectIdField;
            }
            set
            {
                if ((this.ProjectIdField.Equals(value) != true))
                {
                    this.ProjectIdField = value;
                    this.RaisePropertyChanged("ProjectId");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 13)]
        public string Result
        {
            get
            {
                return this.ResultField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ResultField, value) != true))
                {
                    this.ResultField = value;
                    this.RaisePropertyChanged("Result");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 14)]
        public string ToolTipValue
        {
            get
            {
                return this.ToolTipValueField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ToolTipValueField, value) != true))
                {
                    this.ToolTipValueField = value;
                    this.RaisePropertyChanged("ToolTipValue");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 15)]
        public string BOMPath
        {
            get
            {
                return this.BOMPathField;
            }
            set
            {
                if ((object.ReferenceEquals(this.BOMPathField, value) != true))
                {
                    this.BOMPathField = value;
                    this.RaisePropertyChanged("BOMPath");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 16)]
        public string isModular
        {
            get
            {
                return this.isModularField;
            }
            set
            {
                if ((object.ReferenceEquals(this.isModularField, value) != true))
                {
                    this.isModularField = value;
                    this.RaisePropertyChanged("isModular");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 17)]
        public int isCABDE
        {
            get
            {
                return this.isCABDEField;
            }
            set
            {
                if ((this.isCABDEField.Equals(value) != true))
                {
                    this.isCABDEField = value;
                    this.RaisePropertyChanged("isCABDE");
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
