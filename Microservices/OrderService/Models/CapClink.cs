namespace OrderService.Models
{
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "CapClink", Namespace = "http://tempuri.org/")]
    [System.SerializableAttribute()]
    public partial class CapClink : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {

        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        private int WBSElementIdField;

        private int ProductTypeIdField;

        private int StructureElementTypeIdField;

        private bool ExistField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Customer CustomerField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Contract ContractField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Project ProjectField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string WBS1Field;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string WBS2Field;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string WBS3Field;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private StructureElement StructureElementField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private ProductType ProductTypeField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private ShapeCode ShapeCodeField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private ProductCode ProductCodeField;

        private int ParentIdField;

        private int WidthField;

        private int DepthField;

        private int MWLengthField;

        private int CWLengthField;

        private int QtyField;

        private int RevNoField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string AddFlagField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ShapeIdField;

        private int MO1Field;

        private int MO2Field;

        private int CO1Field;

        private int CO2Field;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CapProductField;

        private int CountField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TypeField;

        private int MWQtyField;

        private int CWQtyField;

        private int MWSpaceField;

        private int CWSpaceField;

        private decimal InvoiceMWWeightField;

        private decimal InvoiceCWWeightField;

        private decimal TheoriticalWeightField;

        private int AreaField;

        private int SMIDField;

        private int PMIDField;

        private int ShapeCodesField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ShapeCodeNameField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SlabParentValueField;

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
        public int WBSElementId
        {
            get
            {
                return this.WBSElementIdField;
            }
            set
            {
                if ((this.WBSElementIdField.Equals(value) != true))
                {
                    this.WBSElementIdField = value;
                    this.RaisePropertyChanged("WBSElementId");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 1)]
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 2)]
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
        public bool Exist
        {
            get
            {
                return this.ExistField;
            }
            set
            {
                if ((this.ExistField.Equals(value) != true))
                {
                    this.ExistField = value;
                    this.RaisePropertyChanged("Exist");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 4)]
        public Customer Customer
        {
            get
            {
                return this.CustomerField;
            }
            set
            {
                if ((object.ReferenceEquals(this.CustomerField, value) != true))
                {
                    this.CustomerField = value;
                    this.RaisePropertyChanged("Customer");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 5)]
        public Contract Contract
        {
            get
            {
                return this.ContractField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ContractField, value) != true))
                {
                    this.ContractField = value;
                    this.RaisePropertyChanged("Contract");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 6)]
        public Project Project
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

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 7)]
        public string WBS1
        {
            get
            {
                return this.WBS1Field;
            }
            set
            {
                if ((object.ReferenceEquals(this.WBS1Field, value) != true))
                {
                    this.WBS1Field = value;
                    this.RaisePropertyChanged("WBS1");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 8)]
        public string WBS2
        {
            get
            {
                return this.WBS2Field;
            }
            set
            {
                if ((object.ReferenceEquals(this.WBS2Field, value) != true))
                {
                    this.WBS2Field = value;
                    this.RaisePropertyChanged("WBS2");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 9)]
        public string WBS3
        {
            get
            {
                return this.WBS3Field;
            }
            set
            {
                if ((object.ReferenceEquals(this.WBS3Field, value) != true))
                {
                    this.WBS3Field = value;
                    this.RaisePropertyChanged("WBS3");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 10)]
        public StructureElement StructureElement
        {
            get
            {
                return this.StructureElementField;
            }
            set
            {
                if ((object.ReferenceEquals(this.StructureElementField, value) != true))
                {
                    this.StructureElementField = value;
                    this.RaisePropertyChanged("StructureElement");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 11)]
        public ProductType ProductType
        {
            get
            {
                return this.ProductTypeField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ProductTypeField, value) != true))
                {
                    this.ProductTypeField = value;
                    this.RaisePropertyChanged("ProductType");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 12)]
        //public ShapeCode ShapeCode
        //{
        //    get
        //    {
        //        return this.ShapeCodeField;
        //    }
        //    set
        //    {
        //        if ((object.ReferenceEquals(this.ShapeCodeField, value) != true))
        //        {
        //            this.ShapeCodeField = value;
        //            this.RaisePropertyChanged("ShapeCode");
        //        }
        //    }
        //}
        public string? SHAPECODE { get; set; }
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 13)]
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 14)]
        public int ParentId
        {
            get
            {
                return this.ParentIdField;
            }
            set
            {
                if ((this.ParentIdField.Equals(value) != true))
                {
                    this.ParentIdField = value;
                    this.RaisePropertyChanged("ParentId");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 15)]
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 16)]
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 17)]
        public int MWLength
        {
            get
            {
                return this.MWLengthField;
            }
            set
            {
                if ((this.MWLengthField.Equals(value) != true))
                {
                    this.MWLengthField = value;
                    this.RaisePropertyChanged("MWLength");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 18)]
        public int CWLength
        {
            get
            {
                return this.CWLengthField;
            }
            set
            {
                if ((this.CWLengthField.Equals(value) != true))
                {
                    this.CWLengthField = value;
                    this.RaisePropertyChanged("CWLength");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 19)]
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 20)]
        public int RevNo
        {
            get
            {
                return this.RevNoField;
            }
            set
            {
                if ((this.RevNoField.Equals(value) != true))
                {
                    this.RevNoField = value;
                    this.RaisePropertyChanged("RevNo");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 21)]
        public string AddFlag
        {
            get
            {
                return this.AddFlagField;
            }
            set
            {
                if ((object.ReferenceEquals(this.AddFlagField, value) != true))
                {
                    this.AddFlagField = value;
                    this.RaisePropertyChanged("AddFlag");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 22)]
        public string ShapeId
        {
            get
            {
                return this.ShapeIdField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ShapeIdField, value) != true))
                {
                    this.ShapeIdField = value;
                    this.RaisePropertyChanged("ShapeId");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 23)]
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 24)]
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 25)]
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 26)]
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

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 27)]
        public string CapProduct
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 28)]
        public int Count
        {
            get
            {
                return this.CountField;
            }
            set
            {
                if ((this.CountField.Equals(value) != true))
                {
                    this.CountField = value;
                    this.RaisePropertyChanged("Count");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 29)]
        public string Type
        {
            get
            {
                return this.TypeField;
            }
            set
            {
                if ((object.ReferenceEquals(this.TypeField, value) != true))
                {
                    this.TypeField = value;
                    this.RaisePropertyChanged("Type");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 30)]
        public int MWQty
        {
            get
            {
                return this.MWQtyField;
            }
            set
            {
                if ((this.MWQtyField.Equals(value) != true))
                {
                    this.MWQtyField = value;
                    this.RaisePropertyChanged("MWQty");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 31)]
        public int CWQty
        {
            get
            {
                return this.CWQtyField;
            }
            set
            {
                if ((this.CWQtyField.Equals(value) != true))
                {
                    this.CWQtyField = value;
                    this.RaisePropertyChanged("CWQty");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 32)]
        public int MWSpace
        {
            get
            {
                return this.MWSpaceField;
            }
            set
            {
                if ((this.MWSpaceField.Equals(value) != true))
                {
                    this.MWSpaceField = value;
                    this.RaisePropertyChanged("MWSpace");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 33)]
        public int CWSpace
        {
            get
            {
                return this.CWSpaceField;
            }
            set
            {
                if ((this.CWSpaceField.Equals(value) != true))
                {
                    this.CWSpaceField = value;
                    this.RaisePropertyChanged("CWSpace");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 34)]
        public decimal InvoiceMWWeight
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 35)]
        public decimal InvoiceCWWeight
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 36)]
        public decimal TheoriticalWeight
        {
            get
            {
                return this.TheoriticalWeightField;
            }
            set
            {
                if ((this.TheoriticalWeightField.Equals(value) != true))
                {
                    this.TheoriticalWeightField = value;
                    this.RaisePropertyChanged("TheoriticalWeight");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 37)]
        public int Area
        {
            get
            {
                return this.AreaField;
            }
            set
            {
                if ((this.AreaField.Equals(value) != true))
                {
                    this.AreaField = value;
                    this.RaisePropertyChanged("Area");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 38)]
        public int SMID
        {
            get
            {
                return this.SMIDField;
            }
            set
            {
                if ((this.SMIDField.Equals(value) != true))
                {
                    this.SMIDField = value;
                    this.RaisePropertyChanged("SMID");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 39)]
        public int PMID
        {
            get
            {
                return this.PMIDField;
            }
            set
            {
                if ((this.PMIDField.Equals(value) != true))
                {
                    this.PMIDField = value;
                    this.RaisePropertyChanged("PMID");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 40)]
        public int ShapeCodes
        {
            get
            {
                return this.ShapeCodesField;
            }
            set
            {
                if ((this.ShapeCodesField.Equals(value) != true))
                {
                    this.ShapeCodesField = value;
                    this.RaisePropertyChanged("ShapeCodes");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 41)]
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

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 42)]
        public string SlabParentValue
        {
            get
            {
                return this.SlabParentValueField;
            }
            set
            {
                if ((object.ReferenceEquals(this.SlabParentValueField, value) != true))
                {
                    this.SlabParentValueField = value;
                    this.RaisePropertyChanged("SlabParentValue");
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

    public partial class Customer : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {

        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CustomerNameField;

        private int CustomerIdField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CustomerNumberField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Contract CustomerContractField;

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
        public string CustomerName
        {
            get
            {
                return this.CustomerNameField;
            }
            set
            {
                if ((object.ReferenceEquals(this.CustomerNameField, value) != true))
                {
                    this.CustomerNameField = value;
                    this.RaisePropertyChanged("CustomerName");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 1)]
        public int CustomerId
        {
            get
            {
                return this.CustomerIdField;
            }
            set
            {
                if ((this.CustomerIdField.Equals(value) != true))
                {
                    this.CustomerIdField = value;
                    this.RaisePropertyChanged("CustomerId");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
        public string CustomerNumber
        {
            get
            {
                return this.CustomerNumberField;
            }
            set
            {
                if ((object.ReferenceEquals(this.CustomerNumberField, value) != true))
                {
                    this.CustomerNumberField = value;
                    this.RaisePropertyChanged("CustomerNumber");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 3)]
        public Contract CustomerContract
        {
            get
            {
                return this.CustomerContractField;
            }
            set
            {
                if ((object.ReferenceEquals(this.CustomerContractField, value) != true))
                {
                    this.CustomerContractField = value;
                    this.RaisePropertyChanged("CustomerContract");
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
        private Project ProjectField;

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
        public Project Project
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

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "Project", Namespace = "http://tempuri.org/")]
    [System.SerializableAttribute()]
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

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "ProductType", Namespace = "http://tempuri.org/")]
    [System.SerializableAttribute()]
    public partial class ProductType : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {

        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ProductTypeNameField;

        private int ProductTypeIdField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PrefixField;

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
        public string ProductTypeName
        {
            get
            {
                return this.ProductTypeNameField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ProductTypeNameField, value) != true))
                {
                    this.ProductTypeNameField = value;
                    this.RaisePropertyChanged("ProductTypeName");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 1)]
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

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
        public string Prefix
        {
            get
            {
                return this.PrefixField;
            }
            set
            {
                if ((object.ReferenceEquals(this.PrefixField, value) != true))
                {
                    this.PrefixField = value;
                    this.RaisePropertyChanged("Prefix");
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


 

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "ShapeCode", Namespace = "http://tempuri.org/")]
    [System.SerializableAttribute()]
    public partial class ShapeCode : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {

        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        private int ShapeIDField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ShapeCodeNameField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private ShapeParameter[] ShapeParamField;

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
        private ShapeParameter[] ShapeParameterListField;

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
        public ShapeParameter[] ShapeParam
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
        public ShapeParameter[] ShapeParameterList
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


    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "ShapeParameter", Namespace = "http://tempuri.org/")]
    [System.SerializableAttribute()]
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

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "ProductCode", Namespace = "http://tempuri.org/")]
    [System.SerializableAttribute()]
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

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "StructureElement", Namespace = "http://tempuri.org/")]
    [System.SerializableAttribute()]
    public partial class StructureElement : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {

        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string StructureElementTypeField;

        private int StructureElementTypeIdField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PrefixField;

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
        public string StructureElementType
        {
            get
            {
                return this.StructureElementTypeField;
            }
            set
            {
                if ((object.ReferenceEquals(this.StructureElementTypeField, value) != true))
                {
                    this.StructureElementTypeField = value;
                    this.RaisePropertyChanged("StructureElementType");
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

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
        public string Prefix
        {
            get
            {
                return this.PrefixField;
            }
            set
            {
                if ((object.ReferenceEquals(this.PrefixField, value) != true))
                {
                    this.PrefixField = value;
                    this.RaisePropertyChanged("Prefix");
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

