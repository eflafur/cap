﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BankXMLManager.SepaService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="BankXMLOutput", Namespace="http://schemas.datacontract.org/2004/07/SepaManager.Base")]
    [System.SerializableAttribute()]
    public partial class BankXMLOutput : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string FileNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Xml.XmlElement XmlDocumentField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string XmlStringDocumentField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string FileName {
            get {
                return this.FileNameField;
            }
            set {
                if ((object.ReferenceEquals(this.FileNameField, value) != true)) {
                    this.FileNameField = value;
                    this.RaisePropertyChanged("FileName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Xml.XmlElement XmlDocument {
            get {
                return this.XmlDocumentField;
            }
            set {
                if ((object.ReferenceEquals(this.XmlDocumentField, value) != true)) {
                    this.XmlDocumentField = value;
                    this.RaisePropertyChanged("XmlDocument");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string XmlStringDocument {
            get {
                return this.XmlStringDocumentField;
            }
            set {
                if ((object.ReferenceEquals(this.XmlStringDocumentField, value) != true)) {
                    this.XmlStringDocumentField = value;
                    this.RaisePropertyChanged("XmlStringDocument");
                }
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
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="BankXMLManager.SepaService.IBankXMLService")]
    public interface IBankXMLService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBankXMLService/CreateXml", ReplyAction="http://tempuri.org/IBankXMLService/CreateXmlResponse")]
        long CreateXml(string FileName, string PkOperazione);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IBankXMLService/CreateXml", ReplyAction="http://tempuri.org/IBankXMLService/CreateXmlResponse")]
        System.IAsyncResult BeginCreateXml(string FileName, string PkOperazione, System.AsyncCallback callback, object asyncState);
        
        long EndCreateXml(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBankXMLService/GetXmlDocument", ReplyAction="http://tempuri.org/IBankXMLService/GetXmlDocumentResponse")]
        BankXMLManager.SepaService.BankXMLOutput GetXmlDocument(long SepaHederID);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IBankXMLService/GetXmlDocument", ReplyAction="http://tempuri.org/IBankXMLService/GetXmlDocumentResponse")]
        System.IAsyncResult BeginGetXmlDocument(long SepaHederID, System.AsyncCallback callback, object asyncState);
        
        BankXMLManager.SepaService.BankXMLOutput EndGetXmlDocument(System.IAsyncResult result);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IBankXMLServiceChannel : BankXMLManager.SepaService.IBankXMLService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class CreateXmlCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public CreateXmlCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public long Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((long)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GetXmlDocumentCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public GetXmlDocumentCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public BankXMLManager.SepaService.BankXMLOutput Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((BankXMLManager.SepaService.BankXMLOutput)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class BankXMLServiceClient : System.ServiceModel.ClientBase<BankXMLManager.SepaService.IBankXMLService>, BankXMLManager.SepaService.IBankXMLService {
        
        private BeginOperationDelegate onBeginCreateXmlDelegate;
        
        private EndOperationDelegate onEndCreateXmlDelegate;
        
        private System.Threading.SendOrPostCallback onCreateXmlCompletedDelegate;
        
        private BeginOperationDelegate onBeginGetXmlDocumentDelegate;
        
        private EndOperationDelegate onEndGetXmlDocumentDelegate;
        
        private System.Threading.SendOrPostCallback onGetXmlDocumentCompletedDelegate;
        
        public BankXMLServiceClient() {
        }
        
        public BankXMLServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public BankXMLServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public BankXMLServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public BankXMLServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public event System.EventHandler<CreateXmlCompletedEventArgs> CreateXmlCompleted;
        
        public event System.EventHandler<GetXmlDocumentCompletedEventArgs> GetXmlDocumentCompleted;
        
        public long CreateXml(string FileName, string PkOperazione) {
            return base.Channel.CreateXml(FileName, PkOperazione);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public System.IAsyncResult BeginCreateXml(string FileName, string PkOperazione, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginCreateXml(FileName, PkOperazione, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public long EndCreateXml(System.IAsyncResult result) {
            return base.Channel.EndCreateXml(result);
        }
        
        private System.IAsyncResult OnBeginCreateXml(object[] inValues, System.AsyncCallback callback, object asyncState) {
            string FileName = ((string)(inValues[0]));
            string PkOperazione = ((string)(inValues[1]));
            return this.BeginCreateXml(FileName, PkOperazione, callback, asyncState);
        }
        
        private object[] OnEndCreateXml(System.IAsyncResult result) {
            long retVal = this.EndCreateXml(result);
            return new object[] {
                    retVal};
        }
        
        private void OnCreateXmlCompleted(object state) {
            if ((this.CreateXmlCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.CreateXmlCompleted(this, new CreateXmlCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void CreateXmlAsync(string FileName, string PkOperazione) {
            this.CreateXmlAsync(FileName, PkOperazione, null);
        }
        
        public void CreateXmlAsync(string FileName, string PkOperazione, object userState) {
            if ((this.onBeginCreateXmlDelegate == null)) {
                this.onBeginCreateXmlDelegate = new BeginOperationDelegate(this.OnBeginCreateXml);
            }
            if ((this.onEndCreateXmlDelegate == null)) {
                this.onEndCreateXmlDelegate = new EndOperationDelegate(this.OnEndCreateXml);
            }
            if ((this.onCreateXmlCompletedDelegate == null)) {
                this.onCreateXmlCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnCreateXmlCompleted);
            }
            base.InvokeAsync(this.onBeginCreateXmlDelegate, new object[] {
                        FileName,
                        PkOperazione}, this.onEndCreateXmlDelegate, this.onCreateXmlCompletedDelegate, userState);
        }
        
        public BankXMLManager.SepaService.BankXMLOutput GetXmlDocument(long SepaHederID) {
            return base.Channel.GetXmlDocument(SepaHederID);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public System.IAsyncResult BeginGetXmlDocument(long SepaHederID, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginGetXmlDocument(SepaHederID, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public BankXMLManager.SepaService.BankXMLOutput EndGetXmlDocument(System.IAsyncResult result) {
            return base.Channel.EndGetXmlDocument(result);
        }
        
        private System.IAsyncResult OnBeginGetXmlDocument(object[] inValues, System.AsyncCallback callback, object asyncState) {
            long SepaHederID = ((long)(inValues[0]));
            return this.BeginGetXmlDocument(SepaHederID, callback, asyncState);
        }
        
        private object[] OnEndGetXmlDocument(System.IAsyncResult result) {
            BankXMLManager.SepaService.BankXMLOutput retVal = this.EndGetXmlDocument(result);
            return new object[] {
                    retVal};
        }
        
        private void OnGetXmlDocumentCompleted(object state) {
            if ((this.GetXmlDocumentCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.GetXmlDocumentCompleted(this, new GetXmlDocumentCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void GetXmlDocumentAsync(long SepaHederID) {
            this.GetXmlDocumentAsync(SepaHederID, null);
        }
        
        public void GetXmlDocumentAsync(long SepaHederID, object userState) {
            if ((this.onBeginGetXmlDocumentDelegate == null)) {
                this.onBeginGetXmlDocumentDelegate = new BeginOperationDelegate(this.OnBeginGetXmlDocument);
            }
            if ((this.onEndGetXmlDocumentDelegate == null)) {
                this.onEndGetXmlDocumentDelegate = new EndOperationDelegate(this.OnEndGetXmlDocument);
            }
            if ((this.onGetXmlDocumentCompletedDelegate == null)) {
                this.onGetXmlDocumentCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnGetXmlDocumentCompleted);
            }
            base.InvokeAsync(this.onBeginGetXmlDocumentDelegate, new object[] {
                        SepaHederID}, this.onEndGetXmlDocumentDelegate, this.onGetXmlDocumentCompletedDelegate, userState);
        }
    }
}