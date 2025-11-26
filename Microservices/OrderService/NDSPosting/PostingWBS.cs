namespace OrderService.NDSPosting
{
    public partial class PostingWBS : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {

        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        private int PostHeaderIdField;

        private int WBSElementIdField;

        private int ProjectIdField;

        private int StructureElementTypeIdField;

        private int ProductTypeIdField;

        private int RefPostHeaderIdField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string WBS1Field;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string WBS2Field;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string WBS3Field;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string WBS4Field;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string WBS5Field;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string BBSNOField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string BBSSDescField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string BBSDescriptionStringField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SORField;

        private bool IsCappingField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string StructureElementField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ProductTypeField;

        private int PostedQtyField;

        private int PostedcappingQtyField;

        private int PostedCLinkQtyField;

        private decimal PostedWeightField;

        private decimal PostedCappingWeightField;

        private decimal PostedCLinkWeightField;

        private int PostedByIdField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PostedByField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PostedDateField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ReleaseByField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ReleaseDateField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ResultField;

        private bool SelectField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string StatusCodeField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PostedToolTipValueField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ReleasedToolTipValueField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SalesOrderIdField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private NDSPosting.PostGroupMark PostGMField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private NDSPosting.NDSStatus NDSStatusField;

        private int ReturnValueField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string GroupIdField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ReqDeliveryDateField;

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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 2)]
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 3)]
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 4)]
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 5)]
        public int RefPostHeaderId
        {
            get
            {
                return this.RefPostHeaderIdField;
            }
            set
            {
                if ((this.RefPostHeaderIdField.Equals(value) != true))
                {
                    this.RefPostHeaderIdField = value;
                    this.RaisePropertyChanged("RefPostHeaderId");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 6)]
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

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 7)]
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

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 8)]
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

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 9)]
        public string WBS4
        {
            get
            {
                return this.WBS4Field;
            }
            set
            {
                if ((object.ReferenceEquals(this.WBS4Field, value) != true))
                {
                    this.WBS4Field = value;
                    this.RaisePropertyChanged("WBS4");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 10)]
        public string WBS5
        {
            get
            {
                return this.WBS5Field;
            }
            set
            {
                if ((object.ReferenceEquals(this.WBS5Field, value) != true))
                {
                    this.WBS5Field = value;
                    this.RaisePropertyChanged("WBS5");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 11)]
        public string BBSNO
        {
            get
            {
                return this.BBSNOField;
            }
            set
            {
                if ((object.ReferenceEquals(this.BBSNOField, value) != true))
                {
                    this.BBSNOField = value;
                    this.RaisePropertyChanged("BBSNO");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 12)]
        public string BBSSDesc
        {
            get
            {
                return this.BBSSDescField;
            }
            set
            {
                if ((object.ReferenceEquals(this.BBSSDescField, value) != true))
                {
                    this.BBSSDescField = value;
                    this.RaisePropertyChanged("BBSSDesc");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 13)]
        public string BBSDescriptionString
        {
            get
            {
                return this.BBSDescriptionStringField;
            }
            set
            {
                if ((object.ReferenceEquals(this.BBSDescriptionStringField, value) != true))
                {
                    this.BBSDescriptionStringField = value;
                    this.RaisePropertyChanged("BBSDescriptionString");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 14)]
        public string SOR
        {
            get
            {
                return this.SORField;
            }
            set
            {
                if ((object.ReferenceEquals(this.SORField, value) != true))
                {
                    this.SORField = value;
                    this.RaisePropertyChanged("SOR");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 15)]
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

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 16)]
        public string StructureElement
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

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 17)]
        public string ProductType
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 18)]
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 19)]
        public int PostedcappingQty
        {
            get
            {
                return this.PostedcappingQtyField;
            }
            set
            {
                if ((this.PostedcappingQtyField.Equals(value) != true))
                {
                    this.PostedcappingQtyField = value;
                    this.RaisePropertyChanged("PostedcappingQty");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 20)]
        public int PostedCLinkQty
        {
            get
            {
                return this.PostedCLinkQtyField;
            }
            set
            {
                if ((this.PostedCLinkQtyField.Equals(value) != true))
                {
                    this.PostedCLinkQtyField = value;
                    this.RaisePropertyChanged("PostedCLinkQty");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 21)]
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 22)]
        public decimal PostedCappingWeight
        {
            get
            {
                return this.PostedCappingWeightField;
            }
            set
            {
                if ((this.PostedCappingWeightField.Equals(value) != true))
                {
                    this.PostedCappingWeightField = value;
                    this.RaisePropertyChanged("PostedCappingWeight");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 23)]
        public decimal PostedCLinkWeight
        {
            get
            {
                return this.PostedCLinkWeightField;
            }
            set
            {
                if ((this.PostedCLinkWeightField.Equals(value) != true))
                {
                    this.PostedCLinkWeightField = value;
                    this.RaisePropertyChanged("PostedCLinkWeight");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 24)]
        public int PostedById
        {
            get
            {
                return this.PostedByIdField;
            }
            set
            {
                if ((this.PostedByIdField.Equals(value) != true))
                {
                    this.PostedByIdField = value;
                    this.RaisePropertyChanged("PostedById");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 25)]
        public string PostedBy
        {
            get
            {
                return this.PostedByField;
            }
            set
            {
                if ((object.ReferenceEquals(this.PostedByField, value) != true))
                {
                    this.PostedByField = value;
                    this.RaisePropertyChanged("PostedBy");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 26)]
        public string PostedDate
        {
            get
            {
                return this.PostedDateField;
            }
            set
            {
                if ((object.ReferenceEquals(this.PostedDateField, value) != true))
                {
                    this.PostedDateField = value;
                    this.RaisePropertyChanged("PostedDate");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 27)]
        public string ReleaseBy
        {
            get
            {
                return this.ReleaseByField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ReleaseByField, value) != true))
                {
                    this.ReleaseByField = value;
                    this.RaisePropertyChanged("ReleaseBy");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 28)]
        public string ReleaseDate
        {
            get
            {
                return this.ReleaseDateField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ReleaseDateField, value) != true))
                {
                    this.ReleaseDateField = value;
                    this.RaisePropertyChanged("ReleaseDate");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 29)]
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 30)]
        public bool Select
        {
            get
            {
                return this.SelectField;
            }
            set
            {
                if ((this.SelectField.Equals(value) != true))
                {
                    this.SelectField = value;
                    this.RaisePropertyChanged("Select");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 31)]
        public string StatusCode
        {
            get
            {
                return this.StatusCodeField;
            }
            set
            {
                if ((object.ReferenceEquals(this.StatusCodeField, value) != true))
                {
                    this.StatusCodeField = value;
                    this.RaisePropertyChanged("StatusCode");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 32)]
        public string PostedToolTipValue
        {
            get
            {
                return this.PostedToolTipValueField;
            }
            set
            {
                if ((object.ReferenceEquals(this.PostedToolTipValueField, value) != true))
                {
                    this.PostedToolTipValueField = value;
                    this.RaisePropertyChanged("PostedToolTipValue");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 33)]
        public string ReleasedToolTipValue
        {
            get
            {
                return this.ReleasedToolTipValueField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ReleasedToolTipValueField, value) != true))
                {
                    this.ReleasedToolTipValueField = value;
                    this.RaisePropertyChanged("ReleasedToolTipValue");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 34)]
        public string SalesOrderId
        {
            get
            {
                return this.SalesOrderIdField;
            }
            set
            {
                if ((object.ReferenceEquals(this.SalesOrderIdField, value) != true))
                {
                    this.SalesOrderIdField = value;
                    this.RaisePropertyChanged("SalesOrderId");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 35)]
        public NDSPosting.PostGroupMark PostGM
        {
            get
            {
                return this.PostGMField;
            }
            set
            {
                if ((object.ReferenceEquals(this.PostGMField, value) != true))
                {
                    this.PostGMField = value;
                    this.RaisePropertyChanged("PostGM");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 36)]
        public NDSPosting.NDSStatus NDSStatus
        {
            get
            {
                return this.NDSStatusField;
            }
            set
            {
                if ((object.ReferenceEquals(this.NDSStatusField, value) != true))
                {
                    this.NDSStatusField = value;
                    this.RaisePropertyChanged("NDSStatus");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 37)]
        public int ReturnValue
        {
            get
            {
                return this.ReturnValueField;
            }
            set
            {
                if ((this.ReturnValueField.Equals(value) != true))
                {
                    this.ReturnValueField = value;
                    this.RaisePropertyChanged("ReturnValue");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 38)]
        public string GroupId
        {
            get
            {
                return this.GroupIdField;
            }
            set
            {
                if ((object.ReferenceEquals(this.GroupIdField, value) != true))
                {
                    this.GroupIdField = value;
                    this.RaisePropertyChanged("GroupId");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 39)]
        public string ReqDeliveryDate
        {
            get
            {
                return this.ReqDeliveryDateField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ReqDeliveryDateField, value) != true))
                {
                    this.ReqDeliveryDateField = value;
                    this.RaisePropertyChanged("ReqDeliveryDate");
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
