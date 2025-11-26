namespace OrderService.NDSPosting
{
    public partial class Project : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {

        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ProjectNameField;

        private int ProjectIdField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ProjectDescriptionField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SAPProjectCodeField;

        private int SAPProjectIdField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SAPProjectDescriptionField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string BBSSAPProjectDescriptionField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ShipToPartyField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PhysicalProjectNameField;

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
        public string ProjectName
        {
            get
            {
                return this.ProjectNameField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ProjectNameField, value) != true))
                {
                    this.ProjectNameField = value;
                    this.RaisePropertyChanged("ProjectName");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 1)]
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

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
        public string ProjectDescription
        {
            get
            {
                return this.ProjectDescriptionField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ProjectDescriptionField, value) != true))
                {
                    this.ProjectDescriptionField = value;
                    this.RaisePropertyChanged("ProjectDescription");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 3)]
        public string SAPProjectCode
        {
            get
            {
                return this.SAPProjectCodeField;
            }
            set
            {
                if ((object.ReferenceEquals(this.SAPProjectCodeField, value) != true))
                {
                    this.SAPProjectCodeField = value;
                    this.RaisePropertyChanged("SAPProjectCode");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 4)]
        public int SAPProjectId
        {
            get
            {
                return this.SAPProjectIdField;
            }
            set
            {
                if ((this.SAPProjectIdField.Equals(value) != true))
                {
                    this.SAPProjectIdField = value;
                    this.RaisePropertyChanged("SAPProjectId");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 5)]
        public string SAPProjectDescription
        {
            get
            {
                return this.SAPProjectDescriptionField;
            }
            set
            {
                if ((object.ReferenceEquals(this.SAPProjectDescriptionField, value) != true))
                {
                    this.SAPProjectDescriptionField = value;
                    this.RaisePropertyChanged("SAPProjectDescription");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 6)]
        public string BBSSAPProjectDescription
        {
            get
            {
                return this.BBSSAPProjectDescriptionField;
            }
            set
            {
                if ((object.ReferenceEquals(this.BBSSAPProjectDescriptionField, value) != true))
                {
                    this.BBSSAPProjectDescriptionField = value;
                    this.RaisePropertyChanged("BBSSAPProjectDescription");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 7)]
        public string ShipToParty
        {
            get
            {
                return this.ShipToPartyField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ShipToPartyField, value) != true))
                {
                    this.ShipToPartyField = value;
                    this.RaisePropertyChanged("ShipToParty");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 8)]
        public string PhysicalProjectName
        {
            get
            {
                return this.PhysicalProjectNameField;
            }
            set
            {
                if ((object.ReferenceEquals(this.PhysicalProjectNameField, value) != true))
                {
                    this.PhysicalProjectNameField = value;
                    this.RaisePropertyChanged("PhysicalProjectName");
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
