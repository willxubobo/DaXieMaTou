﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18408
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CMICT.CSP.Web.CBOSServices {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://codswitchtable.standard.platform.services.cbos.nbport.com", ConfigurationName="CBOSServices.CodSwicthTableService")]
    public interface CodSwicthTableService {
        
        // CODEGEN: Parameter 'getCodSwitchMessageReturn' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="getCodSwitchMessageReturn")]
        CMICT.CSP.Web.CBOSServices.getCodSwitchMessageResponse getCodSwitchMessage(CMICT.CSP.Web.CBOSServices.getCodSwitchMessageRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        System.Threading.Tasks.Task<CMICT.CSP.Web.CBOSServices.getCodSwitchMessageResponse> getCodSwitchMessageAsync(CMICT.CSP.Web.CBOSServices.getCodSwitchMessageRequest request);
        
        // CODEGEN: Parameter 'getCodSwitchMessage2Return' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="getCodSwitchMessage2Return")]
        CMICT.CSP.Web.CBOSServices.getCodSwitchMessage2Response getCodSwitchMessage2(CMICT.CSP.Web.CBOSServices.getCodSwitchMessage2Request request);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        System.Threading.Tasks.Task<CMICT.CSP.Web.CBOSServices.getCodSwitchMessage2Response> getCodSwitchMessage2Async(CMICT.CSP.Web.CBOSServices.getCodSwitchMessage2Request request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18408")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://codswitchtable.standard.platform.services.cbos.nbport.com")]
    public partial class ArrayOf_xsd_string : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string[] itemField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("item", IsNullable=true, Order=0)]
        public string[] item {
            get {
                return this.itemField;
            }
            set {
                this.itemField = value;
                this.RaisePropertyChanged("item");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18408")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://codswitchtable.standard.platform.services.cbos.nbport.com")]
    public partial class CodSwicthTable : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string cODE_CBOSField;
        
        private string cODE_NAME_C_CBOSField;
        
        private string cODE_NAME_C_SWITCHField;
        
        private string cODE_NAME_E_CBOSField;
        
        private string cODE_NAME_E_SWITCHField;
        
        private string cODE_SWITCHField;
        
        private string sWITCH_TAB_NAMEField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=0)]
        public string CODE_CBOS {
            get {
                return this.cODE_CBOSField;
            }
            set {
                this.cODE_CBOSField = value;
                this.RaisePropertyChanged("CODE_CBOS");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=1)]
        public string CODE_NAME_C_CBOS {
            get {
                return this.cODE_NAME_C_CBOSField;
            }
            set {
                this.cODE_NAME_C_CBOSField = value;
                this.RaisePropertyChanged("CODE_NAME_C_CBOS");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=2)]
        public string CODE_NAME_C_SWITCH {
            get {
                return this.cODE_NAME_C_SWITCHField;
            }
            set {
                this.cODE_NAME_C_SWITCHField = value;
                this.RaisePropertyChanged("CODE_NAME_C_SWITCH");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=3)]
        public string CODE_NAME_E_CBOS {
            get {
                return this.cODE_NAME_E_CBOSField;
            }
            set {
                this.cODE_NAME_E_CBOSField = value;
                this.RaisePropertyChanged("CODE_NAME_E_CBOS");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=4)]
        public string CODE_NAME_E_SWITCH {
            get {
                return this.cODE_NAME_E_SWITCHField;
            }
            set {
                this.cODE_NAME_E_SWITCHField = value;
                this.RaisePropertyChanged("CODE_NAME_E_SWITCH");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=5)]
        public string CODE_SWITCH {
            get {
                return this.cODE_SWITCHField;
            }
            set {
                this.cODE_SWITCHField = value;
                this.RaisePropertyChanged("CODE_SWITCH");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=6)]
        public string SWITCH_TAB_NAME {
            get {
                return this.sWITCH_TAB_NAMEField;
            }
            set {
                this.sWITCH_TAB_NAMEField = value;
                this.RaisePropertyChanged("SWITCH_TAB_NAME");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getCodSwitchMessage", WrapperNamespace="http://codswitchtable.standard.platform.services.cbos.nbport.com", IsWrapped=true)]
    public partial class getCodSwitchMessageRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://codswitchtable.standard.platform.services.cbos.nbport.com", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string userID;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://codswitchtable.standard.platform.services.cbos.nbport.com", Order=1)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string codType;
        
        public getCodSwitchMessageRequest() {
        }
        
        public getCodSwitchMessageRequest(string userID, string codType) {
            this.userID = userID;
            this.codType = codType;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getCodSwitchMessageResponse", WrapperNamespace="http://codswitchtable.standard.platform.services.cbos.nbport.com", IsWrapped=true)]
    public partial class getCodSwitchMessageResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://codswitchtable.standard.platform.services.cbos.nbport.com", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("getCodSwitchMessageReturn")]
        public CMICT.CSP.Web.CBOSServices.ArrayOf_xsd_string[] getCodSwitchMessageReturn;
        
        public getCodSwitchMessageResponse() {
        }
        
        public getCodSwitchMessageResponse(CMICT.CSP.Web.CBOSServices.ArrayOf_xsd_string[] getCodSwitchMessageReturn) {
            this.getCodSwitchMessageReturn = getCodSwitchMessageReturn;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getCodSwitchMessage2", WrapperNamespace="http://codswitchtable.standard.platform.services.cbos.nbport.com", IsWrapped=true)]
    public partial class getCodSwitchMessage2Request {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://codswitchtable.standard.platform.services.cbos.nbport.com", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string userID;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://codswitchtable.standard.platform.services.cbos.nbport.com", Order=1)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string codType;
        
        public getCodSwitchMessage2Request() {
        }
        
        public getCodSwitchMessage2Request(string userID, string codType) {
            this.userID = userID;
            this.codType = codType;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getCodSwitchMessage2Response", WrapperNamespace="http://codswitchtable.standard.platform.services.cbos.nbport.com", IsWrapped=true)]
    public partial class getCodSwitchMessage2Response {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://codswitchtable.standard.platform.services.cbos.nbport.com", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("getCodSwitchMessage2Return")]
        public CMICT.CSP.Web.CBOSServices.CodSwicthTable[] getCodSwitchMessage2Return;
        
        public getCodSwitchMessage2Response() {
        }
        
        public getCodSwitchMessage2Response(CMICT.CSP.Web.CBOSServices.CodSwicthTable[] getCodSwitchMessage2Return) {
            this.getCodSwitchMessage2Return = getCodSwitchMessage2Return;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface CodSwicthTableServiceChannel : CMICT.CSP.Web.CBOSServices.CodSwicthTableService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class CodSwicthTableServiceClient : System.ServiceModel.ClientBase<CMICT.CSP.Web.CBOSServices.CodSwicthTableService>, CMICT.CSP.Web.CBOSServices.CodSwicthTableService {
        
        public CodSwicthTableServiceClient() {
        }
        
        public CodSwicthTableServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public CodSwicthTableServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CodSwicthTableServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CodSwicthTableServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        CMICT.CSP.Web.CBOSServices.getCodSwitchMessageResponse CMICT.CSP.Web.CBOSServices.CodSwicthTableService.getCodSwitchMessage(CMICT.CSP.Web.CBOSServices.getCodSwitchMessageRequest request) {
            return base.Channel.getCodSwitchMessage(request);
        }
        
        public CMICT.CSP.Web.CBOSServices.ArrayOf_xsd_string[] getCodSwitchMessage(string userID, string codType) {
            CMICT.CSP.Web.CBOSServices.getCodSwitchMessageRequest inValue = new CMICT.CSP.Web.CBOSServices.getCodSwitchMessageRequest();
            inValue.userID = userID;
            inValue.codType = codType;
            CMICT.CSP.Web.CBOSServices.getCodSwitchMessageResponse retVal = ((CMICT.CSP.Web.CBOSServices.CodSwicthTableService)(this)).getCodSwitchMessage(inValue);
            return retVal.getCodSwitchMessageReturn;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CMICT.CSP.Web.CBOSServices.getCodSwitchMessageResponse> CMICT.CSP.Web.CBOSServices.CodSwicthTableService.getCodSwitchMessageAsync(CMICT.CSP.Web.CBOSServices.getCodSwitchMessageRequest request) {
            return base.Channel.getCodSwitchMessageAsync(request);
        }
        
        public System.Threading.Tasks.Task<CMICT.CSP.Web.CBOSServices.getCodSwitchMessageResponse> getCodSwitchMessageAsync(string userID, string codType) {
            CMICT.CSP.Web.CBOSServices.getCodSwitchMessageRequest inValue = new CMICT.CSP.Web.CBOSServices.getCodSwitchMessageRequest();
            inValue.userID = userID;
            inValue.codType = codType;
            return ((CMICT.CSP.Web.CBOSServices.CodSwicthTableService)(this)).getCodSwitchMessageAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        CMICT.CSP.Web.CBOSServices.getCodSwitchMessage2Response CMICT.CSP.Web.CBOSServices.CodSwicthTableService.getCodSwitchMessage2(CMICT.CSP.Web.CBOSServices.getCodSwitchMessage2Request request) {
            return base.Channel.getCodSwitchMessage2(request);
        }
        
        public CMICT.CSP.Web.CBOSServices.CodSwicthTable[] getCodSwitchMessage2(string userID, string codType) {
            CMICT.CSP.Web.CBOSServices.getCodSwitchMessage2Request inValue = new CMICT.CSP.Web.CBOSServices.getCodSwitchMessage2Request();
            inValue.userID = userID;
            inValue.codType = codType;
            CMICT.CSP.Web.CBOSServices.getCodSwitchMessage2Response retVal = ((CMICT.CSP.Web.CBOSServices.CodSwicthTableService)(this)).getCodSwitchMessage2(inValue);
            return retVal.getCodSwitchMessage2Return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CMICT.CSP.Web.CBOSServices.getCodSwitchMessage2Response> CMICT.CSP.Web.CBOSServices.CodSwicthTableService.getCodSwitchMessage2Async(CMICT.CSP.Web.CBOSServices.getCodSwitchMessage2Request request) {
            return base.Channel.getCodSwitchMessage2Async(request);
        }
        
        public System.Threading.Tasks.Task<CMICT.CSP.Web.CBOSServices.getCodSwitchMessage2Response> getCodSwitchMessage2Async(string userID, string codType) {
            CMICT.CSP.Web.CBOSServices.getCodSwitchMessage2Request inValue = new CMICT.CSP.Web.CBOSServices.getCodSwitchMessage2Request();
            inValue.userID = userID;
            inValue.codType = codType;
            return ((CMICT.CSP.Web.CBOSServices.CodSwicthTableService)(this)).getCodSwitchMessage2Async(inValue);
        }
    }
}
