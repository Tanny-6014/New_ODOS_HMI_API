namespace OrderService.NDSPosting
{
    public partial class Contract : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {

        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        private int ContractIdField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ContractNameField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ContractDescriptionField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SAPContractCodeField;

        private int SAPContractIdField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SAPContractDescriptionField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private NDSPosting.Project ProjectField;

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
        public int ContractId
        {
            get
            {
                return this.ContractIdField;
            }
            set
            {
                if ((this.ContractIdField.Equals(value) != true))
                {
                    this.ContractIdField = value;
                    this.RaisePropertyChanged("ContractId");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string ContractName
        {
            get
            {
                return this.ContractNameField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ContractNameField, value) != true))
                {
                    this.ContractNameField = value;
                    this.RaisePropertyChanged("ContractName");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
        public string ContractDescription
        {
            get
            {
                return this.ContractDescriptionField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ContractDescriptionField, value) != true))
                {
                    this.ContractDescriptionField = value;
                    this.RaisePropertyChanged("ContractDescription");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 3)]
        public string SAPContractCode
        {
            get
            {
                return this.SAPContractCodeField;
            }
            set
            {
                if ((object.ReferenceEquals(this.SAPContractCodeField, value) != true))
                {
                    this.SAPContractCodeField = value;
                    this.RaisePropertyChanged("SAPContractCode");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 4)]
        public int SAPContractId
        {
            get
            {
                return this.SAPContractIdField;
            }
            set
            {
                if ((this.SAPContractIdField.Equals(value) != true))
                {
                    this.SAPContractIdField = value;
                    this.RaisePropertyChanged("SAPContractId");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 5)]
        public string SAPContractDescription
        {
            get
            {
                return this.SAPContractDescriptionField;
            }
            set
            {
                if ((object.ReferenceEquals(this.SAPContractDescriptionField, value) != true))
                {
                    this.SAPContractDescriptionField = value;
                    this.RaisePropertyChanged("SAPContractDescription");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 6)]
        public NDSPosting.Project Project
        {
            get
            {
                return this.ProjectField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ProjectField, value) != true))
                {
                    this.ProjectField = value;
                    this.RaisePropertyChanged("Project");
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
